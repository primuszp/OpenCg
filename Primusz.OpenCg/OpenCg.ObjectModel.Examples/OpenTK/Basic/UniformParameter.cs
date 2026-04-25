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

// 03_uniform_parameter.c - OpenGL-based example using a Cg
// vertex and a Cg fragment programs from Chapter 3 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).

#endregion

#region Porting Credits

// Ported from C to C# by Tobias Bohnen for the CgNet v1.0 Copyright (c) 2010.
// Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2011.

#endregion Porting Credits

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[03] Uniform Parameter")]
    class UniformParameter : BaseExample
    {
        #region Members

        private const string VertexProgramFileName = "Data\\Shaders\\C3E1v_anycolor.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C2E2f_passthru.cg";
        private const string CgVertexEntryFuncName = "C3E1v_anycolor";
        private const string CgFragmentEntryFuncName = "C2E2f_passthru";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject vertexParamConstantColor;

        #endregion

        public UniformParameter()
            : base("Cg Tutorial 03: Uniform Parameter", 400, 400)
        { }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
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

            vertexParamConstantColor = cgVertexProgram.GetNamedParameter("constantColor");

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
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            DrawStars();

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        private void DrawStars()
        {
            float[] green = { 0.2f, 0.8f, 0.3f };
            SetParameter3fv(vertexParamConstantColor, green);

            DrawStar(-0.1f, 0, 5, 0.5f, 0.2f);
            DrawStar(-0.84f, 0.1f, 5, 0.3f, 0.12f);
            DrawStar(0.92f, -0.5f, 5, 0.25f, 0.11f);

            float[] red = { 0.7f, 0.1f, 0.1f  };
            SetParameter3fv(vertexParamConstantColor, red);

            DrawStar(0.3f, 0.97f, 5, 0.3f, 0.1f);
            DrawStar(0.94f, 0.3f, 5, 0.5f, 0.2f);
            DrawStar(-0.97f, -0.8f, 5, 0.6f, 0.2f);
        }

        private void DrawStar(float x, float y, int starPoints, float R, float r)
        {
            double piOverStarPoints = 3.14159 / starPoints, angle = 0.0;

            UpdateProgramParameters(cgVertexProgram);

            GL.Begin(BeginMode.TriangleFan);

            /* Center of star */

            GL.Vertex2(x, y);

            /* Emit exterior vertices for star's points. */

            for (int i = 0; i < starPoints; i++)
            {
                GL.Vertex2(x + R * Math.Cos(angle), y + R * Math.Sin(angle));
                angle += piOverStarPoints;
                GL.Vertex2(x + r * Math.Cos(angle), y + r * Math.Sin(angle));
                angle += piOverStarPoints;
            }

            /* End by repeating first exterior vertex of star. */

            angle = 0;

            GL.Vertex2(x + R * Math.Cos(angle), y + R * Math.Sin(angle));
            GL.End();
        }
    }
}
