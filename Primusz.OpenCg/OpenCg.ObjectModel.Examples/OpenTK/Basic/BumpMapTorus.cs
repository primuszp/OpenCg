using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenCg.Graphics.ObjectModel;
using OpenCg.Graphics.ObjectModel.OpenGL;
using CgProgramObject = OpenCg.Graphics.ObjectModel.Program;
using CgParameterObject = OpenCg.Graphics.ObjectModel.Parameter;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[23] Bump Map Torus")]
    class BumpMapTorus : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C8E6v_torus.cg";
        private const string CgVertexEntryFuncName = "C8E6v_torus";
        private const string FragmentProgramFileName = "Data\\Shaders\\C8E4f_specSurf.cg";
        private const string CgFragmentEntryFuncName = "C8E4f_specSurf";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamLightPosition;
        private CgParameterObject cgParamEyePosition;
        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamTorusInfo;

        private CgParameterObject cgParamAmbient;
        private CgParameterObject cgParamLMd;
        private CgParameterObject cgParamLMs;
        private CgParameterObject cgParamNormalMap;
        private CgParameterObject cgParamNormalizeCube;
        private CgParameterObject cgParamNormalizeCube2;

        private int normalMapTexture;
        private int normalizeCubeTexture;

        private float myEyeAngle = 0.0f;
        private bool animating = true;

        private readonly float[] projectionMatrix = new float[16];

        public BumpMapTorus()
            : base("Cg Tutorial 24: Bump Map Torus", 400, 400)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.TextureCubeMap);

            normalMapTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, normalMapTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 128, 128, 0,
                          PixelFormat.Rgb, PixelType.UnsignedByte, ImageBrick.Array);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            normalizeCubeTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, normalizeCubeTexture);
            int faceSize = 32 * 32 * 3;
            for (int i = 0; i < 6; i++)
            {
                byte[] faceData = new byte[faceSize];
                System.Array.Copy(ImageNormcm.Array, i * faceSize, faceData, 0, faceSize);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb8, 32, 32, 0,
                              PixelFormat.Rgb, PixelType.UnsignedByte, faceData);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            context = OpenCg.Graphics.ObjectModel.Context.Create();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context.Handle, true);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = context.CreateProgramFromFile( CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = context.CreateProgramFromFile( CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamLightPosition = cgVertexProgram.GetNamedParameter("lightPosition");
            cgParamEyePosition = cgVertexProgram.GetNamedParameter("eyePosition");
            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");
            cgParamTorusInfo = cgVertexProgram.GetNamedParameter("torusInfo");

            cgParamAmbient = cgFragmentProgram.GetNamedParameter("ambient");
            cgParamLMd = cgFragmentProgram.GetNamedParameter("LMd");
            cgParamLMs = cgFragmentProgram.GetNamedParameter("LMs");
            cgParamNormalMap = cgFragmentProgram.GetNamedParameter("normalMap");
            cgParamNormalizeCube = cgFragmentProgram.GetNamedParameter("normalizeCube");
            cgParamNormalizeCube2 = cgFragmentProgram.GetNamedParameter("normalizeCube2");

            SetParameter1f(cgParamAmbient, 0.3f);
            SetParameter4f(cgParamLMd, 0.9f, 0.6f, 0.3f, 1.0f);
            SetParameter4f(cgParamLMs, 1.0f, 1.0f, 1.0f, 1.0f);
            SetParameter2f(cgParamTorusInfo, 6.0f, 2.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float eyex = 18.0f * (float)Math.Sin(myEyeAngle);
            float eyey = 8.0f * (float)Math.Sin(myEyeAngle);
            float eyez = 18.0f * (float)Math.Cos(myEyeAngle);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Matrix4 view = Matrix4.LookAt(new Vector3(eyex, eyey, eyez), Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref view);

            float[] viewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            BuildLookAtMatrix(eyex, eyey, eyez, 0, 0, 0, 0, 1, 0, viewMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);

            float[] currentLightPos = { -8.0f, 0.0f, 15.0f };
            SetParameter3fv(cgParamLightPosition, currentLightPos);
            SetParameter3f(cgParamEyePosition, eyex, eyey, eyez);

            SetTextureParameter(cgParamNormalMap, normalMapTexture);
            SetTextureParameter(cgParamNormalizeCube, normalizeCubeTexture);
            SetTextureParameter(cgParamNormalizeCube2, normalizeCubeTexture);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

            int steps = 60;
            for (int i = 0; i < steps; i++)
            {
                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= steps; j++)
                {
                    float u = (float)j / steps;
                    float v0 = (float)i / steps;
                    float v1 = (float)(i + 1) / steps;
                    GL.Vertex2(u, v0);
                    GL.Vertex2(u, v1);
                }
                GL.End();
            }

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);
            GL.Translate(currentLightPos[0], currentLightPos[1], currentLightPos[2]);
            GL.Color3(0.8f, 0.8f, 0.1f);
            NativeMethods.glutSolidSphere(0.4, 12, 12);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(normalMapTexture);
            GL.DeleteTexture(normalizeCubeTexture);
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                myEyeAngle += 3.0f * (float)e.Time;
                if (myEyeAngle > 2 * Pi) myEyeAngle -= 2 * Pi;
            }
            if (IsKeyDown(Keys.Space)) animating = !animating;
            if (IsKeyDown(Keys.Escape)) Close();
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75.0f), e.Width / (float)e.Height, 0.1f, 100.0f);
            GL.LoadMatrix(ref proj);
            BuildPerspectiveMatrix(75.0, e.Width / (double)e.Height, 0.1, 100.0, projectionMatrix);
            base.OnResize(e);
        }
    }
}
