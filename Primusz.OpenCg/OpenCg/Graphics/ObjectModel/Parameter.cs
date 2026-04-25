using System;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Parameter : CgObject
    {
        private readonly CgParameter handle;

        internal Parameter(CgParameter handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsParameter(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg parameter.", "handle");
            }

            this.handle = handle;
        }

        public CgParameter Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public bool IsGlobal
        {
            get { return Cg.IsParameterGlobal(Handle); }
        }

        public System.Collections.Generic.IEnumerable<Annotation> Annotations
        {
            get { return Enumerate(() => FirstAnnotation, annotation => annotation.NextAnnotation); }
        }

        public Annotation FirstAnnotation
        {
            get { return Annotation.Wrap(Cg.GetFirstParameterAnnotation(Handle), false); }
        }

        public bool IsReferenced
        {
            get { return Cg.IsParameterReferenced(Handle); }
        }

        public string Name
        {
            get { return Cg.GetParameterName(Handle); }
        }

        public Parameter NextParameter
        {
            get { return Wrap(Cg.GetNextParameter(Handle), false); }
        }

        public Program Program
        {
            get { return Program.Wrap(Cg.GetParameterProgram(Handle), false); }
        }

        public string Semantic
        {
            get { return Cg.GetParameterSemantic(Handle); }
            set { Cg.SetParameterSemantic(Handle, value); }
        }

        public CgType Type
        {
            get { return Cg.GetParameterType(Handle); }
        }

        public Annotation CreateAnnotation(string name, CgType type)
        {
            return Annotation.Wrap(Cg.CreateParameterAnnotation(Handle, name, type), true);
        }

        public void ConnectTo(Parameter target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            Cg.ConnectParameter(Handle, target.Handle);
        }

        public void Disconnect()
        {
            Cg.DisconnectParameter(Handle);
        }

        public Annotation GetNamedAnnotation(string name)
        {
            return Annotation.Wrap(Cg.GetNamedParameterAnnotation(Handle, name), false);
        }

        public StateAssignment GetFirstSamplerStateAssignment()
        {
            return StateAssignment.Wrap(Cg.GetFirstSamplerStateAssignment(Handle), false);
        }

        public StateAssignment GetNamedSamplerStateAssignment(string name)
        {
            return StateAssignment.Wrap(Cg.GetNamedSamplerStateAssignment(Handle, name), false);
        }

        public StateAssignment CreateSamplerStateAssignment(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            return StateAssignment.Wrap(Cg.CreateSamplerStateAssignment(Handle, state.Handle), true);
        }

        public void Set(float x)
        {
            Cg.SetParameter1f(Handle, x);
        }

        public void Set(float x, float y)
        {
            Cg.SetParameter2f(Handle, x, y);
        }

        public void Set(float x, float y, float z)
        {
            Cg.SetParameter3f(Handle, x, y, z);
        }

        public void Set(float x, float y, float z, float w)
        {
            Cg.SetParameter4f(Handle, x, y, z, w);
        }

        public void Set(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            switch (values.Length)
            {
                case 1:
                    Cg.SetParameter1fv(Handle, values);
                    break;
                case 2:
                    Cg.SetParameter2fv(Handle, values);
                    break;
                case 3:
                    Cg.SetParameter3fv(Handle, values);
                    break;
                case 4:
                    Cg.SetParameter4fv(Handle, values);
                    break;
                default:
                    throw new ArgumentException("Expected 1 to 4 values.", "values");
            }
        }

        public void Set(int x)
        {
            Cg.SetParameter1i(Handle, x);
        }

        public void Set(int x, int y)
        {
            Cg.SetParameter2i(Handle, x, y);
        }

        public void Set(int x, int y, int z)
        {
            Cg.SetParameter3i(Handle, x, y, z);
        }

        public void Set(int x, int y, int z, int w)
        {
            Cg.SetParameter4i(Handle, x, y, z, w);
        }

        public void Set(int[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            switch (values.Length)
            {
                case 1:
                    Cg.SetParameter1iv(Handle, values);
                    break;
                case 2:
                    Cg.SetParameter2iv(Handle, values);
                    break;
                case 3:
                    Cg.SetParameter3iv(Handle, values);
                    break;
                case 4:
                    Cg.SetParameter4iv(Handle, values);
                    break;
                default:
                    throw new ArgumentException("Expected 1 to 4 values.", "values");
            }
        }

        public void SetMatrix(float[] matrix, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                Cg.SetMatrixParameterfc(Handle, matrix);
            }
            else if (order == CgEnum.RowMajor)
            {
                Cg.SetMatrixParameterfr(Handle, matrix);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public float[] GetFloatValues(int elementCount, CgEnum order)
        {
            var values = new float[elementCount];
            if (order == CgEnum.ColumnMajor)
            {
                Cg.GetParameterValuefc(Handle, elementCount, values);
            }
            else if (order == CgEnum.RowMajor)
            {
                Cg.GetParameterValuefr(Handle, elementCount, values);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }

            return values;
        }

        internal static Parameter Wrap(CgParameter handle, bool ownsHandle)
        {
            return Cg.IsParameter(handle) ? new Parameter(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
            if (Cg.IsParameter(handle))
            {
                Cg.DestroyParameter(handle);
            }
        }
    }
}
