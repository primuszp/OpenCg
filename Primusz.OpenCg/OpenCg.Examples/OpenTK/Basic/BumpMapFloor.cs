using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenCg.Examples.OpenTK.Basic
{
    class BumpMapFloor : BaseExample
    {
        private const string VertexProgramFileName   = "Data\\Shaders\\C8E5v_bumpAny.cg";
        private const string CgVertexEntryFuncName   = "C8E5v_bumpAny";
        private const string FragmentProgramFileName = "Data\\Shaders\\C8E2f_bumpSurf.cg";
        private const string CgFragmentEntryFuncName = "C8E2f_bumpSurf";

        private CgProfile cgVertexProfile   = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;

        private CgParameter cgParamLightPosition;
        private CgParameter cgParamEyePosition;
        private CgParameter cgParamModelViewProj;
        private CgParameter cgParamNormalMap;
        private CgParameter cgParamNormalizeCube;

        private int normalMapTexture;
        private int normalizeCubeTexture;

        private float lightAngle = 4.0f;
        private float lightZ     = 4.0f;
        private float lightZDir  = 0.125f;
        private float eyeAngle   = 0.0f;
        private float eyeHeight  = 0.0f;

        // 0=none, 1=rotate+sweep, 2=sweep only, 3=rotate only
        private int animating = 1;

        private bool  mouseMoving = false;
        private float mouseBeginX = 0;
        private float mouseBeginY = 0;


        public BumpMapFloor()
            : base("Cg Tutorial 23: Bump Map Floor", 400, 400)
        { }

        protected override void OnLoad()
        {
            VSync = VSyncMode.On;
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            // Mipmapped 128x128 brick normal map
            normalMapTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, normalMapTexture);
            int offset = 0;
            for (int size = 128, level = 0; size > 0; size /= 2, level++)
            {
                int bytes = size * size * 3;
                byte[] levelData = new byte[bytes];
                System.Array.Copy(ImageBrick.Array, offset, levelData, 0, bytes);
                GL.TexImage2D(TextureTarget.Texture2D, level, PixelInternalFormat.Rgb8,
                              size, size, 0, PixelFormat.Rgb, PixelType.UnsignedByte, levelData);
                offset += bytes;
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.LinearMipmapLinear);

            // Normalization vector cube map (32x32 per face, no mipmaps)
            normalizeCubeTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, normalizeCubeTexture);
            int faceBytes = 32 * 32 * 3;
            for (int i = 0; i < 6; i++)
            {
                byte[] faceData = new byte[faceBytes];
                System.Array.Copy(ImageNormcm.Array, i * faceBytes, faceData, 0, faceBytes);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb8,
                              32, 32, 0, PixelFormat.Rgb, PixelType.UnsignedByte, faceData);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS,
                            (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT,
                            (int)TextureWrapMode.ClampToEdge);

            context = Cg.CreateContext();
            Cg.SetErrorCallback(errorDelegate);
            CgGL.SetManageTextureParameters(context, true);
            Cg.SetParameterSettingMode(context, CgEnum.DeferredParameterSetting);

            cgVertexProfile = CgProfile.Arbvp1;
            CgGL.SetOptimalOptions(cgVertexProfile);
            cgVertexProgram = Cg.CreateProgramFromFile(context, CgEnum.Source,
                VertexProgramFileName, cgVertexProfile, CgVertexEntryFuncName, null);
            CgGL.LoadProgram(cgVertexProgram);

            cgFragmentProfile = CgProfile.Arbfp1;
            CgGL.SetOptimalOptions(cgFragmentProfile);
            cgFragmentProgram = Cg.CreateProgramFromFile(context, CgEnum.Source,
                FragmentProgramFileName, cgFragmentProfile, CgFragmentEntryFuncName, null);
            CgGL.LoadProgram(cgFragmentProgram);

            cgParamLightPosition = Cg.GetNamedParameter(cgVertexProgram, "lightPosition");
            cgParamEyePosition   = Cg.GetNamedParameter(cgVertexProgram, "eyePosition");
            cgParamModelViewProj = Cg.GetNamedParameter(cgVertexProgram, "modelViewProj");

            cgParamNormalMap     = Cg.GetNamedParameter(cgFragmentProgram, "normalMap");
            cgParamNormalizeCube = Cg.GetNamedParameter(cgFragmentProgram, "normalizeCube");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            const float lightRadius = 5.1f;
            float[] lightPosition = {
                lightRadius * (float)Math.Sin(lightAngle),
                lightRadius * (float)Math.Cos(lightAngle),
                lightZ
            };
            float[] eyePosition = {
                20.0f * (float)Math.Sin(eyeAngle),
                eyeHeight,
                20.0f * (float)Math.Cos(eyeAngle)
            };

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = Matrix4.LookAt(
                new Vector3(eyePosition[0], eyePosition[1], eyePosition[2]),
                Vector3.Zero, Vector3.UnitY);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref view);

            CgGL.BindProgram(cgVertexProgram);
            CgGL.SetStateMatrixParameter(cgParamModelViewProj,
                (int)MatrixType.ModelviewProjectionMatrix,
                (int)MatrixTransform.MatrixIdentity);
            Cg.SetParameter3fv(cgParamLightPosition, lightPosition);
            Cg.SetParameter3fv(cgParamEyePosition,   eyePosition);
            CgGL.EnableProfile(cgVertexProfile);

            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            CgGL.SetTextureParameter(cgParamNormalMap,     normalMapTexture);
            CgGL.SetTextureParameter(cgParamNormalizeCube, normalizeCubeTexture);

            Cg.UpdateProgramParameters(cgVertexProgram);
            Cg.UpdateProgramParameters(cgFragmentProgram);

            DrawRoom();

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            // Render light source as yellow ball using fixed-function pipe
            GL.Translate(lightPosition[0], lightPosition[1], lightPosition[2]);
            GL.Color3(0.8f, 0.8f, 0.1f);
            NativeMethods.glutSolidSphere(0.4, 12, 12);

            SwapBuffers();
        }

        private void DrawRoom()
        {
            GL.Begin(PrimitiveType.Quads);

            /* back wall */
            GL.Normal3(0.0f, 0.0f, 1.0f);
            GL.MultiTexCoord3(TextureUnit.Texture1, 1.0f, 0.0f, 0.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-7f, -7f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2( 7f, -7f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2( 7f,  7f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-7f,  7f);

            /* floor */
            GL.Normal3(0.0f, 1.0f, 0.0f);
            GL.MultiTexCoord3(TextureUnit.Texture1, 1.0f, 0.0f, 0.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-7f, -7f, 14f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3( 7f, -7f, 14f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3( 7f, -7f,  0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-7f, -7f,  0f);

            /* ceiling */
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.MultiTexCoord3(TextureUnit.Texture1, 1.0f, 0.0f, 0.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-7f,  7f,  0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3( 7f,  7f,  0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3( 7f,  7f, 14f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-7f,  7f, 14f);

            /* left wall */
            GL.Normal3(1.0f, 0.0f, 0.0f);
            GL.MultiTexCoord3(TextureUnit.Texture1, 0.0f, 0.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-7f, -7f,  0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-7f,  7f,  0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-7f,  7f, 14f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-7f, -7f, 14f);

            /* right wall */
            GL.Normal3(-1.0f, 0.0f, 0.0f);
            GL.MultiTexCoord3(TextureUnit.Texture1, 0.0f, 0.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(7f, -7f,  0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(7f, -7f, 14f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(7f,  7f, 14f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(7f,  7f,  0f);

            /* front wall */
            GL.Normal3(0.0f, 0.0f, -1.0f);
            GL.MultiTexCoord3(TextureUnit.Texture1, 1.0f, 0.0f, 0.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-7f, -7f, 14f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-7f,  7f, 14f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3( 7f,  7f, 14f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3( 7f, -7f, 14f);

            GL.End();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            float dt = (float)e.Time;
            // Rotate light around Z axis (states 1 and 3)
            if ((animating & 1) != 0)
            {
                lightAngle += 0.75f * dt;  // 0.0125 * 60fps
                if (lightAngle > 2 * Pi) lightAngle -= 2 * Pi;
            }
            // Sweep light back and forth along Z axis (states 1 and 2)
            if (((animating + 1) & 2) != 0)
            {
                lightZ += lightZDir * 60f * dt;  // 0.125 * 60fps
                if (lightZ >= 13.0f) lightZDir = -0.125f;
                if (lightZ <= 1.0f)  lightZDir =  0.125f;
            }

            if (IsKeyDown(Keys.Space) && !WasKeyDown(Keys.Space))
                animating = (animating + 1) % 4;

            if (IsKeyDown(Keys.F) && !WasKeyDown(Keys.F)) lightZ += 0.2f;
            if (IsKeyDown(Keys.B) && !WasKeyDown(Keys.B)) lightZ -= 0.2f;

            if (IsKeyDown(Keys.Escape)) Close();

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                mouseMoving = true;
                mouseBeginX = MouseState.X;
                mouseBeginY = MouseState.Y;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                mouseMoving = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            const float heightMax =  20f;
            const float heightMin = -20f;

            if (mouseMoving)
            {
                eyeAngle  += 0.005f * (mouseBeginX - e.X);
                eyeHeight += 0.03f  * (e.Y - mouseBeginY);
                if (eyeHeight > heightMax) eyeHeight = heightMax;
                if (eyeHeight < heightMin) eyeHeight = heightMin;
                mouseBeginX = e.X;
                mouseBeginY = e.Y;
            }
            base.OnMouseMove(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(75.0f), e.Width / (float)e.Height, 0.1f, 100.0f);
            GL.LoadMatrix(ref proj);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteTexture(normalMapTexture);
            GL.DeleteTexture(normalizeCubeTexture);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyContext(context);
        }
    }
}
