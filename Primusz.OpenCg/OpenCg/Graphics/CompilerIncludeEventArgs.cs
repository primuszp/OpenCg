using System;

namespace OpenCg.Graphics
{
    public sealed class CompilerIncludeEventArgs : EventArgs
    {
        public CompilerIncludeEventArgs(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
    }
}
