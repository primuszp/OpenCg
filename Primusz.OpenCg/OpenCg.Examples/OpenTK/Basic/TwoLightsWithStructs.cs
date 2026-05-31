using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenCg.Examples.OpenTK.Basic
{
    #region Original Credits / License

    // OpenGL-based lighting example with structs using Cg program
    // from Chapter 5 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).
    // Requires the OpenGL Utility Toolkit (GLUT) and Cg runtime (version 1.5 or higher).

    #endregion

    #region Porting Credits

    // Ported from C to C# by [Your Name] for the OpenCg v1.0.1 Copyright (c) 2023.

    #endregion Porting Credits

    class TwoLightsWithStructs : BaseExample
    {
        #region Members

        private string vertexProgramFileName = "Data\\Shaders\\C5E4v_twoLights.cg";
        private string cgVertexEntryFuncName = "C5E4v_twoLights";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram, cgFragmentProgram;

        private CgParameter cgParamModelViewProj, cgParamGlobalAmbient, cgParamEyePosition,
            cgParamLights0Position, cgParamLights0Color, cgParamLights1Position, cgParamLights1Color,
            cgParamMaterialKe, cgParamMaterialKa, cgParamMaterialKd, cgParamMaterialKs, cgParamMaterialShininess;

        private readonly float[] globalAmbient = { 0.1f, 0.1f, 0.1f }; /* Dim */
        private readonly float[] light0Color = { 0.95f, 0.95f, 0.95f }; /* White */
        private readonly float[] light1Color = { 0.95f, 0.0f, 0.0f }; /* Red */
        private readonly float[] projectionMatrix = new float[16];
        private const double LightAngularSpeed = 0.48f; // Matches the old 0.008/frame speed at ~60 FPS.

        private double lightAngle = -0.4f; // Angle light rotates around scene.
        private bool animating = true;

        #endregion

        #region Constructors

        public TwoLightsWithStructs()
            : base("Cg Tutorial 11: Two Lights with Structs", 400, 400)
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
            cgParamEyePosition = Cg.GetNamedParameter(cgVertexProgram, "eyePosition");
            cgParamLights0Position = Cg.GetNamedParameter(cgVertexProgram, "lights[0].position");
            cgParamLights0Color = Cg.GetNamedParameter(cgVertexProgram, "lights[0].color");
            cgParamLights1Position = Cg.GetNamedParameter(cgVertexProgram, "lights[1].position");
            cgParamLights1Color = Cg.GetNamedParameter(cgVertexProgram, "lights[1].color");
            cgParamMaterialKe = Cg.GetNamedParameter(cgVertexProgram, "material.Ke");
            cgParamMaterialKa = Cg.GetNamedParameter(cgVertexProgram, "material.Ka");
            cgParamMaterialKd = Cg.GetNamedParameter(cgVertexProgram, "material.Kd");
            cgParamMaterialKs = Cg.GetNamedParameter(cgVertexProgram, "material.Ks");
            cgParamMaterialShininess = Cg.GetNamedParameter(cgVertexProgram, "material.shininess");

            /* Set light source color parameters once. */

            Cg.SetParameter3fv(cgParamGlobalAmbient, globalAmbient);
            Cg.SetParameter3fv(cgParamLights0Color, light0Color);
            Cg.SetParameter3fv(cgParamLights1Color, light1Color);

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
             "float4 main(float4 c : COLOR) : COLOR { return c; }",
             cgFragmentProfile,         // Profile: OpenGL ARB fragment program */
             "main",                    // Entry function name */
             fArgs);                    // Extra compiler options */

            CgGL.LoadProgram(cgFragmentProgram);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
                /* Add a small angle (in radians). */

                lightAngle += LightAngularSpeed * e.Time;

                if (lightAngle > 2 * Pi)
                {
                    lightAngle -= 2 * Pi;
                }
            }

            if (IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Space)
            {
                animating = !animating;
            }

            base.OnKeyDown(e);
        }

        #region Private Methods

        private void Display()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            /* World-space positions for light and eye. */
            float[] eyePosition = { 0, 0, 13, 1 };
            float[] light0Position = {
                                        5 * (float)Math.Sin(lightAngle),
                                        1.5f,
                                        5 * (float)Math.Cos(lightAngle), 1
                                    };
            float[] light1Position = {
                                        5 * (float)Math.Sin(lightAngle + 1.57f),
                                        1.5f,
                                        5 * (float)Math.Cos(lightAngle + 1.57f), 1
                                    };

            float[] translateMatrix = new float[16], rotateMatrix = new float[16], modelMatrix = new float[16], invModelMatrix = new float[16],
                    viewMatrix = new float[16], modelViewMatrix = new float[16], modelViewProjMatrix = new float[16];

            float[] objSpaceEyePosition = new float[4], objSpaceLight0Position = new float[4], objSpaceLight1Position = new float[4];

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
            Transform(objSpaceLight0Position, invModelMatrix, light0Position);
            Cg.SetParameter3fv(cgParamLights0Position, objSpaceLight0Position);
            Transform(objSpaceLight1Position, invModelMatrix, light1Position);
            Cg.SetParameter3fv(cgParamLights1Position, objSpaceLight1Position);

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
            Transform(objSpaceLight0Position, invModelMatrix, light0Position);
            Cg.SetParameter3fv(cgParamLights0Position, objSpaceLight0Position);
            Transform(objSpaceLight1Position, invModelMatrix, light1Position);
            Cg.SetParameter3fv(cgParamLights1Position, objSpaceLight1Position);

            /* modelViewMatrix = viewMatrix * modelMatrix */
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);

            /* modelViewProj = projectionMatrix * modelViewMatrix */
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            /* Set matrix parameter with row-major matrix. */
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgVertexProgram);
            NativeMethods.glutSolidCone(1.5, 3.5, 30, 30);

            /*** Render lights as emissive colored balls ***/

            /* First light */
            MakeTranslateMatrix(light0Position[0], light0Position[1], light0Position[2], modelMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetEmissiveLight0ColorOnly();
            Cg.SetParameter3f(cgParamLights0Position, 0f, 0f, 0f);
            Cg.SetParameter3f(cgParamLights1Position, 0f, 0f, 0f);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgVertexProgram);
            NativeMethods.glutSolidSphere(0.2, 12, 12);

            /* Second light */
            MakeTranslateMatrix(light1Position[0], light1Position[1], light1Position[2], modelMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetEmissiveLight1ColorOnly();
            Cg.SetParameter3f(cgParamLights0Position, 0f, 0f, 0f);
            Cg.SetParameter3f(cgParamLights1Position, 0f, 0f, 0f);

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

            Cg.SetParameter3fv(cgParamMaterialKe, brassEmissive);
            Cg.SetParameter3fv(cgParamMaterialKa, brassAmbient);
            Cg.SetParameter3fv(cgParamMaterialKd, brassDiffuse);
            Cg.SetParameter3fv(cgParamMaterialKs, brassSpecular);
            Cg.SetParameter1f(cgParamMaterialShininess, brassShininess);
        }

        private void SetEmissiveLight0ColorOnly()
        {
            float[] zero = { 0.0f, 0.0f, 0.0f };

            Cg.SetParameter3fv(cgParamMaterialKe, light0Color);
            Cg.SetParameter3fv(cgParamMaterialKa, zero);
            Cg.SetParameter3fv(cgParamMaterialKd, zero);
            Cg.SetParameter3fv(cgParamMaterialKs, zero);
            Cg.SetParameter1f(cgParamMaterialShininess, 0);
        }

        private void SetEmissiveLight1ColorOnly()
        {
            float[] zero = { 0.0f, 0.0f, 0.0f };

            Cg.SetParameter3fv(cgParamMaterialKe, light1Color);
            Cg.SetParameter3fv(cgParamMaterialKa, zero);
            Cg.SetParameter3fv(cgParamMaterialKd, zero);
            Cg.SetParameter3fv(cgParamMaterialKs, zero);
            Cg.SetParameter1f(cgParamMaterialShininess, 0);
        }

        private void SetRedPlasticMaterial()
        {
            float[] redPlasticEmissive = { 0.0f, 0.0f, 0.0f },
                    redPlasticAmbient = { 0.0f, 0.0f, 0.0f },
                    redPlasticDiffuse = { 0.5f, 0.0f, 0.0f },
                    redPlasticSpecular = { 0.7f, 0.6f, 0.6f };

            float redPlasticShininess = 32.0f;

            Cg.SetParameter3fv(cgParamMaterialKe, redPlasticEmissive);
            Cg.SetParameter3fv(cgParamMaterialKa, redPlasticAmbient);
            Cg.SetParameter3fv(cgParamMaterialKd, redPlasticDiffuse);
            Cg.SetParameter3fv(cgParamMaterialKs, redPlasticSpecular);
            Cg.SetParameter1f(cgParamMaterialShininess, redPlasticShininess);
        }

        #endregion Private Methods
    }
}
