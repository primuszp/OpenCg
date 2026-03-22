using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenCg.Examples.Data.Models;
using OpenCg.Examples;

namespace OpenCg.Examples.OpenTK.Basic
{
    class ChromaticDispersion : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C7E5v_dispersion.cg";
        private const string CgVertexEntryFuncName = "C7E5v_dispersion";
        private const string FragmentProgramFileName = "Data\\Shaders\\C7E6f_dispersion.cg";
        private const string CgFragmentEntryFuncName = "C7E6f_dispersion";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamEyePositionW;
        private CgParameter cgParamModelToWorld;
        private CgParameter cgParamEtaRatio;
        private CgParameter cgParamFresnelBias;
        private CgParameter cgParamFresnelScale;
        private CgParameter cgParamFresnelPower;
        
        private CgParameter cgParamEnvironmentMap0;
        private CgParameter cgParamEnvironmentMap1;
        private CgParameter cgParamEnvironmentMap2;
        private CgParameter cgParamEnvironmentMap3;

        private float eyeHeight = 0.0f;
        private float eyeAngle = 0.53f;
        private float headSpin = 0.0f;
        
        private float[] etaRatio = new float[3] { 1.1f, 1.2f, 1.3f };
        private float fresnelBias = 0.0f;
        private float fresnelScale = 1.0f;
        private float fresnelPower = 4.0f;
        
        private bool animating = true;

        private int environmentTexture;

        private readonly float[] projectionMatrix = new float[16];

        public ChromaticDispersion()
            : base("Cg Tutorial 20: Chromatic Dispersion", 600, 600)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            environmentTexture = DDSLoader.LoadTextureCubeMap("Data\\Images\\CloudyHillsCubemap.dds");

            context = Cg.CreateContext();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context, true);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            CgGL.LoadProgram(cgVertexProgram);

            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");
            cgParamEyePositionW = Cg.GetNamedParameter(cgVertexProgram, "eyePositionW");
            cgParamModelToWorld = Cg.GetNamedParameter(cgVertexProgram, "modelToWorld");
            cgParamEtaRatio = Cg.GetNamedParameter(cgVertexProgram, "etaRatio");
            cgParamFresnelBias = Cg.GetNamedParameter(cgVertexProgram, "fresnelBias");
            cgParamFresnelScale = Cg.GetNamedParameter(cgVertexProgram, "fresnelScale");
            cgParamFresnelPower = Cg.GetNamedParameter(cgVertexProgram, "fresnelPower");

            Cg.SetParameter3fv(cgParamEtaRatio, etaRatio);
            Cg.SetParameter1f(cgParamFresnelBias, fresnelBias);
            Cg.SetParameter1f(cgParamFresnelScale, fresnelScale);
            Cg.SetParameter1f(cgParamFresnelPower, fresnelPower);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            CgGL.LoadProgram(cgFragmentProgram);

            cgParamEnvironmentMap0 = Cg.GetNamedParameter(cgFragmentProgram, "environmentMap0");
            cgParamEnvironmentMap1 = Cg.GetNamedParameter(cgFragmentProgram, "environmentMap1");
            cgParamEnvironmentMap2 = Cg.GetNamedParameter(cgFragmentProgram, "environmentMap2");
            cgParamEnvironmentMap3 = Cg.GetNamedParameter(cgFragmentProgram, "environmentMap3");

            CgGL.SetTextureParameter(cgParamEnvironmentMap0, environmentTexture);
            CgGL.SetTextureParameter(cgParamEnvironmentMap1, environmentTexture);
            CgGL.SetTextureParameter(cgParamEnvironmentMap2, environmentTexture);
            CgGL.SetTextureParameter(cgParamEnvironmentMap3, environmentTexture);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            float eyeRadius = 6.0f;
            Vector3 eyePosition = new Vector3(
                (float)(Math.Sin(eyeAngle) * eyeRadius),
                eyeHeight,
                (float)(Math.Cos(eyeAngle) * eyeRadius)
            );

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float[] viewMatrix = new float[16];
            float[] modelMatrix = new float[16];
            float[] modelViewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];

            BuildLookAtMatrix(eyePosition.X, eyePosition.Y, eyePosition.Z,
                0, 0, 0, 0, 1, 0, viewMatrix);
            MakeRotateMatrix(headSpin, 0, 1, 0, modelMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.SetParameter3f(cgParamEyePositionW, eyePosition.X, eyePosition.Y, eyePosition.Z);
            Cg.SetMatrixParameterfr(cgParamModelToWorld, modelMatrix);

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

            drawSurroundings(eyePosition);

            SwapBuffers();
        }

        private void drawSurroundings(Vector3 eyePosition)
        {
            float[][] vertex = {
                new float[] { 1, -1, -1 },  new float[] { 1, 1, -1 },  new float[] { 1, 1, 1 },  new float[] { 1, -1, 1 },
                new float[] { -1, -1, -1 },  new float[] { -1, 1, -1 },  new float[] { -1, 1, 1 },  new float[] { -1, -1, 1 },
                new float[] { -1, 1, -1 },  new float[] { 1, 1, -1 },  new float[] { 1, 1, 1 },  new float[] { -1, 1, 1 },
                new float[] { -1, -1, -1 },  new float[] { 1, -1, -1 },  new float[] { 1, -1, 1 },  new float[] { -1, -1, 1 },
                new float[] { -1, -1, 1 },  new float[] { 1, -1, 1 },  new float[] { 1, 1, 1 },  new float[] { -1, 1, 1 },
                new float[] { -1, -1, -1 },  new float[] { 1, -1, -1 },  new float[] { 1, 1, -1 },  new float[] { -1, 1, -1 }
            };

            float surroundingsDistance = 8.0f;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Matrix4 viewMat = Matrix4.LookAt(eyePosition, Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref viewMat);
            
            GL.Scale(surroundingsDistance, surroundingsDistance, surroundingsDistance);

            GL.Enable(EnableCap.TextureCubeMap);
            GL.BindTexture(TextureTarget.TextureCubeMap, environmentTexture);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Replace);
            
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < 24; i++)
            {
                GL.TexCoord3(vertex[i][0], vertex[i][1], vertex[i][2]);
                GL.Vertex3(vertex[i][0], vertex[i][1], vertex[i][2]);
            }
            GL.End();
            GL.Disable(EnableCap.TextureCubeMap);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                headSpin -= 30.0f * (float)e.Time;
            }

            if (IsKeyDown(Keys.Space) && !WasKeyDown(Keys.Space)) animating = !animating;
            if (IsKeyDown(Keys.Escape)) Close();

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            BuildPerspectiveMatrix(40.0, e.Width / (double)e.Height, 1.0, 50.0, projectionMatrix);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(environmentTexture);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
        }
    }
}
