using System;
using System.Collections.Generic;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Technique : CgObject
    {
        private readonly CgTechnique handle;

        internal Technique(CgTechnique handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsTechnique(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg technique.", "handle");
            }

            this.handle = handle;
        }

        public CgTechnique Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public IEnumerable<Annotation> Annotations
        {
            get { return Enumerate(() => FirstAnnotation, annotation => annotation.NextAnnotation); }
        }

        public Effect Effect
        {
            get { return Effect.Wrap(Cg.GetTechniqueEffect(Handle), false); }
        }

        public Annotation FirstAnnotation
        {
            get { return Annotation.Wrap(Cg.GetFirstTechniqueAnnotation(Handle), false); }
        }

        public Pass FirstPass
        {
            get { return Pass.Wrap(Cg.GetFirstPass(Handle), false); }
        }

        public bool IsValidated
        {
            get { return Cg.IsTechniqueValidated(Handle); }
        }

        public string Name
        {
            get { return Cg.GetTechniqueName(Handle); }
        }

        public Technique NextTechnique
        {
            get { return Wrap(Cg.GetNextTechnique(Handle), false); }
        }

        public IEnumerable<Pass> Passes
        {
            get { return Enumerate(() => FirstPass, pass => pass.NextPass); }
        }

        public Annotation CreateAnnotation(string name, CgType type)
        {
            return Annotation.Wrap(Cg.CreateTechniqueAnnotation(Handle, name, type), true);
        }

        public Pass CreatePass(string name)
        {
            return Pass.Wrap(Cg.CreatePass(Handle, name), true);
        }

        public Annotation GetNamedAnnotation(string name)
        {
            return Annotation.Wrap(Cg.GetNamedTechniqueAnnotation(Handle, name), false);
        }

        public Pass GetNamedPass(string name)
        {
            return Pass.Wrap(Cg.GetNamedPass(Handle, name), false);
        }

        public bool Validate()
        {
            return Cg.ValidateTechnique(Handle);
        }

        internal static Technique Wrap(CgTechnique handle, bool ownsHandle)
        {
            return Cg.IsTechnique(handle) ? new Technique(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
        }
    }
}
