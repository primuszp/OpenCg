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
using OpenCg.ObjectModel.Examples;

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[19] Cube Map Refraction")]
    class CubeMapRefraction : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C7E3v_refraction.cg";
        private const string CgVertexEntryFuncName = "C7E3v_refraction";
        private const string FragmentProgramFileName = "Data\\Shaders\\C7E4f_refraction.cg";
        private const string CgFragmentEntryFuncName = "C7E4f_refraction";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamEyePositionW;
        private CgParameterObject cgParamModelToWorld;
        private CgParameterObject cgParamEtaRatio;
        
        private CgParameterObject cgParamTransmittance;
        private CgParameterObject cgParamDecalMap;
        private CgParameterObject cgParamEnvironmentMap;

        private float eyeHeight = 0.0f;
        private float eyeAngle = 0.53f;
        private float headSpin = 0.0f;
        private float etaRatio = 1.5f;
        private float transmittance = 0.6f;
        private bool animating = true;

        private int decalTexture;
        private int environmentTexture;
        private float[] monkeyHeadTexcoords;

        private readonly float[] projectionMatrix = new float[16];

        public CubeMapRefraction()
            : base("Cg Tutorial 19: Cube Map Refraction", 600, 600)
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

            context = OpenCg.Graphics.ObjectModel.Context.Create();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context.Handle, true);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = context.CreateProgramFromFile( CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");
            cgParamEyePositionW = cgVertexProgram.GetNamedParameter("eyePositionW");
            cgParamModelToWorld = cgVertexProgram.GetNamedParameter("modelToWorld");
            cgParamEtaRatio = cgVertexProgram.GetNamedParameter("etaRatio");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = context.CreateProgramFromFile( CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamTransmittance = cgFragmentProgram.GetNamedParameter("transmittance");
            cgParamDecalMap = cgFragmentProgram.GetNamedParameter("decalMap");
            cgParamEnvironmentMap = cgFragmentProgram.GetNamedParameter("environmentMap");

            SetParameter1f(cgParamEtaRatio, etaRatio);
            SetParameter1f(cgParamTransmittance, transmittance);
            SetTextureParameter(cgParamDecalMap, decalTexture);
            SetTextureParameter(cgParamEnvironmentMap, environmentTexture);
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

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3f(cgParamEyePositionW, eyePosition.X, eyePosition.Y, eyePosition.Z);
            SetParameter1f(cgParamEtaRatio, etaRatio);
            SetMatrixParameterfr(cgParamModelToWorld, modelMatrix);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            
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
                etaRatio += 0.05f * (float)e.Time;
                if (etaRatio > 2.0f) etaRatio = 2.0f;
            }
            if (IsKeyDown(Keys.Minus) || IsKeyDown(Keys.KeyPadSubtract)) 
            {
                etaRatio -= 0.05f * (float)e.Time;
                if (etaRatio < 0.0f) etaRatio = 0.0f;
            }
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
            GL.DeleteTexture(decalTexture);
            GL.DeleteTexture(environmentTexture);
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }
    }
}
