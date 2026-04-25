using System;
using System.Collections.Generic;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Pass : CgObject
    {
        private readonly CgPass handle;

        internal Pass(CgPass handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsPass(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg pass.", "handle");
            }

            this.handle = handle;
        }

        public CgPass Handle
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

        public Annotation FirstAnnotation
        {
            get { return Annotation.Wrap(Cg.GetFirstPassAnnotation(Handle), false); }
        }

        public StateAssignment FirstStateAssignment
        {
            get { return StateAssignment.Wrap(Cg.GetFirstStateAssignment(Handle), false); }
        }

        public string Name
        {
            get { return Cg.GetPassName(Handle); }
        }

        public Pass NextPass
        {
            get { return Wrap(Cg.GetNextPass(Handle), false); }
        }

        public IEnumerable<StateAssignment> StateAssignments
        {
            get { return Enumerate(() => FirstStateAssignment, assignment => assignment.NextStateAssignment); }
        }

        public Technique Technique
        {
            get { return Technique.Wrap(Cg.GetPassTechnique(Handle), false); }
        }

        public Annotation CreateAnnotation(string name, CgType type)
        {
            return Annotation.Wrap(Cg.CreatePassAnnotation(Handle, name, type), true);
        }

        public StateAssignment CreateStateAssignment(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            return StateAssignment.Wrap(Cg.CreateStateAssignment(Handle, state.Handle), true);
        }

        public StateAssignment CreateStateAssignment(State state, int index)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            return StateAssignment.Wrap(Cg.CreateStateAssignmentIndex(Handle, state.Handle, index), true);
        }

        public Annotation GetNamedAnnotation(string name)
        {
            return Annotation.Wrap(Cg.GetNamedPassAnnotation(Handle, name), false);
        }

        public Program GetProgram(CgDomain domain)
        {
            return Program.Wrap(Cg.GetPassProgram(Handle, domain), false);
        }

        public StateAssignment GetNamedStateAssignment(string name)
        {
            return StateAssignment.Wrap(Cg.GetNamedStateAssignment(Handle, name), false);
        }

        public void ResetState()
        {
            Cg.ResetPassState(Handle);
        }

        public void SetState()
        {
            Cg.SetPassState(Handle);
        }

        public void UpdateParameters()
        {
            Cg.UpdatePassParameters(Handle);
        }

        internal static Pass Wrap(CgPass handle, bool ownsHandle)
        {
            return Cg.IsPass(handle) ? new Pass(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
        }
    }
}
