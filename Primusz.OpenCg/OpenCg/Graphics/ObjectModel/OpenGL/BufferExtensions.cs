using OpenCg.Graphics.OpenGL;

namespace OpenCg.Graphics.ObjectModel.OpenGL
{
    public static class BufferExtensions
    {
        public static int GetBufferObject(this Buffer buffer)
        {
            return CgGL.GetBufferObject(buffer.Handle);
        }
    }
}
