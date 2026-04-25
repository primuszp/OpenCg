using System;

namespace OpenCg.ObjectModel.Examples
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class ExampleAttribute : Attribute
    {
        public ExampleAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}
