using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenCg.Graphics.ObjectModel;
using OpenCg.Graphics.ObjectModel.OpenGL;
using CgProgramObject = OpenCg.Graphics.ObjectModel.Program;
using CgParameterObject = OpenCg.Graphics.ObjectModel.Parameter;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[21] Specular Bump Map")]
    class SpecularBumpMap : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C8E3v_specWall.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C8E4f_specSurf.cg";
        private const string CgVertexEntryFuncName = "C8E3v_specWall";
        private const string CgFragmentEntryFuncName = "C8E4f_specSurf";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamLightPosition;
        private CgParameterObject cgParamEyePosition;
        private CgParameterObject cgParamModelViewProj;

        private CgParameterObject cgParamAmbient;
        private CgParameterObject cgParamLMd;
        private CgParameterObject cgParamLMs;
        private CgParameterObject cgParamNormalMap;
        private CgParameterObject cgParamNormalizeCube;
        private CgParameterObject cgParamNormalizeCube2;

        private int normalMapTexture;
        private int normalizeCubeTexture;

        private float lightAngle = 4.0f;
        private const float LightRotationSpeed = 0.5f;
        private bool animating = true;

        private readonly float[] projectionMatrix = new float[16];

        public SpecularBumpMap()
            : base("Cg Tutorial 22: Specular Bump Map", 600, 400)
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

            cgParamAmbient = cgFragmentProgram.GetNamedParameter("ambient");
            cgParamLMd = cgFragmentProgram.GetNamedParameter("LMd");
            cgParamLMs = cgFragmentProgram.GetNamedParameter("LMs");
            cgParamNormalMap = cgFragmentProgram.GetNamedParameter("normalMap");
            cgParamNormalizeCube = cgFragmentProgram.GetNamedParameter("normalizeCube");
            cgParamNormalizeCube2 = cgFragmentProgram.GetNamedParameter("normalizeCube2");

            SetParameter1f(cgParamAmbient, 0.2f);
            SetParameter3f(cgParamLMd, 0.8f, 0.7f, 0.2f);
            SetParameter3f(cgParamLMs, 0.5f, 0.5f, 0.8f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 eyePos = new Vector3(0, 0, 20);
            Vector3 lightPos = new Vector3(12.5f * (float)Math.Sin(lightAngle), 12.5f * (float)Math.Cos(lightAngle), 4.0f);

            Matrix4 view = Matrix4.LookAt(eyePos, Vector3.Zero, Vector3.UnitY);
            
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);

            float[] viewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            BuildLookAtMatrix(eyePos.X, eyePos.Y, eyePos.Z, 0, 0, 0, 0, 1, 0, viewMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3f(cgParamLightPosition, lightPos.X, lightPos.Y, lightPos.Z);
            SetParameter3f(cgParamEyePosition, eyePos.X, eyePos.Y, eyePos.Z);

            SetTextureParameter(cgParamNormalMap, normalMapTexture);
            SetTextureParameter(cgParamNormalizeCube, normalizeCubeTexture);
            SetTextureParameter(cgParamNormalizeCube2, normalizeCubeTexture);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-7.0f, -7.0f, 0.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(7.0f, -7.0f, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(7.0f,  7.0f, 0.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-7.0f,  7.0f, 0.0f);
            GL.End();

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);
            
            // Light representation
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);
            GL.Translate(lightPos.X, lightPos.Y, lightPos.Z);
            GL.Color3(1.0f, 1.0f, 0.0f);
            NativeMethods.glutSolidSphere(0.4, 12, 12);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                lightAngle += LightRotationSpeed * (float)e.Time;
            }
            if (IsKeyDown(Keys.Space) && !WasKeyDown(Keys.Space)) animating = !animating;
            if (IsKeyDown(Keys.Escape)) Close();
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f), e.Width / (float)e.Height, 0.1f, 100.0f);
            GL.LoadMatrix(ref proj);
            BuildPerspectiveMatrix(60.0, e.Width / (double)e.Height, 0.1, 100.0, projectionMatrix);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(normalMapTexture);
            GL.DeleteTexture(normalizeCubeTexture);
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }
    }
}
