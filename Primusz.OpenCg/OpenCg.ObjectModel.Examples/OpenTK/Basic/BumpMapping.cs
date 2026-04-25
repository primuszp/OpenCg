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
    [ExampleAttribute("OpenTK/Basic/[16] Bump Mapping")]
    class BumpMapping : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C8E1v_bumpWall.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C8E2f_bumpSurf.cg";
        private const string CgVertexEntryFuncName = "C8E1v_bumpWall";
        private const string CgFragmentEntryFuncName = "C8E2f_bumpSurf";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamLightPosition;
        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamNormalMap;
        private CgParameterObject cgParamNormalizeCube;

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

            context = OpenCg.Graphics.ObjectModel.Context.Create();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context.Handle, true);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = context.CreateProgramFromFile( CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgParamLightPosition = cgVertexProgram.GetNamedParameter("lightPosition");
            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = context.CreateProgramFromFile( CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamNormalMap = cgFragmentProgram.GetNamedParameter("normalMap");
            cgParamNormalizeCube = cgFragmentProgram.GetNamedParameter("normalizeCube");
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

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            Matrix4 viewMat = Matrix4.LookAt(new Vector3(0, 0, 20), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref viewMat);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3fv(cgParamLightPosition, lightPosition);

            SetTextureParameter(cgParamNormalMap, normalMapTexture);
            SetTextureParameter(cgParamNormalizeCube, normalizeCubeTexture);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

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
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }
    }
}