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

    // OpenGL-based spotlight attenuation example
    // using Cg program from Chapter 5 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).
    // Requires the OpenGL Utility Toolkit (GLUT) and Cg runtime (version 1.5 or higher).

    #endregion

    class Spotlight : BaseExample
    {
        #region Members

        private const string VertexProgramFileName = "Data\\Shaders\\C5E2v_fragmentLighting.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C5E10_spotAttenLighting.cg";
        private const string CgVertexEntryFuncName = "C5E2v_fragmentLighting";
        private const string CgFragmentEntryFuncName = "oneLight";
        private const double LightAngularSpeed = 0.48f;

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamGlobalAmbient;
        private CgParameter cgParamEyePosition;
        private CgParameter cgParamLightPosition;
        private CgParameter cgParamLightColor;
        private CgParameter cgParamLightConstantAtten;
        private CgParameter cgParamLightLinearAtten;
        private CgParameter cgParamLightQuadraticAtten;
        private CgParameter cgParamLightDirection;
        private CgParameter cgParamLightCosInnerCone;
        private CgParameter cgParamLightCosOuterCone;
        private CgParameter cgParamMaterialKe;
        private CgParameter cgParamMaterialKa;
        private CgParameter cgParamMaterialKd;
        private CgParameter cgParamMaterialKs;
        private CgParameter cgParamMaterialShininess;

        private readonly float[] projectionMatrix = new float[16];
        private readonly float[] globalAmbient = { 0.4f, 0.4f, 0.4f };
        private readonly float[] lightColor = { 1.0f, 1.0f, 1.0f };

        private double lightAngle = -0.4f;
        private bool animating = true;

        #endregion

        public Spotlight()
            : base("Cg Tutorial 13: Spotlight", 400, 400)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);
            GL.Enable(EnableCap.DepthTest);

            context = Cg.CreateContext();

            Cg.SetErrorCallback(errorDelegate);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            string[] vArgs = CgGL.GetOptimalOptions(cgVertexProfile);
            if (cgVertexProfile != CgProfile.Unknown && CgGL.IsProfileSupported(cgVertexProfile))
            {
                CgGL.SetOptimalOptions(cgVertexProfile);
            }

            cgVertexProgram = Cg.CreateProgramFromFile(
                context,
                CgEnum.Source,
                VertexProgramFileName,
                cgVertexProfile,
                CgVertexEntryFuncName,
                vArgs);

            CgGL.LoadProgram(cgVertexProgram);
            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgFragmentProfile);
            if (cgFragmentProfile != CgProfile.Unknown && CgGL.IsProfileSupported(cgFragmentProfile))
            {
                CgGL.SetOptimalOptions(cgFragmentProfile);
            }

            cgFragmentProgram = Cg.CreateProgramFromFile(
                context,
                CgEnum.Source,
                FragmentProgramFileName,
                cgFragmentProfile,
                CgFragmentEntryFuncName,
                fArgs);

            CgGL.LoadProgram(cgFragmentProgram);

            cgParamGlobalAmbient = Cg.GetNamedParameter(cgFragmentProgram, "globalAmbient");
            cgParamEyePosition = Cg.GetNamedParameter(cgFragmentProgram, "eyePosition");
            cgParamLightPosition = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].position");
            cgParamLightColor = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].color");
            cgParamLightConstantAtten = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].kC");
            cgParamLightLinearAtten = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].kL");
            cgParamLightQuadraticAtten = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].kQ");
            cgParamLightDirection = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].direction");
            cgParamLightCosInnerCone = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].cosInnerCone");
            cgParamLightCosOuterCone = Cg.GetNamedParameter(cgFragmentProgram, "lights[0].cosOuterCone");
            cgParamMaterialKe = Cg.GetNamedParameter(cgFragmentProgram, "material.Ke");
            cgParamMaterialKa = Cg.GetNamedParameter(cgFragmentProgram, "material.Ka");
            cgParamMaterialKd = Cg.GetNamedParameter(cgFragmentProgram, "material.Kd");
            cgParamMaterialKs = Cg.GetNamedParameter(cgFragmentProgram, "material.Ks");
            cgParamMaterialShininess = Cg.GetNamedParameter(cgFragmentProgram, "material.shininess");

            Cg.SetParameter3fv(cgParamGlobalAmbient, globalAmbient);
            Cg.SetParameter3fv(cgParamLightColor, lightColor);
            Cg.SetParameter1f(cgParamLightConstantAtten, 1.0f);
            Cg.SetParameter1f(cgParamLightLinearAtten, 0.0f);
            Cg.SetParameter1f(cgParamLightQuadraticAtten, 0.0001f);
            Cg.SetParameter1f(cgParamLightCosInnerCone, 0.95f);
            Cg.SetParameter1f(cgParamLightCosOuterCone, 0.85f);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            Reshape(e.Width, e.Height);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (animating)
            {
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

        protected override void OnUnload()
        {
            base.OnUnload();
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
        }

        private void Display()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float[] eyePosition = { 0, 0, 13, 1 };
            float[] lightPosition =
            {
                5 * (float)Math.Sin(lightAngle),
                1.5f,
                5 * (float)Math.Cos(lightAngle),
                1
            };
            float[] lightDirection = { -lightPosition[0], -lightPosition[1], -lightPosition[2] };

            float[] translateMatrix = new float[16];
            float[] rotateMatrix = new float[16];
            float[] modelMatrix = new float[16];
            float[] invModelMatrix = new float[16];
            float[] viewMatrix = new float[16];
            float[] modelViewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            float[] objSpaceEyePosition = new float[4];
            float[] objSpaceLightPosition = new float[4];
            float[] objSpaceLightDirection = new float[3];

            BuildLookAtMatrix(eyePosition[0], eyePosition[1], eyePosition[2], 0, 0, 0, 0, 1, 0, viewMatrix);

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            SetBrassMaterial();

            MakeRotateMatrix(70, 1, 1, 1, rotateMatrix);
            MakeTranslateMatrix(2, 0, 0, translateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            Cg.SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            Cg.SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);
            TransformDirection(objSpaceLightDirection, invModelMatrix, lightDirection);
            NormalizeVector(objSpaceLightDirection);
            Cg.SetParameter3fv(cgParamLightDirection, objSpaceLightDirection);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidSphere(2.0, 40, 40);

            SetRedPlasticMaterial();

            MakeTranslateMatrix(-2, -1.5f, 0, translateMatrix);
            MakeRotateMatrix(90, 1, 0, 0, rotateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            Cg.SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            Cg.SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);
            TransformDirection(objSpaceLightDirection, invModelMatrix, lightDirection);
            NormalizeVector(objSpaceLightDirection);
            Cg.SetParameter3fv(cgParamLightDirection, objSpaceLightDirection);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidCone(1.5, 3.5, 30, 30);

            SetGreenEmeraldMaterial();

            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);
            Cg.SetParameter3fv(cgParamEyePosition, eyePosition);
            Cg.SetParameter3fv(cgParamLightPosition, lightPosition);
            Array.Copy(lightDirection, objSpaceLightDirection, 3);
            NormalizeVector(objSpaceLightDirection);
            Cg.SetParameter3fv(cgParamLightDirection, objSpaceLightDirection);
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(12, -2, -12);
            GL.Vertex3(-12, -2, -12);
            GL.Vertex3(-12, -2, 12);
            GL.Vertex3(12, -2, 12);

            GL.Normal3(0, 0, 1);
            GL.Vertex3(-12, -2, -12);
            GL.Vertex3(12, -2, -12);
            GL.Vertex3(12, 10, -12);
            GL.Vertex3(-12, 10, -12);

            GL.Normal3(0, -1, 0);
            GL.Vertex3(-12, 10, -12);
            GL.Vertex3(12, 10, -12);
            GL.Vertex3(12, 10, 12);
            GL.Vertex3(-12, 10, 12);

            GL.Normal3(1, 0, 0);
            GL.Vertex3(-12, -2, 12);
            GL.Vertex3(-12, -2, -12);
            GL.Vertex3(-12, 10, -12);
            GL.Vertex3(-12, 10, 12);

            GL.Normal3(-1, 0, 0);
            GL.Vertex3(12, -2, -12);
            GL.Vertex3(12, -2, 12);
            GL.Vertex3(12, 10, 12);
            GL.Vertex3(12, 10, -12);
            GL.End();

            MakeTranslateMatrix(lightPosition[0], lightPosition[1], lightPosition[2], modelMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetEmissiveLightColorOnly();
            Cg.SetParameter3f(cgParamLightPosition, 0f, 0f, 0f);
            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidCone(0.15, 0.95, 30, 30);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        private void Reshape(int width, int height)
        {
            double aspectRatio = (float)width / height;
            BuildPerspectiveMatrix(40.0, aspectRatio, 1.0, 100.0, projectionMatrix);
        }

        private void SetBrassMaterial()
        {
            float[] brassEmissive = { 0.0f, 0.0f, 0.0f };
            float[] brassAmbient = { 0.33f, 0.22f, 0.03f };
            float[] brassDiffuse = { 0.78f, 0.57f, 0.11f };
            float[] brassSpecular = { 0.99f, 0.91f, 0.81f };

            Cg.SetParameter3fv(cgParamMaterialKe, brassEmissive);
            Cg.SetParameter3fv(cgParamMaterialKa, brassAmbient);
            Cg.SetParameter3fv(cgParamMaterialKd, brassDiffuse);
            Cg.SetParameter3fv(cgParamMaterialKs, brassSpecular);
            Cg.SetParameter1f(cgParamMaterialShininess, 27.8f);
        }

        private void SetRedPlasticMaterial()
        {
            float[] redPlasticEmissive = { 0.0f, 0.0f, 0.0f };
            float[] redPlasticAmbient = { 0.0f, 0.0f, 0.0f };
            float[] redPlasticDiffuse = { 0.5f, 0.0f, 0.0f };
            float[] redPlasticSpecular = { 0.7f, 0.6f, 0.6f };

            Cg.SetParameter3fv(cgParamMaterialKe, redPlasticEmissive);
            Cg.SetParameter3fv(cgParamMaterialKa, redPlasticAmbient);
            Cg.SetParameter3fv(cgParamMaterialKd, redPlasticDiffuse);
            Cg.SetParameter3fv(cgParamMaterialKs, redPlasticSpecular);
            Cg.SetParameter1f(cgParamMaterialShininess, 32.0f);
        }

        private void SetGreenEmeraldMaterial()
        {
            float[] greenEmeraldEmissive = { 0.0f, 0.0f, 0.0f };
            float[] greenEmeraldAmbient = { 0.0215f, 0.1745f, 0.0215f };
            float[] greenEmeraldDiffuse = { 0.07568f, 0.61424f, 0.07568f };
            float[] greenEmeraldSpecular = { 0.633f, 0.727811f, 0.633f };

            Cg.SetParameter3fv(cgParamMaterialKe, greenEmeraldEmissive);
            Cg.SetParameter3fv(cgParamMaterialKa, greenEmeraldAmbient);
            Cg.SetParameter3fv(cgParamMaterialKd, greenEmeraldDiffuse);
            Cg.SetParameter3fv(cgParamMaterialKs, greenEmeraldSpecular);
            Cg.SetParameter1f(cgParamMaterialShininess, 76.8f);
        }

        private void SetEmissiveLightColorOnly()
        {
            float[] zero = { 0.0f, 0.0f, 0.0f };

            Cg.SetParameter3fv(cgParamMaterialKe, lightColor);
            Cg.SetParameter3fv(cgParamMaterialKa, zero);
            Cg.SetParameter3fv(cgParamMaterialKd, zero);
            Cg.SetParameter3fv(cgParamMaterialKs, zero);
            Cg.SetParameter1f(cgParamMaterialShininess, 0.0f);
        }

        private static void TransformDirection(float[] dst, float[] mat, float[] vec)
        {
            for (int i = 0; i < 3; i++)
            {
                dst[i] = mat[i * 4 + 0] * vec[0] +
                         mat[i * 4 + 1] * vec[1] +
                         mat[i * 4 + 2] * vec[2];
            }
        }

        private static void NormalizeVector(float[] vector)
        {
            float magnitude = (float)Math.Sqrt(vector[0] * vector[0] + vector[1] * vector[1] + vector[2] * vector[2]);
            if (Math.Abs(magnitude) > float.Epsilon)
            {
                vector[0] /= magnitude;
                vector[1] /= magnitude;
                vector[2] /= magnitude;
            }
        }

    }
}
