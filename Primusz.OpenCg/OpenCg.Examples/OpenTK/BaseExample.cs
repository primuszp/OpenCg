﻿using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenCg.Graphics;

namespace OpenCg.Examples.OpenTK
{
    class BaseExample : GameWindow, IExample
    {
        #region Members

        protected CgContext context;
        protected Cg.ErrorCallbackFuncDelegate errorDelegate;

        protected const float Pi = 3.14159265358979323846f;

        #endregion

        protected BaseExample(string title, int width, int height)
            : base(width, height)
        {
            Title = title;
            Icon = new Icon(GetEmbeddedResourceStream("Resources/cg.ico"));
        }

        private void CgErrorDelegate()
        {
            CgError error = Cg.GetError();
            string errorString = Cg.GetErrorString(error);

            if (error != CgError.No)
            {
                if (error == CgError.Compiler)
                    Console.WriteLine("{0}", Cg.GetLastListing(context));

                if (error == CgError.InvalidParamHandle)
                    Console.WriteLine("Could not get Cg parameter!");

                Console.WriteLine("Error: {0}", errorString);
                Environment.Exit(0);
            }
        }

        #region IExample Members

        public void Start()
        {
            // Callback Delegates
            errorDelegate += CgErrorDelegate;
            Run(30.0, 0.0);
        }

        #endregion

        #region Protected Static Methods

        protected static void BuildLookAtMatrix(double eyex, double eyey, double eyez,
            double centerx, double centery, double centerz, double upx, double upy, double upz, float[] m)
        {
            double[] x = new double[3], y = new double[3], z = new double[3];

            /* Difference eye and center vectors to make Z vector. */

            z[0] = eyex - centerx;
            z[1] = eyey - centery;
            z[2] = eyez - centerz;

            /* Normalize Z. */

            double mag = Math.Sqrt(z[0] * z[0] + z[1] * z[1] + z[2] * z[2]);

            if (Math.Abs(mag) > double.Epsilon)
            {
                z[0] /= mag;
                z[1] /= mag;
                z[2] /= mag;
            }

            /* Up vector makes Y vector. */

            y[0] = upx;
            y[1] = upy;
            y[2] = upz;

            /* X vector = Y cross Z. */

            x[0] = y[1] * z[2] - y[2] * z[1];
            x[1] = -y[0] * z[2] + y[2] * z[0];
            x[2] = y[0] * z[1] - y[1] * z[0];

            /* Recompute Y = Z cross X. */

            y[0] = z[1] * x[2] - z[2] * x[1];
            y[1] = -z[0] * x[2] + z[2] * x[0];
            y[2] = z[0] * x[1] - z[1] * x[0];

            /* Normalize X. */

            mag = Math.Sqrt(x[0] * x[0] + x[1] * x[1] + x[2] * x[2]);

            if (Math.Abs(mag) > double.Epsilon)
            {
                x[0] /= mag;
                x[1] /= mag;
                x[2] /= mag;
            }

            /* Normalize Y. */

            mag = Math.Sqrt(y[0] * y[0] + y[1] * y[1] + y[2] * y[2]);

            if (Math.Abs(mag) > double.Epsilon)
            {
                y[0] /= mag;
                y[1] /= mag;
                y[2] /= mag;
            }

            /* Build resulting view matrix. */

            m[0 * 4 + 0] = (float)x[0];
            m[0 * 4 + 1] = (float)x[1];
            m[0 * 4 + 2] = (float)x[2];
            m[0 * 4 + 3] = (float)(-x[0] * eyex + -x[1] * eyey + -x[2] * eyez);

            m[1 * 4 + 0] = (float)y[0];
            m[1 * 4 + 1] = (float)y[1];
            m[1 * 4 + 2] = (float)y[2];
            m[1 * 4 + 3] = (float)(-y[0] * eyex + -y[1] * eyey + -y[2] * eyez);

            m[2 * 4 + 0] = (float)z[0];
            m[2 * 4 + 1] = (float)z[1];
            m[2 * 4 + 2] = (float)z[2];
            m[2 * 4 + 3] = (float)(-z[0] * eyex + -z[1] * eyey + -z[2] * eyez);

            m[3 * 4 + 0] = 0.0f;
            m[3 * 4 + 1] = 0.0f;
            m[3 * 4 + 2] = 0.0f;
            m[3 * 4 + 3] = 1.0f;
        }

