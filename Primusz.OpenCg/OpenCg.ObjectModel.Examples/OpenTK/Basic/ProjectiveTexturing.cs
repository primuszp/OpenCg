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
    [ExampleAttribute("OpenTK/Basic/[17] Projective Texturing")]
    class ProjectiveTexturing : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C9E5v_projTex.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C9E6f_projTex.cg";
        private const string CgVertexEntryFuncName = "C9E5v_projTex";
        private const string CgFragmentEntryFuncName = "C9E6f_projTex";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamKd;
        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamLightPosition;
        private CgParameterObject cgParamTextureMatrix;
        private CgParameterObject cgParamProjectiveMap;

        private int projectiveMapTexture;

        private const float EyeAngle = 0.53f;
        private const float EyeHeight = 5.0f;
        private const float LightHeight = 2.0f;

        private float lightAngle = -0.4f;
        private bool animating = true;

        private readonly float[] projectionMatrix = new float[16];
        private readonly float[] kd = { 1, 1, 1 };

        public ProjectiveTexturing()
            : base("Cg Tutorial 27: Projective Texturing", 600, 400)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);
            GL.Enable(EnableCap.DepthTest);

            projectiveMapTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, projectiveMapTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 128, 128, 0,
                          PixelFormat.Rgb, PixelType.UnsignedByte, ImageDemon.Array);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            float[] border = { 1.0f, 1.0f, 1.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, border);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            context = OpenCg.Graphics.ObjectModel.Context.Create();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context.Handle, true);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = context.CreateProgramFromFile( CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgParamKd = cgVertexProgram.GetNamedParameter("Kd");
            SetParameter3fv(cgParamKd, kd);
            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");
            cgParamLightPosition = cgVertexProgram.GetNamedParameter("lightPosition");
            cgParamTextureMatrix = cgVertexProgram.GetNamedParameter("textureMatrix");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = context.CreateProgramFromFile( CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamProjectiveMap = cgFragmentProgram.GetNamedParameter("projectiveMap");
            SetTextureParameter(cgParamProjectiveMap, projectiveMapTexture);
        }

        private static void MakeClipToTextureMatrix(float[] m)
        {
            m[0] = 0.5f; m[1] = 0;    m[2] = 0;    m[3] = 0.5f;
            m[4] = 0;    m[5] = 0.5f; m[6] = 0;    m[7] = 0.5f;
            m[8] = 0;    m[9] = 0;    m[10] = 0.5f; m[11] = 0.5f;
            m[12] = 0;   m[13] = 0;   m[14] = 0;    m[15] = 1;
        }

        private static void BuildTextureMatrix(float[] lightViewMatrix, float[] modelMatrix, float[] textureMatrix)
        {
            float[] textureProjectionMatrix = new float[16];
            float[] clipToTextureMatrix = new float[16];
            float[] eyeToClipMatrix = new float[16];
            float[] modelViewMatrix = new float[16];

            BuildPerspectiveMatrix(50.0, 1.0, 0.25, 20.0, textureProjectionMatrix);
            MakeClipToTextureMatrix(clipToTextureMatrix);
            MultMatrix(eyeToClipMatrix, clipToTextureMatrix, textureProjectionMatrix);
            MultMatrix(modelViewMatrix, lightViewMatrix, modelMatrix);
            MultMatrix(textureMatrix, eyeToClipMatrix, modelViewMatrix);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float[] eyePosition = {
                10 * (float)Math.Sin(EyeAngle), EyeHeight, 10 * (float)Math.Cos(EyeAngle), 1
            };
            float[] lightPosition = {
                4.5f * (float)Math.Sin(lightAngle), LightHeight, 4.5f * (float)Math.Cos(lightAngle), 1
            };

            float[] eyeViewMatrix = new float[16];
            float[] lightViewMatrix = new float[16];
            BuildLookAtMatrix(eyePosition[0], eyePosition[1], eyePosition[2], 0, 0, 0, 0, 1, 0, eyeViewMatrix);
            BuildLookAtMatrix(lightPosition[0], lightPosition[1], lightPosition[2], 0, 0, 0, 0, -1, 0, lightViewMatrix);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            float[] translateMatrix = new float[16];
            float[] rotateMatrix = new float[16];
            float[] modelMatrix = new float[16];
            float[] invModelMatrix = new float[16];
            float[] modelViewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            float[] textureMatrix = new float[16];
            float[] objSpaceLightPosition = new float[4];

            // Sphere: model = translate(2,0,0) * rotate(70, (1,1,1))
            MakeRotateMatrix(70, 1, 1, 1, rotateMatrix);
            MakeTranslateMatrix(2, 0, 0, translateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            MultMatrix(modelViewMatrix, eyeViewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);
            BuildTextureMatrix(lightViewMatrix, modelMatrix, textureMatrix);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);
            SetMatrixParameterfr(cgParamTextureMatrix, textureMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidSphere(2.0, 40, 40);

            // Cube: model = translate(-2,-1,0) * rotateX(90)
            MakeTranslateMatrix(-2, -1, 0, translateMatrix);
            MakeRotateMatrix(90, 1, 0, 0, rotateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            MultMatrix(modelViewMatrix, eyeViewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);
            BuildTextureMatrix(lightViewMatrix, modelMatrix, textureMatrix);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);
            SetMatrixParameterfr(cgParamTextureMatrix, textureMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidCube(2.5);

            // Floor/Walls: model = identity, light in world space
            MultMatrix(modelViewProjMatrix, projectionMatrix, eyeViewMatrix);
            MakeTranslateMatrix(0, 0, 0, modelMatrix);
            BuildTextureMatrix(lightViewMatrix, modelMatrix, textureMatrix);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3fv(cgParamLightPosition, lightPosition);
            SetMatrixParameterfr(cgParamTextureMatrix, textureMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

            GL.Enable(EnableCap.CullFace);
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0f,  1f,  0f); GL.Vertex3( 12f,-2,-12); GL.Vertex3(-12f,-2,-12); GL.Vertex3(-12f,-2, 12); GL.Vertex3( 12f,-2, 12);
            GL.Normal3(0f,  0f,  1f); GL.Vertex3(-12f,-2,-12); GL.Vertex3( 12f,-2,-12); GL.Vertex3( 12f,10,-12); GL.Vertex3(-12f,10,-12);
            GL.Normal3(0f,  0f, -1f); GL.Vertex3( 12f,-2, 12); GL.Vertex3(-12f,-2, 12); GL.Vertex3(-12f,10, 12); GL.Vertex3( 12f,10, 12);
            GL.Normal3(0f, -1f,  0f); GL.Vertex3(-12f,10,-12); GL.Vertex3( 12f,10,-12); GL.Vertex3( 12f,10, 12); GL.Vertex3(-12f,10, 12);
            GL.Normal3(1f,  0f,  0f); GL.Vertex3(-12f,-2, 12); GL.Vertex3(-12f,-2,-12); GL.Vertex3(-12f,10,-12); GL.Vertex3(-12f,10, 12);
            GL.Normal3(-1f, 0f,  0f); GL.Vertex3( 12f,-2,-12); GL.Vertex3( 12f,-2, 12); GL.Vertex3( 12f,10, 12); GL.Vertex3( 12f,10,-12);
            GL.End();
            GL.Disable(EnableCap.CullFace);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            // Light sphere (fixed-function)
            MakeTranslateMatrix(lightPosition[0], lightPosition[1], lightPosition[2], modelMatrix);
            MultMatrix(modelViewMatrix, eyeViewMatrix, modelMatrix);
            // Transpose row-major to column-major for GL.LoadMatrix
            float[] col = new float[16];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    col[c * 4 + r] = modelViewMatrix[r * 4 + c];
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadMatrix(col);
            GL.Color3(1f, 1f, 1f);
            NativeMethods.glutSolidSphere(0.15, 30, 30);
            GL.PopMatrix();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                lightAngle += 0.5f * (float)e.Time;
                if (lightAngle > 2 * Pi) lightAngle -= 2 * Pi;
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
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(40.0f), e.Width / (float)e.Height, 1.0f, 50.0f);
            GL.LoadMatrix(ref projection);
            BuildPerspectiveMatrix(40.0, e.Width / (double)e.Height, 1.0, 50.0, projectionMatrix);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(projectiveMapTexture);
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }
    }
}
