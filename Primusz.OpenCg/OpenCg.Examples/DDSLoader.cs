using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using Pfim;

namespace OpenCg.Examples
{
    static class DDSLoader
    {
        // DDS header layout (all offsets from byte 0):
        //   0  DWORD magic        = "DDS "
        //   4  DDS_HEADER (124 bytes):
        //  12    dwHeight
        //  16    dwWidth
        //  28    dwMipMapCount
        //  84    pixelformat.dwFourCC   ("DXT1" = 0x31545844)
        // 108    dwCaps
        // 112    dwCaps2
        // Total header = 128 bytes
        private const int DDS_HEADER_SIZE = 128;
        private const int FOURCC_DXT1     = 0x31545844;

        public static int LoadTexture2D(string path)
        {
            using IImage image = Pfimage.FromFile(path);
            if (image.Compressed)
                image.Decompress();

            GetFormats(image.Format, out var internalFormat, out var pixelFormat);

            int tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat,
                image.Width, image.Height, 0, pixelFormat, PixelType.UnsignedByte, image.Data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return tex;
        }

        public static int LoadTextureCubeMap(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);

            int height = BitConverter.ToInt32(fileBytes, 12);
            int width  = BitConverter.ToInt32(fileBytes, 16);
            int fourCC = BitConverter.ToInt32(fileBytes, 84);

            // Bytes per 4x4 compressed block: DXT1 = 8, DXT3/DXT5 = 16
            int blockBytes  = (fourCC == FOURCC_DXT1) ? 8 : 16;
            int faceDataSize = Math.Max(1, (width + 3) / 4) * Math.Max(1, (height + 3) / 4) * blockBytes;

            // Build a reusable single-face DDS buffer: original header + face data.
            // Clear cubemap (dwCaps2 @ 112) and mipmap (dwMipMapCount @ 28) fields
            // so Pfim treats it as a plain 2D texture.
            byte[] faceDds = new byte[DDS_HEADER_SIZE + faceDataSize];
            Array.Copy(fileBytes, 0, faceDds, 0, DDS_HEADER_SIZE);
            BitConverter.GetBytes(0).CopyTo(faceDds, 28);   // dwMipMapCount = 0
            BitConverter.GetBytes(0).CopyTo(faceDds, 112);  // dwCaps2 = 0 (no cubemap)

            int tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, tex);

            for (int i = 0; i < 6; i++)
            {
                int srcOffset = DDS_HEADER_SIZE + i * faceDataSize;
                Array.Copy(fileBytes, srcOffset, faceDds, DDS_HEADER_SIZE, faceDataSize);

                using var ms = new MemoryStream(faceDds, 0, faceDds.Length, false, true);
                using IImage face = Dds.Create(ms, new PfimConfig());
                if (face.Compressed)
                    face.Decompress();

                GetFormats(face.Format, out var internalFormat, out var pixelFormat);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, internalFormat,
                    face.Width, face.Height, 0, pixelFormat, PixelType.UnsignedByte, face.Data);
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            return tex;
        }

        private static void GetFormats(ImageFormat format,
            out PixelInternalFormat internalFormat, out PixelFormat pixelFormat)
        {
            switch (format)
            {
                case ImageFormat.Rgba32:
                    internalFormat = PixelInternalFormat.Rgba8;
                    pixelFormat = PixelFormat.Bgra;
                    break;
                case ImageFormat.Rgb24:
                    internalFormat = PixelInternalFormat.Rgb8;
                    pixelFormat = PixelFormat.Bgr;
                    break;
                default:
                    throw new Exception($"DDSLoader: unsupported pixel format: {format}");
            }
        }
    }
}
