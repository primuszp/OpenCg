using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

#region Original Credits / License

// 08_vertex_transform.c - OpenGL-based vertex transform example
// using Cg program from Chapter 4 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).

#endregion

#region Porting Credits

// Ported from C to C# by Tobias Bohnen for the CgNet v1.0 Copyright (c) 2010.
// Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2011.

#endregion Porting Credits

namespace OpenCg.Examples.OpenTK.Basic
{
    class VertexTransform : BaseExample
    {
        #region Members

        private const string vertexProgramFileName = "..\\..\\Data\\Shaders\\C4E1v_transform.cg";
        private const string cgVertexEntryFuncName = "C4E1v_transform";

        private CgProfile cgVertexProfile, cgFragmentProfile;
        private CgProgram cgVertexProgram, cgFragmentProgram;
        
        private CgParameter vertexParamModelViewProj, fragmentParamColor;

        private static readonly float[] ProjectionMatrix = new float[16];

        // Angle eye rotates around scene.
        private static float eyeAngle;

        #endregion

        public VertexTransform()
            : base("Cg Tutorial 08: Vertex Transform", 400, 400)
        { }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            /* Add a small angle (in radians). */

            eyeAngle += 0.008f;
            
            if (eyeAngle > 2 * Pi)
            {
                eyeAngle -= (2 * Pi);
            }

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
               vertexProgramFileName,    // Name of file containing program
               cgVertexProfile,          // Profile: OpenGL ARB vertex program
               cgVertexEntryFuncName,    // Entry function name
               vArgs);                   // Extra compiler options

            CgGL.LoadProgram(cgVertexProgram);

            vertexParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgFragmentProfile);

            if (cgFragmentProfile != CgProfile.Unknown)
            {
                if (CgGL.IsProfileSupported(cgFragmentProfile))
                {
                    CgGL.SetOptimalOptions(cgFragmentProfile);
                }
            }

            cgFragmentProgram = Cg.CreateProgram(
                context,                   // Cg runtime context */
                CgEnum.Source,             // Program in human-readable form */
                "float4 main(uniform float4 c) : COLOR { return c; }",
                cgFragmentProfile,         // Profile: OpenGL ARB vertex program */
                "main",                    // Entry function name */
                fArgs);                    // Extra compiler options */

            CgGL.LoadProgram(cgFragmentProgram);

            fragmentParamColor = Cg.GetNamedParameter(cgFragmentProgram, "c");
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
            Environment.Exit(0);
        }

        private void Reshape()
        {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

            double aspectRatio = (float)Width / Height;
            const double fieldOfView = 40.0;

            /* Build projection matrix once. */

            BuildPerspectiveMatrix(fieldOfView, aspectRatio,
                                   1.0, 20.0, /* Znear and Zfar */
                                   ProjectionMatrix);
        }

        private void Display()
        {
            float[] viewMatrix = new float[16];
            BuildLookAtMatrix(13 * Math.Sin(eyeAngle), 0, 13 * Math.Cos(eyeAngle), /* eye position */
                              0, 0, 0, /* view center */
                              0, 1, 0, /* up vector */
                              viewMatrix);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            /* modelView = rotateMatrix * translateMatrix */

            float[] rotateMatrix = new float[16];
            float[] translateMatrix = new float[16];
            float[] modelMatrix = new float[16];
            float[] modelViewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];

            MakeRotateMatrix(70, 1, 1, 1, rotateMatrix);
            MakeTranslateMatrix(2, 0, 0, translateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, ProjectionMatrix, modelViewMatrix);

            /* Set matrix parameter with row-major matrix. */
            Cg.SetMatrixParameterfr(vertexParamModelViewProj, modelViewProjMatrix);
            
            /* Green */
            Cg.SetParameter4f(fragmentParamColor, 0.1f, 0.7f, 0.1f, 1f);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

            NativeMethods.glutWireSphere(2.0, 30, 30);

            /*** Render red wireframe cone ***/

            MakeTranslateMatrix(-2, -1.5f, 0, translateMatrix);
            MakeRotateMatrix(90, 1, 0, 0, rotateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, ProjectionMatrix, modelViewMatrix);

            /* Set matrix parameter with row-major matrix. */
            Cg.SetMatrixParameterfr(vertexParamModelViewProj, modelViewProjMatrix);

            /* Red */
            Cg.SetParameter4f(fragmentParamColor, 0.8f, 0.1f, 0.1f, 1f);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

            NativeMethods.glutWireCone(1.5, 3.5, 20, 20);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }
    }
}
