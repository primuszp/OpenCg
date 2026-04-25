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

#region Original Credits / License

// 05_texture_sampling.c - OpenGL-based example using a Cg
//   vertex and a Cg fragment programs from Chapter 3 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).

#endregion

#region Porting Credits

// Ported from C to C# by Tobias Bohnen for the CgNet v1.0 Copyright (c) 2010.
// Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2011.

#endregion Porting Credits

namespace OpenCg.ObjectModel.Examples.OpenTK.Basic
{
    [ExampleAttribute("OpenTK/Basic/[05] Texture Sampling")]
    class TextureSampling : BaseExample
    {
        #region Members

        private const string VertexProgramFileName = "Data\\Shaders\\C3E2v_varying.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\C3E3f_texture.cg";
        private const string CgVertexEntryFuncName = "C3E2v_varying";
        private const string CgFragmentEntryFuncName = "C3E3f_texture";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgramObject cgVertexProgram;
        private CgProgramObject cgFragmentProgram;
        private CgParameterObject fragmentParamDecal;

        #endregion

        public TextureSampling()
            : base("Cg Tutorial 05: Texture Sampling", 400, 400)
        { }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            Reshape();
            Display();
        }

        protected override void OnLoad()
        {
            /* Tightly packed texture data. */
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 666);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 128, 128, 0,
                          PixelFormat.Rgb, PixelType.UnsignedByte, ImageDemon.Array);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            context = OpenCg.Graphics.ObjectModel.Context.Create();

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

            cgVertexProgram = context.CreateProgramFromFile(                  // Cg runtime context
               CgEnum.Source,            // Program in human-readable form
               VertexProgramFileName,    // Name of file containing program
               cgVertexProfile,          // Profile: OpenGL ARB vertex program
               CgVertexEntryFuncName,    // Entry function name
               vArgs);                   // Extra compiler options

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgVertexProgram);

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgFragmentProfile);

            if (cgFragmentProfile != CgProfile.Unknown)
            {
                if (CgGL.IsProfileSupported(cgFragmentProfile))
                {
                    CgGL.SetOptimalOptions(cgFragmentProfile);
                }
            }

            cgFragmentProgram = context.CreateProgramFromFile(                   // Cg runtime context */
               CgEnum.Source,             // Program in human-readable form */
               FragmentProgramFileName,   // Name of file containing program */
               cgFragmentProfile,         // Profile: OpenGL ARB vertex program */
               CgFragmentEntryFuncName,   // Entry function name */
               fArgs);                    // Extra compiler options */

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Load(cgFragmentProgram);

            fragmentParamDecal = cgFragmentProgram.GetNamedParameter("decal");
            SetTextureParameter(fragmentParamDecal, 666);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            DisposeProgram(cgFragmentProgram);
            DisposeProgram(cgVertexProgram);
            context?.Dispose();
        }

        private void Reshape()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            GL.Ortho(0, 0, ClientSize.X, ClientSize.Y, -1, +1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Display()
        {
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            OpenCg.Graphics.ObjectModel.OpenGL.ProgramExtensions.Bind(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            EnableTextureParameter(fragmentParamDecal);

            GL.Begin(BeginMode.Triangles);
            {
                GL.TexCoord2(0, 0);
                GL.Vertex2(-0.8f, 0.8f);

                GL.TexCoord2(1, 0);
                GL.Vertex2(0.8f, 0.8f);

                GL.TexCoord2(0.5f, 1);
                GL.Vertex2(0.0f, -0.8f);
            }
            GL.End();

            CgGL.DisableProfile(cgVertexProfile);
            CgGL.DisableProfile(cgFragmentProfile);

            DisableTextureParameter(fragmentParamDecal);

            SwapBuffers();
        }
    }
}