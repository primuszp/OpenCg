using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Annotation : CgObject
    {
        private readonly CgAnnotation handle;

        internal Annotation(CgAnnotation handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsAnnotation(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg annotation.", "handle");
            }

            this.handle = handle;
        }

        public CgAnnotation Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public int DependentParameterCount
        {
            get { return Cg.GetNumDependentAnnotationParameters(Handle); }
        }

        public string Name
        {
            get { return Cg.GetAnnotationName(Handle); }
        }

        public Annotation NextAnnotation
        {
            get { return Wrap(Cg.GetNextAnnotation(Handle), false); }
        }

        public CgType Type
        {
            get { return Cg.GetAnnotationType(Handle); }
        }

        public bool[] GetBoolValues()
        {
            int count;
            var values = Cg.GetBoolAnnotationValues(Handle, out count);
            return values == null ? Array.Empty<bool>() : values.Select(value => (bool)value).ToArray();
        }

        public Parameter GetDependentParameter(int index)
        {
            return Parameter.Wrap(Cg.GetDependentAnnotationParameter(Handle, index), false);
        }

        public IEnumerable<Parameter> GetDependentParameters()
        {
            for (int i = 0; i < DependentParameterCount; i++)
            {
                yield return GetDependentParameter(i);
            }
        }

        public float[] GetFloatValues()
        {
            int count;
            return Cg.GetFloatAnnotationValues(Handle, out count) ?? Array.Empty<float>();
        }

        public int[] GetIntValues()
        {
            int count;
            return Cg.GetIntAnnotationValues(Handle, out count) ?? Array.Empty<int>();
        }

        public string GetStringValue()
        {
            return Cg.GetStringAnnotationValue(Handle);
        }

        public string[] GetStringValues()
        {
            int count;
            return Cg.GetStringAnnotationValues(Handle, out count) ?? Array.Empty<string>();
        }

        public bool Set(bool value)
        {
            return Cg.SetBoolAnnotation(Handle, value ? Cg.True : Cg.False);
        }

        public bool Set(float value)
        {
            return Cg.SetFloatAnnotation(Handle, value);
        }

        public bool Set(int value)
        {
            return Cg.SetIntAnnotation(Handle, value);
        }

        public bool Set(string value)
        {
            return Cg.SetStringAnnotation(Handle, value);
        }

        internal static Annotation Wrap(CgAnnotation handle, bool ownsHandle)
        {
            return Cg.IsAnnotation(handle) ? new Annotation(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
        }
    }
}
