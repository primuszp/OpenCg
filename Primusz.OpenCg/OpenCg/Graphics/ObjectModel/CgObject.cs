using System;
using System.Collections.Generic;

namespace OpenCg.Graphics.ObjectModel
{
    public abstract class CgObject : IDisposable
    {
        private bool disposed;

        protected CgObject(bool ownsHandle)
        {
            OwnsHandle = ownsHandle;
        }

        ~CgObject()
        {
            Dispose(false);
        }

        public bool OwnsHandle { get; private set; }

        public bool IsDisposed
        {
            get { return disposed; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected static IEnumerable<T> Enumerate<T>(Func<T> first, Func<T, T> next)
            where T : CgObject
        {
            for (T current = first(); current != null; current = next(current))
            {
                yield return current;
            }
        }

        protected void MarkBorrowed()
        {
            OwnsHandle = false;
        }

        protected void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        protected abstract void ReleaseHandle();

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (OwnsHandle)
            {
                ReleaseHandle();
            }

            disposed = true;
        }
    }
}
