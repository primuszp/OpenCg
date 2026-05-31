using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenCg.Examples.OpenTK.Basic
{
    class BumpMapTorus : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C8E6v_torus.cg";
        private const string CgVertexEntryFuncName = "C8E6v_torus";
        private const string FragmentProgramFileName = "Data\\Shaders\\C8E4f_specSurf.cg";
        private const string CgFragmentEntryFuncName = "C8E4f_specSurf";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamLightPosition;
        private CgParameter cgParamEyePosition;
        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamTorusInfo;

        private CgParameter cgParamAmbient;
        private CgParameter cgParamLMd;
        private CgParameter cgParamLMs;
        private CgParameter cgParamNormalMap;
        private CgParameter cgParamNormalizeCube;
        private CgParameter cgParamNormalizeCube2;

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

            context = Cg.CreateContext();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context, true);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            CgGL.LoadProgram(cgVertexProgram);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            CgGL.LoadProgram(cgFragmentProgram);

            cgParamLightPosition = Cg.GetNamedParameter(cgVertexProgram, "lightPosition");
            cgParamEyePosition = Cg.GetNamedParameter(cgVertexProgram, "eyePosition");
            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");
            cgParamTorusInfo = Cg.GetNamedParameter(cgVertexProgram, "torusInfo");

            cgParamAmbient = Cg.GetNamedParameter(cgFragmentProgram, "ambient");
            cgParamLMd = Cg.GetNamedParameter(cgFragmentProgram, "LMd");
            cgParamLMs = Cg.GetNamedParameter(cgFragmentProgram, "LMs");
            cgParamNormalMap = Cg.GetNamedParameter(cgFragmentProgram, "normalMap");
            cgParamNormalizeCube = Cg.GetNamedParameter(cgFragmentProgram, "normalizeCube");
            cgParamNormalizeCube2 = Cg.GetNamedParameter(cgFragmentProgram, "normalizeCube2");

            Cg.SetParameter1f(cgParamAmbient, 0.3f);
            Cg.SetParameter4f(cgParamLMd, 0.9f, 0.6f, 0.3f, 1.0f);
            Cg.SetParameter4f(cgParamLMs, 1.0f, 1.0f, 1.0f, 1.0f);
            Cg.SetParameter2f(cgParamTorusInfo, 6.0f, 2.0f);
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

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);

            float[] currentLightPos = { -8.0f, 0.0f, 15.0f };
            Cg.SetParameter3fv(cgParamLightPosition, currentLightPos);
            Cg.SetParameter3f(cgParamEyePosition, eyex, eyey, eyez);

            CgGL.SetTextureParameter(cgParamNormalMap, normalMapTexture);
            CgGL.SetTextureParameter(cgParamNormalizeCube, normalizeCubeTexture);
            CgGL.SetTextureParameter(cgParamNormalizeCube2, normalizeCubeTexture);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

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
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
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
