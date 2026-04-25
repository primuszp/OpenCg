using System;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class State : CgObject
    {
        private readonly CgState handle;

        internal State(CgState handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsState(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg state.", "handle");
            }

            this.handle = handle;
        }

        public CgState Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public Context Context
        {
            get { return new Context(Cg.GetStateContext(Handle), false); }
        }

        public int EnumerantCount
        {
            get { return Cg.GetNumStateEnumerants(Handle); }
        }

        public CgProfile LatestProfile
        {
            get { return Cg.GetStateLatestProfile(Handle); }
            set { Cg.SetStateLatestProfile(Handle, value); }
        }

        public string Name
        {
            get { return Cg.GetStateName(Handle); }
        }

        public State NextState
        {
            get { return Wrap(Cg.GetNextState(Handle), false); }
        }

        public Cg.StateCallbackDelegate ResetCallback
        {
            get { return Cg.GetStateResetCallback(Handle); }
        }

        public Cg.StateCallbackDelegate SetCallback
        {
            get { return Cg.GetStateSetCallback(Handle); }
        }

        public CgType Type
        {
            get { return Cg.GetStateType(Handle); }
        }

        public Cg.StateCallbackDelegate ValidateCallback
        {
            get { return Cg.GetStateValidateCallback(Handle); }
        }

        public void AddEnumerant(string name, int value)
        {
            Cg.AddStateEnumerant(Handle, name, value);
        }

        public string GetEnumerant(int index, out int value)
        {
            return Cg.GetStateEnumerant(Handle, index, out value);
        }

        public string GetEnumerantName(int value)
        {
            return Cg.GetStateEnumerantName(Handle, value);
        }

        public int GetEnumerantValue(string name)
        {
            return Cg.GetStateEnumerantValue(Handle, name);
        }

        public void SetCallbacks(Cg.StateCallbackDelegate set, Cg.StateCallbackDelegate reset, Cg.StateCallbackDelegate validate)
        {
            Cg.SetStateCallbacks(Handle, set, reset, validate);
        }

        internal static State Wrap(CgState handle, bool ownsHandle)
        {
            return Cg.IsState(handle) ? new State(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
        }
    }
}
