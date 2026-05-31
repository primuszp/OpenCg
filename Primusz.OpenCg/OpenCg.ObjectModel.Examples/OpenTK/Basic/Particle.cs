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
    [ExampleAttribute("OpenTK/Basic/[15] Particle System")]
    class Particle : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C6E2v_particle.cg";
        private const string CgVertexEntryFuncName = "C6E2v_particle";
        private const string FragmentProgramFileName = "Data\\Shaders\\C6E2v_particle.cg";
        private const string CgFragmentEntryFuncName = "texcoord2color";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamGlobalTime;
        private CgParameterObject cgParamAcceleration;
        private CgParameterObject cgParamModelViewProj;

        private const int NumParticles = 800;

        private float globalTime = 0.0f;
        private bool animating = true;

        // Ortho(-1, 1, -1, 1, 0, 4) in row-major
        private readonly float[] projectionMatrix = {
            1, 0,     0,    0,
            0, 1,     0,    0,
            0, 0,    -0.5f,-1,
            0, 0,     0,    1
        };

        struct ParticleData
        {
            public float[] pInitial;
            public float[] vInitial;
            public float tInitial;
            public bool alive;
        }

        private ParticleData[] particles = new ParticleData[NumParticles];
        private Random rand = new Random(42);

        public Particle()
            : base("Cg Tutorial 15: Particle System", 600, 600)
        { }

        private float RandomRange(float min, float max)
        {
            return min + (max - min) * (float)rand.NextDouble();
        }

        private void ResetParticles()
        {
            globalTime = 0.0f;
            for (int i = 0; i < NumParticles; i++)
            {
                particles[i].pInitial = new float[3];
                particles[i].vInitial = new float[3];
                float radius = 0.25f;
                float initialElevation = -0.5f;

                particles[i].pInitial[0] = radius * (float)Math.Cos(i * 0.5f);
                particles[i].pInitial[1] = initialElevation;
                particles[i].pInitial[2] = radius * (float)Math.Sin(i * 0.5f);
                particles[i].alive = false;
                particles[i].tInitial = RandomRange(0, 10);
            }
        }

        private void AdvanceParticles(double dt)
        {
            float deathTime = globalTime - 1.0f;
            for (int i = 0; i < NumParticles; i++)
            {
                if (!particles[i].alive && particles[i].tInitial <= globalTime)
                {
                    particles[i].vInitial[0] = RandomRange(-1, 1);
                    particles[i].vInitial[1] = RandomRange(0, 6);
                    particles[i].vInitial[2] = RandomRange(-0.5f, 0.5f);
                    particles[i].tInitial = globalTime;
                    particles[i].alive = true;
                }

                if (particles[i].alive && particles[i].tInitial <= deathTime)
                {
                    particles[i].alive = false;
                    particles[i].tInitial = globalTime + 0.01f;
                }
            }
        }

        protected override void OnLoad()
        {
            ResetParticles();

            GL.ClearColor(0.2f, 0.6f, 1.0f, 1.0f);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.PointSize(6.0f);

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

            cgParamGlobalTime = cgVertexProgram.GetNamedParameter("globalTime");
            cgParamAcceleration = cgVertexProgram.GetNamedParameter("acceleration");
            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            float acceleration = -9.8f;
            float viewAngle = globalTime * 2.8f;

            GL.Clear(ClearBufferMask.ColorBufferBit);

            float eyeX = (float)Math.Cos(viewAngle), eyeY = 0.3f, eyeZ = (float)Math.Sin(viewAngle);
            float[] viewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            BuildLookAtMatrix(eyeX, eyeY, eyeZ, 0, 0, 0, 0, 1, 0, viewMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            SetParameter1f(cgParamGlobalTime, globalTime);
            SetParameter4f(cgParamAcceleration, 0, acceleration, 0, 0);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < NumParticles; i++)
            {
                if (particles[i].alive)
                {
                    GL.TexCoord3(particles[i].vInitial[0], particles[i].vInitial[1], particles[i].vInitial[2]);
                    GL.MultiTexCoord1(TextureUnit.Texture1, particles[i].tInitial);
                    GL.Vertex3(particles[i].pInitial[0], particles[i].pInitial[1], particles[i].pInitial[2]);
                }
            }
            GL.End();

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                globalTime += 0.01f;
                AdvanceParticles(e.Time);
            }

            if (IsKeyDown(Keys.Space) && !WasKeyDown(Keys.Space)) animating = !animating;
            if (IsKeyDown(Keys.R)) ResetParticles();
            if (IsKeyDown(Keys.Escape)) Close();

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
    }
}