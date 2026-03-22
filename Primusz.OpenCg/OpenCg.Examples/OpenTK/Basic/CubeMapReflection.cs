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
    class CubeMapReflection : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C7E1v_reflection.cg";
        private const string CgVertexEntryFuncName = "C7E1v_reflection";
        private const string FragmentProgramFileName = "Data\\Shaders\\C7E2f_reflection.cg";
        private const string CgFragmentEntryFuncName = "C7E2f_reflection";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamEyePositionW;
        private CgParameter cgParamModelToWorld;
        private CgParameter cgParamReflectivity;
        private CgParameter cgParamDecalMap;
        private CgParameter cgParamEnvironmentMap;

        private float eyeHeight = 0.0f;
        private float eyeAngle = 0.53f;
        private float headSpin = 0.0f;
        private float reflectivity = 0.6f;
        private bool animating = true;

        private int decalTexture;
        private int environmentTexture;
        private float[] monkeyHeadTexcoords;

        private readonly float[] projectionMatrix = new float[16];

        public CubeMapReflection()
            : base("Cg Tutorial 18: Cube Map Reflection", 600, 600)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            decalTexture = DDSLoader.LoadTexture2D("Data\\Images\\TilePattern.dds");
            environmentTexture = DDSLoader.LoadTextureCubeMap("Data\\Images\\CloudyHillsCubemap.dds");

            int numVertices = MonkeyHead.Vertices.Length / 3;
            const float scaleFactor = 1.5f;
            monkeyHeadTexcoords = new float[numVertices * 2];
            for (int i = 0; i < numVertices; i++)
            {
                monkeyHeadTexcoords[i * 2 + 0] = scaleFactor * MonkeyHead.Vertices[i * 3 + 0];
                monkeyHeadTexcoords[i * 2 + 1] = scaleFactor * MonkeyHead.Vertices[i * 3 + 1];
            }

            context = Cg.CreateContext();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context, true);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

            cgVertexProfile = CgProfile.Arbvp1;
            CgGL.SetOptimalOptions(cgVertexProfile);
            cgVertexProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            CgGL.LoadProgram(cgVertexProgram);

            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");
            cgParamEyePositionW = Cg.GetNamedParameter(cgVertexProgram, "eyePositionW");
            cgParamModelToWorld = Cg.GetNamedParameter(cgVertexProgram, "modelToWorld");

            cgFragmentProfile = CgProfile.Arbfp1;
            CgGL.SetOptimalOptions(cgFragmentProfile);
            cgFragmentProgram = Cg.CreateProgramFromFile(context, CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            CgGL.LoadProgram(cgFragmentProgram);

            cgParamReflectivity = Cg.GetNamedParameter(cgFragmentProgram, "reflectivity");
            cgParamDecalMap = Cg.GetNamedParameter(cgFragmentProgram, "decalMap");
            cgParamEnvironmentMap = Cg.GetNamedParameter(cgFragmentProgram, "environmentMap");

            Cg.SetParameter1f(cgParamReflectivity, reflectivity);
            CgGL.SetTextureParameter(cgParamDecalMap, decalTexture);
            CgGL.SetTextureParameter(cgParamEnvironmentMap, environmentTexture);
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
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, MonkeyHead.Vertices);
            GL.NormalPointer(NormalPointerType.Float, 0, MonkeyHead.Normals);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, monkeyHeadTexcoords);
            GL.DrawElements(PrimitiveType.Triangles, MonkeyHead.Indices.Length, DrawElementsType.UnsignedShort, MonkeyHead.Indices);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.VertexArray);

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
            if (IsKeyDown(Keys.Equal) || IsKeyDown(Keys.KeyPadAdd)) 
            {
                reflectivity += 0.05f;
                if (reflectivity > 1.0f) reflectivity = 1.0f;
                Cg.SetParameter1f(cgParamReflectivity, reflectivity);
            }
            if (IsKeyDown(Keys.Minus) || IsKeyDown(Keys.KeyPadSubtract)) 
            {
                reflectivity -= 0.05f;
                if (reflectivity < 0.0f) reflectivity = 0.0f;
                Cg.SetParameter1f(cgParamReflectivity, reflectivity);
            }
            if (IsKeyDown(Keys.Escape)) Close();

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(40.0f), e.Width / (float)e.Height, 1.0f, 50.0f);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            BuildPerspectiveMatrix(40.0, e.Width / (double)e.Height, 1.0, 50.0, projectionMatrix);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(decalTexture);
            GL.DeleteTexture(environmentTexture);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
        }
    }
}
