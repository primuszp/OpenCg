using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenCg.Graphics.ObjectModel;
using OpenCg.Graphics.ObjectModel.OpenGL;
using CgProgramObject = OpenCg.Graphics.ObjectModel.Program;
using CgParameterObject = OpenCg.Graphics.ObjectModel.Parameter;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

#region Original Credits / License

// 06_vertex_twisting.c - OpenGL-based example using a Cg
// vertex and a Cg fragment programs from Chapter 3 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).

#endregion Original Credits / License

#region Porting Credits

// Ported from C to C# by Marek Wyborski for the Tao Framework (02/05/07).
// Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2011.

#endregion Porting Credits

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[06] Vertex Twisting")]
    class VertexTwisting : BaseExample
    {
        #region Members

        private const string VertexProgramFileName = "Data\\Shaders\\C3E4v_twist.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C2E2f_passthru.cg";
        private const string CgVertexEntryFuncName = "C3E4v_twist";
        private const string CgFragmentEntryFuncName = "C2E2f_passthru";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject vertexParamTwisting;
        private const float TwistingSpeedScale = 60.0f; // Preserves the old per-frame behavior at ~60 FPS.

        private float twisting = 2.9f,       /* Twisting angle in radians. */
                      twistDirection = 0.1f; /* Animation delta for twist. */

        private bool wireframe;

        #endregion

        public VertexTwisting()
            : base("Cg Tutorial 06: Vertex Twisting", 400, 400)
        { }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Twist((float)e.Time);

            if (IsKeyDown(Keys.W))
            {
                wireframe = !wireframe;

                GL.PolygonMode(MaterialFace.FrontAndBack, wireframe ? PolygonMode.Line : PolygonMode.Fill);
            }

            if (IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            Reshape();
            Display();
        }

        protected override void OnLoad()
        {
            context = OpenCg.Graphics.ObjectModel.Context.Create();

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

            cgVertexProgram = context.CreateProgramFromFile(                  // Cg runtime context
               CgEnum.Source,            // Program in human-readable form
               VertexProgramFileName,    // Name of file containing program
               cgVertexProfile,          // Profile: OpenGL ARB vertex program
               CgVertexEntryFuncName,    // Entry function name
               vArgs);                   // Extra compiler options

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            vertexParamTwisting = cgVertexProgram.GetNamedParameter("twisting");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgFragmentProfile);

            if (cgFragmentProfile != CgProfile.Unknown)
            {
                if (CgGL.IsProfileSupported(cgFragmentProfile))
                {
                    CgGL.SetOptimalOptions(cgFragmentProfile);
                }
            }

            cgFragmentProgram = context.CreateProgramFromFile(                   // Cg runtime context */
               CgEnum.Source,             // Program in human-readable form */
               FragmentProgramFileName,   // Name of file containing program */
               cgFragmentProfile,         // Profile: OpenGL ARB vertex program */
               CgFragmentEntryFuncName,   // Entry function name */
               fArgs);                    // Extra compiler options */

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            DisposeProgram(cgFragmentProgram);
            DisposeProgram(cgVertexProgram);
            context?.Dispose();
        }

        private void Reshape()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            GL.Ortho(0, 0, ClientSize.X, ClientSize.Y, -1, +1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Display()
        {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetParameter1f(vertexParamTwisting, twisting);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            DrawSubDividedTriangle(5);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        private static void TriangleDivide(int depth, float[] a, float[] b, float[] c, float[] ca, float[] cb, float[] cc)
        {
            if (depth == 0)
            {
                GL.Color3(ca);
                GL.Vertex2(a);
                GL.Color3(cb);
                GL.Vertex2(b);
                GL.Color3(cc);
                GL.Vertex2(c);
            }
            else
            {
                float[] d = { (a[0] + b[0]) / 2, (a[1] + b[1]) / 2 },
                        e = { (b[0] + c[0]) / 2, (b[1] + c[1]) / 2 },
                        f = { (c[0] + a[0]) / 2, (c[1] + a[1]) / 2 };

                float[] cd = { (ca[0] + cb[0]) / 2, (ca[1] + cb[1]) / 2, (ca[2] + cb[2]) / 2 },
                        ce = { (cb[0] + cc[0]) / 2, (cb[1] + cc[1]) / 2, (cb[2] + cc[2]) / 2 },
                        cf = { (cc[0] + ca[0]) / 2, (cc[1] + ca[1]) / 2, (cc[2] + ca[2]) / 2 };

                depth -= 1;

                TriangleDivide(depth, a, d, f, ca, cd, cf);
                TriangleDivide(depth, d, b, e, cd, cb, ce);
                TriangleDivide(depth, f, e, c, cf, ce, cc);
                TriangleDivide(depth, d, e, f, cd, ce, cf);
            }
        }
        
        // Large vertex displacements such as are possible with C3E4v_twist
        // require a high degree of tessellation.  This routine draws a
        // triangle recursively subdivided to provide sufficient tessellation.
        private void DrawSubDividedTriangle(int subdivisions)
        {
            float[] a = { -0.8f, 0.8f },
                    b = { 0.8f, 0.8f },
                    c = { 0.0f, -0.8f },
                    ca = { 0, 0, 1 },
                    cb = { 0, 0, 1 },
                    cc = { 0.7f, 0.7f, 1 };

            GL.Begin(BeginMode.Triangles);
            {
                TriangleDivide(subdivisions, a, b, c, ca, cb, cc);
            }
            GL.End();
        }

        private void Twist(float deltaTime)
        {
            if (twisting > 3)
            {
                twistDirection = -0.05f;
            }
            else if (twisting < -3)
            {
                twistDirection = 0.05f;
            }
            twisting += twistDirection * TwistingSpeedScale * deltaTime;
        }
    }
}
