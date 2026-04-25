using System;

namespace OpenCg.Graphics
{
    public sealed class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(CgError error)
        {
            Error = error;
            ErrorString = Cg.GetErrorString(error);
        }

        public CgError Error { get; private set; }

        public string ErrorString { get; private set; }
    }
}
