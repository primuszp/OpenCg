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

    // OpenGL-based bulge deformation example
    // using Cg program from Chapter 6 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).
    // Requires the OpenGL Utility Toolkit (GLUT) and Cg runtime (version 1.5 or higher).

    #endregion
    [ExampleAttribute("OpenTK/Basic/[14] Bulge")]

    class Bulge : BaseExample
    {
        #region Members

        private const string VertexProgramFileName = "Data\\Shaders\\C6E1v_bulge.cg";
        private const string CgVertexEntryFuncName = "C6E1v_bulge";
        private const string LightVertexProgramSource =
            "void main(inout float4 p : POSITION, uniform float4x4 modelViewProj, out float4 c : COLOR) " +
            "{ p = mul(modelViewProj, p); c = float4(1,1,0,1); }";
        private const string PassthroughFragmentProgramSource = "float4 main(float4 c : COLOR) : COLOR { return c; }";
        private const double LightAngularSpeed = 0.48f;
        private const double TimeFlowSpeed = 0.6f;

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;
        private CgProgramObject cgLightVertexProgram;

        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamTime;
        private CgParameterObject cgParamFrequency;
        private CgParameterObject cgParamScaleFactor;
        private CgParameterObject cgParamKd;
        private CgParameterObject cgParamShininess;
        private CgParameterObject cgParamEyePosition;
        private CgParameterObject cgParamLightPosition;
        private CgParameterObject cgParamLightColor;
        private CgParameterObject cgLightParamModelViewProj;

        private readonly float[] projectionMatrix = new float[16];
        private readonly float[] lightColor = { 0.95f, 0.95f, 0.95f };

        private double lightAngle = -0.4f;
        private double bulgeTime;
        private double lightVelocity = LightAngularSpeed;
        private double timeFlow = TimeFlowSpeed;
        private bool animating = true;

        #endregion

        public Bulge()
            : base("Cg Tutorial 14: Bulge", 400, 400)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.5f, 0.0f);
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
            cgParamTime = cgVertexProgram.GetNamedParameter("time");
            cgParamFrequency = cgVertexProgram.GetNamedParameter("frequency");
            cgParamScaleFactor = cgVertexProgram.GetNamedParameter("scaleFactor");
            cgParamKd = cgVertexProgram.GetNamedParameter("Kd");
            cgParamShininess = cgVertexProgram.GetNamedParameter("shininess");
            cgParamEyePosition = cgVertexProgram.GetNamedParameter("eyePosition");
            cgParamLightPosition = cgVertexProgram.GetNamedParameter("lightPosition");
            cgParamLightColor = cgVertexProgram.GetNamedParameter("lightColor");

            SetParameter3fv(cgParamLightColor, lightColor);
            SetParameter1f(cgParamScaleFactor, 0.3f);
            SetParameter1f(cgParamFrequency, 2.4f);
            SetParameter1f(cgParamShininess, 35f);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgFragmentProfile);
            if (cgFragmentProfile != CgProfile.Unknown && CgGL.IsProfileSupported(cgFragmentProfile))
            {
                CgGL.SetOptimalOptions(cgFragmentProfile);
            }

            cgFragmentProgram = context.CreateProgram(
                CgEnum.Source,
                PassthroughFragmentProgramSource,
                cgFragmentProfile,
                "main",
                fArgs);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgLightVertexProgram = context.CreateProgram(
                CgEnum.Source,
                LightVertexProgramSource,
                cgVertexProfile,
                "main",
                vArgs);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgLightVertexProgram);
            cgLightParamModelViewProj = cgLightVertexProgram.GetNamedParameter("modelViewProj");
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
                lightAngle += lightVelocity * e.Time;
                if (lightAngle > Pi / 2)
                {
                    lightAngle = Pi / 2;
                    lightVelocity = -lightVelocity;
                }
                else if (lightAngle < -Pi / 2)
                {
                    lightAngle = -Pi / 2;
                    lightVelocity = -lightVelocity;
                }

                bulgeTime += timeFlow * e.Time;
                if (bulgeTime > 10.0)
                {
                    bulgeTime = 10.0;
                    timeFlow = -timeFlow;
                }
                else if (bulgeTime < 0.0)
                {
                    bulgeTime = 0.0;
                    timeFlow = -timeFlow;
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
            DisposeProgram(cgLightVertexProgram);
            DisposeProgram(cgFragmentProgram);
            DisposeProgram(cgVertexProgram);
            context?.Dispose();
        }

        private void Display()
        {
            float[] eyePosition = { 0, 0, 13, 1 };
            float[] lightPosition =
            {
                5 * (float)Math.Sin(lightAngle),
                1.5f,
                5 * (float)Math.Cos(lightAngle),
                1
            };

            float[] translateMatrix = new float[16];
            float[] rotateMatrix = new float[16];
            float[] modelMatrix = new float[16];
            float[] invModelMatrix = new float[16];
            float[] viewMatrix = new float[16];
            float[] modelViewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            float[] objSpaceEyePosition = new float[4];
            float[] objSpaceLightPosition = new float[4];

            SetParameter1f(cgParamTime, (float)bulgeTime);

            BuildLookAtMatrix(eyePosition[0], eyePosition[1], eyePosition[2], 0, 0, 0, 0, 1, 0, viewMatrix);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CgGL.EnableProfile(cgVertexProfile);
            CgGL.EnableProfile(cgFragmentProfile);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);

            MakeRotateMatrix(70, 1, 1, 1, rotateMatrix);
            MakeTranslateMatrix(2.2f, 1.0f, 0.2f, translateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3f(cgParamKd, 0.1f, 0.7f, 0.1f);
            UpdateProgramParameters(cgVertexProgram);
            NativeMethods.glutSolidSphere(1.0, 40, 40);

            MakeTranslateMatrix(-2.0f, -1.5f, 0.0f, translateMatrix);
            MakeRotateMatrix(55, 1, 0, 0, rotateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetParameter3f(cgParamKd, 0.8f, 0.1f, 0.1f);
            UpdateProgramParameters(cgVertexProgram);
            NativeMethods.glutSolidTorus(0.15, 1.7, 40, 40);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgLightVertexProgram);

            MakeTranslateMatrix(lightPosition[0], lightPosition[1], lightPosition[2], modelMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            SetMatrixParameterfr(cgLightParamModelViewProj, modelViewProjMatrix);
            UpdateProgramParameters(cgLightVertexProgram);
            NativeMethods.glutSolidSphere(0.1, 12, 12);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        private void Reshape(int width, int height)
        {
            double aspectRatio = (float)width / height;
            BuildPerspectiveMatrix(40.0, aspectRatio, 1.0, 20.0, projectionMatrix);
        }

    }
}
