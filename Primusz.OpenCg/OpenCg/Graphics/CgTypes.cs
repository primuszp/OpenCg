using System;
using System.Runtime.InteropServices;

namespace OpenCg.Graphics
{
    /// <summary>
    /// Represent a Cg bool.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgBool
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private Int32 Data;

        public CgBool(int boolean)
        {
            Data = boolean;
        }

        public static implicit operator CgBool(int boolean)
        {
            return new CgBool(boolean);
        }

        public static implicit operator bool(CgBool boolean)
        {
            return boolean.Data == 1;
        }

        public static CgBool operator ==(CgBool me, CgBool other)
        {
            return me.Data == other.Data ? (Cg.True) : (Cg.False);
        }

        public static CgBool operator !=(CgBool me, CgBool other)
        {
            return me.Data != other.Data ? (Cg.True) : (Cg.False);
        }

        public bool Equals(CgBool other)
        {
            return (Equals(this, other));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CgBool))
            {
                return false;
            }

            return (Equals(this, (CgBool)obj));
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }

    /// <summary>
    /// Represent a Cg context.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgContext
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg handle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgHandle
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg program.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgProgram
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgEffect
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg technique object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgTechnique
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg Buffer object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgBuffer
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg parameter object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgParameter
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgObj
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg pass object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgPass
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg state object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgState
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg stateassignment object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgStateAssignment
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg annotation object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CgAnnotation
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }
}