        protected static void BuildPerspectiveMatrix(double fieldOfView,
            double aspectRatio, double zNear, double zFar, float[] m)
        {
            double radians = fieldOfView / 2.0 * Math.PI / 180.0;

            double deltaZ = zFar - zNear;
            double sine = Math.Sin(radians);

            /* Should be non-zero to avoid division by zero. */

            double cotangent = Math.Cos(radians) / sine;

            m[0 * 4 + 0] = (float)(cotangent / aspectRatio);
            m[0 * 4 + 1] = 0.0f;
            m[0 * 4 + 2] = 0.0f;
            m[0 * 4 + 3] = 0.0f;

            m[1 * 4 + 0] = 0.0f;
            m[1 * 4 + 1] = (float)cotangent;
            m[1 * 4 + 2] = 0.0f;
            m[1 * 4 + 3] = 0.0f;

            m[2 * 4 + 0] = 0.0f;
            m[2 * 4 + 1] = 0.0f;
            m[2 * 4 + 2] = (float)(-(zFar + zNear) / deltaZ);
            m[2 * 4 + 3] = (float)(-2 * zNear * zFar / deltaZ);

            m[3 * 4 + 0] = 0.0f;
            m[3 * 4 + 1] = 0.0f;
            m[3 * 4 + 2] = -1;
            m[3 * 4 + 3] = 0;
        }

