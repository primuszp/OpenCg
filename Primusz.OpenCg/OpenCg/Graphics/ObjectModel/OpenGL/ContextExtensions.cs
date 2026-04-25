using OpenCg.Graphics.OpenGL;

namespace OpenCg.Graphics.ObjectModel.OpenGL
{
    public static class ContextExtensions
    {
        public static Buffer CreateGLBuffer<T>(this Context context, int size, T[] data, int bufferUsage)
            where T : struct
        {
            return new Buffer(CgGL.CreateBuffer(context.Handle, size, data, bufferUsage), true);
        }

        public static Buffer CreateGLBuffer<T>(this Context context, int size, T[,] data, int bufferUsage)
            where T : struct
        {
            return new Buffer(CgGL.CreateBuffer(context.Handle, size, data, bufferUsage), true);
        }

        public static Buffer CreateGLBuffer<T>(this Context context, int size, T[,,] data, int bufferUsage)
            where T : struct
        {
            return new Buffer(CgGL.CreateBuffer(context.Handle, size, data, bufferUsage), true);
        }

        public static Buffer CreateGLBuffer<T>(this Context context, int size, ref T data, int bufferUsage)
            where T : struct
        {
            return new Buffer(CgGL.CreateBuffer(context.Handle, size, ref data, bufferUsage), true);
        }

        public static bool GetManageTextureParameters(this Context context)
        {
            return CgGL.GetManageTextureParameters(context.Handle);
        }

        public static void RegisterStates(this Context context)
        {
            CgGL.RegisterStates(context.Handle);
        }

        public static void SetManageTextureParameters(this Context context, bool flag)
        {
            CgGL.SetManageTextureParameters(context.Handle, flag);
        }
    }
}
