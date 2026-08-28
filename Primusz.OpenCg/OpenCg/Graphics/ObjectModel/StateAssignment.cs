using System;
using System.Linq;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class StateAssignment : CgObject
    {
        private readonly CgStateAssignment handle;

        internal StateAssignment(CgStateAssignment handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsStateAssignment(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg state assignment.", "handle");
            }

            this.handle = handle;
        }

        public CgStateAssignment Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public Parameter ConnectedParameter
        {
            get { return Parameter.Wrap(Cg.GetConnectedStateAssignmentParameter(Handle), false); }
        }

        public int DependentParameterCount
        {
            get { return Cg.GetNumDependentStateAssignmentParameters(Handle); }
        }

        public int DependentProgramArrayParameterCount
        {
            get { return Cg.GetNumDependentProgramArrayStateAssignmentParameters(Handle); }
        }

        public int Index
        {
            get { return Cg.GetStateAssignmentIndex(Handle); }
        }

        public StateAssignment NextStateAssignment
        {
            get { return Wrap(Cg.GetNextStateAssignment(Handle), false); }
        }

        public Pass Pass
        {
            get { return Pass.Wrap(Cg.GetStateAssignmentPass(Handle), false); }
        }

        public State State
        {
            get { return State.Wrap(Cg.GetStateAssignmentState(Handle), false); }
        }

        public bool CallResetCallback()
        {
            return Cg.CallStateResetCallback(Handle);
        }

        public bool CallSetCallback()
        {
            return Cg.CallStateSetCallback(Handle);
        }

        public bool CallValidateCallback()
        {
            return Cg.CallStateValidateCallback(Handle);
        }

        public bool[] GetBoolValues()
        {
            int[] count = new int[1];
            var values = Cg.GetBoolStateAssignmentValues(Handle, count);
            return values == null ? Array.Empty<bool>() : values.Select(value => (bool)value).ToArray();
        }

        public Parameter GetDependentParameter(int index)
        {
            return Parameter.Wrap(Cg.GetDependentStateAssignmentParameter(Handle, index), false);
        }

        public Parameter GetDependentProgramArrayParameter(int index)
        {
            return Parameter.Wrap(Cg.GetDependentProgramArrayStateAssignmentParameter(Handle, index), false);
        }

        public float[] GetFloatValues()
        {
            return Cg.GetFloatStateAssignmentValues(Handle, new int[1]) ?? Array.Empty<float>();
        }

        public int[] GetIntValues()
        {
            return Cg.GetIntStateAssignmentValues(Handle, new int[1]) ?? Array.Empty<int>();
        }

        public Program GetProgramValue()
        {
            return Program.Wrap(Cg.GetProgramStateAssignmentValue(Handle), false);
        }

        public Parameter GetSamplerParameter()
        {
            return Parameter.Wrap(Cg.GetSamplerStateAssignmentParameter(Handle), false);
        }

        public State GetSamplerState()
        {
            return State.Wrap(Cg.GetSamplerStateAssignmentState(Handle), false);
        }

        public Parameter GetSamplerValue()
        {
            return Parameter.Wrap(Cg.GetSamplerStateAssignmentValue(Handle), false);
        }

        public string GetStringValue()
        {
            return Cg.GetStringStateAssignmentValue(Handle);
        }

        public Parameter GetTextureValue()
        {
            return Parameter.Wrap(Cg.GetTextureStateAssignmentValue(Handle), false);
        }

        public bool Set(bool value)
        {
            return Cg.SetBoolStateAssignment(Handle, value ? Cg.True : Cg.False);
        }

        public bool Set(bool[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return Cg.SetBoolArrayStateAssignment(Handle, values.Select(value => value ? Cg.True : Cg.False).ToArray());
        }

        public bool Set(float value)
        {
            return Cg.SetFloatStateAssignment(Handle, value);
        }

        public bool Set(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return Cg.SetFloatArrayStateAssignment(Handle, values);
        }

        public bool Set(int value)
        {
            return Cg.SetIntStateAssignment(Handle, value);
        }

        public bool Set(int[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return Cg.SetIntArrayStateAssignment(Handle, values);
        }

        public bool Set(string value)
        {
            return Cg.SetStringStateAssignment(Handle, value);
        }

        public bool SetProgram(Program program)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program");
            }

            return Cg.SetProgramStateAssignment(Handle, program.Handle);
        }

        public bool SetSampler(Parameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            return Cg.SetSamplerStateAssignment(Handle, parameter.Handle);
        }

        public bool SetTexture(Parameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            return Cg.SetTextureStateAssignment(Handle, parameter.Handle);
        }

        internal static StateAssignment Wrap(CgStateAssignment handle, bool ownsHandle)
        {
            return Cg.IsStateAssignment(handle) ? new StateAssignment(handle, ownsHandle) : null;
        }

        protected override void ReleaseHandle()
        {
        }
    }
}