        /// <summary>
        /// Invert a row-major (C-style) 4x4 matrix.
        /// </summary>
        protected static void InvertMatrix(float[] output, float[] m)
        {
            double[] r0 = new double[8], r1 = new double[8], r2 = new double[8], r3 = new double[8];

            r0[0] = MatGet(m, 0, 0);
            r0[1] = MatGet(m, 0, 1);
            r0[2] = MatGet(m, 0, 2);
            r0[3] = MatGet(m, 0, 3);
            r0[4] = 1.0;
            r0[5] = r0[6] = r0[7] = 0.0;

            r1[0] = MatGet(m, 1, 0);
            r1[1] = MatGet(m, 1, 1);
            r1[2] = MatGet(m, 1, 2);
            r1[3] = MatGet(m, 1, 3);
            r1[5] = 1.0;
            r1[4] = r1[6] = r1[7] = 0.0;

            r2[0] = MatGet(m, 2, 0);
            r2[1] = MatGet(m, 2, 1);
            r2[2] = MatGet(m, 2, 2);
            r2[3] = MatGet(m, 2, 3);
            r2[6] = 1.0;
            r2[4] = r2[5] = r2[7] = 0.0;

            r3[0] = MatGet(m, 3, 0);
            r3[1] = MatGet(m, 3, 1);
            r3[2] = MatGet(m, 3, 2);
            r3[3] = MatGet(m, 3, 3);
            r3[7] = 1.0;
            r3[4] = r3[5] = r3[6] = 0.0;

            /* Choose myPivot, or die. */

            if (Math.Abs(r3[0]) > Math.Abs(r2[0]))
                SwapRows(ref r3, ref r2);

            if (Math.Abs(r2[0]) > Math.Abs(r1[0]))
                SwapRows(ref r2, ref r1);

            if (Math.Abs(r1[0]) > Math.Abs(r0[0]))
                SwapRows(ref r1, ref r0);

            if (Math.Abs(r0[0]) < double.Epsilon)
            {
                Debug.Assert(false, "could not invert matrix");
            }

            /* Eliminate first variable. */

            var m1 = r1[0] / r0[0];
            var m2 = r2[0] / r0[0];
            var m3 = r3[0] / r0[0];
            var s = r0[1];

            r1[1] -= m1 * s;
            r2[1] -= m2 * s;
            r3[1] -= m3 * s;
            s = r0[2];
            r1[2] -= m1 * s;
            r2[2] -= m2 * s;
            r3[2] -= m3 * s;
            s = r0[3];
            r1[3] -= m1 * s;
            r2[3] -= m2 * s;
            r3[3] -= m3 * s;
            s = r0[4];
            if (Math.Abs(s) > double.Epsilon)
            {
                r1[4] -= m1 * s;
                r2[4] -= m2 * s;
                r3[4] -= m3 * s;
            }
            s = r0[5];
            if (Math.Abs(s) > double.Epsilon)
            {
                r1[5] -= m1 * s;
                r2[5] -= m2 * s;
                r3[5] -= m3 * s;
            }
            s = r0[6];
            if (Math.Abs(s) > double.Epsilon)
            {
                r1[6] -= m1 * s;
                r2[6] -= m2 * s;
                r3[6] -= m3 * s;
            }
            s = r0[7];
            if (Math.Abs(s) > double.Epsilon)
            {
                r1[7] -= m1 * s;
                r2[7] -= m2 * s;
                r3[7] -= m3 * s;
            }

            /* Choose myPivot, or die. */

            if (Math.Abs(r3[1]) > Math.Abs(r2[1]))
                SwapRows(ref r3, ref r2);
            if (Math.Abs(r2[1]) > Math.Abs(r1[1]))
                SwapRows(ref r2, ref r1);

            if (Math.Abs(r1[1]) < double.Epsilon)
            {
                Debug.Assert(false, "could not invert matrix");
            }

            /* Eliminate second variable. */
            m2 = r2[1] / r1[1];
            m3 = r3[1] / r1[1];
            r2[2] -= m2 * r1[2];
            r3[2] -= m3 * r1[2];
            r2[3] -= m2 * r1[3];
            r3[3] -= m3 * r1[3];
            s = r1[4];
            if (Math.Abs(s) > double.Epsilon)
            {
                r2[4] -= m2 * s;
                r3[4] -= m3 * s;
            }
            s = r1[5];
            if (Math.Abs(s) > double.Epsilon)
            {
                r2[5] -= m2 * s;
                r3[5] -= m3 * s;
            }
            s = r1[6];
            if (Math.Abs(s) > double.Epsilon)
            {
                r2[6] -= m2 * s;
                r3[6] -= m3 * s;
            }
            s = r1[7];
            if (Math.Abs(s) > double.Epsilon)
            {
                r2[7] -= m2 * s;
                r3[7] -= m3 * s;
            }

            /* Choose myPivot, or die. */
            if (Math.Abs(r3[2]) > Math.Abs(r2[2]))
                SwapRows(ref r3, ref r2);

            if (Math.Abs(r2[2]) < double.Epsilon)
            {
                Debug.Assert(false, "could not invert matrix");
            }

            /* Eliminate third variable. */
            m3 = r3[2] / r2[2];
            r3[3] -= m3 * r2[3];
            r3[4] -= m3 * r2[4];
            r3[5] -= m3 * r2[5];
            r3[6] -= m3 * r2[6];
            r3[7] -= m3 * r2[7];

            /* Last check. */
            if (Math.Abs(r3[3]) < double.Epsilon)
            {
                Debug.Assert(false, "could not invert matrix");
            }

            s = 1.0 / r3[3]; /* Now back substitute row 3. */
            r3[4] *= s;
            r3[5] *= s;
            r3[6] *= s;
            r3[7] *= s;

            m2 = r2[3]; /* Now back substitute row 2. */
            s = 1.0 / r2[2];
            r2[4] = s * (r2[4] - r3[4] * m2);
            r2[5] = s * (r2[5] - r3[5] * m2);
            r2[6] = s * (r2[6] - r3[6] * m2);
            r2[7] = s * (r2[7] - r3[7] * m2);
            m1 = r1[3];
            r1[4] -= r3[4] * m1;
            r1[5] -= r3[5] * m1;
            r1[6] -= r3[6] * m1;
            r1[7] -= r3[7] * m1;
            var m0 = r0[3];
            r0[4] -= r3[4] * m0;
            r0[5] -= r3[5] * m0;
            r0[6] -= r3[6] * m0;
            r0[7] -= r3[7] * m0;

            m1 = r1[2]; /* Now back substitute row 1. */
            s = 1.0 / r1[1];
            r1[4] = s * (r1[4] - r2[4] * m1);
            r1[5] = s * (r1[5] - r2[5] * m1);
            r1[6] = s * (r1[6] - r2[6] * m1);
            r1[7] = s * (r1[7] - r2[7] * m1);
            m0 = r0[2];
            r0[4] -= r2[4] * m0;
            r0[5] -= r2[5] * m0;
            r0[6] -= r2[6] * m0;
            r0[7] -= r2[7] * m0;

            m0 = r0[1]; /* Now back substitute row 0. */
            s = 1.0 / r0[0];
            r0[4] = s * (r0[4] - r1[4] * m0);
            r0[5] = s * (r0[5] - r1[5] * m0);
            r0[6] = s * (r0[6] - r1[6] * m0);
            r0[7] = s * (r0[7] - r1[7] * m0);

            MatSet(output, 0, 0, r0[4]);
            MatSet(output, 0, 1, r0[5]);
            MatSet(output, 0, 2, r0[6]);
            MatSet(output, 0, 3, r0[7]);
            MatSet(output, 1, 0, r1[4]);
            MatSet(output, 1, 1, r1[5]);
            MatSet(output, 1, 2, r1[6]);
            MatSet(output, 1, 3, r1[7]);
            MatSet(output, 2, 0, r2[4]);
            MatSet(output, 2, 1, r2[5]);
            MatSet(output, 2, 2, r2[6]);
            MatSet(output, 2, 3, r2[7]);
            MatSet(output, 3, 0, r3[4]);
            MatSet(output, 3, 1, r3[5]);
            MatSet(output, 3, 2, r3[6]);
            MatSet(output, 3, 3, r3[7]);
        }

