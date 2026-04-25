using System;
using OpenCg.Graphics.OpenGL;

namespace OpenCg.Graphics.ObjectModel.OpenGL
{
    public static class ParameterExtensions
    {
        public static void DisableClientState(this Parameter parameter)
        {
            CgGL.DisableClientState(parameter.Handle);
        }

        public static void DisableTexture(this Parameter parameter)
        {
            CgGL.DisableTextureParameter(parameter.Handle);
        }

        public static void EnableClientState(this Parameter parameter)
        {
            CgGL.EnableClientState(parameter.Handle);
        }

        public static void EnableTexture(this Parameter parameter)
        {
            CgGL.EnableTextureParameter(parameter.Handle);
        }

        public static void Get(this Parameter parameter, float[] values)
        {
            CgGL.GetParameter(parameter.Handle, values);
        }

        public static void Get(this Parameter parameter, double[] values)
        {
            CgGL.GetParameter(parameter.Handle, values);
        }

        public static void GetArray(this Parameter parameter, int offset, int elementCount, float[] values)
        {
            CgGL.GetParameterArray(parameter.Handle, offset, elementCount, values);
        }

        public static void GetArray(this Parameter parameter, int offset, int elementCount, double[] values)
        {
            CgGL.GetParameterArray(parameter.Handle, offset, elementCount, values);
        }

        public static int GetTextureEnum(this Parameter parameter)
        {
            return CgGL.GetTextureEnum(parameter.Handle);
        }

        public static int GetTextureParameter(this Parameter parameter)
        {
            return CgGL.GetTextureParameter(parameter.Handle);
        }

        public static void GetMatrix(this Parameter parameter, float[] matrix, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.GetMatrixParameterfc(parameter.Handle, matrix);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.GetMatrixParameterfr(parameter.Handle, matrix);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void GetMatrix(this Parameter parameter, double[] matrix, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.GetMatrixParameterdc(parameter.Handle, matrix);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.GetMatrixParameterdr(parameter.Handle, matrix);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void GetMatrixArray(this Parameter parameter, int offset, int elementCount, float[] matrices, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.GetMatrixParameterArrayfc(parameter.Handle, offset, elementCount, matrices);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.GetMatrixParameterArrayfr(parameter.Handle, offset, elementCount, matrices);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void GetMatrixArray(this Parameter parameter, int offset, int elementCount, double[] matrices, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.GetMatrixParameterArraydc(parameter.Handle, offset, elementCount, matrices);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.GetMatrixParameterArraydr(parameter.Handle, offset, elementCount, matrices);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void Set(this Parameter parameter, float x)
        {
            CgGL.SetParameter(parameter.Handle, x);
        }

        public static void Set(this Parameter parameter, float x, float y)
        {
            CgGL.SetParameter(parameter.Handle, x, y);
        }

        public static void Set(this Parameter parameter, float x, float y, float z)
        {
            CgGL.SetParameter(parameter.Handle, x, y, z);
        }

        public static void Set(this Parameter parameter, float x, float y, float z, float w)
        {
            CgGL.SetParameter(parameter.Handle, x, y, z, w);
        }

        public static void Set(this Parameter parameter, float[] values)
        {
            CgGL.SetParameter(parameter.Handle, values);
        }

        public static void Set(this Parameter parameter, double x)
        {
            CgGL.SetParameter(parameter.Handle, x);
        }

        public static void Set(this Parameter parameter, double x, double y)
        {
            CgGL.SetParameter(parameter.Handle, x, y);
        }

        public static void Set(this Parameter parameter, double x, double y, double z)
        {
            CgGL.SetParameter(parameter.Handle, x, y, z);
        }

        public static void Set(this Parameter parameter, double x, double y, double z, double w)
        {
            CgGL.SetParameter(parameter.Handle, x, y, z, w);
        }

        public static void Set(this Parameter parameter, double[] values)
        {
            CgGL.SetParameter(parameter.Handle, values);
        }

        public static void SetArray(this Parameter parameter, int offset, int elementCount, float[] values)
        {
            CgGL.SetParameterArray(parameter.Handle, offset, elementCount, values);
        }

        public static void SetArray(this Parameter parameter, int offset, int elementCount, double[] values)
        {
            CgGL.SetParameterArray(parameter.Handle, offset, elementCount, values);
        }

        public static void SetMatrix(this Parameter parameter, float[] matrix, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.SetMatrixParameterfc(parameter.Handle, matrix);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.SetMatrixParameterfr(parameter.Handle, matrix);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void SetMatrix(this Parameter parameter, double[] matrix, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.SetMatrixParameterdc(parameter.Handle, matrix);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.SetMatrixParameterdr(parameter.Handle, matrix);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void SetMatrixArray(this Parameter parameter, int offset, int elementCount, float[] matrices, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.SetMatrixParameterArrayfc(parameter.Handle, offset, elementCount, matrices);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.SetMatrixParameterArrayfr(parameter.Handle, offset, elementCount, matrices);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void SetMatrixArray(this Parameter parameter, int offset, int elementCount, double[] matrices, CgEnum order)
        {
            if (order == CgEnum.ColumnMajor)
            {
                CgGL.SetMatrixParameterArraydc(parameter.Handle, offset, elementCount, matrices);
            }
            else if (order == CgEnum.RowMajor)
            {
                CgGL.SetMatrixParameterArraydr(parameter.Handle, offset, elementCount, matrices);
            }
            else
            {
                throw new ArgumentException("Expected RowMajor or ColumnMajor.", "order");
            }
        }

        public static void SetPointer(this Parameter parameter, int componentCount, int type, int stride, IntPtr pointer)
        {
            CgGL.SetParameterPointer(parameter.Handle, componentCount, type, stride, pointer);
        }

        public static void SetStateMatrix(this Parameter parameter, MatrixType matrix, MatrixTransform transform)
        {
            CgGL.SetStateMatrixParameter(parameter.Handle, (int)matrix, (int)transform);
        }

        public static void SetTexture(this Parameter parameter, int textureObject)
        {
            CgGL.SetTextureParameter(parameter.Handle, textureObject);
        }

        public static void SetupSampler(this Parameter parameter, int textureObject)
        {
            CgGL.SetupSampler(parameter.Handle, textureObject);
        }
    }
}
