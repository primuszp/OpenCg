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

    // OpenGL-based bulge deformation example
    // using Cg program from Chapter 6 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).
    // Requires the OpenGL Utility Toolkit (GLUT) and Cg runtime (version 1.5 or higher).

    #endregion

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

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;
        private CgProgram cgLightVertexProgram;

        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamTime;
        private CgParameter cgParamFrequency;
        private CgParameter cgParamScaleFactor;
        private CgParameter cgParamKd;
        private CgParameter cgParamShininess;
        private CgParameter cgParamEyePosition;
        private CgParameter cgParamLightPosition;
        private CgParameter cgParamLightColor;
        private CgParameter cgLightParamModelViewProj;

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
            cgParamTime = Cg.GetNamedParameter(cgVertexProgram, "time");
            cgParamFrequency = Cg.GetNamedParameter(cgVertexProgram, "frequency");
            cgParamScaleFactor = Cg.GetNamedParameter(cgVertexProgram, "scaleFactor");
            cgParamKd = Cg.GetNamedParameter(cgVertexProgram, "Kd");
            cgParamShininess = Cg.GetNamedParameter(cgVertexProgram, "shininess");
            cgParamEyePosition = Cg.GetNamedParameter(cgVertexProgram, "eyePosition");
            cgParamLightPosition = Cg.GetNamedParameter(cgVertexProgram, "lightPosition");
            cgParamLightColor = Cg.GetNamedParameter(cgVertexProgram, "lightColor");

            Cg.SetParameter3fv(cgParamLightColor, lightColor);
            Cg.SetParameter1f(cgParamScaleFactor, 0.3f);
            Cg.SetParameter1f(cgParamFrequency, 2.4f);
            Cg.SetParameter1f(cgParamShininess, 35f);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgFragmentProfile);
            if (cgFragmentProfile != CgProfile.Unknown && CgGL.IsProfileSupported(cgFragmentProfile))
            {
                CgGL.SetOptimalOptions(cgFragmentProfile);
            }

            cgFragmentProgram = Cg.CreateProgram(
                context,
                CgEnum.Source,
                PassthroughFragmentProgramSource,
                cgFragmentProfile,
                "main",
                fArgs);

            CgGL.LoadProgram(cgFragmentProgram);

            cgLightVertexProgram = Cg.CreateProgram(
                context,
                CgEnum.Source,
                LightVertexProgramSource,
                cgVertexProfile,
                "main",
                vArgs);

            CgGL.LoadProgram(cgLightVertexProgram);
            cgLightParamModelViewProj = Cg.GetNamedParameter(cgLightVertexProgram, "modelViewProj");
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
            Cg.DestroyProgram(cgLightVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyContext(context);
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

            Cg.SetParameter1f(cgParamTime, (float)bulgeTime);

            BuildLookAtMatrix(eyePosition[0], eyePosition[1], eyePosition[2], 0, 0, 0, 0, 1, 0, viewMatrix);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CgGL.EnableProfile(cgVertexProfile);
            CgGL.EnableProfile(cgFragmentProfile);

            CgGL.BindProgram(cgVertexProgram);
            CgGL.BindProgram(cgFragmentProgram);

            MakeRotateMatrix(70, 1, 1, 1, rotateMatrix);
            MakeTranslateMatrix(2.2f, 1.0f, 0.2f, translateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            Cg.SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            Cg.SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.SetParameter3f(cgParamKd, 0.1f, 0.7f, 0.1f);
            Cg.UpdateProgramParameters(cgVertexProgram);
            NativeMethods.glutSolidSphere(1.0, 40, 40);

            MakeTranslateMatrix(-2.0f, -1.5f, 0.0f, translateMatrix);
            MakeRotateMatrix(55, 1, 0, 0, rotateMatrix);
            MultMatrix(modelMatrix, translateMatrix, rotateMatrix);
            InvertMatrix(invModelMatrix, modelMatrix);

            Transform(objSpaceEyePosition, invModelMatrix, eyePosition);
            Cg.SetParameter3fv(cgParamEyePosition, objSpaceEyePosition);
            Transform(objSpaceLightPosition, invModelMatrix, lightPosition);
            Cg.SetParameter3fv(cgParamLightPosition, objSpaceLightPosition);

            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            Cg.SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            Cg.SetParameter3f(cgParamKd, 0.8f, 0.1f, 0.1f);
            Cg.UpdateProgramParameters(cgVertexProgram);
            NativeMethods.glutSolidTorus(0.15, 1.7, 40, 40);

            CgGL.BindProgram(cgLightVertexProgram);

            MakeTranslateMatrix(lightPosition[0], lightPosition[1], lightPosition[2], modelMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, modelViewMatrix);

            Cg.SetMatrixParameterfr(cgLightParamModelViewProj, modelViewProjMatrix);
            Cg.UpdateProgramParameters(cgLightVertexProgram);
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
