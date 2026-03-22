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
    class BumpMapping : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C8E1v_bumpWall.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C8E2f_bumpSurf.cg";
        private const string CgVertexEntryFuncName = "C8E1v_bumpWall";
        private const string CgFragmentEntryFuncName = "C8E2f_bumpSurf";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamLightPosition;
        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamNormalMap;
        private CgParameter cgParamNormalizeCube;

        private int normalMapTexture;
        private int normalizeCubeTexture;

        private float lightAngle = 4.0f;
        private bool animating = true;

        private readonly float[] projectionMatrix = new float[16];

        public BumpMapping()
            : base("Cg Tutorial 16: Bump Mapping", 600, 400)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f);
            GL.Enable(EnableCap.DepthTest);

            // Normal map (Brick) - 128x128
            normalMapTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, normalMapTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 128, 128, 0,
                          PixelFormat.Rgb, PixelType.UnsignedByte, ImageBrick.Array);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Normalization cube map - 32x32 per face
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
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            context = Cg.CreateContext();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context, true);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            CgGL.LoadProgram(cgVertexProgram);

            cgParamLightPosition = Cg.GetNamedParameter(cgVertexProgram, "lightPosition");
            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            CgGL.LoadProgram(cgFragmentProgram);

            cgParamNormalMap = Cg.GetNamedParameter(cgFragmentProgram, "normalMap");
            cgParamNormalizeCube = Cg.GetNamedParameter(cgFragmentProgram, "normalizeCube");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float[] lightPosition = {
                12.5f * (float)Math.Sin(lightAngle),
                12.5f * (float)Math.Cos(lightAngle),
                4.0f
            };

            float[] viewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            BuildLookAtMatrix(0, 0, 20, 0, 0, 0, 0, 1, 0, viewMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            Matrix4 viewMat = Matrix4.LookAt(new Vector3(0, 0, 20), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref viewMat);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.SetParameter3fv(cgParamLightPosition, lightPosition);

            CgGL.SetTextureParameter(cgParamNormalMap, normalMapTexture);
            CgGL.SetTextureParameter(cgParamNormalizeCube, normalizeCubeTexture);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex3(-7, -7, 0);
            GL.TexCoord2(1, 0); GL.Vertex3( 7, -7, 0);
            GL.TexCoord2(1, 1); GL.Vertex3( 7,  7, 0);
            GL.TexCoord2(0, 1); GL.Vertex3(-7,  7, 0);
            GL.End();

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            GL.Translate(lightPosition[0], lightPosition[1], lightPosition[2]);
            GL.Color3(0.8f, 0.8f, 0.1f);
            NativeMethods.glutSolidSphere(0.4, 12, 12);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                lightAngle += 0.008f;
                if (lightAngle > 2 * Pi) lightAngle -= 2 * Pi;
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
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75.0f), e.Width / (float)e.Height, 0.1f, 100.0f);
            GL.LoadMatrix(ref projection);
            BuildPerspectiveMatrix(75.0, e.Width / (double)e.Height, 0.1, 100.0, projectionMatrix);
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