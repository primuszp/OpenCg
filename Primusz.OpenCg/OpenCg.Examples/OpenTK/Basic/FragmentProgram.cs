using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

#region Original Credits / License

// 02_vertex_and_fragment_program.c - OpenGL-based example using a Cg
// vertex and a Cg fragment programs from Chapter 2 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).

#endregion

#region Porting Credits

// Ported from C to C# by Tobias Bohnen for the CgNet v1.0 Copyright (c) 2010.
// Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2011.

#endregion Porting Credits

namespace OpenCg.Examples.OpenTK.Basic
{
    class FragmentProgram : BaseExample
    {
        #region Private fields

        private const string VertexProgramFileName = "..\\..\\Data\\Shaders\\C2E1v_green.cg";
        private const string FragmentProgramFileName = "..\\..\\Data\\Shaders\\C2E2f_passthru.cg";
        private const string CgVertexEntryFuncName = "C2E1v_green";
        private const string CgFragmentEntryFuncName = "C2E2f_passthru";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        #endregion

        public FragmentProgram()
            : base("Cg Tutorial 02: Fragment program", 400, 400)
        { }

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
            string[] vArgs = CgGL.GetOptimalOptions(cgVertexProfile);

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
               VertexProgramFileName,    // Name of file containing program
               cgVertexProfile,          // Profile: OpenGL ARB vertex program
               CgVertexEntryFuncName,    // Entry function name
               vArgs);                   // Extra compiler options

            CgGL.LoadProgram(cgVertexProgram);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgVertexProfile);

            if (cgFragmentProfile != CgProfile.Unknown)
            {
                if (CgGL.IsProfileSupported(cgFragmentProfile))
                {
                    CgGL.SetOptimalOptions(cgFragmentProfile);
                }
            }

            cgFragmentProgram = Cg.CreateProgramFromFile(
               context,                   // Cg runtime context */
               CgEnum.Source,             // Program in human-readable form */
               FragmentProgramFileName,   // Name of file containing program */
               cgFragmentProfile,         // Profile: OpenGL ARB vertex program */
               CgFragmentEntryFuncName,   // Entry function name */
               fArgs);                    // Extra compiler options */

            CgGL.LoadProgram(cgFragmentProgram);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyContext(context);
            Environment.Exit(0);
        }

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

            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            DrawStars();

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        private void DrawStars()
        {
            /*                     star    outer   inner  */
            /*        x      y     points  radius  radius */
            /*       =====  =====  ======  ======  ====== */
            DrawStar(-0.10f, +0.00f, 5, 0.50f, 0.20f);
            DrawStar(-0.84f, +0.10f, 5, 0.30f, 0.12f);
            DrawStar(+0.92f, -0.50f, 5, 0.25f, 0.11f);
            DrawStar(+0.30f, +0.97f, 5, 0.30f, 0.10f);
            DrawStar(+0.94f, +0.30f, 5, 0.50f, 0.20f);
            DrawStar(-0.97f, -0.80f, 5, 0.60f, 0.20f);
        }

        private void DrawStar(float x, float y, int starPoints, float R, float r)
        {
            double piOverStarPoints = Math.PI / starPoints, angle = 0.0;

            GL.Begin(PrimitiveType.TriangleFan);
            {
                /* Center of star */

                GL.Vertex2(x, y);

                /* Emit exterior vertices for star's points. */

                for (int i = 0; i < starPoints; i++)
                {
                    GL.Vertex2((float)(x + R * Math.Cos(angle)), (float)(y + R * Math.Sin(angle)));
                    angle += piOverStarPoints;
                    GL.Vertex2((float)(x + r * Math.Cos(angle)), (float)(y + r * Math.Sin(angle)));
                    angle += piOverStarPoints;
                }

                /* End by repeating first exterior vertex of star. */

                angle = 0;

                GL.Vertex2((float)(x + R * Math.Cos(angle)), (float)(y + R * Math.Sin(angle)));
            }
            GL.End();
        }
    }
}