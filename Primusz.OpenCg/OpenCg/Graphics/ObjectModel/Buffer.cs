using System;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Buffer : CgObject
    {
        private readonly CgBuffer handle;

        internal Buffer(CgBuffer handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsBuffer(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg buffer.", "handle");
            }

            this.handle = handle;
        }

        public CgBuffer Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public int Size
        {
            get { return Cg.GetBufferSize(Handle); }
        }

        public static Buffer Create(Context context, int size, IntPtr data, CgBufferUsage usage)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new Buffer(Cg.CreateBuffer(context.Handle, size, data, usage), true);
        }

        public IntPtr Map(CgBufferAccess access)
        {
            return Cg.MapBuffer(Handle, access);
        }

        public void SetData(int size, IntPtr data)
        {
            Cg.SetBufferData(Handle, size, data);
        }

        public void SetSubData(int offset, int size, IntPtr data)
        {
            Cg.SetBufferSubData(Handle, offset, size, data);
        }

        public void Unmap()
        {
            Cg.UnmapBuffer(Handle);
        }

        protected override void ReleaseHandle()
        {
            if (Cg.IsBuffer(handle))
            {
                Cg.DestroyBuffer(handle);
            }
        }
    }
}
