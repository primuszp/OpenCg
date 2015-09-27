using System;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace OpenCg.Graphics.OpenGL
{
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public static partial class CgGL
    {
        #region Private Constants

        #region CgGL Native Library

        /// <summary>
        /// Specifies CgGL's native library archive.
        /// </summary>
        /// <remarks>
        /// Specifies cgGL.dll everywhere.
        /// </remarks>
        /// 
        private const string CgGLNativeLibrary = "cgGL.dll";

        #endregion

        #region Calling Convention

        /// <summary>
        /// Specifies the calling convention.
        /// </summary>
        /// <remarks>
        /// Specifies <see cref="CallingConvention.Cdecl" /> for Windows and Linux.
        /// </remarks>
        private const CallingConvention Convention = CallingConvention.Cdecl;

        #endregion

        #endregion

        #region Native Functions

        [DllImport(CgGLNativeLibrary,  CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLBindProgram(CgProgram program);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBuffer cgGLCreateBuffer(CgContext context, int size, IntPtr data, int bufferUsage);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLDisableClientState(CgParameter param);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLDisableProfile(CgProfile profile);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLDisableProgramProfiles(CgProgram program);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLDisableTextureParameter(CgParameter param);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLEnableClientState(CgParameter param);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLEnableProfile(CgProfile profile);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLEnableProgramProfiles(CgProgram program);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLEnableTextureParameter(CgParameter param);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGLGetBufferObject(CgBuffer buffer);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProfile cgGLGetLatestProfile(CgGLEnum profile_type);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGLGetManageTextureParameters(CgContext context);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [Out]double* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [Out]double[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [Out]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [Out]double* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [Out]double[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [Out]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [Out]float* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [Out]float[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [Out]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [Out]float* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [Out]float[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [Out]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterdc(CgParameter param, [In]double* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterdc(CgParameter param, [In]double[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterdc(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterdr(CgParameter param, [In]double* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterdr(CgParameter param, [In]double[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterdr(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterfc(CgParameter param, [In]float* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterfc(CgParameter param, [In]float[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterfc(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetMatrixParameterfr(CgParameter param, [In]float* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterfr(CgParameter param, [In]float[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetMatrixParameterfr(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGLGetOptimalOptions(CgProfile profile);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter1d(CgParameter param, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter1d(CgParameter param, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter1d(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter1f(CgParameter param, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter1f(CgParameter param, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter1f(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter2d(CgParameter param, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter2d(CgParameter param, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter2d(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter2f(CgParameter param, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter2f(CgParameter param, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter2f(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter3d(CgParameter param, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter3d(CgParameter param, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter3d(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter3f(CgParameter param, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter3f(CgParameter param, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter3f(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter4d(CgParameter param, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter4d(CgParameter param, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter4d(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameter4f(CgParameter param, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter4f(CgParameter param, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameter4f(CgParameter param, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray1d(CgParameter param, long offset, long nelements, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray1d(CgParameter param, long offset, long nelements, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray1d(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray1f(CgParameter param, long offset, long nelements, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray1f(CgParameter param, long offset, long nelements, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray1f(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray2d(CgParameter param, long offset, long nelements, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray2d(CgParameter param, long offset, long nelements, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray2d(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray2f(CgParameter param, long offset, long nelements, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray2f(CgParameter param, long offset, long nelements, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray2f(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray3d(CgParameter param, long offset, long nelements, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray3d(CgParameter param, long offset, long nelements, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray3d(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray3f(CgParameter param, long offset, long nelements, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray3f(CgParameter param, long offset, long nelements, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray3f(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray4d(CgParameter param, long offset, long nelements, [Out]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray4d(CgParameter param, long offset, long nelements, [Out]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray4d(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLGetParameterArray4f(CgParameter param, long offset, long nelements, [Out]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray4f(CgParameter param, long offset, long nelements, [Out]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLGetParameterArray4f(CgParameter param, long offset, long nelements, [Out]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGLGetProgramID(CgProgram program);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetDebugMode(CgBool debug);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetManageTextureParameters(CgContext context, bool flag);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGLGetTextureEnum(CgParameter param);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGLGetTextureParameter(CgParameter param);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgGLIsProfileSupported(CgProfile profile);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgGLIsProgramLoaded(CgProgram program);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLLoadProgram(CgProgram program);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLRegisterStates(CgContext context);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [In]double* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [In]double[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArraydc(CgParameter param, long offset, long nelements, [In]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [In]double* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [In]double[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArraydr(CgParameter param, long offset, long nelements, [In]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [In]float* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [In]float[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArrayfc(CgParameter param, long offset, long nelements, [In]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [In]float* v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [In]float[] v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterArrayfr(CgParameter param, long offset, long nelements, [In]IntPtr v);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterdc(CgParameter param, [In]double* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterdc(CgParameter param, [In]double[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterdc(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterdr(CgParameter param, [In]double* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterdr(CgParameter param, [In]double[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterdr(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterfc(CgParameter param, [In]float* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterfc(CgParameter param, [In]float[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterfc(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetMatrixParameterfr(CgParameter param, [In]float* matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterfr(CgParameter param, [In]float[] matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetMatrixParameterfr(CgParameter param, [In]IntPtr matrix);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetOptimalOptions(CgProfile profile);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter1d(CgParameter param, double x);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter1dv(CgParameter param, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter1dv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter1dv(CgParameter param, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter1f(CgParameter param, float x);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter1fv(CgParameter param, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter1fv(CgParameter param, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter1fv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter2d(CgParameter param, double x, double y);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter2dv(CgParameter param, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter2dv(CgParameter param, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter2dv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter2f(CgParameter param, float x, float y);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter2fv(CgParameter param, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter2fv(CgParameter param, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter2fv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter3d(CgParameter param, double x, double y, double z);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter3dv(CgParameter param, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter3dv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter3dv(CgParameter param, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter3f(CgParameter param, float x, float y, float z);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter3fv(CgParameter param, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter3fv(CgParameter param, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter3fv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter4d(CgParameter param, double x, double y, double z, double w);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter4dv(CgParameter param, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter4dv(CgParameter param, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter4dv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter4f(CgParameter param, float x, float y, float z, float w);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter4fv(CgParameter param, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameter4fv(CgParameter param, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameter4fv(CgParameter param, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray1d(CgParameter param, long offset, long nelements, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray1d(CgParameter param, long offset, long nelements, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray1d(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray1f(CgParameter param, long offset, long nelements, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray1f(CgParameter param, long offset, long nelements, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray1f(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray2d(CgParameter param, long offset, long nelements, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray2d(CgParameter param, long offset, long nelements, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray2d(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray2f(CgParameter param, long offset, long nelements, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray2f(CgParameter param, long offset, long nelements, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray2f(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray3d(CgParameter param, long offset, long nelements, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray3d(CgParameter param, long offset, long nelements, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray3d(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray3f(CgParameter param, long offset, long nelements, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray3f(CgParameter param, long offset, long nelements, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray3f(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray4d(CgParameter param, long offset, long nelements, [In]double* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray4d(CgParameter param, long offset, long nelements, [In]double[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray4d(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray4f(CgParameter param, long offset, long nelements, [In]IntPtr values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterArray4f(CgParameter param, long offset, long nelements, [In]float* values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterArray4f(CgParameter param, long offset, long nelements, [In]float[] values);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void cgGLSetParameterPointer(CgParameter param, int fsize, int type, int stride, [In]void* pointer);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetParameterPointer(CgParameter param, int fsize, int type, int stride, [In]IntPtr pointer);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetStateMatrixParameter(CgParameter param, int matrix, int transform);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetTextureParameter(CgParameter param, int texobj);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLSetupSampler(CgParameter param, int texobj);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLUnbindProgram(CgProfile profile);

        [DllImport(CgGLNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGLUnloadProgram(CgProgram program);

        #endregion
    }
}
