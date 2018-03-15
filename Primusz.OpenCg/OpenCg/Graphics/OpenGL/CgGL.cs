using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace OpenCg.Graphics.OpenGL
{
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public static partial class CgGL
    {
        #region Profile Functions

        /// <summary>
        /// Sets the best compiler options available by card, driver and selected profile. (Cg v1.1)
        /// </summary>
        /// <param name="profile">
        /// Profile.
        /// </param>
        public static void SetOptimalOptions(CgProfile profile)
        { cgGLSetOptimalOptions(profile); }

        /// <summary>
        /// Get the best set of compiler options for a profile. (Cg v2.2) 
        /// </summary>
        /// <param name="profile">The profile whose optimal arguments are requested.</param>
        /// <returns>Returns a null-terminated array of strings representing the optimal set of compiler options for profile. Returns NULL if profile isn't supported by the current driver or GPU.</returns>
        public static string[] GetOptimalOptions(CgProfile profile)
        {
            return Cg.IntPtrToStringArray(cgGLGetOptimalOptions(profile));
        }

        /// <summary>
        /// Returns the best profile available. (Cg v1.1)
        /// </summary>
        /// <param name="profile_type">
        /// CG_GL_VERTEX or CG_GL_FRAGMENT program to look for the best matching profile. CG_GL_GEOMETRY support was introduced in Cg 2.0.
        /// </param>
        /// <returns>
        /// Returns the best profile available.
        /// </returns>
        public static CgProfile GetLatestProfile(CgGLEnum profile_type)
        { return cgGLGetLatestProfile(profile_type); }

        /// <summary>
        /// Checks if the profile is supported. (Cg v1.1)
        /// </summary>
        /// <param name="profile">
        /// The profile to check the support of.
        /// </param>
        /// <returns>
        /// Returns true if the profile is supported.
        /// </returns>
        public static CgBool IsProfileSupported(CgProfile profile)
        { return cgGLIsProfileSupported(profile); }

        /// <summary>
        /// Enables the selected profile. (Cg v1.1)
        /// </summary>
        /// <param name="profile">
        /// Profile to enable.
        /// </param>
        public static void EnableProfile(CgProfile profile)
        { cgGLEnableProfile(profile); }


        /// <summary>
        /// Disables the selected profile. (Cg v1.1)
        /// </summary>
        /// <param name="profile">
        /// Profile to disable.
        /// </param>
        public static void DisableProfile(CgProfile profile)
        { cgGLDisableProfile(profile); }


        #endregion

        #region Program Managment Functions

        /// <summary>
        /// Returns CG_TRUE if program has been loaded with cgGLLoadProgram and CG_FALSE otherwise. (Cg v1.2) 
        /// </summary>
        /// <param name="program">
        /// The program which will be checked.
        /// </param>
        /// <returns>
        /// Returns CG_TRUE if program has been loaded. Returns CG_FALSE otherwise. 
        /// </returns>
        public static CgBool IsProgramLoaded(CgProgram program)
        { return cgGLIsProgramLoaded(program); }

        /// <summary>
        /// Returns the program's ID. (Cg v1.2)
        /// </summary>
        /// <param name="program">
        /// The program for which the OpenGL program ID will be retrieved.
        /// </param>
        /// <returns>
        /// Returns a GLuint associated with the GL program object for profiles that use program object. Returns 0 for profiles that do not have OpenGL programs (e.g. fp20).
        /// </returns>
        public static int GetProgramID(CgProgram program)
        { return cgGLGetProgramID(program); }

        /// <summary>
        /// Loads program to OpenGL pipeline. (Cg v1.1)
        /// </summary>
        /// <param name="program">
        /// The program which will be loaded.
        /// </param>
        public static void LoadProgram(CgProgram program)
        { cgGLLoadProgram(program); }

        /// <summary>
        /// Destroy the OpenGL shader object associated with a program. (Cg v2.1)
        /// </summary>
        /// <param name="program">
        /// The program for which to destroy the shader object. The CgProgram handle is still valid after this call.
        /// </param>
        public static void UnloadProgram(CgProgram program)
        { cgGLUnloadProgram(program); }

        /// <summary>
        /// Enables the profiles for all of the programs in a combined program. (Cg v1.5)
        /// </summary>
        /// <param name="program">The combined program for which the profiles will be enabled.</param>
        public static void EnableProgramProfiles(CgProgram program)
        { cgGLEnableProgramProfiles(program); }

        /// <summary>
        /// Disables the profiles for all of the programs in a combined program. (Cg v1.5)
        /// </summary>
        /// <param name="program">The combined program for which the profiles will be disabled.</param>
        public static void DisableProgramProfiles(CgProgram program)
        { cgGLDisableProgramProfiles(program); }

        /// <summary>
        /// Bind the program to the current OpenGL API state. (Cg v1.1)
        /// </summary>
        /// <param name="program">
        /// The program to bind to the current state.
        /// </param>
        public static void BindProgram(CgProgram program)
        { cgGLBindProgram(program); }

        /// <summary>
        /// Unbinds the program bound in a profile. (Cg v1.2)
        /// </summary>
        /// <param name="profile">
        /// The profile from which to unbind any bound program.
        /// </param>
        public static void UnbindProgram(CgProfile profile)
        { cgGLUnbindProgram(profile); }

        /// <summary>
        /// Control whether the cgGL runtime calls glGetError. (Cg v1.5)
        /// </summary>
        /// <param name="debug">Flag indicating whether the library should use OpenGL error checking. CG_TRUE or CG_FALSE</param>
        public static void SetDebugMode(CgBool debug)
        { cgGLSetDebugMode(debug); }

        #endregion

        #region Parameter Managment Functions

        /// <summary>
        /// Sets parameter with attribute array. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to be set.</param>
        /// <param name="fsize">Number of coordinates per vertex.</param>
        /// <param name="type">The data type of each coordinate. Possible values are GL_UNSIGNED_BYTE, GL_SHORT, GL_INT, GL_FLOAT, and GL_DOUBLE.</param>
        /// <param name="stride">The byte offset between consecutive vertices. When stride is 0 the array is assumed to be tightly packed.</param>
        /// <param name="pointer">The pointer to the first coordinate in the vertex array.</param>
        public unsafe static void SetParameterPointer(CgParameter param, int fsize, int type, int stride, [In]void* pointer)
        { cgGLSetParameterPointer(param, fsize, type, stride, pointer); }

        /// <summary>
        /// Sets parameter with attribute array. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to be set.</param>
        /// <param name="fsize">Number of coordinates per vertex.</param>
        /// <param name="type">The data type of each coordinate. Possible values are GL_UNSIGNED_BYTE, GL_SHORT, GL_INT, GL_FLOAT, and GL_DOUBLE.</param>
        /// <param name="stride">The byte offset between consecutive vertices. When stride is 0 the array is assumed to be tightly packed.</param>
        /// <param name="pointer">The pointer to the first coordinate in the vertex array.</param>
        public static void SetParameterPointer(CgParameter param, int fsize, int type, int stride, [In]IntPtr pointer)
        { cgGLSetParameterPointer(param, fsize, type, stride, pointer); }

        /// <summary>
        /// Enables a vertex attribute in the OpenGL state. (Cg v1.1) 
        /// </summary>
        /// <param name="param">The varying parameter for which the client state will be enabled.</param>
        public static void EnableClientState(CgParameter param)
        { cgGLEnableClientState(param); }

        /// <summary>
        /// Disables a vertex attribute in OpenGL state. (Cg v1.1)
        /// </summary>
        /// <param name="param">The varying parameter for which the client state will be disabled.</param>
        public static void DisableClientState(CgParameter param)
        { cgGLDisableClientState(param); }

        #region SetParameter

        public static void SetParameter(CgParameter param, double x)
        {
            cgGLSetParameter1d(param, x);
        }

        public static void SetParameter(CgParameter param, double[] v)
        {
            switch (v.Length)
            {
                case 1:
                    cgGLSetParameter1dv(param, v);
                    break;
                case 2:
                    cgGLSetParameter2dv(param, v);
                    break;
                case 3:
                    cgGLSetParameter3dv(param, v);
                    break;
                case 4:
                    cgGLSetParameter4dv(param, v);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static void SetParameter(CgParameter param, float[] v)
        {
            switch (v.Length)
            {
                case 1:
                    cgGLSetParameter1fv(param, v);
                    break;
                case 2:
                    cgGLSetParameter2fv(param, v);
                    break;
                case 3:
                    cgGLSetParameter3fv(param, v);
                    break;
                case 4:
                    cgGLSetParameter4fv(param, v);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static void SetParameter(CgParameter param, float x)
        { cgGLSetParameter1f(param, x); }

        public static void SetParameter(CgParameter param, double x, double y)
        { cgGLSetParameter2d(param, x, y); }

        public static void SetParameter(CgParameter param, float x, float y)
        { cgGLSetParameter2f(param, x, y); }

        public static void SetParameter(CgParameter param, double x, double y, double z)
        { cgGLSetParameter3d(param, x, y, z); }

        public static void SetParameter(CgParameter param, float x, float y, float z)
        { cgGLSetParameter3f(param, x, y, z); }

        public static void SetParameter(CgParameter param, double x, double y, double z, double w)
        { cgGLSetParameter4d(param, x, y, z, w); }

        public static void SetParameter(CgParameter param, float x, float y, float z, float w)
        { cgGLSetParameter4f(param, x, y, z, w); }

        #endregion

        #region cgGLSetParameterNf

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        public static void SetParameter1f(CgParameter param, float x)
        { cgGLSetParameter1f(param, x); }


        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        /// <param name="y">The value to which param will be set.</param>
        public static void SetParameter2f(CgParameter param, float x, float y)
        { cgGLSetParameter2f(param, x, y); }


        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        /// <param name="y">The value to which param will be set.</param>
        /// <param name="z">The value to which param will be set.</param>
        public static void SetParameter3f(CgParameter param, float x, float y, float z)
        { cgGLSetParameter3f(param, x, y, z); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        /// <param name="y">The value to which param will be set.</param>
        /// <param name="z">The value to which param will be set.</param>
        /// <param name="w">The value to which param will be set.</param>
        public static void SetParameter4f(CgParameter param, float x, float y, float z, float w)
        { cgGLSetParameter4f(param, x, y, z, w); }

        #endregion

        #region cgGLSetParameterNfv

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter1fv(CgParameter param, [In]float* values)
        { cgGLSetParameter1fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter1fv(CgParameter param, [In]float[] values)
        { cgGLSetParameter1fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter1fv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter1fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter2fv(CgParameter param, [In]float* values)
        { cgGLSetParameter2fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter2fv(CgParameter param, [In]float[] values)
        { cgGLSetParameter2fv(param, values); }


        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter2fv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter2fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter3fv(CgParameter param, [In]float* values)
        { cgGLSetParameter3fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter3fv(CgParameter param, [In]float[] values)
        { cgGLSetParameter3fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter3fv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter3fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter4fv(CgParameter param, [In]float* values)
        { cgGLSetParameter4fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter4fv(CgParameter param, [In]float[] values)
        { cgGLSetParameter4fv(param, values); }

        /// <summary>
        /// Sets the float values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        ///<param name="values">Array of values used to set param.</param>
        public static void SetParameter4fv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter4fv(param, values); }


        #endregion

        #region cgGLSetParameterNd

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use  cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        public static void SetParameter1d(CgParameter param, double x)
        { cgGLSetParameter1d(param, x); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use  cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        /// <param name="y">The value to which param will be set.</param>
        public static void SetParameter2d(CgParameter param, double x, double y)
        { cgGLSetParameter2d(param, x, y); }


        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use  cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        /// <param name="y">The value to which param will be set.</param>
        /// <param name="z">The value to which param will be set.</param>
        public static void SetParameter3d(CgParameter param, double x, double y, double z)
        { cgGLSetParameter3d(param, x, y, z); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use  cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        /// <param name="y">The value to which param will be set.</param>
        /// <param name="z">The value to which param will be set.</param>
        /// <param name="w">The value to which param will be set.</param>
        public static void SetParameter4d(CgParameter param, double x, double y, double z, double w)
        { cgGLSetParameter4d(param, x, y, z, w); }

        #endregion

        #region cgGLSetParameterNdv

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter1dv(CgParameter param, [In]double* values)
        { cgGLSetParameter1dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public static void SetParameter1dv(CgParameter param, [In]double[] values)
        { cgGLSetParameter1dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public static void SetParameter1dv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter1dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter2dv(CgParameter param, [In]double* values)
        { cgGLSetParameter2dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public static void SetParameter2dv(CgParameter param, [In]double[] values)
        { cgGLSetParameter2dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public static void SetParameter2dv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter2dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter3dv(CgParameter param, [In]double* values)
        { cgGLSetParameter3dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public static void SetParameter3dv(CgParameter param, [In]double[] values)
        { cgGLSetParameter3dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter3dv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter3dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public unsafe static void SetParameter4dv(CgParameter param, [In]double* values)
        { cgGLSetParameter4dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public static void SetParameter4dv(CgParameter param, [In]double[] values)
        { cgGLSetParameter4dv(param, values); }

        /// <summary>
        /// Sets the double values to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter that will be set.</param>
        /// <param name="values">Array of values used to set param.</param>
        public static void SetParameter4dv(CgParameter param, [In]IntPtr values)
        { cgGLSetParameter4dv(param, values); }

        #endregion

        #region GetParameter

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter(CgParameter param, [Out]float[] values)
        {
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);

            try
            {
                switch (values.Length)
                {
                    case 1:
                        cgGLGetParameter1f(param, handle.AddrOfPinnedObject());
                        break;
                    case 2:
                        cgGLGetParameter2f(param, handle.AddrOfPinnedObject());
                        break;
                    case 3:
                        cgGLGetParameter3f(param, handle.AddrOfPinnedObject());
                        break;
                    case 4:
                        cgGLGetParameter4f(param, handle.AddrOfPinnedObject());
                        break;
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter(CgParameter param, [Out]double[] values)
        {
            GCHandle handle = GCHandle.Alloc(values);

            try
            {
                switch (values.Length)
                {
                    case 1:
                        cgGLGetParameter1d(param, handle.AddrOfPinnedObject());
                        break;
                    case 2:
                        cgGLGetParameter2d(param, handle.AddrOfPinnedObject());
                        break;
                    case 3:
                        cgGLGetParameter3d(param, handle.AddrOfPinnedObject());
                        break;
                    case 4:
                        cgGLGetParameter4d(param, handle.AddrOfPinnedObject());
                        break;
                }
            }
            finally
            {
                handle.Free();
            }
        }

        #endregion

        #region cgGLGetParameterNf

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter1f(CgParameter param, [Out]float* values)
        { cgGLGetParameter1f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter1f(CgParameter param, [Out]float[] values)
        { cgGLGetParameter1f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter1f(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter1f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter2f(CgParameter param, [Out]float* values)
        { cgGLGetParameter2f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter2f(CgParameter param, [Out]float[] values)
        { cgGLGetParameter2f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter2f(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter2f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter3f(CgParameter param, [Out]float* values)
        { cgGLGetParameter3f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter3f(CgParameter param, [Out]float[] values)
        { cgGLGetParameter3f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter3f(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter3f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter4f(CgParameter param, [Out]float* values)
        { cgGLGetParameter4f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter4f(CgParameter param, [Out]float[] values)
        { cgGLGetParameter4f(param, values); }

        /// <summary>
        /// Gets the float value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter4f(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter4f(param, values); }

        #endregion

        #region cgGLGetParameterNd

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter1d(CgParameter param, [Out]double* values)
        { cgGLGetParameter1d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter1d(CgParameter param, [Out]double[] values)
        { cgGLGetParameter1d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter1d(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter1d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter2d(CgParameter param, [Out]double* values)
        { cgGLGetParameter2d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter2d(CgParameter param, [Out]double[] values)
        { cgGLGetParameter2d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter2d(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter2d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter3d(CgParameter param, [Out]double* values)
        { cgGLGetParameter3d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter3d(CgParameter param, [Out]double[] values)
        { cgGLGetParameter3d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter3d(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter3d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public unsafe static void GetParameter4d(CgParameter param, [Out]double* values)
        { cgGLGetParameter4d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter4d(CgParameter param, [Out]double[] values)
        { cgGLGetParameter4d(param, values); }

        /// <summary>
        /// Gets the double value to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The parameter from which the values will be retrieved.</param>
        /// <param name="values">Destination buffer into which the values will be written.</param>
        public static void GetParameter4d(CgParameter param, [Out]IntPtr values)
        { cgGLGetParameter4d(param, values); }

        #endregion

        #region Arrays

        #region SetParameterArray

        public static void SetParameterArray(CgParameter param, int offset, int nelements, float[] values)
        {
            switch (values.Length / nelements)
            {
                case 1:
                    cgGLSetParameterArray1f(param, offset, nelements, values);
                    break;
                case 2:
                    cgGLSetParameterArray2f(param, offset, nelements, values);
                    break;
                case 3:
                    cgGLSetParameterArray3f(param, offset, nelements, values);
                    break;
                case 4:
                    cgGLSetParameterArray4f(param, offset, nelements, values);
                    break;
            }
        }

        public static void SetParameterArray(CgParameter param, int offset, int nelements, double[] values)
        {
            switch (values.Length / nelements)
            {
                case 1:
                    cgGLSetParameterArray1d(param, offset, nelements, values);
                    break;
                case 2:
                    cgGLSetParameterArray2d(param, offset, nelements, values);
                    break;
                case 3:
                    cgGLSetParameterArray3d(param, offset, nelements, values);
                    break;
                case 4:
                    cgGLSetParameterArray4d(param, offset, nelements, values);
                    break;
            }
        }

        #endregion

        #region cgGLSetParameterArrayNf

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray1f(CgParameter param, long offset, long nelements, [In]float* values)
        { cgGLSetParameterArray1f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray1f(CgParameter param, long offset, long nelements, [In]float[] values)
        { cgGLSetParameterArray1f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray1f(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray1f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray2f(CgParameter param, long offset, long nelements, [In]float* values)
        { cgGLSetParameterArray2f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray2f(CgParameter param, long offset, long nelements, [In]float[] values)
        { cgGLSetParameterArray2f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray2f(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray2f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray3f(CgParameter param, long offset, long nelements, [In]float* values)
        { cgGLSetParameterArray3f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray3f(CgParameter param, long offset, long nelements, [In]float[] values)
        { cgGLSetParameterArray3f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray3f(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray3f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray4f(CgParameter param, long offset, long nelements, [In]float* values)
        { cgGLSetParameterArray4f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray4f(CgParameter param, long offset, long nelements, [In]float[] values)
        { cgGLSetParameterArray4f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray4f(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray4f(param, offset, nelements, values); }

        #endregion

        #region cgGLSetParameterArrayNd

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray1d(CgParameter param, long offset, long nelements, [In]double* values)
        { cgGLSetParameterArray1d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray1d(CgParameter param, long offset, long nelements, [In]double[] values)
        { cgGLSetParameterArray1d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray1d(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray1d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray2d(CgParameter param, long offset, long nelements, [In]double* values)
        { cgGLSetParameterArray2d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray2d(CgParameter param, long offset, long nelements, [In]double[] values)
        { cgGLSetParameterArray2d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray2d(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray2d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray3d(CgParameter param, long offset, long nelements, [In]double* values)
        { cgGLSetParameterArray3d(param, offset, nelements, values); }


        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray3d(CgParameter param, long offset, long nelements, [In]double[] values)
        { cgGLSetParameterArray3d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray3d(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray3d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void SetParameterArray4d(CgParameter param, long offset, long nelements, [In]double* values)
        { cgGLSetParameterArray4d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray4d(CgParameter param, long offset, long nelements, [In]double[] values)
        { cgGLSetParameterArray4d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void SetParameterArray4d(CgParameter param, long offset, long nelements, [In]IntPtr values)
        { cgGLSetParameterArray4d(param, offset, nelements, values); }

        #endregion

        #region GetParameterArray

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray(CgParameter param, int offset, int nelements, [Out]float[] values)
        {
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);

            try
            {
                switch (values.Length / nelements)
                {
                    case 1:
                        cgGLGetParameterArray1f(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                    case 2:
                        cgGLGetParameterArray2f(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                    case 3:
                        cgGLGetParameterArray3f(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                    case 4:
                        cgGLGetParameterArray4f(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray(CgParameter param, int offset, int nelements, [Out]double[] values)
        {
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);

            try
            {
                switch (values.Length / nelements)
                {
                    case 1:
                        cgGLGetParameterArray1d(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                    case 2:
                        cgGLGetParameterArray2d(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                    case 3:
                        cgGLGetParameterArray3d(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                    case 4:
                        cgGLGetParameterArray4d(param, offset, nelements, handle.AddrOfPinnedObject());
                        break;
                }
            }
            finally
            {
                handle.Free();
            }
        }

        #endregion

        #region cgGLGetParameterArrayNf

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray1f(CgParameter param, long offset, long nelements, [Out]float* values)
        { cgGLGetParameterArray1f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray1f(CgParameter param, long offset, long nelements, [Out]float[] values)
        { cgGLGetParameterArray1f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray1f(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray1f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray2f(CgParameter param, long offset, long nelements, [Out]float* values)
        { cgGLGetParameterArray2f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray2f(CgParameter param, long offset, long nelements, [Out]float[] values)
        { cgGLGetParameterArray2f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray2f(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray2f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray3f(CgParameter param, long offset, long nelements, [Out]float* values)
        { cgGLGetParameterArray3f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray3f(CgParameter param, long offset, long nelements, [Out]float[] values)
        { cgGLGetParameterArray3f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray3f(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray3f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray4f(CgParameter param, long offset, long nelements, [Out]float* values)
        { cgGLGetParameterArray4f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray4f(CgParameter param, long offset, long nelements, [Out]float[] values)
        { cgGLGetParameterArray4f(param, offset, nelements, values); }

        /// <summary>
        /// Sets the float values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray4f(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray4f(param, offset, nelements, values); }

        #endregion

        #region cgGLGetParameterArrayNd

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray1d(CgParameter param, long offset, long nelements, [Out]double* values)
        { cgGLGetParameterArray1d(param, offset, nelements, values); }


        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray1d(CgParameter param, long offset, long nelements, [Out]double[] values)
        { cgGLGetParameterArray1d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray1d(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray1d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray2d(CgParameter param, long offset, long nelements, [Out]double* values)
        { cgGLGetParameterArray2d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray2d(CgParameter param, long offset, long nelements, [Out]double[] values)
        { cgGLGetParameterArray2d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray2d(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray2d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray3d(CgParameter param, long offset, long nelements, [Out]double* values)
        { cgGLGetParameterArray3d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray3d(CgParameter param, long offset, long nelements, [Out]double[] values)
        { cgGLGetParameterArray3d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray3d(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray3d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public unsafe static void GetParameterArray4d(CgParameter param, long offset, long nelements, [Out]double* values)
        { cgGLGetParameterArray4d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray4d(CgParameter param, long offset, long nelements, [Out]double[] values)
        { cgGLGetParameterArray4d(param, offset, nelements, values); }

        /// <summary>
        /// Sets the double values to the specific parameter. (Cg v1.1)
        /// </summary>
        /// <param name="param">Parameter to set values to.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="values">The array of values used to set the parameter. This must be a contiguous set of nelements values.</param>
        public static void GetParameterArray4d(CgParameter param, long offset, long nelements, [Out]IntPtr values)
        { cgGLGetParameterArray4d(param, offset, nelements, values); }

        #endregion

        #endregion

        #endregion

        #region Matrix Parameter Managment Functions

        /// <summary>
        /// Sets the values of the parameter to a matrix in the OpenGL state. (Cg v1.1)
        /// </summary>
        /// <param name="param">
        /// Parameter that will be set.
        /// </param>
        /// <param name="matrix">
        /// Which matrix should be retreived from the OpenGL state.
        /// </param>
        /// <param name="transform">
        /// Optional transformation that will be aplied to the OpenGL state matrix before it is retreived to the parameter.
        /// </param>
        public static void SetStateMatrixParameter(CgParameter param, int matrix, int transform)
        { cgGLSetStateMatrixParameter(param, matrix, transform); }

        #region cgGLSetMatrixParameter

        #region cgGLSetMatrixParameterdr

        /// <summary>
        /// Sets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public unsafe static void SetMatrixParameterdr(CgParameter param, [In]double* matrix)
        { cgGLSetMatrixParameterdr(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterdr(CgParameter param, [In]double[] matrix)
        { cgGLSetMatrixParameterdr(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterdr(CgParameter param, [In]IntPtr matrix)
        { cgGLSetMatrixParameterdr(param, matrix); }

        #endregion

        #region cgGLSetMatrixParameterfr

        /// <summary>
        /// Sets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public unsafe static void SetMatrixParameterfr(CgParameter param, [In]float* matrix)
        { cgGLSetMatrixParameterfr(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterfr(CgParameter param, [In]float[] matrix)
        { cgGLSetMatrixParameterfr(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterfr(CgParameter param, [In]IntPtr matrix)
        { cgGLSetMatrixParameterfr(param, matrix); }

        #endregion

        #region cgGLSetMatrixParameterdc

        /// <summary>
        /// Sets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public unsafe static void SetMatrixParameterdc(CgParameter param, [In]double* matrix)
        { cgGLSetMatrixParameterdc(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterdc(CgParameter param, [In]double[] matrix)
        { cgGLSetMatrixParameterdc(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterdc(CgParameter param, [In]IntPtr matrix)
        { cgGLSetMatrixParameterdc(param, matrix); }

        #endregion

        #region cgGLSetMatrixParameterfc

        /// <summary>
        /// Sets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public unsafe static void SetMatrixParameterfc(CgParameter param, [In]float* matrix)
        { cgGLSetMatrixParameterfc(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterfc(CgParameter param, [In]float[] matrix)
        { cgGLSetMatrixParameterfc(param, matrix); }

        /// <summary>
        /// Sets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix parameter. The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterfc(CgParameter param, [In]IntPtr matrix)
        { cgGLSetMatrixParameterfc(param, matrix); }

        #endregion

        #endregion

        #region cgGLGetMatrixParameter

        #region cgGLGetMatrixParameterdr

        /// <summary>
        /// Gets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public unsafe static void GetMatrixParameterdr(CgParameter param, [In]double* matrix)
        { cgGLGetMatrixParameterdr(param, matrix); }

        /// <summary>
        /// Gets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public static void GetMatrixParameterdr(CgParameter param, [In]double[] matrix)
        { cgGLGetMatrixParameterdr(param, matrix); }

        /// <summary>
        /// Gets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public static void GetMatrixParameterdr(CgParameter param, [In]IntPtr matrix)
        { cgGLGetMatrixParameterdr(param, matrix); }

        #endregion

        #region cgGLGetMatrixParameterfr

        /// <summary>
        /// Gets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public unsafe static void GetMatrixParameterfr(CgParameter param, [In]float* matrix)
        { cgGLGetMatrixParameterfr(param, matrix); }


        /// <summary>
        /// Gets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public static void GetMatrixParameterfr(CgParameter param, [In]float[] matrix)
        { cgGLGetMatrixParameterfr(param, matrix); }

        /// <summary>
        /// Gets the value of matrix parameters in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public static void GetMatrixParameterfr(CgParameter param, [In]IntPtr matrix)
        { cgGLGetMatrixParameterfr(param, matrix); }

        #endregion

        #region cgGLGetMatrixParameterdc

        /// <summary>
        /// Gets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public unsafe static void GetMatrixParameterdc(CgParameter param, [In]double* matrix)
        { cgGLGetMatrixParameterdc(param, matrix); }

        /// <summary>
        /// Gets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public unsafe static void GetMatrixParameterdc(CgParameter param, [In]double[] matrix)
        { cgGLGetMatrixParameterdc(param, matrix); }

        /// <summary>
        /// Gets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public static void GetMatrixParameterdc(CgParameter param, [In]IntPtr matrix)
        { cgGLGetMatrixParameterdc(param, matrix); }

        #endregion

        #region cgGLGetMatrixParameterfc

        /// <summary>
        /// Gets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public unsafe static void GetMatrixParameterfc(CgParameter param, [In]float* matrix)
        { cgGLGetMatrixParameterfc(param, matrix); }

        /// <summary>
        /// Gets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public static void GetMatrixParameterfc(CgParameter param, [In]float[] matrix)
        { cgGLGetMatrixParameterfc(param, matrix); }

        /// <summary>
        /// Gets the value of matrix parameters in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix parameter from which the values will be retrieved.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be retrieved. The size must be the number of rows times the number of columns of param.</param>
        public static void GetMatrixParameterfc(CgParameter param, [In]IntPtr matrix)
        { cgGLGetMatrixParameterfc(param, matrix); }

        #endregion

        #endregion

        #region cgGLSetMatrixParameterArray

        #region cgGLSetMatrixParameterArrayfc

        /// <summary>
        /// Sets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public unsafe static void SetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [In]float* v)
        { cgGLSetMatrixParameterArrayfc(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [In]float[] v)
        { cgGLSetMatrixParameterArrayfc(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [In]IntPtr v)
        { cgGLSetMatrixParameterArrayfc(param, offset, nelements, v); }

        #endregion

        #region cgGLSetMatrixParameterArraydc

        /// <summary>
        /// Sets an array matrix parameters (double) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public unsafe static void SetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [In]double* v)
        { cgGLSetMatrixParameterArraydc(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (double) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [In]double[] v)
        { cgGLSetMatrixParameterArraydc(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (double) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [In]IntPtr v)
        { cgGLSetMatrixParameterArraydc(param, offset, nelements, v); }

        #endregion cgGLSetMatrixParameterArraydc

        #region cgGLSetMatrixParameterArrayfr

        /// <summary>
        /// Sets an array matrix parameters (float) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public unsafe static void SetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [In]float* v)
        { cgGLSetMatrixParameterArrayfr(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (float) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [In]float[] v)
        { cgGLSetMatrixParameterArrayfr(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (float) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [In]IntPtr v)
        { cgGLSetMatrixParameterArrayfr(param, offset, nelements, v); }

        #endregion cgGLSetMatrixParameterArrayfr

        #region cgGLSetMatrixParameterArraydr

        /// <summary>
        /// Sets an array matrix parameters (double) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public unsafe static void SetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [In]double* v)
        { cgGLSetMatrixParameterArraydr(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (double) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [In]double[] v)
        { cgGLSetMatrixParameterArraydr(param, offset, nelements, v); }

        /// <summary>
        /// Sets an array matrix parameters (double) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter that will be set.</param>
        /// <param name="offset">An offset into the array parameter at which to start setting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to set. A value of 0 will default to the number of elements in the array minus the offset value.</param>
        /// <param name="v">The array of values to which to set the parameter. This must be a contiguous set of values with size nelements times the number of elements in the matrix.</param>
        public static void SetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [In]IntPtr v)
        { cgGLSetMatrixParameterArraydr(param, offset, nelements, v); }

        #endregion cgGLSetMatrixParameterArraydr

        #endregion

        #region cgGLGetMatrixParameterArray

        #region cgGLGetMatrixParameterArrayfc

        /// <summary>
        /// Gets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public unsafe static void GetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [Out]float* v)
        { cgGLGetMatrixParameterArrayfc(param, offset, nelements, v); }

        /// <summary>
        /// Gets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [Out]float[] v)
        { cgGLGetMatrixParameterArrayfc(param, offset, nelements, v); }

        /// <summary>
        /// Gets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [Out]IntPtr v)
        { cgGLGetMatrixParameterArrayfc(param, offset, nelements, v); }

        #endregion

        #region cgGLGetMatrixParameterArraydc

        /// <summary>
        /// Gets an array matrix parameters (double) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public unsafe static void GetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [Out]double* v)
        { cgGLGetMatrixParameterArraydc(param, offset, nelements, v); }

        /// <summary>
        /// Gets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [Out]double[] v)
        { cgGLGetMatrixParameterArraydc(param, offset, nelements, v); }

        /// <summary>
        /// Gets an array matrix parameters (float) in column order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [Out]IntPtr v)
        { cgGLGetMatrixParameterArraydc(param, offset, nelements, v); }

        #endregion

        #region cgGLGetMatrixParameterArrayfr

        /// <summary>
        /// Gets an array matrix parameters (float) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public unsafe static void GetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [Out]float* v)
        { cgGLGetMatrixParameterArrayfr(param, offset, nelements, v); }

        /// <summary>
        /// Gets an array matrix parameters (float) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [Out]float[] v)
        { cgGLGetMatrixParameterArrayfr(param, offset, nelements, v); }

        /// <summary>
        /// Gets an array matrix parameters (float) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [Out]IntPtr v)
        { cgGLGetMatrixParameterArrayfr(param, offset, nelements, v); }

        #endregion

        #region cgGLGetMatrixParameterArraydr

        /// <summary>
        /// Gets an array matrix parameters (double) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public unsafe static void GetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [Out]double* v)
        { cgGLGetMatrixParameterArraydr(param, offset, nelements, v); }


        /// <summary>
        /// Gets an array matrix parameters (double) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [Out]double[] v)
        { cgGLGetMatrixParameterArraydr(param, offset, nelements, v); }

        /// <summary>
        /// Gets an array matrix parameters (double) in row order. (Cg v1.1)
        /// </summary>
        /// <param name="param">The matrix array parameter from which the values will be retrieved.</param>
        /// <param name="offset">An offset into the array parameter at which to start getting elements. A value of 0 will begin at the first element of the array.</param>
        /// <param name="nelements">The number of elements to get. A value of 0 will default to the total number of elements in the array minus the value of offset.</param>
        /// <param name="v">The array into which to retrieve the values. The size of v must be nelements times the number of elements in the matrix.</param>
        public static void GetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [Out]IntPtr v)
        { cgGLGetMatrixParameterArraydr(param, offset, nelements, v); }

        #endregion

        #endregion

        #endregion

        #region Textures Parameter Managment Functions

        /// <summary>
        /// Sets texture object to the specified parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The texture parameter that will be set.</param>
        /// <param name="texobj">An OpenGL texture object name to which the parameter will be set.</param>
        public static void SetTextureParameter(CgParameter param, int texobj)
        { cgGLSetTextureParameter(param, texobj); }

        /// <summary>
        /// Retreives the value of a texture parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The texture parameter for which the OpenGL texture object will be retrieved.</param>
        /// <returns>Returns the OpenGL object to which the texture was set. Returns 0 if the parameter has not been set.</returns>
        public static void GetTextureParameter(CgParameter param)
        { cgGLGetTextureParameter(param); }

        /// <summary>
        /// Enables (binds) the texture unit associated with the given texture parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The texture parameter which will be enabled.</param>
        public static void EnableTextureParameter(CgParameter param)
        { cgGLEnableTextureParameter(param); }

        /// <summary>
        /// Disables the texture unit associated with the given texture parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// </remarks>
        /// </summary>
        /// <param name="param">The texture parameter which will be disabled.</param>
        public static void DisableTextureParameter(CgParameter param)
        { cgGLDisableTextureParameter(param); }


        /// <summary>
        /// Retreives the OpenGL enumeration for the texture unit associated with the texture parameter. (Cg v1.1)
        /// <remarks>
        /// Use cgGetNamedParameter to obtain the valid pointer to param.
        /// It can be one of the GL_TEXTURE#_ARB if valid.
        /// </remarks>
        /// </summary>
        /// <param name="param">The texture parameter for which the OpenGL texture unit enumerant will be retrieved.</param>
        /// <returns>Returns a GLenum of the form GL_TEXTURE#_ARB. Returns GL_INVALID_OPERATION if an error occurs.</returns>
        public static int GetTextureEnum(CgParameter param)
        { return cgGLGetTextureEnum(param); }

        /// <summary>
        /// Enables or disables the automatic texture management for the given rendering context. (Cg v1.2)
        /// <remarks>
        /// Use CG_TRUE or CG_FALSE to enable/disable automatic texture management.
        /// </remarks>
        /// </summary>
        /// <param name="context">The context in which the automatic texture management behavior will be changed.</param>
        /// <param name="flag">A boolean switch which controls automatic texture management by the runtime.</param>
        public static void SetManageTextureParameters(CgContext context, bool flag)
        { cgGLSetManageTextureParameters(context, flag); }

        /// <summary>
        /// Retreives the manage texture parameters flag from a context. (Cg v1.2)
        /// </summary>
        /// <param name="context">The context from which the automatic texture management setting will be retrieved.</param>
        /// <returns>Returns the manage textures setting for context.</returns>
        public static int GetManageTextureParameters(CgContext context)
        { return cgGLGetManageTextureParameters(context); }

        /// <summary>
        /// Initializes a sampler's state and texture object handle. (Cg v1.4) 
        /// </summary>
        /// <param name="param">The sampler parameter that will be set.</param>
        /// <param name="texobj">An OpenGL texture object name to which the parameter will be set.</param>
        public static void SetupSampler(CgParameter param, int texobj)
        { cgGLSetupSampler(param, texobj); }

        /// <summary>
        /// Registers graphics pass states for CgFX files. (Cg v1.4)
        /// </summary>
        /// <param name="context">The context in which to register the states.</param>
        public static void RegisterStates(CgContext context)
        { cgGLRegisterStates(context); }

        #endregion

        #region Buffer Managment Functions

        /// <summary>
        /// Creates an OpenGL buffer object. (Cg v2.0)
        /// </summary>
        /// <param name="context">The context to which the new buffer will be added.</param>
        /// <param name="size">The length in bytes of the buffer to create.</param>
        /// <param name="data">The inital data to be copied into the buffer. NULL will fill the buffer with zero.</param>
        /// <param name="bufferUsage">One of the usage flags specified as valid for glBufferData. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.</param>
        /// <returns>Returns a CgBuffer handle on success. Returns NULL if any error occurs.</returns>
        public static CgBuffer CreateBuffer<T2>(CgContext context, int size, [In, Out] T2[] data, int bufferUsage) where T2 : struct
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return (cgGLCreateBuffer(context, size, handle.AddrOfPinnedObject(), bufferUsage));
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Creates an OpenGL buffer object. (Cg v2.0)
        /// </summary>
        /// <param name="context">The context to which the new buffer will be added.</param>
        /// <param name="size">The length in bytes of the buffer to create.</param>
        /// <param name="data">The inital data to be copied into the buffer. NULL will fill the buffer with zero.</param>
        /// <param name="bufferUsage">One of the usage flags specified as valid for glBufferData. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.</param>
        /// <returns>Returns a CgBuffer handle on success. Returns NULL if any error occurs.</returns>
        public static CgBuffer CreateBuffer<T2>(CgContext context, int size, [In, Out] T2[,] data, int bufferUsage) where T2 : struct
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return (cgGLCreateBuffer(context, size, handle.AddrOfPinnedObject(), bufferUsage));
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Creates an OpenGL buffer object. (Cg v2.0)
        /// </summary>
        /// <param name="context">The context to which the new buffer will be added.</param>
        /// <param name="size">The length in bytes of the buffer to create.</param>
        /// <param name="data">The inital data to be copied into the buffer. NULL will fill the buffer with zero.</param>
        /// <param name="bufferUsage">One of the usage flags specified as valid for glBufferData. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.</param>
        /// <returns>Returns a CgBuffer handle on success. Returns NULL if any error occurs.</returns>
        public static CgBuffer CreateBuffer<T2>(CgContext context, int size, [In, Out] T2[,,] data, int bufferUsage) where T2 : struct
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return (cgGLCreateBuffer(context, size, handle.AddrOfPinnedObject(), bufferUsage));
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Creates an OpenGL buffer object. (Cg v2.0)
        /// </summary>
        /// <param name="context">The context to which the new buffer will be added.</param>
        /// <param name="size">The length in bytes of the buffer to create.</param>
        /// <param name="data">The inital data to be copied into the buffer. NULL will fill the buffer with zero.</param>
        /// <param name="bufferUsage">One of the usage flags specified as valid for glBufferData. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.</param>
        /// <returns>Returns a CgBuffer handle on success. Returns NULL if any error occurs.</returns>
        public static CgBuffer CreateBuffer<T2>(CgContext context, int size, [In, Out] ref T2 data, int bufferUsage) where T2 : struct
        {
            GCHandle handle = GCHandle.Alloc((T2)data, GCHandleType.Pinned);

            try
            {
                return (cgGLCreateBuffer(context, size, handle.AddrOfPinnedObject(), bufferUsage));
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Returns the OpenGL buffer object associated with a buffer. (Cg v2.0)
        /// </summary>
        /// <param name="buffer">The buffer for which the associated OpenGL buffer object will be retrieved.</param>
        /// <returns>Returns the OpenGL buffer object associated with buffer. Returns 0 if an error occurs.</returns>
        /// <example>GLuint id = cgGLGetBufferObject(myBuffer);</example>
        public static int GetBufferObject(CgBuffer buffer)
        { return cgGLGetBufferObject(buffer); }

        #endregion

        #region Helpers

        public static void AutoSetDebugMode()
        {
#if DEBUG
            cgGLSetDebugMode(Cg.True);
#else
            cgGLSetDebugMode(Cg.False);
#endif
        }

        #endregion
    }
}
