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

    // OpenGL-based spotlight attenuation example
    // using Cg program from Chapter 5 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).
    // Requires the OpenGL Utility Toolkit (GLUT) and Cg runtime (version 1.5 or higher).

    #endregion
    [ExampleAttribute("OpenTK/Basic/[13] Spotlight")]

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

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamGlobalAmbient;
        private CgParameterObject cgParamEyePosition;
        private CgParameterObject cgParamLightPosition;
        private CgParameterObject cgParamLightColor;
        private CgParameterObject cgParamLightConstantAtten;
        private CgParameterObject cgParamLightLinearAtten;
        private CgParameterObject cgParamLightQuadraticAtten;
        private CgParameterObject cgParamLightDirection;
        private CgParameterObject cgParamLightCosInnerCone;
        private CgParameterObject cgParamLightCosOuterCone;
        private CgParameterObject cgParamMaterialKe;
        private CgParameterObject cgParamMaterialKa;
        private CgParameterObject cgParamMaterialKd;
        private CgParameterObject cgParamMaterialKs;
        private CgParameterObject cgParamMaterialShininess;

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

            context = OpenCg.Graphics.ObjectModel.Context.Create();

            Cg.SetErrorCallback(errorDelegate);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            string[] vArgs = CgGL.GetOptimalOptions(cgVertexProfile);
            if (cgVertexProfile != CgProfile.Unknown && CgGL.IsProfileSupported(cgVertexProfile))
            {
                CgGL.SetOptimalOptions(cgVertexProfile);
            }

            cgVertexProgram = context.CreateProgramFromFile(
                CgEnum.Source,
                VertexProgramFileName,
                cgVertexProfile,
                CgVertexEntryFuncName,
                vArgs);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);
            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgFragmentProfile);
            if (cgFragmentProfile != CgProfile.Unknown && CgGL.IsProfileSupported(cgFragmentProfile))
            {
                CgGL.SetOptimalOptions(cgFragmentProfile);
            }

            cgFragmentProgram = context.CreateProgramFromFile(
                CgEnum.Source,
                FragmentProgramFileName,
                cgFragmentProfile,
                CgFragmentEntryFuncName,
                fArgs);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamGlobalAmbient = cgFragmentProgram.GetNamedParameter("globalAmbient");
            cgParamEyePosition = cgFragmentProgram.GetNamedParameter("eyePosition");
            cgParamLightPosition = cgFragmentProgram.GetNamedParameter("lights[0].position");
            cgParamLightColor = cgFragmentProgram.GetNamedParameter("lights[0].color");
            cgParamLightConstantAtten = cgFragmentProgram.GetNamedParameter("lights[0].kC");
            cgParamLightLinearAtten = cgFragmentProgram.GetNamedParameter("lights[0].kL");
            cgParamLightQuadraticAtten = cgFragmentProgram.GetNamedParameter("lights[0].kQ");
            cgParamLightDirection = cgFragmentProgram.GetNamedParameter("lights[0].direction");
            cgParamLightCosInnerCone = cgFragmentProgram.GetNamedParameter("lights[0].cosInnerCone");
            cgParamLightCosOuterCone = cgFragmentProgram.GetNamedParameter("lights[0].cosOuterCone");
            cgParamMaterialKe = cgFragmentProgram.GetNamedParameter("material.Ke");
            cgParamMaterialKa = cgFragmentProgram.GetNamedParameter("material.Ka");
            cgParamMaterialKd = cgFragmentProgram.GetNamedParameter("material.Kd");
            cgParamMaterialKs = cgFragmentProgram.GetNamedParameter("material.Ks");
            cgParamMaterialShininess = cgFragmentProgram.GetNamedParameter("material.shininess");

            SetParameter3fv(cgParamGlobalAmbient, globalAmbient);
            SetParameter3fv(cgParamLightColor, lightColor);
            SetParameter1f(cgParamLightConstantAtten, 1.0f);
            SetParameter1f(cgParamLightLinearAtten, 0.0f);
            SetParameter1f(cgParamLightQuadraticAtten, 0.0001f);
            SetParameter1f(cgParamLightCosInnerCone, 0.95f);
            SetParameter1f(cgParamLightCosOuterCone, 0.85f);
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
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
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

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            SetBrassMaterial();

            MakeRotateMatrix(70, 1, 1, 1, rotateMatrix);
            MakeTranslateMatrix(2, 0, 0, translateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);
            TransformDirection(objSpaceLightDirection, invModelMatrix, lightDirection);
            NormalizeVector(objSpaceLightDirection);
            SetParameter3fv(cgParamLightDirection, objSpaceLightDirection);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidSphere(2.0, 40, 40);

            SetRedPlasticMaterial();

            MakeTranslateMatrix(-2, -1.5f, 0, translateMatrix);
            MakeRotateMatrix(90, 1, 0, 0, rotateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);
            TransformDirection(objSpaceLightDirection, invModelMatrix, lightDirection);
            NormalizeVector(objSpaceLightDirection);
            SetParameter3fv(cgParamLightDirection, objSpaceLightDirection);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
            NativeMethods.glutSolidCone(1.5, 3.5, 30, 30);

            SetGreenEmeraldMaterial();

            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);
            SetParameter3fv(cgParamEyePosition, eyePosition);
            SetParameter3fv(cgParamLightPosition, lightPosition);
            Array.Copy(lightDirection, objSpaceLightDirection, 3);
            NormalizeVector(objSpaceLightDirection);
            SetParameter3fv(cgParamLightDirection, objSpaceLightDirection);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

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
            SetParameter3f(cgParamLightPosition, 0f, 0f, 0f);
            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);
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

            SetParameter3fv(cgParamMaterialKe, brassEmissive);
            SetParameter3fv(cgParamMaterialKa, brassAmbient);
            SetParameter3fv(cgParamMaterialKd, brassDiffuse);
            SetParameter3fv(cgParamMaterialKs, brassSpecular);
            SetParameter1f(cgParamMaterialShininess, 27.8f);
        }

        private void SetRedPlasticMaterial()
        {
            float[] redPlasticEmissive = { 0.0f, 0.0f, 0.0f };
            float[] redPlasticAmbient = { 0.0f, 0.0f, 0.0f };
            float[] redPlasticDiffuse = { 0.5f, 0.0f, 0.0f };
            float[] redPlasticSpecular = { 0.7f, 0.6f, 0.6f };

            SetParameter3fv(cgParamMaterialKe, redPlasticEmissive);
            SetParameter3fv(cgParamMaterialKa, redPlasticAmbient);
            SetParameter3fv(cgParamMaterialKd, redPlasticDiffuse);
            SetParameter3fv(cgParamMaterialKs, redPlasticSpecular);
            SetParameter1f(cgParamMaterialShininess, 32.0f);
        }

        private void SetGreenEmeraldMaterial()
        {
            float[] greenEmeraldEmissive = { 0.0f, 0.0f, 0.0f };
            float[] greenEmeraldAmbient = { 0.0215f, 0.1745f, 0.0215f };
            float[] greenEmeraldDiffuse = { 0.07568f, 0.61424f, 0.07568f };
            float[] greenEmeraldSpecular = { 0.633f, 0.727811f, 0.633f };

            SetParameter3fv(cgParamMaterialKe, greenEmeraldEmissive);
            SetParameter3fv(cgParamMaterialKa, greenEmeraldAmbient);
            SetParameter3fv(cgParamMaterialKd, greenEmeraldDiffuse);
            SetParameter3fv(cgParamMaterialKs, greenEmeraldSpecular);
            SetParameter1f(cgParamMaterialShininess, 76.8f);
        }

        private void SetEmissiveLightColorOnly()
        {
            float[] zero = { 0.0f, 0.0f, 0.0f };

            SetParameter3fv(cgParamMaterialKe, lightColor);
            SetParameter3fv(cgParamMaterialKa, zero);
            SetParameter3fv(cgParamMaterialKd, zero);
            SetParameter3fv(cgParamMaterialKs, zero);
            SetParameter1f(cgParamMaterialShininess, 0.0f);
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
