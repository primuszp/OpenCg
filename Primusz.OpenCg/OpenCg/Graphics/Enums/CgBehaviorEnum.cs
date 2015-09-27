using System;

namespace OpenCg.Graphics
{
    [Serializable]
    public enum CgBehavior
    {
        Unknown = 0,
        /// <summary>
        /// Latest behavior supported at runtime. 
        /// </summary>
        Latest = 1,
        Behavior2200 = 1000,
        /// <summary>
        /// Default behavior.
        /// </summary>
        Behavior3000 = 2000,
        Behavior3100 = 3000,
        /// <summary>
        /// Latest behavior supported at compile time.
        /// </summary>
        Current = Behavior3100
    }
}