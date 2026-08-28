using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using Pfim;

namespace OpenCg.ObjectModel.Examples
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
        private const int FOURCC_DXT3     = 0x33545844;
        private const int FOURCC_DXT5     = 0x35545844;
        private const int DDSCAPS2_CUBEMAP = 0x00000200;

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

            if (fileBytes.Length < DDS_HEADER_SIZE ||
                fileBytes[0] != (byte)'D' || fileBytes[1] != (byte)'D' ||
                fileBytes[2] != (byte)'S' || fileBytes[3] != (byte)' ')
            {
                throw new InvalidDataException("DDSLoader: the file does not contain a valid DDS header.");
            }

            int height = BitConverter.ToInt32(fileBytes, 12);
            int width  = BitConverter.ToInt32(fileBytes, 16);
            int mipMapCount = Math.Max(1, BitConverter.ToInt32(fileBytes, 28));
            int fourCC = BitConverter.ToInt32(fileBytes, 84);
            int caps2 = BitConverter.ToInt32(fileBytes, 112);

            if (width <= 0 || height <= 0 || (caps2 & DDSCAPS2_CUBEMAP) == 0)
            {
                throw new InvalidDataException("DDSLoader: the file is not a valid cubemap DDS texture.");
            }

            if (mipMapCount > GetMaximumMipCount(width, height))
            {
                throw new InvalidDataException("DDSLoader: the cubemap declares too many mip levels.");
            }

            // Bytes per 4x4 compressed block: DXT1 = 8, DXT3/DXT5 = 16
            int blockBytes;
            switch (fourCC)
            {
                case FOURCC_DXT1:
                    blockBytes = 8;
                    break;
                case FOURCC_DXT3:
                case FOURCC_DXT5:
                    blockBytes = 16;
                    break;
                default:
                    throw new InvalidDataException("DDSLoader: only DXT1, DXT3, and DXT5 cubemaps are supported.");
            }

            int faceDataSize = Math.Max(1, (width + 3) / 4) * Math.Max(1, (height + 3) / 4) * blockBytes;
            int faceMipChainSize = GetMipChainSize(width, height, mipMapCount, blockBytes);

            if (fileBytes.Length < DDS_HEADER_SIZE + 6L * faceMipChainSize)
            {
                throw new InvalidDataException("DDSLoader: the cubemap data is truncated.");
            }

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
                // A DDS cubemap stores the full mip chain for one face before the next face.
                int srcOffset = DDS_HEADER_SIZE + i * faceMipChainSize;
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
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
            return tex;
        }

        private static int GetMipChainSize(int width, int height, int mipMapCount, int blockBytes)
        {
            int size = 0;
            for (int level = 0; level < mipMapCount; level++)
            {
                size += Math.Max(1, (width + 3) / 4) * Math.Max(1, (height + 3) / 4) * blockBytes;
                width = Math.Max(1, width / 2);
                height = Math.Max(1, height / 2);
            }

            return size;
        }

        private static int GetMaximumMipCount(int width, int height)
        {
            int count = 1;
            while (width > 1 || height > 1)
            {
                width = Math.Max(1, width / 2);
                height = Math.Max(1, height / 2);
                count++;
            }

            return count;
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
