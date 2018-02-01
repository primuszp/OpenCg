using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenCg.Examples.OpenTK.Basic
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

    class VertexLighting : BaseExample
    {
        #region Members

        private string vertexProgramFileName = "..\\..\\Data\\Shaders\\C5E1v_basicLight.cg";
        private string cgVertexEntryFuncName = "C5E1v_basicLight";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram, cgFragmentProgram;

        private CgParameter cgParamModelViewProj, cgParamGlobalAmbient, cgParamLightColor,
            cgParamLightPosition, cgParamEyePosition, cgParamKe, cgParamKa, cgParamKd, cgParamKs, cgParamShininess;

        private readonly float[] globalAmbient = { 0.1f, 0.1f, 0.1f }; /* Dim */
        private readonly float[] lightColor = { 0.95f, 0.95f, 0.95f }; /* White */
        private readonly float[] projectionMatrix = new float[16];

        private double lightAngle = -0.4f; // Angle light rotates around scene.

        #endregion

        #region Constructors

        public VertexLighting()
            : base("Cg Tutorial 09: Vertex Lighting", 400, 400)
        { }

        #endregion

        protected override void OnResize(EventArgs e)
        {
            Reshape(Width, Height);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f); // Blue background
            GL.Enable(EnableCap.DepthTest);

            context = Cg.CreateContext();

            Cg.SetErrorCallback(errorDelegate);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

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

            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");
            cgParamGlobalAmbient = Cg.GetNamedParameter(cgVertexProgram, "globalAmbient");
            cgParamLightColor = Cg.GetNamedParameter(cgVertexProgram, "lightColor");
            cgParamLightPosition = Cg.GetNamedParameter(cgVertexProgram, "lightPosition");
            cgParamEyePosition = Cg.GetNamedParameter(cgVertexProgram, "eyePosition");
            cgParamKe = Cg.GetNamedParameter(cgVertexProgram, "Ke");
            cgParamKa = Cg.GetNamedParameter(cgVertexProgram, "Ka");
            cgParamKd = Cg.GetNamedParameter(cgVertexProgram, "Kd");
            cgParamKs = Cg.GetNamedParameter(cgVertexProgram, "Ks");
            cgParamShininess = Cg.GetNamedParameter(cgVertexProgram, "shininess");

            /* Set light source color parameters once. */

            Cg.SetParameter3fv(cgParamGlobalAmbient, globalAmbient);
            Cg.SetParameter3fv(cgParamLightColor, lightColor);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);

            string[] fArgs = CgGL.GetOptimalOptions(cgVertexProfile);

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
             "float4 main(float4 c : COLOR) : COLOR { return c; }",
             cgFragmentProfile,         // Profile: OpenGL ARB vertex program */
             "main",                    // Entry function name */
             fArgs);                    // Extra compiler options */

            CgGL.LoadProgram(cgFragmentProgram);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
            Environment.Exit(0);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            /* Add a small angle (in radians). */

            lightAngle += 0.008f;

            if (lightAngle > 2 * Pi)
            {
                lightAngle -= 2 * Pi;
            }

            if (Keyboard[Key.Escape])
            {
                Exit();
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


            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            CgGL.BindProgram(cgFragmentProgram);
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
            Cg.SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            Cg.SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            /* Set matrix parameter with row-major matrix. */
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgVertexProgram);
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
            Cg.SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            Cg.SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            /* Set matrix parameter with row-major matrix. */
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgVertexProgram);
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
            Cg.SetParameter3f(cgParamLightPosition, 0f, 0f, 0f);

            /* Set matrix parameter with row-major matrix. */
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgVertexProgram);
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

            Cg.SetParameter3fv(cgParamKe, brassEmissive);
            Cg.SetParameter3fv(cgParamKa, brassAmbient);
            Cg.SetParameter3fv(cgParamKd, brassDiffuse);
            Cg.SetParameter3fv(cgParamKs, brassSpecular);
            Cg.SetParameter1f(cgParamShininess, brassShininess);
        }

        private void SetEmissiveLightColorOnly()
        {
            float[] zero = { 0.0f, 0.0f, 0.0f };

            Cg.SetParameter3fv(cgParamKe, lightColor);
            Cg.SetParameter3fv(cgParamKa, zero);
            Cg.SetParameter3fv(cgParamKd, zero);
            Cg.SetParameter3fv(cgParamKs, zero);
            Cg.SetParameter1f(cgParamShininess, 0);
        }

        private void SetRedPlasticMaterial()
        {
            float[] redPlasticEmissive = { 0.0f, 0.0f, 0.0f },
                    redPlasticAmbient = { 0.0f, 0.0f, 0.0f },
                    redPlasticDiffuse = { 0.5f, 0.0f, 0.0f },
                    redPlasticSpecular = { 0.7f, 0.6f, 0.6f };

            float redPlasticShininess = 32.0f;

            Cg.SetParameter3fv(cgParamKe, redPlasticEmissive);
            Cg.SetParameter3fv(cgParamKa, redPlasticAmbient);
            Cg.SetParameter3fv(cgParamKd, redPlasticDiffuse);
            Cg.SetParameter3fv(cgParamKs, redPlasticSpecular);
            Cg.SetParameter1f(cgParamShininess, redPlasticShininess);
        }

        #endregion Private Methods
    }
}