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

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    #region Original Credits / License

    // OpenGL-based per-vertex lighting example
    // using Cg program from Chapter 5 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).
    // Requires the OpenGL Utility Toolkit (GLUT) and Cg runtime (version 1.5 or higher).

    #endregion

    #region Porting Credits

    // Ported from C to C# by Tobias Bohnen for the CgNet v1.0 Copyright (c) 2010.
    // Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2015.

    #endregion Porting Credits
    [ExampleAttribute("OpenTK/Basic/[10] Fragment Lighting")]

    class FragmentLighting : BaseExample
    {
        #region Members

        private string vertexProgramFileName = "Data\\Shaders\\C5E2v_fragmentLighting.cg";
        private string cgVertexEntryFuncName = "C5E2v_fragmentLighting";

        private string fragmentProgramFileName = "Data\\Shaders\\C5E3f_basicLight.cg";
        private string cgFragmentEntryFuncName = "C5E3f_basicLight";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram, cgFragmentProgram;

        private CgParameterObject cgParamModelViewProj, cgParamGlobalAmbient, cgParamLightColor,
            cgParamLightPosition, cgParamEyePosition, cgParamKe, cgParamKa, cgParamKd, cgParamKs, cgParamShininess;

        private readonly float[] globalAmbient = { 0.1f, 0.1f, 0.1f }; /* Dim */
        private readonly float[] lightColor = { 0.95f, 0.95f, 0.95f }; /* White */
        private readonly float[] projectionMatrix = new float[16];
        private const double LightAngularSpeed = 0.48f; // Matches the old 0.008/frame speed at ~60 FPS.

        private double lightAngle = -0.4f; // Angle light rotates around scene.

        #endregion

        #region Constructors

        public FragmentLighting()
            : base("Cg Tutorial 09: Fragment Lighting", 400, 400)
        { }

        #endregion

        protected override void OnResize(ResizeEventArgs e)
        {
            Reshape(e.Width, e.Height);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f); // Blue background
            GL.Enable(EnableCap.DepthTest);

            context = OpenCg.Graphics.ObjectModel.Context.Create();

            Cg.SetErrorCallback(errorDelegate);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

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
              vertexProgramFileName,    // Name of file containing program
              cgVertexProfile,          // Profile: OpenGL ARB vertex program
              cgVertexEntryFuncName,    // Entry function name
              vArgs);                   // Extra compiler options

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);

            string[] fArgs = CgGL.GetOptimalOptions(cgVertexProfile);

            if (cgFragmentProfile != CgProfile.Unknown)
            {
                if (CgGL.IsProfileSupported(cgFragmentProfile))
                {
                    CgGL.SetOptimalOptions(cgFragmentProfile);
                }
            }

            cgFragmentProgram = context.CreateProgramFromFile(                   // Cg runtime context */
             CgEnum.Source,             // Program in human-readable form */
             fragmentProgramFileName,   // Name of file containing program
             cgFragmentProfile,         // Profile: OpenGL ARB vertex program */
             cgFragmentEntryFuncName,   // Entry function name */
             fArgs);                    // Extra compiler options */

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamGlobalAmbient = cgFragmentProgram.GetNamedParameter("globalAmbient");
            cgParamLightColor = cgFragmentProgram.GetNamedParameter("lightColor");
            cgParamLightPosition = cgFragmentProgram.GetNamedParameter("lightPosition");
            cgParamEyePosition = cgFragmentProgram.GetNamedParameter("eyePosition");
            cgParamKe = cgFragmentProgram.GetNamedParameter("Ke");
            cgParamKa = cgFragmentProgram.GetNamedParameter("Ka");
            cgParamKd = cgFragmentProgram.GetNamedParameter("Kd");
            cgParamKs = cgFragmentProgram.GetNamedParameter("Ks");
            cgParamShininess = cgFragmentProgram.GetNamedParameter("shininess");

            /* Set light source color parameters once. */

            SetParameter3fv(cgParamGlobalAmbient, globalAmbient);
            SetParameter3fv(cgParamLightColor, lightColor);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            /* Add a small angle (in radians). */

            lightAngle += LightAngularSpeed * e.Time;

            if (lightAngle > 2 * Pi)
            {
                lightAngle -= 2 * Pi;
            }

            if (IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        #region Private Methods

        private void Display()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            /* World-space positions for light and eye. */
            float[] eyePosition = { 0, 0, 13, 1 };
            float[] lightPosition = {
                                        5 * (float)Math.Sin(lightAngle),
                                        1.5f,
                                        5 * (float)Math.Cos(lightAngle), 1
                                    };

            float[] translateMatrix = new float[16], rotateMatrix = new float[16], modelMatrix = new float[16], invModelMatrix = new float[16],
                    viewMatrix = new float[16], modelViewMatrix = new float[16], modelViewProjMatrix = new float[16];

            float[] objSpaceEyePosition = new float[4], objSpaceLightPosition = new float[4];

            BuildLookAtMatrix(eyePosition[0], eyePosition[1], eyePosition[2],
                              0, 0, 0,
                              0, 1, 0,
                              viewMatrix);


            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            SetBrassMaterial();

            /* modelView = rotateMatrix * translateMatrix */
            MakeRotateMatrix(70, 1, 1, 1, rotateMatrix);
            MakeTranslateMatrix(2, 0, 0, translateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);

            /* invModelMatrix = inverse(modelMatrix) */
            InvertMatrix(invModelMatrix, modelMatrix);

            /* Transform world-space eye and light positions to sphere's object-space. */
            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            /* Set matrix parameter with row-major matrix. */
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidSphere(2.0, 40, 40);

            /*** Render red plastic solid cone ***/

            SetRedPlasticMaterial();

            /* modelView = viewMatrix * translateMatrix */
            MakeTranslateMatrix(-2, -1.5f, 0, translateMatrix);
            MakeRotateMatrix(90, 1, 0, 0, rotateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);

            /* invModelMatrix = inverse(modelMatrix) */
            InvertMatrix(invModelMatrix, modelMatrix);

            /* Transform world-space eye and light positions to sphere's object-space. */
            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            /* Set matrix parameter with row-major matrix. */
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidCone(1.5, 3.5, 30, 30);

            /*** Render light as emissive white ball ***/

            /* modelView = translateMatrix */
            MakeTranslateMatrix(lightPosition[0], lightPosition[1], lightPosition[2], modelMatrix);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetEmissiveLightColorOnly();
            /* Avoid degenerate lightPosition. */
            SetParameter3f(cgParamLightPosition, 0f, 0f, 0f);

            /* Set matrix parameter with row-major matrix. */
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidSphere(0.2, 12, 12);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        private void Reshape(int width, int height)
        {
            double aspectRatio = (float)width / height;
            const double fieldOfView = 40.0;

            /* Build projection matrix once. */
            BuildPerspectiveMatrix(fieldOfView, aspectRatio,
                1.0, 20.0, /* Znear and Zfar */
                projectionMatrix);
        }

        private void SetBrassMaterial()
        {
            float[] brassEmissive = { 0.0f, 0.0f, 0.0f },
                    brassAmbient = { 0.33f, 0.22f, 0.03f },
                    brassDiffuse = { 0.78f, 0.57f, 0.11f },
                    brassSpecular = { 0.99f, 0.91f, 0.81f };

            float brassShininess = 27.8f;

            SetParameter3fv(cgParamKe, brassEmissive);
            SetParameter3fv(cgParamKa, brassAmbient);
            SetParameter3fv(cgParamKd, brassDiffuse);
            SetParameter3fv(cgParamKs, brassSpecular);
            SetParameter1f(cgParamShininess, brassShininess);
        }

        private void SetEmissiveLightColorOnly()
        {
            float[] zero = { 0.0f, 0.0f, 0.0f };

            SetParameter3fv(cgParamKe, lightColor);
            SetParameter3fv(cgParamKa, zero);
            SetParameter3fv(cgParamKd, zero);
            SetParameter3fv(cgParamKs, zero);
            SetParameter1f(cgParamShininess, 0);
        }

        private void SetRedPlasticMaterial()
        {
            float[] redPlasticEmissive = { 0.0f, 0.0f, 0.0f },
                    redPlasticAmbient = { 0.0f, 0.0f, 0.0f },
                    redPlasticDiffuse = { 0.5f, 0.0f, 0.0f },
                    redPlasticSpecular = { 0.7f, 0.6f, 0.6f };

            float redPlasticShininess = 32.0f;

            SetParameter3fv(cgParamKe, redPlasticEmissive);
            SetParameter3fv(cgParamKa, redPlasticAmbient);
            SetParameter3fv(cgParamKd, redPlasticDiffuse);
            SetParameter3fv(cgParamKs, redPlasticSpecular);
            SetParameter1f(cgParamShininess, redPlasticShininess);
        }

        #endregion Private Methods
    }
}
