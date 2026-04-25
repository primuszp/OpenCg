using System;
using System.Collections.Generic;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Program : CgObject
    {
        private readonly CgProgram handle;

        internal Program(CgProgram handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsProgram(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg program.", "handle");
            }

            this.handle = handle;
        }

        public CgProgram Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public Context Context
        {
            get { return new Context(Cg.GetProgramContext(Handle), false); }
        }

        public IEnumerable<Annotation> Annotations
        {
            get { return Enumerate(() => FirstAnnotation, annotation => annotation.NextAnnotation); }
        }

        public Annotation FirstAnnotation
        {
            get { return Annotation.Wrap(Cg.GetFirstProgramAnnotation(Handle), false); }
        }

        public bool IsCompiled
        {
            get { return Cg.IsProgramCompiled(Handle); }
        }

        public Program NextProgram
        {
            get { return Wrap(Cg.GetNextProgram(Handle), false); }
        }

        public CgProfile Profile
        {
            get { return Cg.GetProgramProfile(Handle); }
            set { Cg.SetProgramProfile(Handle, value); }
        }

        public string[] Options
        {
            get { return Cg.GetProgramOptions(Handle); }
        }

        public int UserTypeCount
        {
            get { return Cg.GetNumUserTypes(Handle); }
        }

        public static Program Combine(params Program[] programs)
        {
            if (programs == null)
            {
                throw new ArgumentNullException("programs");
            }

            var handles = new CgProgram[programs.Length];
            for (int i = 0; i < programs.Length; i++)
            {
                if (programs[i] == null)
                {
                    throw new ArgumentNullException("programs");
                }

                handles[i] = programs[i].Handle;
            }

            return Wrap(Cg.CombinePrograms(handles.Length, handles), true);
        }

        public Program CombineWith(Program other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            return Wrap(Cg.CombinePrograms2(Handle, other.Handle), true);
        }

        public void Compile()
        {
            Cg.CompileProgram(Handle);
        }

        public Program Copy()
        {
            return Wrap(Cg.CopyProgram(Handle), true);
        }

        public Annotation CreateAnnotation(string name, CgType type)
        {
            return Annotation.Wrap(Cg.CreateProgramAnnotation(Handle, name, type), true);
        }

        public float[] Evaluate(int componentCount, int x, int y, int z)
        {
            var values = new float[componentCount * x * y * z];
            Cg.EvaluateProgram(Handle, values, componentCount, x, y, z);
            return values;
        }

        public Parameter GetFirstParameter(CgEnum nameSpace)
        {
            return Parameter.Wrap(Cg.GetFirstParameter(Handle, nameSpace), false);
        }

        public IEnumerable<Parameter> GetParameters(CgEnum nameSpace)
        {
            return Enumerate(() => GetFirstParameter(nameSpace), parameter => parameter.NextParameter);
        }

        public Parameter GetNamedParameter(string name)
        {
            return Parameter.Wrap(Cg.GetNamedParameter(Handle, name), false);
        }

        public Annotation GetNamedAnnotation(string name)
        {
            return Annotation.Wrap(Cg.GetNamedProgramAnnotation(Handle, name), false);
        }

        public CgType GetNamedUserType(string name)
        {
            return Cg.GetNamedUserType(Handle, name);
        }

        public CgType GetUserType(int index)
        {
            return Cg.GetUserType(Handle, index);
        }

        public string GetSource(CgEnum sourceType)
        {
            return Cg.GetProgramString(Handle, sourceType);
        }

        public void UpdateParameters()
        {
            Cg.UpdateProgramParameters(Handle);
        }

        internal static Program Wrap(CgProgram handle, bool ownsHandle)
        {
            return Cg.IsProgram(handle) ? new Program(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
            if (Cg.IsProgram(handle))
            {
                Cg.DestroyProgram(handle);
            }
        }
    }
}
