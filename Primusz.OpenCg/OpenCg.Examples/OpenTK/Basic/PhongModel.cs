using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using OpenCg.Graphics;
using OpenCg.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace OpenCg.Examples.OpenTK.Basic
{
    class PhongModel : BaseExample
    {
        private const string VertexProgramFileName = "Data\\Shaders\\phong_model_vert.cg";
        private const string FragmentProgramFileName = "Data\\Shaders\\phong_model_frag.cg";
        private const string ModelFileName = "Data\\Models\\blaze.obj";
        private const string TextureFileName = "Data\\Images\\blaze.png";

        private CgProfile vertexProfile = CgProfile.Unknown;
        private CgProfile fragmentProfile = CgProfile.Unknown;
        private CgProgram vertexProgram;
        private CgProgram fragmentProgram;
        private CgParameter modelViewProjParameter;
        private CgParameter modelViewParameter;
        private CgParameter modelViewInvTParameter;
        private CgParameter diffuseMapParameter;
        private CgParameter lightPositionParameter;
        private CgParameter ambientColorParameter;
        private CgParameter diffuseColorParameter;
        private CgParameter specularColorParameter;
        private CgParameter shininessParameter;
        private int textureId;
        private ObjMesh mesh;
        private double elapsedTime;

        public PhongModel()
            : base("OpenCg Phong Model", 900, 700)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Normalize);
            GL.ClearColor(0.08f, 0.09f, 0.11f, 1.0f);

            context = Cg.CreateContext();
            Cg.SetErrorCallback(errorDelegate);

            vertexProfile = ChooseProfile(CgGLEnum.Vertex, CgProfile.Arbvp1, CgProfile.Vp30);
            fragmentProfile = ChooseProfile(CgGLEnum.Fragment, CgProfile.Arbfp1, CgProfile.Fp30);

            vertexProgram = CreateAndLoadProgram(VertexProgramFileName, vertexProfile);
            fragmentProgram = CreateAndLoadProgram(FragmentProgramFileName, fragmentProfile);

            modelViewProjParameter = Cg.GetNamedParameter(vertexProgram, "modelViewProj");
            modelViewParameter = Cg.GetNamedParameter(vertexProgram, "modelView");
            modelViewInvTParameter = Cg.GetNamedParameter(vertexProgram, "modelViewInvT");
            diffuseMapParameter = Cg.GetNamedParameter(fragmentProgram, "diffuseMap");
            lightPositionParameter = Cg.GetNamedParameter(fragmentProgram, "lightPosition");
            ambientColorParameter = Cg.GetNamedParameter(fragmentProgram, "ambientColor");
            diffuseColorParameter = Cg.GetNamedParameter(fragmentProgram, "diffuseColor");
            specularColorParameter = Cg.GetNamedParameter(fragmentProgram, "specularColor");
            shininessParameter = Cg.GetNamedParameter(fragmentProgram, "shininess");

            textureId = LoadTexture(TextureFileName);
            CgGL.SetTextureParameter(diffuseMapParameter, textureId);

            mesh = ObjMesh.Load(ModelFileName);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if (textureId != 0)
            {
                GL.DeleteTexture(textureId);
            }

            Cg.DestroyProgram(fragmentProgram);
            Cg.DestroyProgram(vertexProgram);
            Cg.DestroyContext(context);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (IsKeyDown(Keys.Escape))
            {
                Close();
                return;
            }

            elapsedTime += e.Time;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float[] projectionMatrix = new float[16];
            float[] viewMatrix = new float[16];
            float[] scaleMatrix = new float[16];
            float[] orientMatrix = new float[16];
            float[] spinMatrix = new float[16];
            float[] tempMatrix = new float[16];
            float[] modelMatrix = new float[16];
            float[] modelViewMatrix = new float[16];
            float[] modelViewProjectionMatrix = new float[16];
            float[] modelViewInverseMatrix = new float[16];
            float[] modelViewInverseTransposeMatrix = new float[16];
            float[] lightPosition = new float[4];

            BuildPerspectiveMatrix(45.0f, ClientSize.X / (double)Math.Max(1, ClientSize.Y), 0.1, 100.0, projectionMatrix);
            BuildLookAtMatrix(0.0, 1.25, 4.5, 0.0, 0.55, 0.0, 0.0, 1.0, 0.0, viewMatrix);
            MakeScaleMatrix(2.1f, scaleMatrix);
            MakeRotateMatrix(90.0f, 1.0f, 0.0f, 0.0f, orientMatrix);
            MakeRotateMatrix((float)(elapsedTime * 51.5), 0.0f, 1.0f, 0.0f, spinMatrix);
            MultMatrix(tempMatrix, orientMatrix, scaleMatrix);
            MultMatrix(modelMatrix, spinMatrix, tempMatrix);
            MultMatrix(modelViewMatrix, viewMatrix, modelMatrix);
            MultMatrix(modelViewProjectionMatrix, projectionMatrix, modelViewMatrix);
            InvertMatrix(modelViewInverseMatrix, modelViewMatrix);
            TransposeMatrix(modelViewInverseTransposeMatrix, modelViewInverseMatrix);
            TransformPoint(lightPosition, viewMatrix, 2.7f, 3.6f, 2.4f);

            CgGL.BindProgram(vertexProgram);
            CgGL.EnableProfile(vertexProfile);
            CgGL.BindProgram(fragmentProgram);
            CgGL.EnableProfile(fragmentProfile);

            Cg.SetMatrixParameterfr(modelViewProjParameter, modelViewProjectionMatrix);
            Cg.SetMatrixParameterfr(modelViewParameter, modelViewMatrix);
            Cg.SetMatrixParameterfr(modelViewInvTParameter, modelViewInverseTransposeMatrix);
            Cg.SetParameter3f(lightPositionParameter, lightPosition[0], lightPosition[1], lightPosition[2]);
            Cg.SetParameter3f(ambientColorParameter, 0.18f, 0.18f, 0.20f);
            Cg.SetParameter3f(diffuseColorParameter, 0.90f, 0.86f, 0.78f);
            Cg.SetParameter3f(specularColorParameter, 0.80f, 0.76f, 0.68f);
            Cg.SetParameter1f(shininessParameter, 48.0f);

            CgGL.EnableTextureParameter(diffuseMapParameter);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            DrawMesh(mesh);
            CgGL.DisableTextureParameter(diffuseMapParameter);

            CgGL.DisableProfile(fragmentProfile);
            CgGL.DisableProfile(vertexProfile);

            SwapBuffers();
        }

        private CgProfile ChooseProfile(CgGLEnum domain, CgProfile preferredProfile, CgProfile fallbackProfile)
        {
            if (CgGL.IsProfileSupported(preferredProfile))
            {
                CgGL.SetOptimalOptions(preferredProfile);
                return preferredProfile;
            }

            if (CgGL.IsProfileSupported(fallbackProfile))
            {
                CgGL.SetOptimalOptions(fallbackProfile);
                return fallbackProfile;
            }

            CgProfile latest = CgGL.GetLatestProfile(domain);
            if (latest != CgProfile.Unknown && CgGL.IsProfileSupported(latest))
            {
                CgGL.SetOptimalOptions(latest);
                return latest;
            }

            throw new InvalidOperationException("No supported Cg OpenGL profile is available.");
        }

        private CgProgram CreateAndLoadProgram(string fileName, CgProfile profile)
        {
            string[] args = CgGL.GetOptimalOptions(profile);
            CgProgram program = Cg.CreateProgramFromFile(context, CgEnum.Source, fileName, profile, "main", args);

            if (!Cg.IsProgramCompiled(program))
            {
                Cg.CompileProgram(program);
            }

            CgGL.LoadProgram(program);
            return program;
        }

        private static void MakeScaleMatrix(float scale, float[] matrix)
        {
            matrix[0] = scale;
            matrix[1] = 0.0f;
            matrix[2] = 0.0f;
            matrix[3] = 0.0f;
            matrix[4] = 0.0f;
            matrix[5] = scale;
            matrix[6] = 0.0f;
            matrix[7] = 0.0f;
            matrix[8] = 0.0f;
            matrix[9] = 0.0f;
            matrix[10] = scale;
            matrix[11] = 0.0f;
            matrix[12] = 0.0f;
            matrix[13] = 0.0f;
            matrix[14] = 0.0f;
            matrix[15] = 1.0f;
        }

        private static void TransposeMatrix(float[] output, float[] matrix)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    output[row * 4 + column] = matrix[column * 4 + row];
                }
            }
        }

        private static void TransformPoint(float[] output, float[] matrix, float x, float y, float z)
        {
            output[0] = matrix[0] * x + matrix[1] * y + matrix[2] * z + matrix[3];
            output[1] = matrix[4] * x + matrix[5] * y + matrix[6] * z + matrix[7];
            output[2] = matrix[8] * x + matrix[9] * y + matrix[10] * z + matrix[11];
            output[3] = 1.0f;
        }

        private static int LoadTexture(string fileName)
        {
            using Bitmap image = new Bitmap(fileName);
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            BitmapData data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                int id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                return id;
            }
            finally
            {
                image.UnlockBits(data);
            }
        }

        private static void DrawMesh(ObjMesh mesh)
        {
            GL.Begin(BeginMode.Triangles);
            foreach (ObjVertex vertex in mesh.Vertices)
            {
                GL.TexCoord2(vertex.TexCoord);
                GL.Normal3(vertex.Normal);
                GL.Vertex3(vertex.Position);
            }
            GL.End();
        }

        private struct ObjVertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TexCoord;
        }

        private sealed class ObjMesh
        {
            private readonly List<ObjVertex> vertices = new List<ObjVertex>();

            public IList<ObjVertex> Vertices
            {
                get { return vertices; }
            }

            public static ObjMesh Load(string fileName)
            {
                var positions = new List<Vector3>();
                var normals = new List<Vector3>();
                var texCoords = new List<Vector2>();
                var mesh = new ObjMesh();
                var culture = CultureInfo.InvariantCulture;

                foreach (string rawLine in File.ReadLines(fileName))
                {
                    string line = rawLine.Trim();
                    if (line.Length == 0 || line[0] == '#')
                    {
                        continue;
                    }

                    string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0)
                    {
                        continue;
                    }

                    if (parts[0] == "v" && parts.Length >= 4)
                    {
                        positions.Add(new Vector3(
                            float.Parse(parts[1], culture),
                            float.Parse(parts[2], culture),
                            float.Parse(parts[3], culture)));
                    }
                    else if (parts[0] == "vn" && parts.Length >= 4)
                    {
                        normals.Add(Vector3.Normalize(new Vector3(
                            float.Parse(parts[1], culture),
                            float.Parse(parts[2], culture),
                            float.Parse(parts[3], culture))));
                    }
                    else if (parts[0] == "vt" && parts.Length >= 3)
                    {
                        texCoords.Add(new Vector2(
                            float.Parse(parts[1], culture),
                            float.Parse(parts[2], culture)));
                    }
                    else if (parts[0] == "f" && parts.Length >= 4)
                    {
                        for (int i = 2; i < parts.Length - 1; i++)
                        {
                            mesh.vertices.Add(ParseVertex(parts[1], positions, texCoords, normals));
                            mesh.vertices.Add(ParseVertex(parts[i], positions, texCoords, normals));
                            mesh.vertices.Add(ParseVertex(parts[i + 1], positions, texCoords, normals));
                        }
                    }
                }

                return mesh;
            }

            private static ObjVertex ParseVertex(string token, IList<Vector3> positions, IList<Vector2> texCoords, IList<Vector3> normals)
            {
                string[] indices = token.Split('/');
                int positionIndex = ParseIndex(indices[0], positions.Count);
                int texCoordIndex = indices.Length > 1 && indices[1].Length > 0 ? ParseIndex(indices[1], texCoords.Count) : -1;
                int normalIndex = indices.Length > 2 && indices[2].Length > 0 ? ParseIndex(indices[2], normals.Count) : -1;

                return new ObjVertex
                {
                    Position = positions[positionIndex],
                    TexCoord = texCoordIndex >= 0 ? texCoords[texCoordIndex] : Vector2.Zero,
                    Normal = normalIndex >= 0 ? normals[normalIndex] : Vector3.UnitZ
                };
            }

            private static int ParseIndex(string value, int count)
            {
                int index = int.Parse(value, CultureInfo.InvariantCulture);
                return index < 0 ? count + index : index - 1;
            }
        }
    }
}
