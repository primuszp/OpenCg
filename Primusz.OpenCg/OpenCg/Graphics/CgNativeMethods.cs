using System;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace OpenCg.Graphics
{
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public static partial class Cg
    {
        #region Private Constants

        #region Cg Native Library

        /// <summary>
        /// Specifies Cg's native library archive.
        /// </summary>
        /// <remarks>
        /// Specifies cg.dll everywhere.
        /// </remarks>
        private const string CgNativeLibrary = "cg.dll";

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

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgAddStateEnumerant(CgState state, [In] string name, int value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgCallStateResetCallback(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgCallStateSetCallback(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgCallStateValidateCallback(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCombinePrograms(int n, [In] CgProgram[] exeList);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCombinePrograms2([In] CgProgram program1, [In] CgProgram program2);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCombinePrograms3([In] CgProgram program1, [In] CgProgram program2, [In] CgProgram program3);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCombinePrograms4([In] CgProgram program1, [In] CgProgram program2, [In] CgProgram program3, [In] CgProgram program4);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCombinePrograms5([In] CgProgram program1, [In] CgProgram program2, [In] CgProgram program3, [In] CgProgram program4, [In] CgProgram program5);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgCompileProgram(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgConnectParameter(CgParameter from, CgParameter to);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgCopyEffect(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCopyProgram(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgCreateArraySamplerState(CgContext context, [In] string name, CgType type, int nelems);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgCreateArrayState(CgContext context, [In] string name, CgType type, int nelems);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBuffer cgCreateBuffer(CgContext context, int size, [In] IntPtr data, CgBufferUsage bufferUsage);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgContext cgCreateContext();

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgCreateEffect(CgContext context, [In] string code, [In] string[] args);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgCreateEffectAnnotation(CgEffect effect, [In]string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgCreateEffectFromFile(CgContext context, [In] string filename, [In] string[] args);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgCreateEffectParameter(CgEffect effect, [In]string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgCreateEffectParameterArray(CgEffect effect, [In] string name, CgType type, int length);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgCreateEffectParameterMultiDimArray(CgEffect effect, [In]string name, CgType type, int dim, [In]int[] lengths);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgObj cgCreateObj(CgContext context, CgEnum program_type, [In] string source, CgProfile profile, [In] string[] args);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgObj cgCreateObjFromFile(CgContext context, CgEnum program_type, [In] string source_file, CgProfile profile, [In] string[] args);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgCreateParameter(CgContext context, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgCreateParameterAnnotation(CgParameter parameter, [In]string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgCreateParameterArray(CgContext context, CgType type, int length);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgCreateParameterMultiDimArray(CgContext context, CgType type, int dim, [In] int[] lengths);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgPass cgCreatePass(CgTechnique technique, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgCreatePassAnnotation(CgPass pass, [In]string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCreateProgram(CgContext context, CgEnum program_type, [In] string program, CgProfile profile, [In] string entry, [In]string[] args);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgCreateProgramAnnotation(CgProgram program, [In]string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgCreateProgramFromEffect(CgEffect effect, CgProfile profile, [In] string entry, [In] string[] args);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgCreateProgramFromFile(CgContext context, CgEnum program_type, [In] string program_file, CgProfile profile, [In] string entry, [In] string[] args);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgCreateSamplerState(CgContext context, [In] string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgCreateSamplerStateAssignment(CgParameter parameter, CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgCreateState(CgContext context, [In] string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgCreateStateAssignment(CgPass pass, CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgCreateStateAssignmentIndex(CgPass pass, CgState state, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgTechnique cgCreateTechnique(CgEffect effect, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgCreateTechniqueAnnotation(CgTechnique technique, [In]string name, CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgDestroyBuffer(CgBuffer buffer);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgDestroyContext(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgDestroyEffect(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgDestroyObj(CgObj obj);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgDestroyParameter(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgDestroyProgram(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgDisconnectParameter(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgEvaluateProgram(CgProgram program, [In][Out] float[] values, int ncomps, int nx, int ny, int nz);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetAnnotationName(CgAnnotation annotation);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetAnnotationType(CgAnnotation annotation);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetArrayDimension(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetArrayParameter(CgParameter aparam, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetArraySize(CgParameter param, int dimension);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetArrayTotalSize(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetArrayType(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetAutoCompile(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBehavior cgGetBehavior(string behaviorString);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetBehaviorString(CgBehavior behavior);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetBoolAnnotationValues(CgAnnotation annotation, out int nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool[] cgGetBoolStateAssignmentValues(CgStateAssignment stateassignment, int[] nVals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int[] cgGetBooleanAnnotationValues(CgAnnotation annotation, out int nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetBufferSize(CgBuffer buffer);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IncludeCallbackFuncDelegate cgGetCompilerIncludeCallback(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetConnectedParameter(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetConnectedStateAssignmentParameter(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetConnectedToParameter(CgParameter param, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBehavior cgGetContextBehavior(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetDependentAnnotationParameter(CgAnnotation annotation, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetDependentProgramArrayStateAssignmentParameter(CgStateAssignment sa, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetDependentStateAssignmentParameter(CgStateAssignment stateassignment, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgDomain cgGetDomain(string domainString);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetDomainString(CgDomain domain);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgContext cgGetEffectContext(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetEffectName(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBuffer cgGetEffectParameterBuffer(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetEffectParameterBySemantic(CgEffect effect, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetEnum([In] string enum_string);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetEnumString(CgEnum en);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgError cgGetError();

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCallbackFuncDelegate cgGetErrorCallback();

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern ErrorHandlerFuncDelegate cgGetErrorHandler(out IntPtr data);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetErrorString(CgError error);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetFirstDependentParameter(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgGetFirstEffect(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetFirstEffectAnnotation(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetFirstEffectParameter(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgError cgGetFirstError();

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetFirstLeafEffectParameter(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetFirstLeafParameter(CgProgram program, CgEnum name_space);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetFirstParameter(CgProgram prog, CgEnum name_space);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetFirstParameterAnnotation(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgPass cgGetFirstPass(CgTechnique technique);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetFirstPassAnnotation(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgGetFirstProgram(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetFirstProgramAnnotation(CgProgram prog);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgGetFirstSamplerState(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgGetFirstSamplerStateAssignment(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgGetFirstState(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgGetFirstStateAssignment(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetFirstStructParameter(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgTechnique cgGetFirstTechnique(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetFirstTechniqueAnnotation(CgTechnique technique);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern float[] cgGetFloatAnnotationValues(CgAnnotation annotation, out int nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern float[] cgGetFloatStateAssignmentValues(CgStateAssignment stateassignment, int[] nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int[] cgGetIntAnnotationValues(CgAnnotation annotation, out int nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int[] cgGetIntStateAssignmentValues(CgStateAssignment stateassignment, int[] nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetLastErrorString(out CgError error);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetLastListing(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetLockingPolicy();

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetMatrixParameterOrder(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGetMatrixParameterdc(CgParameter param, [In] double[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGetMatrixParameterdr(CgParameter param, [In] double[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGetMatrixParameterfc(CgParameter param, [In] float[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGetMatrixParameterfr(CgParameter param, [In] float[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGetMatrixParameteric(CgParameter param, [In] int[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGetMatrixParameterir(CgParameter param, [In] int[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgGetMatrixSize(CgType type, out int nrows, out int ncols);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgGetNamedEffect(CgContext context, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetNamedEffectAnnotation(CgEffect effect, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetNamedEffectParameter(CgEffect effect, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetNamedParameter(CgProgram program, [In] string parameter);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetNamedParameterAnnotation(CgParameter param, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgPass cgGetNamedPass(CgTechnique technique, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetNamedPassAnnotation(CgPass pass, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetNamedProgramAnnotation(CgProgram prog, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetNamedProgramParameter(CgProgram program, CgEnum name_space, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgGetNamedSamplerState(CgContext context, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgGetNamedSamplerStateAssignment(CgParameter param, [In]string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgGetNamedState(CgContext context, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgGetNamedStateAssignment(CgPass pass, [In]string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetNamedStructParameter(CgParameter param, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetNamedSubParameter(CgParameter param, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgTechnique cgGetNamedTechnique(CgEffect effect, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetNamedTechniqueAnnotation(CgTechnique technique, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetNamedUserType(CgHandle handle, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgAnnotation cgGetNextAnnotation(CgAnnotation annotation);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgGetNextEffect(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetNextLeafParameter(CgParameter current);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetNextParameter(CgParameter current);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgPass cgGetNextPass(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgGetNextProgram(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgGetNextState(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgStateAssignment cgGetNextStateAssignment(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgTechnique cgGetNextTechnique(CgTechnique technique);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumConnectedToParameters(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumDependentAnnotationParameters(CgAnnotation annotation);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumDependentProgramArrayStateAssignmentParameters(CgStateAssignment sa);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumDependentStateAssignmentParameters(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumParentTypes(CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumProgramDomains(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumStateEnumerants(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumSupportedProfiles();

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetNumUserTypes(CgHandle handle);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgResource cgGetParameterBaseResource(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetParameterBaseType(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterBufferIndex(CgParameter parameter);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterBufferOffset(CgParameter parameter);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameterClass cgGetParameterClass(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameterClass cgGetParameterClassEnum(string pString);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetParameterClassString(CgParameterClass pc);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterColumns(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgContext cgGetParameterContext(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterDefaultValuedc(CgParameter param, int nelements, [Out]double[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterDefaultValuedr(CgParameter param, int nelements, [Out] double[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterDefaultValuefc(CgParameter param, int nelements, [Out] float[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterDefaultValuefr(CgParameter param, int nelements, [Out] float[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterDefaultValueic(CgParameter param, int nelements, [Out] int[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterDefaultValueir(CgParameter param, int nelements, [Out] int[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetParameterDirection(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgGetParameterEffect(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterIndex(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetParameterName(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterNamedType(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterOrdinalNumber(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgGetParameterProgram(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgResource cgGetParameterResource(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterResourceIndex(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetParameterResourceName(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterResourceSize(CgParameter parameter);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetParameterResourceType(CgParameter parameter);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterRows(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetParameterSemantic(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetParameterSettingMode(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetParameterType(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterValuedc(CgParameter param, int nelements, [Out] double[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterValuedr(CgParameter param, int nelements, [Out] double[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterValuefc(CgParameter param, int nelements, [Out] float[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterValuefr(CgParameter param, int nelements, [Out] float[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterValueic(CgParameter param, int nelements, [Out] int[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetParameterValueir(CgParameter param, int nelements, [Out] int[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern double* cgGetParameterValues(CgParameter param, CgEnum value_type, int* nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern double[] cgGetParameterValues(CgParameter param, CgEnum value_type, [In] int[] nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetParameterVariability(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetParentType(CgType type, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetPassName(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgGetPassProgram(CgPass pass, CgDomain domain);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgTechnique cgGetPassTechnique(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProfile cgGetProfile([In] string profile_string);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgDomain cgGetProfileDomain(CgProfile profile);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgGetProfileProperty(CgProfile profile, CgEnum query);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetProfileString(CgProfile profile);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBuffer cgGetProgramBuffer(CgProgram program, int bufferIndex);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetProgramBufferMaxIndex(CgProfile profile);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetProgramBufferMaxSize(CgProfile profile);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgContext cgGetProgramContext(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgDomain cgGetProgramDomain(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProfile cgGetProgramDomainProfile(CgProgram program, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgGetProgramDomainProgram(CgProgram program, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetProgramInput(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetProgramOptions(CgProgram prog);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetProgramOutput(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProfile cgGetProgramProfile(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProgram cgGetProgramStateAssignmentValue(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetProgramString(CgProgram program, CgEnum sourceType);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgResource cgGetResource([In] string resource_string);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetResourceString(CgResource resource);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetSamplerStateAssignmentParameter(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgGetSamplerStateAssignmentState(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetSamplerStateAssignmentValue(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgGetSemanticCasePolicy();

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetStateAssignmentIndex(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgPass cgGetStateAssignmentPass(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgState cgGetStateAssignmentState(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgContext cgGetStateContext(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetStateEnumerant(CgState state, int index, out int value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetStateEnumerantName(CgState state, int value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern int cgGetStateEnumerantValue(CgState state, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProfile cgGetStateLatestProfile(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetStateName(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern StateCallbackDelegate cgGetStateResetCallback(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern StateCallbackDelegate cgGetStateSetCallback(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetStateType(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern StateCallbackDelegate cgGetStateValidateCallback(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetString(CgEnum sname);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetStringAnnotationValue(CgAnnotation annotation);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetStringAnnotationValues(CgAnnotation ann, out int nvalues);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetStringParameterValue(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetStringStateAssignmentValue(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgProfile cgGetSupportedProfile(int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEffect cgGetTechniqueEffect(CgTechnique technique);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern string cgGetTechniqueName(CgTechnique technique);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameter cgGetTextureStateAssignmentValue(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetType([In] string type_string);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetTypeBase(CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgParameterClass cgGetTypeClass(CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgGetTypeSizes(CgType type, out int nrows, out int ncols);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgGetTypeString(CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgType cgGetUserType(CgHandle handle, int index);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsAnnotation(CgAnnotation annotation);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsContext(CgContext context);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsEffect(CgEffect effect);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsInterfaceType(CgType type);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsParameter(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsParameterGlobal(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsParameterReferenced(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsParameterUsed(CgParameter param, CgHandle handle);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsParentType(CgType parent, CgType child);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsPass(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsProfileSupported(CgProfile profile);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsProgram(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsProgramCompiled(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsState(CgState state);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsStateAssignment(CgStateAssignment stateassignment);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsTechnique(CgTechnique technique);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgIsTechniqueValidated(CgTechnique technique);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr cgMapBuffer(CgBuffer buffer, CgBufferAccess access);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgResetPassState(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetArraySize(CgParameter param, int size);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetAutoCompile(CgContext context, CgEnum autoCompileMode);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetBoolAnnotation(CgAnnotation annotation, CgBool value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetBoolArrayStateAssignment(CgStateAssignment stateassignment, [In]CgBool[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetBoolStateAssignment(CgStateAssignment stateassignment, CgBool value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetBufferData(CgBuffer buffer, int size, [In] IntPtr data);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetBufferSubData(CgBuffer buffer, int offset, int size, [In] IntPtr data);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetCompilerIncludeCallback(CgContext context, IncludeCallbackFuncDelegate func);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetCompilerIncludeFile(CgContext context, string name, string filename);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetCompilerIncludeString(CgContext context, string name, string source);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetContextBehavior(CgContext context, CgBehavior behavior);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetEffectName(CgEffect effect, [In] string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetEffectParameterBuffer(CgParameter param, CgBuffer buffer);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetErrorCallback(ErrorCallbackFuncDelegate func);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetErrorHandler(ErrorHandlerFuncDelegate func, IntPtr data);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetFloatAnnotation(CgAnnotation annotation, float value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetFloatArrayStateAssignment(CgStateAssignment stateassignment, [In]float[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetFloatStateAssignment(CgStateAssignment stateassignment, float value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetIntAnnotation(CgAnnotation annotation, int value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetIntArrayStateAssignment(CgStateAssignment stateassignment, [In]int[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetIntStateAssignment(CgStateAssignment stateassignment, int value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetLastListing(CgHandle handle, string listing);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgSetLockingPolicy(CgEnum lockingPolicy);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetMatrixParameterdc(CgParameter param, [In] double[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetMatrixParameterdr(CgParameter param, [In] double[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetMatrixParameterfc(CgParameter param, [In] float[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetMatrixParameterfr(CgParameter param, [In] float[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetMatrixParameteric(CgParameter param, [In] int[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetMatrixParameterir(CgParameter param, [In] int[] matrix);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetMultiDimArraySize(CgParameter param, int[] sizes);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter1d(CgParameter param, double x);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter1dv(CgParameter param, [In] double[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter1f(CgParameter param, float x);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter1fv(CgParameter param, [In] float[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter1i(CgParameter param, int x);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter1iv(CgParameter param, [In] int[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter2d(CgParameter param, double x, double y);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter2dv(CgParameter param, [In] double[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter2f(CgParameter param, float x, float y);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter2fv(CgParameter param, [In] float[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter2i(CgParameter param, int x, int y);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter2iv(CgParameter param, [In] int[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter3d(CgParameter param, double x, double y, double z);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter3dv(CgParameter param, [In] double[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter3f(CgParameter param, float x, float y, float z);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter3fv(CgParameter param, [In] float[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter3i(CgParameter param, int x, int y, int z);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter3iv(CgParameter param, [In] int[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter4d(CgParameter param, double x, double y, double z, double w);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter4dv(CgParameter param, [In] double[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter4f(CgParameter param, float x, float y, float z, float w);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter4fv(CgParameter param, [In] float[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter4i(CgParameter param, int x, int y, int z, int w);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameter4iv(CgParameter param, [In] int[] v);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterSemantic(CgParameter param, [In] string semantic);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterSettingMode(CgContext context, CgEnum parameterSettingMode);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterValuedc(CgParameter param, int nelements, [In] double[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterValuedr(CgParameter param, int nelements, [In] double[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterValuefc(CgParameter param, int nelements, [In] float[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterValuefr(CgParameter param, int nelements, [In] float[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterValueic(CgParameter param, int nelements, [In] int[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterValueir(CgParameter param, int nelements, [In] int[] vals);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetParameterVariability(CgParameter param, CgEnum vary);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetPassProgramParameters(CgProgram prog);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetPassState(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetProgramBuffer(CgProgram program, int bufferIndex, CgBuffer buffer);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetProgramProfile(CgProgram prog, CgProfile profile);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetProgramStateAssignment(CgStateAssignment stateassignment, CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetSamplerState(CgParameter param);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetSamplerStateAssignment(CgStateAssignment stateassignment, CgParameter parameter);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgEnum cgSetSemanticCasePolicy(CgEnum casePolicy);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetStateCallbacks(CgState state, StateCallbackDelegate set, StateCallbackDelegate reset, StateCallbackDelegate validate);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetStateLatestProfile(CgState state, CgProfile profile);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetStringAnnotation(CgAnnotation annotation, [In] string value);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgSetStringParameterValue(CgParameter param, [In] string str);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetStringStateAssignment(CgStateAssignment stateassignment, [In]string name);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgSetTextureStateAssignment(CgStateAssignment stateassignment, CgParameter parameter);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgUnmapBuffer(CgBuffer buffer);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgUpdatePassParameters(CgPass pass);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern void cgUpdateProgramParameters(CgProgram program);

        [DllImport(CgNativeLibrary, CallingConvention = Convention), SuppressUnmanagedCodeSecurity]
        private static extern CgBool cgValidateTechnique(CgTechnique technique);

        #endregion
    }
}
