using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenCg.Examples.Data.Models;

namespace OpenCg.Examples.OpenTK.Basic
{
    class ToonShading : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C9E3v_toonShading.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C9E4f_toonShading.cg";
        private const string CgVertexEntryFuncName = "C9E3v_toonShading";
        private const string CgFragmentEntryFuncName = "C9E4f_toonShading";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamLightPosition;
        private CgParameter cgParamEyePosition;
        private CgParameter cgParamShininess;
        private CgParameter cgParamKd;
        private CgParameter cgParamKs;
        private CgParameter cgParamDiffuseRamp;
        private CgParameter cgParamSpecularRamp;
        private CgParameter cgParamEdgeRamp;

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

            context = Cg.CreateContext();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context, true);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            CgGL.LoadProgram(cgVertexProgram);

            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");
            cgParamLightPosition = Cg.GetNamedParameter(cgVertexProgram, "lightPosition");
            cgParamEyePosition = Cg.GetNamedParameter(cgVertexProgram, "eyePosition");
            cgParamShininess = Cg.GetNamedParameter(cgVertexProgram, "shininess");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            CgGL.LoadProgram(cgFragmentProgram);

            cgParamKd = Cg.GetNamedParameter(cgFragmentProgram, "Kd");
            cgParamKs = Cg.GetNamedParameter(cgFragmentProgram, "Ks");
            cgParamDiffuseRamp = Cg.GetNamedParameter(cgFragmentProgram, "diffuseRamp");
            cgParamSpecularRamp = Cg.GetNamedParameter(cgFragmentProgram, "specularRamp");
            cgParamEdgeRamp = Cg.GetNamedParameter(cgFragmentProgram, "edgeRamp");

            Cg.SetParameter4f(cgParamKd, 0.8f, 0.6f, 0.2f, 1.0f);
            Cg.SetParameter4f(cgParamKs, 0.3f, 0.3f, 4.0f, 0.0f);
            Cg.SetParameter1f(cgParamShininess, shininess);

            CgGL.SetTextureParameter(cgParamDiffuseRamp, texDiffuseRamp);
            CgGL.SetTextureParameter(cgParamSpecularRamp, texSpecularRamp);
            CgGL.SetTextureParameter(cgParamEdgeRamp, texEdgeRamp);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 eyePos = new Vector3(8 * (float)Math.Sin(eyeAngle), eyeHeight, 8 * (float)Math.Cos(eyeAngle));
            Vector3 lightPos = new Vector3(2.5f * (float)Math.Sin(lightAngle), lightHeight, 2.5f * (float)Math.Cos(lightAngle));

            Matrix4 view = Matrix4.LookAt(eyePos, Vector3.Zero, Vector3.UnitY);
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(headSpin));

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            CgGL.BindProgram(cgFragmentProgram);
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
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.SetParameter3f(cgParamEyePosition, eyePos.X, eyePos.Y, eyePos.Z);
            Cg.SetParameter3f(cgParamLightPosition, lightPos.X, lightPos.Y, lightPos.Z);
            Cg.SetParameter1f(cgParamShininess, shininess);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

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
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
        }
    }
}
