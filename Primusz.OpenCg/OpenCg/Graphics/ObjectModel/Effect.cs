using System;
using System.Collections.Generic;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Effect : CgObject
    {
        private readonly CgEffect handle;

        internal Effect(CgEffect handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsEffect(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg effect.", "handle");
            }

            this.handle = handle;
        }

        public CgEffect Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public Context Context
        {
            get { return new Context(Cg.GetEffectContext(Handle), false); }
        }

        public IEnumerable<Annotation> Annotations
        {
            get { return Enumerate(() => FirstAnnotation, annotation => annotation.NextAnnotation); }
        }

        public Annotation FirstAnnotation
        {
            get { return Annotation.Wrap(Cg.GetFirstEffectAnnotation(Handle), false); }
        }

        public Parameter FirstParameter
        {
            get { return Parameter.Wrap(Cg.GetFirstEffectParameter(Handle), false); }
        }

        public Technique FirstTechnique
        {
            get { return Technique.Wrap(Cg.GetFirstTechnique(Handle), false); }
        }

        public Effect NextEffect
        {
            get { return Wrap(Cg.GetNextEffect(Handle), false); }
        }

        public IEnumerable<Parameter> Parameters
        {
            get { return Enumerate(() => FirstParameter, parameter => parameter.NextParameter); }
        }

        public IEnumerable<Technique> Techniques
        {
            get { return Enumerate(() => FirstTechnique, technique => technique.NextTechnique); }
        }

        public int UserTypeCount
        {
            get { return Cg.GetNumUserTypes(Handle); }
        }

        public string Name
        {
            get { return Cg.GetEffectName(Handle); }
            set { Cg.SetEffectName(Handle, value); }
        }

        public Effect Copy()
        {
            return Wrap(Cg.CopyEffect(Handle), true);
        }

        public Annotation CreateAnnotation(string name, CgType type)
        {
            return Annotation.Wrap(Cg.CreateEffectAnnotation(Handle, name, type), true);
        }

        public Parameter CreateParameter(string name, CgType type)
        {
            return Parameter.Wrap(Cg.CreateEffectParameter(Handle, name, type), true);
        }

        public Technique CreateTechnique(string name)
        {
            return Technique.Wrap(Cg.CreateTechnique(Handle, name), true);
        }

        public Program CreateProgram(CgProfile profile, string entry, params string[] args)
        {
            return Program.Wrap(Cg.CreateProgramFromEffect(Handle, profile, entry, args), true);
        }

        public Annotation GetNamedAnnotation(string name)
        {
            return Annotation.Wrap(Cg.GetNamedEffectAnnotation(Handle, name), false);
        }

        public Parameter GetParameterBySemantic(string semantic)
        {
            return Parameter.Wrap(Cg.GetEffectParameterBySemantic(Handle, semantic), false);
        }

        public Technique GetNamedTechnique(string name)
        {
            return Technique.Wrap(Cg.GetNamedTechnique(Handle, name), false);
        }

        public CgType GetNamedUserType(string name)
        {
            return Cg.GetNamedUserType(Handle, name);
        }

        public CgType GetUserType(int index)
        {
            return Cg.GetUserType(Handle, index);
        }

        internal static Effect Wrap(CgEffect handle, bool ownsHandle)
        {
            return Cg.IsEffect(handle) ? new Effect(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
            if (Cg.IsEffect(handle))
            {
                Cg.DestroyEffect(handle);
            }
        }
    }
}
