using System;

namespace OpenCg.Graphics
{
    [Serializable]
    public enum CgBufferUsage
    {
        StreamDraw = 0,
        StreamRead = 1,
        StreamCopy = 2,
        StaticDraw = 3,
        StaticRead = 4,
        StaticCopy = 5,
        DynamicDraw = 6,
        DynamicRead = 7,
        DynamicCopy = 8
    }
}
