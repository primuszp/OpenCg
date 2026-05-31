using System;
using System.Collections.Generic;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Context : CgObject
    {
        private readonly CgContext handle;
        private readonly object callbackSyncRoot = new object();
        private EventHandler<CompilerIncludeEventArgs> compilerInclude;
        private readonly Cg.IncludeCallbackFuncDelegate includeCallback;

        internal Context(CgContext handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (!Cg.IsContext(handle))
            {
                throw new ArgumentException("The supplied handle is not a valid Cg context.", "handle");
            }

            this.handle = handle;
            includeCallback = OnCompilerInclude;
        }

        public event EventHandler<CompilerIncludeEventArgs> CompilerInclude
        {
            add
            {
                lock (callbackSyncRoot)
                {
                    if (compilerInclude == null)
                    {
                        Cg.SetCompilerIncludeCallback(Handle, includeCallback);
                    }

                    compilerInclude += value;
                }
            }

            remove
            {
                lock (callbackSyncRoot)
                {
                    compilerInclude -= value;

                    if (compilerInclude == null)
                    {
                        Cg.SetCompilerIncludeCallback(Handle, null);
                    }
                }
            }
        }

        public CgContext Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public CgEnum AutoCompile
        {
            get { return Cg.GetAutoCompile(Handle); }
            set { Cg.SetAutoCompile(Handle, value); }
        }

        public CgBehavior Behavior
        {
            get { return Cg.GetContextBehavior(Handle); }
            set { Cg.SetContextBehavior(Handle, value); }
        }

        public IEnumerable<Effect> Effects
        {
            get { return Enumerate(() => Effect.Wrap(Cg.GetFirstEffect(Handle), false), effect => effect.NextEffect); }
        }

        public IEnumerable<Program> Programs
        {
            get { return Enumerate(() => Program.Wrap(Cg.GetFirstProgram(Handle), false), program => program.NextProgram); }
        }

        public IEnumerable<State> SamplerStates
        {
            get { return Enumerate(() => State.Wrap(Cg.GetFirstSamplerState(Handle), false), state => state.NextState); }
        }

        public IEnumerable<State> States
        {
            get { return Enumerate(() => State.Wrap(Cg.GetFirstState(Handle), false), state => state.NextState); }
        }

        public CgEnum ParameterSettingMode
        {
            get { return Cg.GetParameterSettingMode(Handle); }
            set { Cg.SetParameterSettingMode(Handle, value); }
        }

        public static Context Create()
        {
            return new Context(Cg.CreateContext(), true);
        }

        public Effect CreateEffect(string source, params string[] args)
        {
            return Effect.Wrap(Cg.CreateEffect(Handle, source, args), true);
        }

        public Effect CreateEffectFromFile(string fileName, params string[] args)
        {
            return Effect.Wrap(Cg.CreateEffectFromFile(Handle, fileName, args), true);
        }

        public Buffer CreateBuffer(int size, IntPtr data, CgBufferUsage usage)
        {
            return new Buffer(Cg.CreateBuffer(Handle, size, data, usage), true);
        }

        public Parameter CreateParameter(CgType type)
        {
            return Parameter.Wrap(Cg.CreateParameter(Handle, type), true);
        }

        public Parameter CreateParameterArray(CgType type, int length)
        {
            return Parameter.Wrap(Cg.CreateParameterArray(Handle, type, length), true);
        }

        public Parameter CreateParameterMultiDimArray(CgType type, int dimensions, int[] lengths)
        {
            return Parameter.Wrap(Cg.CreateParameterMultiDimArray(Handle, type, dimensions, lengths), true);
        }

        public Program CreateProgram(CgEnum programType, string source, CgProfile profile, string entry, params string[] args)
        {
            return Program.Wrap(Cg.CreateProgram(Handle, programType, source, profile, entry, args), true);
        }

        public Program CreateProgramFromFile(CgEnum programType, string fileName, CgProfile profile, string entry, params string[] args)
        {
            return Program.Wrap(Cg.CreateProgramFromFile(Handle, programType, fileName, profile, entry, args), true);
        }

        public Obj CreateObj(CgEnum programType, string source, CgProfile profile, params string[] args)
        {
            return new Obj(Cg.CreateObj(Handle, programType, source, profile, args), true);
        }

        public Obj CreateObjFromFile(CgEnum programType, string fileName, CgProfile profile, params string[] args)
        {
            return new Obj(Cg.CreateObjFromFile(Handle, programType, fileName, profile, args), true);
        }

        public State CreateSamplerState(string name, CgType type)
        {
            return State.Wrap(Cg.CreateSamplerState(Handle, name, type), true);
        }

        public State CreateSamplerStateArray(string name, CgType type, int elementCount)
        {
            return State.Wrap(Cg.CreateArraySamplerState(Handle, name, type, elementCount), true);
        }

        public State CreateState(string name, CgType type)
        {
            return State.Wrap(Cg.CreateState(Handle, name, type), true);
        }

        public State CreateStateArray(string name, CgType type, int elementCount)
        {
            return State.Wrap(Cg.CreateArrayState(Handle, name, type, elementCount), true);
        }

        public State GetNamedSamplerState(string name)
        {
            return State.Wrap(Cg.GetNamedSamplerState(Handle, name), false);
        }

        public State GetNamedState(string name)
        {
            return State.Wrap(Cg.GetNamedState(Handle, name), false);
        }

        public void SetCompilerIncludeFile(string name, string fileName)
        {
            Cg.SetCompilerIncludeFile(Handle, name, fileName);
        }

        public void SetCompilerIncludeString(string name, string source)
        {
            Cg.SetCompilerIncludeString(Handle, name, source);
        }

        protected override void ReleaseHandle()
        {
            if (Cg.IsContext(handle))
            {
                Cg.DestroyContext(handle);
            }
        }

        private void OnCompilerInclude(CgContext context, string fileName)
        {
            EventHandler<CompilerIncludeEventArgs> handler;
            lock (callbackSyncRoot)
            {
                handler = compilerInclude;
            }

            if (handler != null && context.Equals(handle))
            {
                handler(this, new CompilerIncludeEventArgs(fileName));
            }
        }
    }
}
