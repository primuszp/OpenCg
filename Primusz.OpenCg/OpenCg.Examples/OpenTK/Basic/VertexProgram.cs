using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

#region Original Credits / License

// OpenGL-based very simple vertex program example
// using Cg program from Chapter 2 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).

#endregion

#region Porting Credits

// Ported from C to C# by Tobias Bohnen for the CgNet v1.0 Copyright (c) 2010.
// Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2011.

#endregion Porting Credits

namespace OpenCg.Examples.OpenTK.Basic
{
    class VertexProgram : BaseExample
    {
        #region Members

        private string vertexProgramFileName = "..\\..\\Data\\Shaders\\C2E1v_green.cg";
        private string cgVertexEntryFuncName = "C2E1v_green";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProgram cgVertexProgram;

        #endregion

        #region Constructors

        public VertexProgram()
            : base("Cg Tutorial 01: Vertex program", 400, 400)
        { }

        #endregion

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
            {
                Exit();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Reshape();
            Display();
        }

        protected override void OnLoad(EventArgs e)
        {
            context = Cg.CreateContext();

            Cg.SetErrorCallback(errorDelegate);
            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);

            string[] args = CgGL.GetOptimalOptions(cgVertexProfile);

            if (cgVertexProfile != CgProfile.Unknown)
            {
                if (CgGL.IsProfileSupported(cgVertexProfile))
                {
                    CgGL.SetOptimalOptions(cgVertexProfile);
                }
            }

            cgVertexProgram = Cg.CreateProgramFromFile(
               context,                  // Cg runtime context
               CgEnum.Source,            // Program in human-readable form
               vertexProgramFileName,    // Name of file containing program
               cgVertexProfile,          // Profile: OpenGL ARB vertex program
               cgVertexEntryFuncName,    // Entry function name
               args);                    // Extra compiler options

            CgGL.LoadProgram(cgVertexProgram);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyContext(context);
            Environment.Exit(0);
        }

        #region Methods

        private void Reshape()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Viewport(0, 0, Width, Height);
            GL.Ortho(0, 0, Width, Height, -1, +1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Display()
        {
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            GL.Begin(PrimitiveType.Triangles);
            {
                GL.Vertex2(-0.8f, +0.8f);
                GL.Vertex2(+0.8f, +0.8f);
                GL.Vertex2(+0.0f, -0.8f);
            }
            GL.End();

            CgGL.DisableProfile(cgVertexProfile);

            SwapBuffers();
        }

        #endregion
    }
}