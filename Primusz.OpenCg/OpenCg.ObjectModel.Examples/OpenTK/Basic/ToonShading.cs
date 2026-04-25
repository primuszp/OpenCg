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
using OpenCg.ObjectModel.Examples.Data.Models;

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[25] Toon Shading")]
    class ToonShading : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C9E3v_toonShading.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C9E4f_toonShading.cg";
        private const string CgVertexEntryFuncName = "C9E3v_toonShading";
        private const string CgFragmentEntryFuncName = "C9E4f_toonShading";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamLightPosition;
        private CgParameterObject cgParamEyePosition;
        private CgParameterObject cgParamShininess;
        private CgParameterObject cgParamKd;
        private CgParameterObject cgParamKs;
        private CgParameterObject cgParamDiffuseRamp;
        private CgParameterObject cgParamSpecularRamp;
        private CgParameterObject cgParamEdgeRamp;

        private int texDiffuseRamp, texSpecularRamp, texEdgeRamp;

        private float eyeHeight = 0.0f;
        private float eyeAngle = 0.53f;
        private float lightAngle = -0.4f;
        private float lightHeight = 1.0f;
        private float headSpin = 0.0f;
        private float shininess = 15.0f;

        private bool animating = true;

        private readonly float[] projectionMatrix = new float[16];

        public ToonShading()
            : base("Cg Tutorial 26: Toon Shading", 600, 600)
        { }

        private int CreateRampTexture(int size, Func<float, float> rampFunc)
        {
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = rampFunc((float)i / (size - 1));
            }

            int tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture1D, tex);
            GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.R32f, size, 0, PixelFormat.Red, PixelType.Float, data);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            return tex;
        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            texDiffuseRamp = CreateRampTexture(256, x => x > 0.5f ? x * x * (3 - 2 * x) : 0.5f);
            texSpecularRamp = CreateRampTexture(256, x => x > 0.2f ? x : 0.0f);
            texEdgeRamp = CreateRampTexture(256, x => x < 0.2f ? 1.0f : 0.85f);

            context = OpenCg.Graphics.ObjectModel.Context.Create();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context.Handle, true);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = context.CreateProgramFromFile( CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");
            cgParamLightPosition = cgVertexProgram.GetNamedParameter("lightPosition");
            cgParamEyePosition = cgVertexProgram.GetNamedParameter("eyePosition");
            cgParamShininess = cgVertexProgram.GetNamedParameter("shininess");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = context.CreateProgramFromFile( CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamKd = cgFragmentProgram.GetNamedParameter("Kd");
            cgParamKs = cgFragmentProgram.GetNamedParameter("Ks");
            cgParamDiffuseRamp = cgFragmentProgram.GetNamedParameter("diffuseRamp");
            cgParamSpecularRamp = cgFragmentProgram.GetNamedParameter("specularRamp");
            cgParamEdgeRamp = cgFragmentProgram.GetNamedParameter("edgeRamp");

            SetParameter4f(cgParamKd, 0.8f, 0.6f, 0.2f, 1.0f);
            SetParameter4f(cgParamKs, 0.3f, 0.3f, 4.0f, 0.0f);
            SetParameter1f(cgParamShininess, shininess);

            SetTextureParameter(cgParamDiffuseRamp, texDiffuseRamp);
            SetTextureParameter(cgParamSpecularRamp, texSpecularRamp);
            SetTextureParameter(cgParamEdgeRamp, texEdgeRamp);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 eyePos = new Vector3(8 * (float)Math.Sin(eyeAngle), eyeHeight, 8 * (float)Math.Cos(eyeAngle));
            Vector3 lightPos = new Vector3(2.5f * (float)Math.Sin(lightAngle), lightHeight, 2.5f * (float)Math.Cos(lightAngle));

            Matrix4 view = Matrix4.LookAt(eyePos, Vector3.Zero, Vector3.UnitY);
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(headSpin));

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);
            GL.MultMatrix(ref model);

            float[] viewMatrix = new float[16];
            float[] modelMatrix = new float[16];
            float[] modelViewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            BuildLookAtMatrix(eyePos.X, eyePos.Y, eyePos.Z, 0, 0, 0, 0, 1, 0, viewMatrix);
            MakeRotateMatrix(headSpin, 0, 1, 0, modelMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3f(cgParamEyePosition, eyePos.X, eyePos.Y, eyePos.Z);
            SetParameter3f(cgParamLightPosition, lightPos.X, lightPos.Y, lightPos.Z);
            SetParameter1f(cgParamShininess, shininess);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, MonkeyHead.Vertices);
            GL.NormalPointer(NormalPointerType.Float, 0, MonkeyHead.Normals);
            GL.DrawElements(PrimitiveType.Triangles, MonkeyHead.Indices.Length, DrawElementsType.UnsignedShort, MonkeyHead.Indices);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            // Light representation
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);
            GL.Translate(lightPos.X, lightPos.Y, lightPos.Z);
            GL.Color3(1.0f, 1.0f, 0.0f);
            NativeMethods.glutSolidSphere(0.1, 10, 10);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                headSpin -= 30.0f * (float)e.Time;
            }
            if (IsKeyDown(Keys.Space) && !WasKeyDown(Keys.Space)) animating = !animating;
            if (IsKeyDown(Keys.Equal) || IsKeyDown(Keys.KeyPadAdd)) shininess *= 1.05f;
            if (IsKeyDown(Keys.Minus) || IsKeyDown(Keys.KeyPadSubtract)) shininess /= 1.05f;
            if (IsKeyDown(Keys.Escape)) Close();
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(40.0f), e.Width / (float)e.Height, 1.0f, 50.0f);
            GL.LoadMatrix(ref proj);
            BuildPerspectiveMatrix(40.0, e.Width / (double)e.Height, 1.0, 50.0, projectionMatrix);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(texDiffuseRamp);
            GL.DeleteTexture(texSpecularRamp);
            GL.DeleteTexture(texEdgeRamp);
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }
    }
}
