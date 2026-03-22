using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace OpenCg.Examples.OpenTK.Basic
{
    class SpecularBumpMap : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C8E3v_specWall.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C8E4f_specSurf.cg";
        private const string CgVertexEntryFuncName = "C8E3v_specWall";
        private const string CgFragmentEntryFuncName = "C8E4f_specSurf";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamLightPosition;
        private CgParameter cgParamEyePosition;
        private CgParameter cgParamModelViewProj;

        private CgParameter cgParamAmbient;
        private CgParameter cgParamLMd;
        private CgParameter cgParamLMs;
        private CgParameter cgParamNormalMap;
        private CgParameter cgParamNormalizeCube;
        private CgParameter cgParamNormalizeCube2;

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

            cgParamAmbient = Cg.GetNamedParameter(cgFragmentProgram, "ambient");
            cgParamLMd = Cg.GetNamedParameter(cgFragmentProgram, "LMd");
            cgParamLMs = Cg.GetNamedParameter(cgFragmentProgram, "LMs");
            cgParamNormalMap = Cg.GetNamedParameter(cgFragmentProgram, "normalMap");
            cgParamNormalizeCube = Cg.GetNamedParameter(cgFragmentProgram, "normalizeCube");
            cgParamNormalizeCube2 = Cg.GetNamedParameter(cgFragmentProgram, "normalizeCube2");

            Cg.SetParameter1f(cgParamAmbient, 0.2f);
            Cg.SetParameter3f(cgParamLMd, 0.8f, 0.7f, 0.2f);
            Cg.SetParameter3f(cgParamLMs, 0.5f, 0.5f, 0.8f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 eyePos = new Vector3(0, 0, 20);
            Vector3 lightPos = new Vector3(12.5f * (float)Math.Sin(lightAngle), 12.5f * (float)Math.Cos(lightAngle), 4.0f);

            Matrix4 view = Matrix4.LookAt(eyePos, Vector3.Zero, Vector3.UnitY);
            
            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);

            float[] viewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            BuildLookAtMatrix(eyePos.X, eyePos.Y, eyePos.Z, 0, 0, 0, 0, 1, 0, viewMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.SetParameter3f(cgParamLightPosition, lightPos.X, lightPos.Y, lightPos.Z);
            Cg.SetParameter3f(cgParamEyePosition, eyePos.X, eyePos.Y, eyePos.Z);

            CgGL.SetTextureParameter(cgParamNormalMap, normalMapTexture);
            CgGL.SetTextureParameter(cgParamNormalizeCube, normalizeCubeTexture);
            CgGL.SetTextureParameter(cgParamNormalizeCube2, normalizeCubeTexture);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

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
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
        }
    }
}
