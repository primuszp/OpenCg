using System;
using System.Collections.Generic;

namespace OpenCg.Graphics.ObjectModel
{
    public sealed class Obj : CgObject
    {
        private readonly CgObj handle;

        internal Obj(CgObj handle, bool ownsHandle)
            : base(ownsHandle)
        {
            if (handle.IsNull)
            {
                throw new ArgumentException("The supplied handle is not a valid Cg object.", "handle");
            }

            this.handle = handle;
        }

        public CgObj Handle
        {
            get
            {
                ThrowIfDisposed();
                return handle;
            }
        }

        public int UserTypeCount
        {
            get { throw new NotSupportedException("Cg user type queries are only supported for programs and effects by the Cg runtime."); }
        }

        public IEnumerable<CgType> UserTypes
        {
            get
            {
                for (int i = 0; i < UserTypeCount; i++)
                {
                    yield return GetUserType(i);
                }
            }
        }

        public static Obj Create(Context context, CgEnum programType, string source, CgProfile profile, params string[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new Obj(Cg.CreateObj(context.Handle, programType, source, profile, args), true);
        }

        public static Obj CreateFromFile(Context context, CgEnum programType, string sourceFile, CgProfile profile, params string[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new Obj(Cg.CreateObjFromFile(context.Handle, programType, sourceFile, profile, args), true);
        }

        public CgType GetNamedUserType(string name)
        {
            throw new NotSupportedException("Cg user type queries are only supported for programs and effects by the Cg runtime.");
        }

        public CgType GetUserType(int index)
        {
            throw new NotSupportedException("Cg user type queries are only supported for programs and effects by the Cg runtime.");
        }

        protected override void ReleaseHandle()
        {
            Cg.DestroyObj(handle);
        }
    }
}
