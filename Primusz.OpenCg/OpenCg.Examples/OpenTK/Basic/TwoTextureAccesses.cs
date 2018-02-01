using System;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

#region Original Credits / License

// 07_two_texture_accesses.c - OpenGL-based example using a Cg
//   vertex and a Cg fragment programs from Chapter 3 of "The Cg Tutorial" (Addison-Wesley, ISBN 0321194969).

#endregion

#region Porting Credits

// Ported from C to C# by Tobias Bohnen for the CgNet v1.0 Copyright (c) 2010.
// Modified it by Péter Primusz for the OpenCg v1.0.1 Copyright (c) 2011.

#endregion Porting Credits

namespace OpenCg.Examples.OpenTK.Basic
{
    class TwoTextureAccesses : BaseExample
    {
        #region Members

        private const string VertexProgramFileName = "..\\..\\Data\\Shaders\\C3E5v_twoTextures.cg";
        private const string FragmentProgramFileName = "..\\..\\Data\\Shaders\\C3E6f_twoTextures.cg";
        private const string CgVertexEntryFuncName = "C3E5v_twoTextures";
        private const string CgFragmentEntryFuncName = "C3E6f_twoTextures";

        private CgProfile cgVertexProfile = CgProfile.Unknown;
        private CgProfile cgFragmentProfile = CgProfile.Unknown;

        private CgProgram cgVertexProgram;
        private CgProgram cgFragmentProgram;
        private CgParameter fragmentParamDecal;
        private CgParameter vertexParamLeftSeparation, vertexParamRightSeparation;
       
        private float separation = 0.1f, separationVelocity = 0.005f;

        #endregion

        public TwoTextureAccesses()
            : base("Cg Tutorial 07: Two Texture Accesses", 400, 400)
        { }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (separation > 0.4f)
                separationVelocity = -0.005f;
            else
            {
                if (separation < -0.4f)
                    separationVelocity = 0.005f;
            }

            separation += separationVelocity;

            if (Keyboard[Key.Escape])
            {
                Exit();   
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Reshape();
            Display();
        }

        protected override void OnLoad(EventArgs e)
        {
            /* Tightly packed texture data. */
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 666);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 128, 128, 0,
                          PixelFormat.Rgb, PixelType.UnsignedByte, ImageDemon.Array);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            context = Cg.CreateContext();

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

            cgVertexProgram = Cg.CreateProgramFromFile(
               context,                  // Cg runtime context
               CgEnum.Source,            // Program in human-readable form
               VertexProgramFileName,    // Name of file containing program
               cgVertexProfile,          // Profile: OpenGL ARB vertex program
               CgVertexEntryFuncName,    // Entry function name
               vArgs);                   // Extra compiler options

            CgGL.LoadProgram(cgVertexProgram);

            vertexParamLeftSeparation = Cg.GetNamedParameter(cgVertexProgram, "leftSeparation");
            vertexParamRightSeparation = Cg.GetNamedParameter(cgVertexProgram, "rightSeparation");

            cgFragmentProfile = CgGL.GetLatestProfile(CgGLEnum.Fragment);
            string[] fArgs = CgGL.GetOptimalOptions(cgVertexProfile);

            if (cgFragmentProfile != CgProfile.Unknown)
            {
                if (CgGL.IsProfileSupported(cgFragmentProfile))
                {
                    CgGL.SetOptimalOptions(cgFragmentProfile);
                }
            }

            cgFragmentProgram = Cg.CreateProgramFromFile(
               context,                   // Cg runtime context */
               CgEnum.Source,             // Program in human-readable form */
               FragmentProgramFileName,   // Name of file containing program */
               cgFragmentProfile,         // Profile: OpenGL ARB vertex program */
               CgFragmentEntryFuncName,   // Entry function name */
               fArgs);                    // Extra compiler options */

            CgGL.LoadProgram(cgFragmentProgram);

            fragmentParamDecal = Cg.GetNamedParameter(cgFragmentProgram, "decal");
            CgGL.SetTextureParameter(fragmentParamDecal, 666);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Cg.DestroyProgram(cgFragmentProgram);
            Cg.DestroyProgram(cgVertexProgram);
            Cg.DestroyContext(context);
            Environment.Exit(0);
        }

        private void Reshape()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Viewport(0, 0, Width, Height);
            GL.Ortho(0, 0, Width, Height, -1, +1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Display()
        {
            GL.ClearColor(0.1f, 0.3f, 0.6f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (separation > 0)
            {
                /* Separate in the horizontal direction. */
                Cg.SetParameter2f(vertexParamLeftSeparation, -separation, 0);
                Cg.SetParameter2f(vertexParamRightSeparation, separation, 0);
            }
            else
            {
                /* Separate in the vertical direction. */
                Cg.SetParameter2f(vertexParamLeftSeparation, 0, -separation);
                Cg.SetParameter2f(vertexParamRightSeparation, 0, separation);
            }

            CgGL.BindProgram(cgVertexProgram);
            CgGL.EnableProfile(cgVertexProfile);

            CgGL.BindProgram(cgFragmentProgram);
            CgGL.EnableProfile(cgFragmentProfile);

            CgGL.EnableTextureParameter(fragmentParamDecal);

            GL.Begin(PrimitiveType.Triangles);
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

            CgGL.DisableTextureParameter(fragmentParamDecal);

            SwapBuffers();
        }
    }
}
