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
using OpenCg.ObjectModel.Examples;

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[24] Uniform Fog")]
    class UniformFog : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\C9E2v_fog.cg";
        private const string CgVertexEntryFuncName = "C9E2v_fog";
        private const string FragmentProgramFileName = "Data\\Shaders\\C9E1f_fog.cg";
        private const string CgFragmentEntryFuncName = "C9E1f_fog";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;

        private CgParameterObject cgParamModelViewProj;
        private CgParameterObject cgParamModelView;
        private CgParameterObject cgParamFogDensity;
        
        private CgParameterObject cgParamFogColor;
        private CgParameterObject cgParamDecal;

        private int texSides, texRoof, texPavement;
        private float eyeHeight = 30.0f;
        private float eyeAngle = 0.53f;
        private float fogDensity = 0.08f;
        private float[] fogColor = { 0.8f, 0.9f, 0.8f };
        private int cityHeightMode = 0;
        
        private Random random = new Random(333);

        private readonly float[] projectionMatrix = new float[16];

        public UniformFog()
            : base("Cg Tutorial 25: Uniform Fog", 800, 400)
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(fogColor[0], fogColor[1], fogColor[2], 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            texSides = DDSLoader.LoadTexture2D("Data\\Images\\BuildingWindows.dds");
            texRoof = DDSLoader.LoadTexture2D("Data\\Images\\BuildingRoof.dds");
            texPavement = DDSLoader.LoadTexture2D("Data\\Images\\Pavement.dds");

            context = OpenCg.Graphics.ObjectModel.Context.Create();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context.Handle, true);
            context.ParameterSettingMode = CgEnum.DeferredParameterSetting;

            cgVertexProfile = CgGL.GetLatestProfile(CgGLEnum.Vertex);
            cgVertexProgram = context.CreateProgramFromFile( CgEnum.Source, VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgParamFogDensity = cgVertexProgram.GetNamedParameter("fogDensity");
            cgParamModelViewProj = cgVertexProgram.GetNamedParameter("modelViewProj");
            cgParamModelView = cgVertexProgram.GetNamedParameter("modelView");

            SetParameter1f(cgParamFogDensity, fogDensity);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            cgFragmentProgram = context.CreateProgramFromFile( CgEnum.Source, FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            cgParamFogColor = cgFragmentProgram.GetNamedParameter("fogColor");
            SetParameter3f(cgParamFogColor, fogColor[0], fogColor[1], fogColor[2]);

            cgParamDecal = cgFragmentProgram.GetNamedParameter("decal");
        }

        private void drawBuilding(float x, float y, float height)
        {
            float[][] roof_coords = { new[] { 1f, 1f }, new[] { 1f, 0f }, new[] { 0f, 0f }, new[] { 0f, 1f } };
            float[] streetColor = { 1f, 1f, 1f };
            float[] buildingTopColor = { 0.8f, 0.8f, 0.8f };
            
            float streetLevel = 0.0f;
            float buildingTopTexCoord = 1.0f + (float)Math.Floor(height / 2.0f);
            float deltaHeight = height / buildingTopTexCoord;
            int roof_config = random.Next() % 4;

            float[] buildingColor = {
                (float)random.NextDouble() * 0.2f + 0.8f,
                (float)random.NextDouble() * 0.2f + 0.8f,
                (float)random.NextDouble() * 0.2f + 0.8f
            };

            GL.BindTexture(TextureTarget.Texture2D, texPavement);
            SetTextureParameter(cgParamDecal, texPavement);
            GL.Color3(streetColor);
            
            GL.Begin(PrimitiveType.TriangleFan);
            GL.TexCoord2(1.0f / 4.0f, 1.0f / 4.0f); GL.Vertex3(x + 1.0f, streetLevel, y + 1.0f);
            GL.TexCoord2(4.0f / 4.0f, 1.0f / 4.0f); GL.Vertex3(x + 4.0f, streetLevel, y + 1.0f);
            GL.TexCoord2(4.0f / 4.0f, 0.0f / 4.0f); GL.Vertex3(x + 4.0f, streetLevel, y + 0.0f);
            GL.TexCoord2(0.0f / 4.0f, 0.0f / 4.0f); GL.Vertex3(x + 0.0f, streetLevel, y + 0.0f);
            GL.TexCoord2(0.0f / 4.0f, 4.0f / 4.0f); GL.Vertex3(x + 0.0f, streetLevel, y + 4.0f);
            GL.TexCoord2(1.0f / 4.0f, 4.0f / 4.0f); GL.Vertex3(x + 1.0f, streetLevel, y + 4.0f);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, texRoof);
            SetTextureParameter(cgParamDecal, texRoof);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(buildingTopColor);
            GL.TexCoord2(roof_coords[(roof_config + 0) % 4][1], roof_coords[(roof_config + 0) % 4][0]); GL.Vertex3(x + 4.0f, height, y + 4.0f);
            GL.TexCoord2(roof_coords[(roof_config + 1) % 4][1], roof_coords[(roof_config + 1) % 4][0]); GL.Vertex3(x + 4.0f, height, y + 1.0f);
            GL.TexCoord2(roof_coords[(roof_config + 2) % 4][1], roof_coords[(roof_config + 2) % 4][0]); GL.Vertex3(x + 1.0f, height, y + 1.0f);
            GL.TexCoord2(roof_coords[(roof_config + 3) % 4][1], roof_coords[(roof_config + 3) % 4][0]); GL.Vertex3(x + 1.0f, height, y + 4.0f);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, texSides);
            SetTextureParameter(cgParamDecal, texSides);
            GL.Color3(buildingColor);

            GL.Begin(PrimitiveType.Quads);
            float tex0 = 0, tex1 = 1, height0 = 0, height1 = deltaHeight;
            for (; tex0 < buildingTopTexCoord; tex0++, tex1++, height0 += deltaHeight, height1 += deltaHeight)
            {
                GL.TexCoord2(0, tex0); GL.Vertex3(x + 1.0f, height0, y + 1.0f);
                GL.TexCoord2(0, tex1); GL.Vertex3(x + 1.0f, height1, y + 1.0f);
                GL.TexCoord2(1, tex1); GL.Vertex3(x + 4.0f, height1, y + 1.0f);
                GL.TexCoord2(1, tex0); GL.Vertex3(x + 4.0f, height0, y + 1.0f);

                GL.TexCoord2(1, tex0); GL.Vertex3(x + 1.0f, height0, y + 4.0f);
                GL.TexCoord2(1, tex1); GL.Vertex3(x + 1.0f, height1, y + 4.0f);
                GL.TexCoord2(0, tex1); GL.Vertex3(x + 1.0f, height1, y + 1.0f);
                GL.TexCoord2(0, tex0); GL.Vertex3(x + 1.0f, height0, y + 1.0f);

                GL.TexCoord2(0, tex0); GL.Vertex3(x + 4.0f, height0, y + 1.0f);
                GL.TexCoord2(0, tex1); GL.Vertex3(x + 4.0f, height1, y + 1.0f);
                GL.TexCoord2(1, tex1); GL.Vertex3(x + 4.0f, height1, y + 4.0f);
                GL.TexCoord2(1, tex0); GL.Vertex3(x + 4.0f, height0, y + 4.0f);

                GL.TexCoord2(1, tex0); GL.Vertex3(x + 4.0f, height0, y + 4.0f);
                GL.TexCoord2(1, tex1); GL.Vertex3(x + 4.0f, height1, y + 4.0f);
                GL.TexCoord2(0, tex1); GL.Vertex3(x + 1.0f, height1, y + 4.0f);
                GL.TexCoord2(0, tex0); GL.Vertex3(x + 1.0f, height0, y + 4.0f);
            }
            GL.End();
        }

        private void drawCity(int heightMode)
        {
            random = new Random(333);
            int CITY_COLS = 22;
            int CITY_ROWS = 22;

            for (int i = 0; i < CITY_COLS; i++)
            {
                for (int j = 0; j < CITY_ROWS; j++)
                {
                    float x = 4.0f * i - (2 * CITY_COLS + 0.5f);
                    float y = 4.0f * j - (2 * CITY_ROWS + 0.5f);
                    float height = 0.0f;

                    switch (heightMode)
                    {
                        case 0:
                            height = 0.1f * (x * x + y * y) * (float)random.NextDouble() + 0.3f * (float)Math.Sqrt(x * x + y * y) * (float)random.NextDouble() + 1.0f;
                            break;
                        case 1:
                            height = 3.0f;
                            break;
                        case 2:
                            height = 0.05f * (x * x + y * y) + 0.15f * (float)Math.Sqrt(x * x + y * y) + 1.0f;
                            break;
                        case 3:
                            height = 0.05f * (x * x) + 0.15f * (float)Math.Sqrt(x * x) + 1.0f;
                            break;
                    }
                    drawBuilding(x, y, height);
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float eyeRadius = 7.0f;
            float eyeX = eyeRadius * (float)Math.Sin(eyeAngle);
            float eyeZ = eyeRadius * (float)Math.Cos(eyeAngle);

            float[] viewMatrix = new float[16];
            float[] modelViewProjMatrix = new float[16];
            BuildLookAtMatrix(eyeX, eyeHeight, eyeZ, 0, 20, 0, 0, 1, 0, viewMatrix);
            MultMatrix(modelViewProjMatrix, projectionMatrix, viewMatrix);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);
            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            SetMatrixParameterfr(cgParamModelViewProj, modelViewProjMatrix);
            SetMatrixParameterfr(cgParamModelView, viewMatrix);

            UpdateProgramParameters(cgVertexProgram);
            UpdateProgramParameters(cgFragmentProgram);

            drawCity(cityHeightMode);

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (IsKeyDown(Keys.Space) && !WasKeyDown(Keys.Space))
            {
                cityHeightMode = (cityHeightMode + 1) % 4;
            }

            if (IsKeyDown(Keys.Left)) eyeAngle -= 1.5f * (float)e.Time;
            if (IsKeyDown(Keys.Right)) eyeAngle += 1.5f * (float)e.Time;
            if (IsKeyDown(Keys.Up)) eyeHeight += 20.0f * (float)e.Time;
            if (IsKeyDown(Keys.Down)) eyeHeight -= 20.0f * (float)e.Time;

            if (IsKeyDown(Keys.F) || IsKeyDown(Keys.Minus) || IsKeyDown(Keys.KeyPadSubtract))
            {
                fogDensity /= 1.05f;
                SetParameter1f(cgParamFogDensity, fogDensity);
            }
            if (IsKeyDown(Keys.G) || IsKeyDown(Keys.Equal) || IsKeyDown(Keys.KeyPadAdd))
            {
                fogDensity *= 1.05f;
                SetParameter1f(cgParamFogDensity, fogDensity);
            }

            if (IsKeyDown(Keys.Escape)) Close();
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            BuildPerspectiveMatrix(40.0, e.Width / (double)e.Height, 1.0, 500.0, projectionMatrix);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(texSides);
            GL.DeleteTexture(texRoof);
            GL.DeleteTexture(texPavement);
            DisposeProgram(cgVertexProgram);
            DisposeProgram(cgFragmentProgram);
            context?.Dispose();
        }
    }
}