        protected static void MakeRotateMatrix(float angle,
            float ax, float ay, float az, float[] m)
        {
            float[] axis = new float[3];

            axis[0] = ax;
            axis[1] = ay;
            axis[2] = az;
            float mag = (float)Math.Sqrt(axis[0] * axis[0] + axis[1] * axis[1] + axis[2] * axis[2]);

            if (Math.Abs(mag) > double.Epsilon)
            {
                axis[0] /= mag;
                axis[1] /= mag;
                axis[2] /= mag;
            }

            float radians = (angle * Pi / 180.0f);
            float sine = (float)Math.Sin(radians);
            float cosine = (float)Math.Cos(radians);
            float ab = axis[0] * axis[1] * (1 - cosine);
            float bc = axis[1] * axis[2] * (1 - cosine);
            float ca = axis[2] * axis[0] * (1 - cosine);
            float tx = axis[0] * axis[0];
            float ty = axis[1] * axis[1];
            float tz = axis[2] * axis[2];

            m[0] = tx + cosine * (1 - tx);
            m[1] = ab + axis[2] * sine;
            m[2] = ca - axis[1] * sine;
            m[3] = 0.0f;
            m[4] = ab - axis[2] * sine;
            m[5] = ty + cosine * (1 - ty);
            m[6] = bc + axis[0] * sine;
            m[7] = 0.0f;
            m[8] = ca + axis[1] * sine;
            m[9] = bc - axis[0] * sine;
            m[10] = tz + cosine * (1 - tz);
            m[11] = 0;
            m[12] = 0;
            m[13] = 0;
            m[14] = 0;
            m[15] = 1;
        }

        protected static void MakeTranslateMatrix(float x, float y, float z, float[] m)
        {
            m[0] = 1;
            m[1] = 0;
            m[2] = 0;
            m[3] = x;
            m[4] = 0;
            m[5] = 1;
            m[6] = 0;
            m[7] = y;
            m[8] = 0;
            m[9] = 0;
            m[10] = 1;
            m[11] = z;
            m[12] = 0;
            m[13] = 0;
            m[14] = 0;
            m[15] = 1;
        }

        /* Simple 4x4 matrix by 4x4 matrix multiply. */
        protected static void MultMatrix(float[] dst, float[] src1, float[] src2)
        {
            float[] tmp = new float[16];
            int i;

            for (i = 0; i < 4; i++)
            {
                int j;
                for (j = 0; j < 4; j++)
                {
                    tmp[i * 4 + j] = src1[i * 4 + 0] * src2[0 * 4 + j] +
                                     src1[i * 4 + 1] * src2[1 * 4 + j] +
                                     src1[i * 4 + 2] * src2[2 * 4 + j] +
                                     src1[i * 4 + 3] * src2[3 * 4 + j];
                }
            }
            /* Copy result to dst (so dst can also be src1 or src2). */
            for (i = 0; i < 16; i++)
            {
                dst[i] = tmp[i];
            }
        }

        /// <summary>
        /// Simple 4x4 matrix by 4-component column vector multiply.
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="mat"></param>
        /// <param name="vec"></param>
        protected static void Transform(float[] dst, float[] mat, float[] vec)
        {
            var tmp = new double[4];
            int i;

            for (i = 0; i < 4; i++)
            {
                tmp[i] = mat[i * 4 + 0] * vec[0] +
                         mat[i * 4 + 1] * vec[1] +
                         mat[i * 4 + 2] * vec[2] +
                         mat[i * 4 + 3] * vec[3];
            }
            /* Apply perspective divide and copy to dst (so dst can vec). */
            for (i = 0; i < 3; i++)
            {
                dst[i] = (float)(tmp[i] * tmp[3]);
            }
            dst[3] = 1;
        }

        #endregion Protected Static Methods

        #region Private Static Methods

        /// <summary>
        /// Get Element of Matrix
        /// </summary>
        /// <param name="m">Matrix</param>
        /// <param name="r">Row</param>
        /// <param name="c">Column</param>
        /// <returns>Value</returns>
        private static float MatGet(IList<float> m, int r, int c)
        {
            return m[r * 4 + c];
        }

        /// <summary>
        /// Set Element in Matrix
        /// </summary>
        /// <param name="m">Matrix</param>
        /// <param name="r">Row</param>
        /// <param name="c">Column</param>
        /// <param name="value">Value</param>
        private static void MatSet(IList<float> m, int r, int c, double value)
        {
            m[r * 4 + c] = (float)value;
        }

        /// <summary>
        /// Change Rows means in this case just change the Refrences
        /// to the Arrays
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private static void SwapRows(ref double[] a, ref double[] b)
        {
            double[] temp = a;
            a = b;
            b = temp;
        }

        #endregion Private Static Methods

        #region Resource Helper

        // See: http://www.vcskicks.com/embedded-resource.php

        public Stream GetEmbeddedResourceStream(string resourceName)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(BaseExample));
            resourceName = FormatResourceName(assembly, resourceName);
            return assembly.GetManifestResourceStream(resourceName);
        }

        private string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                .Replace("\\", ".").Replace("/", ".");
        }

        #endregion
    }
}