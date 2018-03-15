using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace OpenCg.Graphics
{
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public static partial class Cg
    {
        #region Public Constants

        public static CgBool True = 1;
        public static CgBool False = 0;
        public const int Version = 3100;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate CgBool StateCallbackDelegate(CgStateAssignment stateassignment);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorCallbackFuncDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorHandlerFuncDelegate(CgContext context, CgError error, IntPtr appdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void IncludeCallbackFuncDelegate(CgContext context, [In]string filename);

        #endregion Delegates

        #region Functions

        /// <summary>
        /// <para>cgAddStateEnumerant associates a given named integer enumerant value with a state definition.</para>
        /// <para>When that state is later used in a pass in an effect file, the value of the state assignment can optionally be given by providing a named enumerant defined with cgAddStateEnumerant.</para>
        /// <para>The state assignment will then take on the value provided when the enumerant was defined.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgAddStateEnumerant was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state to which to associate the name and value.</param>
        /// <param name="name">The name of the enumerant.</param>
        /// <param name="value">The value of the enumerant.</param>
        public static void AddStateEnumerant(CgState state, [In]string name, int value)
        { cgAddStateEnumerant(state, name, value); }

        /// <summary>
        /// <para>cgCallStateResetCallback calls the graphics state resetting callback function for the given state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgCallStateResetCallback was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <returns>Returns the boolean value returned by the callback function. It should be CG_TRUE upon success. Returns CG_TRUE if no callback function was defined.</returns>
        public static CgBool CallStateResetCallback(CgStateAssignment sa)
        { return cgCallStateResetCallback(sa); }

        /// <summary>
        /// <para>cgCallStateSetCallback calls the graphics state setting callback function for the given state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgCallStateSetCallback was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <returns>Returns the boolean value returned by the callback function. It should be CG_TRUE upon success. Returns CG_TRUE if no callback function was defined.</returns>
        public static CgBool CallStateSetCallback(CgStateAssignment sa)
        { return cgCallStateSetCallback(sa); }

        /// <summary>
        /// <para>cgCallStateValidateCallback calls the state validation callback function for the given state assignment.</para>
        /// <para>The validation callback will return CG_TRUE or CG_FALSE depending on whether the current hardware and driver support the graphics state set by the state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgCallStateValidateCallback was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <returns>Returns the boolean value returned by the validation function. It should be CG_TRUE upon success. Returns CG_TRUE if no callback function was defined.</returns>
        public static CgBool CallStateValidateCallback(CgStateAssignment sa)
        { return cgCallStateValidateCallback(sa); }

        /// <summary>
        /// <para>cgCombinePrograms will take a set of n programs and combine them into a single CgProgram.</para>
        /// <para>This allows a single call to BindProgram (instead of a BindProgram for each individual program) and provides optimizations between the combined set of program inputs and outputs.</para>
        /// <para>ERROR: CG_INVALID_DIMENSION_ERROR is generated if n less than or equal to 1 or n is greater than 3. CG_INVALID_PARAMETER_ERROR is generated if exeList is NULL. CG_INVALID_PROGRAM_HANDLE_ERROR is generated if one of the programs in exeList is invalid. The errors listed in cgCreateProgram might also be generated.</para>
        /// <para>VERSION: cgCombinePrograms was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="n">The number of program objects in exeList.</param>
        /// <param name="exeList">An array of two or more executable programs, each from a different domain.</param>
        /// <returns>Returns a handle to the newly created program on success. Returns NULL if an error occurs.</returns>
        public static CgProgram CombinePrograms(int n, [In]CgProgram[] exeList)
        { return cgCombinePrograms(n, exeList); }

        /// <summary>
        /// <para>cgCombinePrograms2 takes two programs from different domains and combines them into a single CgProgram.</para>
        /// <para>This is a convenience function for cgCombinePrograms.</para>
        /// <para>ERROR: The errors listed in cgCombinePrograms might be generated.</para>
        /// <para>VERSION: cgCombinePrograms2 was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="program1">An executable program from one domain.</param>
        /// <param name="program2">An executable program from a different domain.</param>
        /// <returns>Returns a handle to the newly created program on success. Returns NULL if an error occurs.</returns>
        public static CgProgram CombinePrograms2([In]CgProgram program1, [In]CgProgram program2)
        { return cgCombinePrograms2(program1, program2); }

        /// <summary>
        /// <para>cgCombinePrograms3 takes three programs from different domains and combines them into a single CgProgram.</para>
        /// <para>This is a convenience function for cgCombinePrograms.</para>
        /// <para>ERROR: The errors listed in cgCombinePrograms might be generated.</para>
        /// <para>VERSION: cgCombinePrograms3 was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="program1">An executable program from one domain.</param>
        /// <param name="program2">An executable program from a second domain.</param>
        /// <param name="program3">An executable program from a third domain.</param>
        /// <returns>Returns a handle to the newly created program on success. Returns NULL if an error occurs.</returns>
        public static CgProgram CombinePrograms3([In]CgProgram program1, [In]CgProgram program2, [In]CgProgram program3)
        { return cgCombinePrograms3(program1, program2, program3); }

        /// <summary>
        /// <para>cgCombinePrograms4 takes four programs from different domains and combines them into a single CgProgram.</para>
        /// <para>This is a convenience function for cgCombinePrograms.</para>
        /// <para>ERROR: The errors listed in cgCombinePrograms might be generated.</para>
        /// <para>VERSION: cgCombinePrograms4 was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="program1">An executable program from one domain.</param>
        /// <param name="program2">An executable program from a second domain.</param>
        /// <param name="program3">An executable program from a third domain.</param>
        /// <param name="program4">An executable program from a fourth domain.</param>
        /// <returns>Returns a handle to the newly created program on success. Returns NULL if an error occurs.</returns>
        public static CgProgram CombinePrograms4([In]CgProgram program1, [In]CgProgram program2, [In]CgProgram program3, [In]CgProgram program4)
        { return cgCombinePrograms4(program1, program2, program3, program4); }

        /// <summary>
        /// <para>cgCombinePrograms5 takes five programs from different domains and combines them into a single CgProgram.</para>
        /// <para>This is a convenience function for cgCombinePrograms.</para>
        /// <para>ERROR: The errors listed in cgCombinePrograms might be generated.</para>
        /// <para>VERSION: cgCombinePrograms5 was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="program1">An executable program from one domain.</param>
        /// <param name="program2">An executable program from a second domain.</param>
        /// <param name="program3">An executable program from a third domain.</param>
        /// <param name="program4">An executable program from a fourth domain.</param>
        /// <param name="program5">An executable program from a fifth domain.</param>
        /// <returns>Returns a handle to the newly created program on success. Returns NULL if an error occurs.</returns>
        public static CgProgram CombinePrograms5([In]CgProgram program1, [In]CgProgram program2, [In]CgProgram program3, [In]CgProgram program4, [In]CgProgram program5)
        { return cgCombinePrograms5(program1, program2, program3, program4, program5); }

        /// <summary>
        /// <para>cgCompileProgram compiles the specified Cg program for its target profile.</para>
        /// <para>A program must be compiled before it can be loaded (by the API-specific part of the runtime).</para>
        /// <para>It must also be compiled before its parameters can be inspected.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCompileProgram was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program object to compile.</param>
        public static void CompileProgram(CgProgram program)
        { cgCompileProgram(program); }

        /// <summary>
        /// <para>cgConnectParameter connects a source (from) param to a destination  (to) param.</para>
        /// <para>The resulting connection forces the value and variability of the destination param to be identical to the source param.</para>
        /// <para>A source param may be connected to multiple destination parameters but there may only be one source param per destination param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if either of the from or to parameters are invalid handles. CG_PARAMETER_IS_NOT_SHARED_ERROR is generated if the source param is a program param. CG_BIND_CREATES_CYCLE_ERROR is generated if the connection will result in a cycle. CG_PARAMETERS_DO_NOT_MATCH_ERROR is generated if the parameters do not have the same type or the topologies do not match. CG_ARRAY_TYPES_DO_NOT_MATCH_ERROR is generated if the type of two arrays being connected do not match. CG_ARRAY_DIMENSIONS_DO_NOT_MATCH_ERROR is generated if the dimensions of two arrays being connected do not match.</para>
        /// <para>VERSION: cgConnectParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="from">The source param.</param>
        /// <param name="to">The destination param.</param>
        public static void ConnectParameter(CgParameter from, CgParameter to)
        { cgConnectParameter(from, to); }

        /// <summary>
        /// <para>cgCopyEffect creates a new effect object that is a copy of effect and adds it to the same context as effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgCopyEffect was introduced in Cg 2.0. cgCopyEffect is not operational as of Cg 3.0.</para>
        /// </summary>
        /// <param name="effect">The effect object to be copied.</param>
        /// <returns>Returns a copy of effect on success. Returns NULL if effect is invalid or the copy fails.</returns>
        public static CgEffect CopyEffect(CgEffect effect)
        { return cgCopyEffect(effect); }

        /// <summary>
        /// <para>cgCopyProgram creates a new program object that is a copy of program and adds it to the same context as program.</para>
        /// <para>cgCopyProgram is useful for creating a new instance of a program whose param properties have been modified by the run-time API.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgCopyProgram was introduced in Cg 1.1. cgCopyProgram is operational as of Cg 3.0.</para>
        /// </summary>
        /// <param name="program">The program object to copy.</param>
        /// <returns>Returns a copy of program on success. Returns NULL if program is invalid or the copy fails.</returns>
        public static CgProgram CopyProgram(CgProgram program)
        { return cgCopyProgram(program); }

        /// <summary>
        /// <para>cgCreateArraySamplerState adds a new array-typed sampler state definition to context.</para>
        /// <para>All state in sampler_state blocks must have been defined ahead of time via a call to cgCreateSamplerState or cgCreateArraySamplerState before adding an effect file to the context.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_PARAMETER_ERROR is generated if name is NULL or not a valid identifier, if type is not a simple scalar, vector, or matrix-type, or if nelements is not a positive number.</para>
        /// <para>VERSION: cgCreateArraySamplerState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context in which to define the sampler state.</param>
        /// <param name="name">The name of the new sampler state.</param>
        /// <param name="type">The type of the new sampler state.</param>
        /// <param name="nelements">The number of elements in the array.</param>
        /// <returns>Returns a handle to the newly created CGstate. Returns NULL if there is an error.</returns>
        public static CgState CreateArraySamplerState(CgContext context, [In]string name, CgType type, int nelements)
        { return cgCreateArraySamplerState(context, name, type, nelements); }

        /// <summary>
        /// <para>cgCreateArrayState adds a new array-typed state definition to  context.</para>
        /// <para>Before a CgFX file is added to a context, all state assignments in the file must have previously been defined via a call to cgCreateState or cgCreateArrayState.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_PARAMETER_ERROR is generated if name is NULL or not a valid identifier, if type is not a simple scalar, vector, or matrix-type, or if nelements is not a positive number.</para>
        /// <para>VERSION: cgCreateArrayState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context in which to define the state.</param>
        /// <param name="name">The name of the new state.</param>
        /// <param name="type">The type of the new state.</param>
        /// <param name="nelements">The number of elements in the array.</param>
        /// <returns>Returns a handle to the newly created CGstate. Returns NULL if there is an error.</returns>
        public static CgState CreateArrayState(CgContext context, [In]string name, CgType type, int nelements)
        { return cgCreateArrayState(context, name, type, nelements); }

        /// <summary>
        /// <para>cgCreateBuffer creates a runtime managed Cg buffer object.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgCreateBuffer was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="context">The context to which the new buffer will be added.</param>
        /// <param name="size">The length in bytes of the buffer to create.</param>
        /// <param name="data">Pointer to inital buffer data. NULL will fill the buffer with zero.</param>
        /// <param name="bufferUsage">Indicates the intended usage method of the buffer.</param>
        /// <returns>Returns a CgBuffer handle on success. Returns NULL if an error occurs.</returns>
        public static CgBuffer CreateBuffer(CgContext context, int size, [In]IntPtr data, CgBufferUsage bufferUsage)
        { return cgCreateBuffer(context, size, data, bufferUsage); }

        /// <summary>
        /// <para>cgCreateContext creates a Cg context object and returns its handle.</para>
        /// <para>A Cg context is a container for Cg programs.</para>
        /// <para>All Cg programs must be added to a Cg context.</para>
        /// <para>ERROR: CG_MEMORY_ALLOC_ERROR is generated if a context couldn't be created.</para>
        /// <para>VERSION: cgCreateContext was introduced in Cg 1.1.</para>
        /// </summary>
        /// <returns>Returns a valid CgContext on success. Returns NULL if context creation fails.</returns>
        public static CgContext CreateContext()
        { return cgCreateContext(); }

        /// <summary>
        /// <para>cgCreateEffect generates a new CGeffect object and adds it to the specified Cg context.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCreateEffect was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context to which the new effect will be added.</param>
        /// <param name="source">A string containing the effect's source source.</param>
        /// <param name="args">If args is not NULL it is assumed to be an array of NULL-terminated  strings that will be passed directly to the compiler as arguments.  The last value of the array must be a NULL.</param>
        /// <returns>Returns a CGeffect handle on success. Returns NULL if an error occurs.</returns>
        public static CgEffect CreateEffect(CgContext context, [In]string source, [In]string[] args)
        { return cgCreateEffect(context, source, args); }

        /// <summary>
        /// <para>cgCreateEffectAnnotation adds a new annotation to the effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect. CG_DUPLICATE_NAME_ERROR is generated if name is already used by an annotation for this effect. CG_INVALID_ENUMERANT_ERROR is generated if type is not CG_INT, CG_FLOAT, CG_BOOL, or CG_STRING.</para>
        /// <para>VERSION: cgCreateEffectAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect to which the new annotation will be added.</param>
        /// <param name="name">The name of the new annotation.</param>
        /// <param name="type">The type of the new annotation.</param>
        /// <returns>Returns the new CGannotation handle on success. Returns NULL if an error occurs.</returns>
        public static CgAnnotation CreateEffectAnnotation(CgEffect effect, [In]string name, CgType type)
        { return cgCreateEffectAnnotation(effect, name, type); }

        /// <summary>
        /// <para>cgCreateEffectFromFile generates a new CGeffect object and adds it to the specified Cg context.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_FILE_READ_ERROR is generated if the given filename cannot be read. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCreateEffectFromFile was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context to which the new effect will be added.</param>
        /// <param name="filename">Name of a file that contains the effect's source source.</param>
        /// <param name="args">If args is not NULL it is assumed to be an array of NULL-terminated  strings that will be passed directly to the compiler as arguments.  The last value of the array must be a NULL.</param>
        /// <returns>Returns a CGeffect handle on success. Returns NULL if an error occurs.</returns>
        public static CgEffect CreateEffectFromFile(CgContext context, [In]string filename, [In]string[] args)
        { return cgCreateEffectFromFile(context, filename, args); }

        /// <summary>
        /// <para>cgCreateEffectParameter adds a new param to the specified effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect. CG_INVALID_VALUE_TYPE_ERROR is generated if type is invalid.</para>
        /// <para>VERSION: cgCreateEffectParameter was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect to which the new param will be added.</param>
        /// <param name="name">The name of the new param.</param>
        /// <param name="type">The type of the new param.</param>
        /// <returns>Returns the handle to the new param.</returns>
        public static CgParameter CreateEffectParameter(CgEffect effect, [In]string name, CgType type)
        { return cgCreateEffectParameter(effect, name, type); }

        /// <summary>
        /// <para>cgCreateEffectParameterArray adds a new array param to the specificed effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect. CG_INVALID_VALUE_TYPE_ERROR is generated if type is invalid.</para>
        /// <para>VERSION: cgCreateEffectParameterArray was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect to which the new param will be added.</param>
        /// <param name="name">The name of the new param.</param>
        /// <param name="type">The type of the new param.</param>
        /// <param name="length">The size of the array.</param>
        /// <returns>Returns the handle to the new array param on success. Returns NULL if an error occurs.</returns>
        public static CgParameter CreateEffectParameterArray(CgEffect effect, [In]string name, CgType type, int length)
        { return cgCreateEffectParameterArray(effect, name, type, length); }

        /// <summary>
        /// <para>cgCreateEffectParameterMultiDimArray adds a new multidimensional array param to the specified effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect. CG_INVALID_VALUE_TYPE_ERROR is generated if type is invalid.</para>
        /// <para>VERSION: cgCreateEffectParameterMultiDimArray was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect to which the new param will be added.</param>
        /// <param name="name">The name of the new param.</param>
        /// <param name="type">The type of the new param.</param>
        /// <param name="dim">The dimension of the array.</param>
        /// <param name="lengths">The sizes for each dimension of the array.</param>
        /// <returns>Returns the handle of the new param on success. Returns NULL if an error occurs.</returns>
        public static CgParameter CreateEffectParameterMultiDimArray(CgEffect effect, [In]string name, CgType type, int dim, [In]int[] lengths)
        { return cgCreateEffectParameterMultiDimArray(effect, name, type, dim, lengths); }

        /// <summary>
        /// <para>cgCreateObj creates a new CGobj which is a source source object similar to a .obj or .o in C/C++ programming where various forms of data can be  extracted.</para>
        /// <para>This can be used, for example, to create user defined data types from a Cg source string.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_ENUMERANT_ERROR is generated if program_type is not CG_SOURCE or CG_OBJECT. CG_UNKNOWN_PROFILE_ERROR is generated if profile is not a supported profile. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCreateObj was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="context">The context to which the new object will be added. </param>
        /// <param name="program_type">An enumerant describing the contents of the source string. The following enumerants are allowed: CG_SOURCE - source contains Cg source code. CG_OBJECT - source contains object code that resulted from the precompilation of some Cg source code.</param>
        /// <param name="source">A string containing either the programs source or object code. See program_type for more information. </param>
        /// <param name="profile">The profile enumerant for the program.</param>
        /// <param name="args">If args is not NULL it is assumed to be an array of NULL-terminated strings that will be passed directly to the compiler as arguments. The last value of the array must be a NULL.</param>
        /// <returns>Returns a CGobj handle on success. Returns NULL if an error occurs.</returns>
        public static CgObj CreateObj(CgContext context, CgEnum program_type, [In]string source, CgProfile profile, [In]string[] args)
        { return cgCreateObj(context, program_type, source, profile, args); }

        /// <summary>
        /// <para>cgCreateObjFromFile creates a new CGobj which is a source source object similar to a.</para>
        /// <para>obj or.</para>
        /// <para>o in C/C++ programming where various forms of data can be  extracted.</para>
        /// <para>This can be used, for example, to create user defined data types from a Cg source string.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_ENUMERANT_ERROR is generated if program_type is not CG_SOURCE or CG_OBJECT. CG_UNKNOWN_PROFILE_ERROR is generated if profile is not a supported profile. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCreateObjFromFile was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="context">The context to which the new object will be added. </param>
        /// <param name="program_type">An enumerant describing the contents of the source string. The following enumerants are allowed: CG_SOURCE - source contains Cg source code. CG_OBJECT - source contains object code that resulted from the precompilation of some Cg source code.</param>
        /// <param name="source_file">Name of a file containing source or object code. See program_type for more information. </param>
        /// <param name="profile">The profile enumerant for the program.</param>
        /// <param name="args">If args is not NULL it is assumed to be an array of NULL-terminated strings that will be passed directly to the compiler as arguments. The last value of the array must be a NULL.</param>
        /// <returns>Returns a CGobj handle on success. Returns NULL if an error occurs.</returns>
        public static CgObj CreateObjFromFile(CgContext context, CgEnum program_type, [In]string source_file, CgProfile profile, [In]string[] args)
        { return cgCreateObjFromFile(context, program_type, source_file, profile, args); }

        /// <summary>
        /// <para>cgCreateParameter creates context level shared parameters.</para>
        /// <para>These parameters are primarily used by connecting them to one or more  program parameters with cgConnectParameter.</para>
        /// <para>ERROR: CG_INVALID_VALUE_TYPE_ERROR is generated if type is invalid. CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgCreateParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="context">The context to which the new param will be added.</param>
        /// <param name="type">The type of the new param.</param>
        /// <returns>Returns the handle to the new param.</returns>
        public static CgParameter CreateParameter(CgContext context, CgType type)
        { return cgCreateParameter(context, type); }

        /// <summary>
        /// <para>cgCreateParameterAnnotation adds a new annotation to the specified param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_DUPLICATE_NAME_ERROR is generated if name is already used by an annotation for this param. CG_INVALID_ENUMERANT_ERROR is generated if type is not CG_INT, CG_FLOAT, CG_BOOL, or CG_STRING.</para>
        /// <para>VERSION: cgCreateParameterAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="parm">The param to which the new annotation will be added.</param>
        /// <param name="name">The name of the new annotation.</param>
        /// <param name="type">The type of the new annotation.</param>
        /// <returns>Returns the new CGannotation handle on success. Returns NULL if an error occurs.</returns>
        public static CgAnnotation CreateParameterAnnotation(CgParameter param, [In]string name, CgType type)
        { return cgCreateParameterAnnotation(param, name, type); }

        /// <summary>
        /// <para>cgCreateParameterArray creates context level shared param arrays.</para>
        /// <para>These parameters are primarily used by connecting them to one or more  program param arrays with cgConnectParameter.</para>
        /// <para>ERROR: CG_INVALID_VALUE_TYPE_ERROR is generated if type is invalid. CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgCreateParameterArray was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="context">The context to which the new param will be added.</param>
        /// <param name="type">The type of the new param.</param>
        /// <param name="length">The length of the array being created.</param>
        /// <returns>Returns the handle to the new param array.</returns>
        public static CgParameter CreateParameterArray(CgContext context, CgType type, int length)
        { return cgCreateParameterArray(context, type, length); }

        /// <summary>
        /// <para>cgCreateParameterMultiDimArray creates context level shared  multi-dimensional param arrays.</para>
        /// <para>These parameters are primarily used by connecting them to one or more  program param arrays with cgConnectParameter.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_VALUE_TYPE_ERROR is generated if type is invalid.</para>
        /// <para>VERSION: cgCreateParameterMultiDimArray was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="context">The context to which the new param will be added.</param>
        /// <param name="type">The type of the new param.</param>
        /// <param name="dim">The dimension of the multi-dimensional array.</param>
        /// <param name="lengths">An array of length values, one for each dimension of the array to be created.</param>
        /// <returns>Returns the handle to the new param array.</returns>
        public static CgParameter CreateParameterMultiDimArray(CgContext context, CgType type, int dim, [In]int[] lengths)
        { return cgCreateParameterMultiDimArray(context, type, dim, lengths); }

        /// <summary>
        /// <para>cgCreatePass adds a new pass to the specified technique.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgCreatePass was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="tech">The technique to which the new pass will be added.</param>
        /// <param name="name">The name of the new pass.</param>
        /// <returns>Returns the handle to the new pass on success. Returns NULL if an error occurs.</returns>
        public static CgPass CreatePass(CgTechnique tech, [In]string name)
        { return cgCreatePass(tech, name); }

        /// <summary>
        /// <para>cgCreatePassAnnotation adds a new annotation to a pass.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass. CG_DUPLICATE_NAME_ERROR is generated if name is already used by an annotation for this pass. CG_INVALID_ENUMERANT_ERROR is generated if type is not CG_INT, CG_FLOAT, CG_BOOL, or CG_STRING.</para>
        /// <para>VERSION: cgCreatePassAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="pass">The pass to which the new annotation will be added.</param>
        /// <param name="name">The name of the new annotation.</param>
        /// <param name="type">The type of the new annotation.</param>
        /// <returns>Returns the new CGannotation handle on success. Returns NULL if an error occurs.</returns>
        public static CgAnnotation CreatePassAnnotation(CgPass pass, [In]string name, CgType type)
        { return cgCreatePassAnnotation(pass, name, type); }

        /// <summary>
        /// <para>cgCreateProgram generates a new CgProgram object and adds it to the specified Cg context.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_ENUMERANT_ERROR is generated if program_type is not CG_SOURCE or CG_OBJECT. CG_UNKNOWN_PROFILE_ERROR is generated if profile is not a supported profile. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCreateProgram was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="context">he context to which the new program will be added. </param>
        /// <param name="program_type">An enumerant describing the contents of the program string. The following enumerants are allowed: CG_SOURCE - source contains Cg source code. CG_OBJECT - source contains object code that resulted from the precompilation of some Cg source code.</param>
        /// <param name="program">A string containing either the programs source or object code. See program_type for more information.</param>
        /// <param name="profile">The profile enumerant for the program.</param>
        /// <param name="entry">The entry point to the program in the Cg source. If NULL, the entry point defaults to "main".</param>
        /// <param name="args">If args is not NULL it is assumed to be an array of NULL-terminated strings that will be passed directly to the compiler as arguments. The last value of the array must be a NULL.</param>
        /// <returns>Returns a CgProgram handle on success. Returns NULL if an error occurs.</returns>
        public static CgProgram CreateProgram(CgContext context, CgEnum program_type, [In]string program, CgProfile profile, [In]string entry, string[] args)
        { return cgCreateProgram(context, program_type, program, profile, entry, args); }

        /// <summary>
        /// <para>cgCreateProgramAnnotation adds a new annotation to a program.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_DUPLICATE_NAME_ERROR is generated if name is already used by an annotation for this program. CG_INVALID_ENUMERANT_ERROR is generated if type is not CG_INT, CG_FLOAT, CG_BOOL, or CG_STRING.</para>
        /// <para>VERSION: cgCreateProgramAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="program">The program to which the new annotation will be added.</param>
        /// <param name="name">The name of the new annotation.</param>
        /// <param name="type">The type of the new annotation.</param>
        /// <returns>Returns the new CGannotation handle on success. Returns NULL if an error occurs.</returns>
        public static CgAnnotation CreateProgramAnnotation(CgProgram program, [In]string name, CgType type)
        { return cgCreateProgramAnnotation(program, name, type); }

        /// <summary>
        /// <para>cgCreateProgramFromEffect generates a new CgProgram object and adds it to the effect's Cg context.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect. CG_UNKNOWN_PROFILE_ERROR is generated if profile is not a supported profile. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCreateProgramFromEffect was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect containing the program source source from which to create the program.</param>
        /// <param name="profile">The profile enumerant for the program.</param>
        /// <param name="entry">The entry point to the program in the Cg source.  If NULL, the entry point defaults to "main".</param>
        /// <param name="args">If args is not NULL it is assumed to be an array of NULL-terminated  strings that will be passed directly to the compiler as arguments.  The last value of the array must be a NULL.</param>
        /// <returns>Returns a CgProgram handle on success. Returns NULL if an error occurs.</returns>
        public static IntPtr CreateProgramFromEffect(CgEffect effect, CgProfile profile, [In]string entry, [In]string[] args)
        { return cgCreateProgramFromEffect(effect, profile, entry, args); }

        /// <summary>
        /// <para>cgCreateProgramFromFile  generates a new CgProgram  object and adds it to the specified Cg context.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_ENUMERANT_ERROR is generated if program_type is not CG_SOURCE or CG_OBJECT. CG_UNKNOWN_PROFILE_ERROR is generated if profile is not a supported profile. CG_COMPILER_ERROR is generated if compilation fails.</para>
        /// <para>VERSION: cgCreateProgramFromFile was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="context">The context to which the new program will be added.</param>
        /// <param name="program_type">An enumerant describing the contents of the program_file. The following enumerants are allowed: CG_SOURCE - source contains Cg source code. CG_OBJECT - source contains object code that resulted from the precompilation of some Cg source code.</param>
        /// <param name="program_file">Name of a file containing source or object code. See program_type for more information. </param>
        /// <param name="profile">The profile enumerant for the program. </param>
        /// <param name="entry">The entry point to the program in the Cg source. If NULL, the entry point defaults to "main". </param>
        /// <param name="args">If args is not NULL it is assumed to be an array of NULL-terminated strings that will be passed directly to the compiler as arguments. The last value of the array must be a NULL.</param>
        /// <returns>Returns a CgProgram handle on success. Returns NULL if an error occurs.</returns>
        public static CgProgram CreateProgramFromFile(CgContext context, CgEnum program_type, [In]string program_file, CgProfile profile, [In]string entry, string[] args)
        { return cgCreateProgramFromFile(context, program_type, program_file, profile, entry, args); }

        /// <summary>
        /// <para>cgCreateSamplerState adds a new sampler state definition to the context.</para>
        /// <para>When an effect file is added to the context, all state in sampler_state blocks must have already been defined via a call to cgCreateSamplerState or cgCreateArraySamplerState.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_PARAMETER_ERROR is generated if name is NULL or not a valid identifier, or if type is not a simple scalar, vector, or matrix-type. Array-typed state should be created with cgCreateArrayState.</para>
        /// <para>VERSION: cgCreateSamplerState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context in which to define the new sampler state.</param>
        /// <param name="name">The name of the new sampler state.</param>
        /// <param name="type">The type of the new sampler state.</param>
        /// <returns>Returns a handle to the newly created CGstate. Returns NULL if there is an error.</returns>
        public static CgState CreateSamplerState(CgContext context, [In]string name, CgType type)
        { return cgCreateSamplerState(context, name, type); }

        /// <summary>
        /// <para>cgCreateSamplerStateAssignment creates a new state assignment for the given state and sampler param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgCreateSamplerStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="param">The sampler param to which the new state assignment will be associated.</param>
        /// <param name="state">The state for which to create the new state assignment.</param>
        /// <returns>Returns the handle to the created sampler state assignment.</returns>
        public static CgStateAssignment CreateSamplerStateAssignment(CgParameter param, CgState state)
        { return cgCreateSamplerStateAssignment(param, state); }

        /// <summary>
        /// <para>cgCreateState adds a new state definition to the context.</para>
        /// <para>When a CgFX file is added to the context, all state assignments in the file must have already been defined via a call to cgCreateState or cgCreateArrayState.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_PARAMETER_ERROR is generated if name is NULL or not a valid identifier, or if type is not a simple scalar, vector, or matrix-type. Array-typed state should be created with cgCreateArrayState.</para>
        /// <para>VERSION: cgCreateState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context in which to define the new state.</param>
        /// <param name="name">The name of the new state.</param>
        /// <param name="type">The type of the new state.</param>
        /// <returns>Returns a handle to the newly created CGstate. Returns NULL if there is an error.</returns>
        public static CgState CreateState(CgContext context, [In]string name, CgType type)
        { return cgCreateState(context, name, type); }

        /// <summary>
        /// <para>cgCreateStateAssignment creates a state assignment for the specified pass.</para>
        /// <para>The new state assignment is appended to the pass' existing list of state assignments.</para>
        /// <para>If the state is actually a state array, the created state assignment is created for array index zero.</para>
        /// <para>Use cgCreateStateAssignmentIndex to create state assignments for other indices of an array state.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass. CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgCreateStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="pass">The pass in which to create the state assignment.</param>
        /// <param name="state">The state used to create the state assignment.</param>
        /// <returns>Returns the handle to the created state assignment. Returns NULL if an error occurs.</returns>
        public static CgStateAssignment CreateStateAssignment(CgPass pass, CgState state)
        { return cgCreateStateAssignment(pass, state); }

        /// <summary>
        /// <para>cgCreateStateAssignmentIndex creates a state assignment for the specified pass.</para>
        /// <para>The new state assignment is appended to the pass's existing list of state assignments.</para>
        /// <para>The state assignment is for the given index of for the specified array state.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass. CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state. If the index is negative or index is greater than or equal the number of elements for the state array, no error is generated but NULL is returned.</para>
        /// <para>VERSION: cgCreateStateAssignmentIndex was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="pass">The pass in which to create the state assignment.</param>
        /// <param name="state">The state array used to create the state assignment.</param>
        /// <param name="index">The index for the state array.</param>
        /// <returns>Returns the new state assignment handle. Returns NULL if an error occurs.</returns>
        public static CgStateAssignment CreateStateAssignmentIndex(CgPass pass, CgState state, int index)
        { return cgCreateStateAssignmentIndex(pass, state, index); }

        /// <summary>
        /// <para>cgCreateTechnique adds a new technique to the specified effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgCreateTechnique was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect to which the new technique will be added.</param>
        /// <param name="name">The name for the new technique.</param>
        /// <returns>Returns the handle to the new technique on success. Returns NULL if an error occurs.</returns>
        public static CgTechnique CreateTechnique(CgEffect effect, [In]string name)
        { return cgCreateTechnique(effect, name); }

        /// <summary>
        /// <para>cgCreateTechniqueAnnotation adds a new annotation to the technique.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique. CG_DUPLICATE_NAME_ERROR is generated if name is already used by an annotation for this technique. CG_INVALID_ENUMERANT_ERROR is generated if type is not CG_INT, CG_FLOAT, CG_BOOL, or CG_STRING.</para>
        /// <para>VERSION: cgCreateTechniqueAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="tech">The technique to which the new annotation will be added.</param>
        /// <param name="name">The name of the new annotation.</param>
        /// <param name="type">The type of the new annotation.</param>
        /// <returns>Returns the new CGannotation handle on success. Returns NULL if an error occurs.</returns>
        public static CgAnnotation CreateTechniqueAnnotation(CgTechnique tech, [In]string name, CgType type)
        { return cgCreateTechniqueAnnotation(tech, name, type); }

        /// <summary>
        /// <para>cgDestroyBuffer deletes a buffer.</para>
        /// <para>The buffer object is not actually destroyed until no more programs are bound to the buffer object and any pending use of the buffer has completed.</para>
        /// <para>However, the handle buffer no longer refers to the buffer object (although it may be subsequently allocated to a different created resource).</para>
        /// <para>ERROR: CG_INVALID_BUFFER_HANDLE_ERROR is generated if buffer is not a valid buffer.</para>
        /// <para>VERSION: cgDestroyBuffer was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="buffer">The buffer to delete.</param>
        public static void DestroyBuffer(CgBuffer buffer)
        { cgDestroyBuffer(buffer); }

        /// <summary>
        /// <para>cgDestroyContext deletes a Cg context object and all the programs it contains.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgDestroyContext was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="context">The context to be deleted.</param>
        public static void DestroyContext(CgContext context)
        { cgDestroyContext(context); }

        /// <summary>
        /// <para>cgDestroyEffect removes the specified effect object and all its associated data.</para>
        /// <para>Any CGeffect handles that reference this effect will become invalid after the effect is deleted.</para>
        /// <para>Likewise, all techniques, passes, and parameters contained in the effect also become invalid after the effect is destroyed.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgDestroyEffect was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect object to delete.</param>
        public static void DestroyEffect(CgEffect effect)
        { cgDestroyEffect(effect); }

        /// <summary>
        /// <para>cgDestroyObj removed the specified object and all its associated data.</para>
        /// <para>ERROR: CG_INVALID_OBJ_HANDLE_ERROR is generated if obj is not a valid object handle.</para>
        /// <para>VERSION: cgDestroyObj was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="obj">The object to delete.</param>
        public static void DestroyObj(CgObj obj)
        { cgDestroyObj(obj); }

        /// <summary>
        /// <para>cgDestroyParameter destroys parameters created with  cgCreateParameter,  cgCreateParameterArray, or cgCreateParameterMultiDimArray.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_NOT_ROOT_PARAMETER_ERROR is generated if the param isn't the top-level param of a struct or array that was created. CG_PARAMETER_IS_NOT_SHARED_ERROR is generated if param does not refer to a param created by one of the cgCreateParameter family of entry points. CG_CANNOT_DESTROY_PARAMETER_ERROR is generated if param is a source param in a connection made by cgConnectParameter. cgDisconnectParameter should be used before calling cgDestroyParameter in such a case.</para>
        /// <para>VERSION: cgDestroyParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param to destroy.</param>
        public static void DestroyParameter(CgParameter param)
        { cgDestroyParameter(param); }

        /// <summary>
        /// <para>cgDestroyProgram removes the specified program object and all its associated data.</para>
        /// <para>Any CgProgram variables that reference this program will become invalid after the program is deleted.</para>
        /// <para>Likewise, any objects contained by this program (e.g. CgParameter objects) will also become invalid after the program is deleted.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgDestroyProgram was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program object to delete.</param>
        public static void DestroyProgram(CgProgram program)
        { cgDestroyProgram(program); }

        /// <summary>
        /// <para>cgDisconnectParameter disconnects an existing connection made with cgConnectParameter between two parameters.</para>
        /// <para>Since a given param can only be connected to one source param, only the destination param is required as an argument to cgDisconnectParameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgDisconnectParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The destination param in the connection that will be disconnected.</param>
        public static void DisconnectParameter(CgParameter param)
        { cgDisconnectParameter(param); }

        /// <summary>
        /// <para>cgEvaluateProgram evaluates a Cg program at a set of regularly spaced points in one, two, or three dimensions.</para>
        /// <para>The program must have been compiled with the CG_PROFILE_GENERIC profile.</para>
        /// <para>The value returned from the program via the COLOR semantic is stored in the given buffer for each evaluation point, and any varying parameters to the program with POSITION semantic take on the (x,y,z) position over the range zero to one at which the program is evaluated at each point.</para>
        /// <para>The PSIZE semantic can be used to find the spacing between evaluating points.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_PROFILE_ERROR is generated if program's profile is not CG_PROFILE_GENERIC. CG_INVALID_PARAMETER_ERROR is generated if buf is NULL, any of nx, ny, or nz is less than zero, or ncomps is not 0, 1, 2, or 3.</para>
        /// <para>VERSION: cgEvaluateProgram was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="program">The program to be evalutated.</param>
        /// <param name="buf">Buffer in which to store the results of program evaluation.</param>
        /// <param name="ncomps">Number of components to store for each returned program value.</param>
        /// <param name="nx">Number of points at which to evaluate the program in the x direction.</param>
        /// <param name="ny">Number of points at which to evaluate the program in the y direction.</param>
        /// <param name="nz">Number of points at which to evaluate the program in the z direction.</param>
        public static void EvaluateProgram(CgProgram program, [In][Out]float[] buf, int ncomps, int nx, int ny, int nz)
        {
            cgEvaluateProgram(program, buf, ncomps, nx, ny, nz);
        }

        /// <summary>
        /// <para>cgEvaluateProgram evaluates a Cg program at a set of regularly spaced points in one, two, or three dimensions.</para>
        /// <para>The program must have been compiled with the CG_PROFILE_GENERIC profile.</para>
        /// <para>The value returned from the program via the COLOR semantic is stored in the given buffer for each evaluation point, and any varying parameters to the program with POSITION semantic take on the (x,y,z) position over the range zero to one at which the program is evaluated at each point.</para>
        /// <para>The PSIZE semantic can be used to find the spacing between evaluating points.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_PROFILE_ERROR is generated if program's profile is not CG_PROFILE_GENERIC. CG_INVALID_PARAMETER_ERROR is generated if buf is NULL, any of nx, ny, or nz is less than zero, or ncomps is not 0, 1, 2, or 3.</para>
        /// <para>VERSION: cgEvaluateProgram was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="program">The program to be evalutated.</param>
        /// <param name="ncomps">Number of components to store for each returned program value.</param>
        /// <param name="nx">Number of points at which to evaluate the program in the x direction.</param>
        /// <param name="ny">Number of points at which to evaluate the program in the y direction.</param>
        /// <param name="nz">Number of points at which to evaluate the program in the z direction.</param>
        /// <returns>Buffer in which to store the results of program evaluation.</returns>
        public static float[] EvaluateProgram(CgProgram program, int ncomps, int nx, int ny, int nz)
        {
            var retValue = new float[ncomps * nx * ny * nz];
            cgEvaluateProgram(program, retValue, ncomps, nx, ny, nz);
            return retValue;
        }

        /// <summary>
        /// <para>cgGetAnnotationName allows the application to retrieve the name of a annotation.</para>
        /// <para>This name can be used later to retrieve the annotation using cgGetNamedPassAnnotation, cgGetNamedParameterAnnotation, cgGetNamedTechniqueAnnotation, or cgGetNamedProgramAnnotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation.</para>
        /// <para>VERSION: cgGetAnnotationName was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation from which to get the name.</param>
        /// <returns>Returns the NULL-terminated name string for the annotation. Returns NULL if ann is invalid.</returns>
        public static string GetAnnotationName(CgAnnotation ann)
        { return Marshal.PtrToStringAnsi(cgGetAnnotationName(ann)); }

        /// <summary>
        /// <para>cgGetAnnotationType allows the application to retrieve the type of an annotation in a Cg effect.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation.</para>
        /// <para>VERSION: cgGetAnnotationType was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation from which to get the type.</param>
        /// <returns>Returns the type enumerant of ann. Returns CG_UNKNOWN_TYPE if an error occurs.</returns>
        public static CgType GetAnnotationType(CgAnnotation ann)
        { return cgGetAnnotationType(ann); }

        /// <summary>
        /// <para>cgGetArrayDimension returns the dimension of the array specified by param.</para>
        /// <para>cgGetArrayDimension is used when inspecting an array param in a program.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_ARRAY_PARAM_ERROR is generated if param is not an array param.</para>
        /// <para>VERSION: cgGetArrayDimension was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The array param handle.</param>
        /// <returns>Returns the dimension of param if param references an array. Returns 0 otherwise.</returns>
        public static int GetArrayDimension(CgParameter param)
        { return cgGetArrayDimension(param); }

        /// <summary>
        /// <para>cgGetArrayParameter returns the param of array param specified by index.</para>
        /// <para>cgGetArrayParameter is used when inspecting elements of an array param in a program.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_ARRAY_PARAM_ERROR is generated if param is not an array param. CG_OUT_OF_ARRAY_BOUNDS_ERROR is generated if index is outside the bounds of param.</para>
        /// <para>VERSION: cgGetArrayParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The array param handle.</param>
        /// <param name="index">The index into the array.</param>
        /// <returns>Returns the param at the specified index of param if param references an array, and the index is valid. Returns NULL otherwise.</returns>
        public static CgParameter GetArrayParameter(CgParameter param, int index)
        { return cgGetArrayParameter(param, index); }

        /// <summary>
        /// <para>cgGetArraySize returns the size of the given dimension of the array specified by param.</para>
        /// <para>cgGetArraySize is used when inspecting an array param in a program.</para>
        /// <para>ERROR: CG_INVALID_DIMENSION_ERROR is generated if dimension is less than 0. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetArraySize was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The array param handle.</param>
        /// <param name="dimension">The array dimension whose size will be returned.</param>
        /// <returns>Returns the size of param if param is an array. Returns 0 if param is not an array, or an error occurs.</returns>
        public static int GetArraySize(CgParameter param, int dimension)
        { return cgGetArraySize(param, dimension); }

        /// <summary>
        /// <para>cgGetArrayTotalSize returns the total number of elements of the array specified by param.</para>
        /// <para>The total number of elements is equal to the product of the size of each dimension of the array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetArrayTotalSize was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The array param handle.</param>
        /// <returns>Returns the total size of param if pararm is an array. Returns 0 if param is not an array, or if an error occurs.</returns>
        public static int GetArrayTotalSize(CgParameter param)
        { return cgGetArrayTotalSize(param); }

        /// <summary>
        /// <para>cgGetArrayType returns the type of the members of an array.</para>
        /// <para>If the given array is multi-dimensional, it will return the type of the members of the inner most array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_ARRAY_PARAM_ERROR is generated if param is not an array param.</para>
        /// <para>VERSION: cgGetArrayType was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The array param handle.</param>
        /// <returns>Returns the the type of the inner most array. Returns CG_UNKNOWN_TYPE if an error occurs.</returns>
        public static CgType GetArrayType(CgParameter param)
        { return cgGetArrayType(param); }

        /// <summary>
        /// <para>cgGetAutoCompile returns the auto-compile enumerant for context.</para>
        /// <para>See cgSetAutoCompile for more  information.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetAutoCompile was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Returns the auto-compile enumerant for context. Returns CG_UNKNOWN if context is not a valid context.</returns>
        public static CgEnum GetAutoCompile(CgContext context)
        { return cgGetAutoCompile(context); }

        /// <summary>
        /// <para>cgGetBehavior returns the enumerant assigned to a behavior name.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if behavior_string is NULL.</para>
        /// <para>VERSION: cgGetBehavior was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="behavior_string">A string containing the case-sensitive behavior name.</param>
        /// <returns>Returns the behavior enumerant associated with behavior_string. Returns CG_BEHAVIOR_UNKNOWN if behavior_string is NULL or if no CGbehavior is associated with the given string.</returns>
        public static CgBehavior GetBehavior(string behavior_string)
        { return cgGetBehavior(behavior_string); }

        /// <summary>
        /// <para>cgGetBehaviorString returns the behavior name associated with a given behavior enumerant.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetBehaviorString was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="behavior">The behavior enumerant.</param>
        /// <returns>Returns the behavior string associated with behavior. Returns NULL if behavior is not a valid CGbehavior.</returns>
        public static string GetBehaviorString(CgBehavior behavior)
        { return Marshal.PtrToStringAnsi(cgGetBehaviorString(behavior)); }

        /// <summary>
        /// <para>cgGetBoolAnnotationValues allows the application to  retrieve the value(s) of a boolean typed annotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL.</para>
        /// <para>VERSION: cgGetBoolAnnotationValues was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="ann">The annotation.</param>
        /// <param name="nvalues">Pointer to integer where the number of returned values will be stored.</param>
        /// <returns>Returns a pointer to an array of CgBool values. The number of values in the array is returned via the nvalues param. Returns NULL if no values are available. nvalues will be 0.</returns>
        public static CgBool[] GetBoolAnnotationValues(CgAnnotation ann, out int nvalues)
        {
            var values = cgGetBoolAnnotationValues(ann, out nvalues);

            if (nvalues > 0)
            {
                var retValue = new CgBool[nvalues];
                unsafe
                {
                    var ii = (int*)values;
                    for (int i = 0; i < nvalues; i++)
                    {
                        retValue[i] = ii[i] == True;
                    }
                }
                return retValue;
            }
            return null;
        }

        /// <summary>
        /// <para>cgGetBoolStateAssignmentValues allows the application to  retrieve the value(s) of a boolean typed state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a bool type.</para>
        /// <para>VERSION: cgGetBoolStateAssignmentValues was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <param name="nvalues">Pointer to integer where the number of returned values will be stored.</param>
        /// <returns>Returns a pointer to an array of CgBool values. The number of values in the array is returned via the nvalues param. Returns NULL if an error occurs or if no values are available. nvalues will be 0 in the latter case.</returns>
        public static CgBool[] GetBoolStateAssignmentValues(CgStateAssignment sa, int[] nvalues)
        { return cgGetBoolStateAssignmentValues(sa, nvalues); }

        /// <summary>
        /// <para>cgGetBooleanAnnotationValues is deprecated.</para>
        /// <para>Use cgGetBoolAnnotationValues instead.</para>
        /// </summary>
        [Obsolete("Use GetBoolAnnotationValues instead.")]
        public static int[] GetBooleanAnnotationValues(CgAnnotation ann, out int nvalues)
        { return cgGetBooleanAnnotationValues(ann, out nvalues); }

        /// <summary>
        /// <para>cgGetBufferSize returns the size in bytes of buffer.</para>
        /// <para>ERROR: CG_INVALID_BUFFER_HANDLE_ERROR is generated if buffer is not a valid buffer.</para>
        /// <para>VERSION: cgGetBufferSize was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="buffer">The buffer for which the size will be retrieved.</param>
        /// <returns>Returns the size in bytes of buffer. Returns -1 if an error occurs.</returns>
        public static int GetBufferSize(CgBuffer buffer)
        { return cgGetBufferSize(buffer); }

        /// <summary>
        /// <para>cgGetCompilerIncludeCallback returns the current callback function used for handing include statements.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetCompilerIncludeCallback was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="context">The context of the desired include callback function.</param>
        /// <returns>Returns the current include callback function. Returns NULL if no callback function is set.</returns>
        public static IncludeCallbackFuncDelegate GetCompilerIncludeCallback(CgContext context)
        { return cgGetCompilerIncludeCallback(context); }

        /// <summary>
        /// <para>Returns the source param to which param is connected.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetConnectedParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The destination param.</param>
        /// <returns>Returns the connected source param if param is connected to one. Returns NULL otherwise.</returns>
        public static CgParameter GetConnectedParameter(CgParameter param)
        { return cgGetConnectedParameter(param); }

        /// <summary>
        /// <para>cgGetConnectedStateAssignmentParameter returns the effect param from which a given state assignment's value is determined.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetConnectedStateAssignmentParameter was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="sa">A state assignment whose value is determined using an effect param.</param>
        /// <returns>Returns the effect param used by sa. Returns 0 if sa is not using a param for its value, if the state assignment is set to an expression, or if an error occurs.</returns>
        public static CgParameter GetConnectedStateAssignmentParameter(CgStateAssignment sa)
        { return cgGetConnectedStateAssignmentParameter(sa); }

        /// <summary>
        /// <para>Returns one of the destination parameters connected to param.</para>
        /// <para>cgGetNumConnectedToParameters should be used to determine the number of destination parameters connected to param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_OUT_OF_ARRAY_BOUNDS_ERROR is generated if index is less than 0 or greater than or equal to the number of parameters connected to param.</para>
        /// <para>VERSION: cgGetConnectedToParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The source param.</param>
        /// <param name="index">Since there may be multiple destination (to) parameters connected to  param, index is need to specify which one is returned.  index must be within the range of 0 to N - 1 where N is the number of connected destination parameters.</param>
        /// <returns>Returns one of the destination parameters connected to param. Returns NULL if an error occurs.</returns>
        public static CgParameter GetConnectedToParameter(CgParameter param, int index)
        { return cgGetConnectedToParameter(param, index); }

        /// <summary>
        /// <para>cgGetContextBehavior allows the application to retrieve the behavior enumerant for a context.</para>
        /// <para>The valid enumerants and their meanings are described in cgSetContextBehavior.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetContextBehavior was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="context">The context for which the behavior enumerant will be returned.</param>
        /// <returns>Returns the behavior enumerant for context. Returns CG_BEHAVIOR_UNKNOWN if an error occurs.</returns>
        public static CgBehavior GetContextBehavior(CgContext context)
        { return cgGetContextBehavior(context); }

        /// <summary>
        /// <para>Annotations in CgFX files may include references to one or more effect parameters on the right hand side of the annotation that are used for computing the annotation's value.</para>
        /// <para>cgGetDependentAnnotationParameter returns one of these parameters, as indicated by the given index.</para>
        /// <para>cgGetNumDependentAnnotationParameters can be used to determine the total number of such parameters.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_OUT_OF_ARRAY_BOUNDS_ERROR is generated if index is less than zero or greater than or equal to the number of dependent parameters, as returned by cgGetNumDependentAnnotationParameters.</para>
        /// <para>VERSION: cgGetDependentAnnotationParameter was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation handle.</param>
        /// <param name="index">The index of the param to return.</param>
        /// <returns>Returns a handle to the selected dependent annotation on success. Returns NULL if an error occurs.</returns>
        public static CgParameter GetDependentAnnotationParameter(CgAnnotation ann, int index)
        { return cgGetDependentAnnotationParameter(ann, index); }

        /// <summary>
        /// <para>State assignments in CgFX files may include references to an array indexed by an effect param (or expression) on the right hand side of the state assignment  that is used for computing the state assignment's value.</para>
        /// <para>Usually this array holds the compile statements of shader programs and by changing the index of the shader array, it's possible to switch to a different program or profile on-the-fly.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_OUT_OF_ARRAY_BOUNDS_ERROR is generated if index is less than zero or greater than or equal to the number of dependent parameters, as returned by cgGetNumDependentProgramArrayStateAssignmentParameters.</para>
        /// <para>VERSION: cgGetDependentProgramArrayStateAssignmentParameter was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <param name="index">The index of the param to return.</param>
        /// <returns>Returns a handle to the selected dependent param on success. Returns NULL if sa is not a program state assignment or an error occurs.</returns>
        public static CgParameter GetDependentProgramArrayStateAssignmentParameter(CgStateAssignment sa, int index)
        { return cgGetDependentProgramArrayStateAssignmentParameter(sa, index); }

        /// <summary>
        /// <para>State assignments in CgFX files may include references to one or more effect parameters on the right hand side of the state assignment that are used for computing the state assignment's value.</para>
        /// <para>cgGetDependentStateAssignmentParameter returns one of these parameters, as indicated by the given index.</para>
        /// <para>cgGetNumDependentStateAssignmentParameters can be used to determine the total number of such parameters.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_OUT_OF_ARRAY_BOUNDS_ERROR is generated if index is less than zero or greater than or equal to the number of dependent parameters, as returned by cgGetNumDependentStateAssignmentParameters.</para>
        /// <para>VERSION: cgGetDependentStateAssignmentParameter was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <param name="index">The index of the param to return.</param>
        /// <returns>Returns a handle to the selected dependent param on success. Returns NULL if an error occurs.</returns>
        public static CgParameter GetDependentStateAssignmentParameter(CgStateAssignment sa, int index)
        { return cgGetDependentStateAssignmentParameter(sa, index); }

        /// <summary>
        /// <para>cgGetDomain returns the enumerant assigned to a domain name.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if domain_string is NULL.</para>
        /// <para>VERSION: cgGetDomain was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="domain_string">A string containing the case-sensitive domain name.</param>
        /// <returns>Returns the domain enumerant of domain_string. Returns CG_UNKNOWN if the given domain does not exist.</returns>
        public static CgDomain GetDomain(string domain_string)
        { return cgGetDomain(domain_string); }

        /// <summary>
        /// <para>cgGetDomainString returns the domain name associated with a domain enumerant.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetDomainString was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="domain">The domain enumerant.</param>
        /// <returns>Returns the domain string of the enumerant domain. Returns NULL if domain is not a valid domain.</returns>
        public static string GetDomainString(CgDomain domain)
        { return Marshal.PtrToStringAnsi(cgGetDomainString(domain)); }

        /// <summary>
        /// <para>cgGetEffectContext allows the application to retrieve a handle to the context to which a given effect belongs.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetEffectContext was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect.</param>
        /// <returns>Returns the context to which effect belongs. Returns NULL if an error occurs.</returns>
        public static CgContext GetEffectContext(CgEffect effect)
        { return cgGetEffectContext(effect); }

        /// <summary>
        /// <para>cgGetEffectName returns the name from the specified effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetEffectName was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect from which the name will be retrieved.</param>
        /// <returns>Returns the name from the specified effect. Returns NULL if the effect doesn't have a valid name or an error occurs.</returns>
        public static string GetEffectName(CgEffect effect)
        { return cgGetEffectName(effect); }

        /// <summary>
        /// <para>cgGetEffectParameterBuffer returns the CgBuffer object set by cgSetEffectParameterBuffer.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetEffectParameterBuffer was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="param">The effect param associated with a Cg buffer (using the BUFFER semantic) set by cgSetEffectParameterBuffer.</param>
        /// <returns>Returns the CgBuffer object set by cgSetEffectParameterBuffer. Returns NULL if param is invalid or does not have a CgBuffer set by cgSetEffectParameterBuffer.</returns>
        public static CgBuffer GetEffectParameterBuffer(CgParameter param)
        { return cgGetEffectParameterBuffer(param); }

        /// <summary>
        /// <para>cgGetEffectParameterBySemantic returns the param in an effect which is associated with the given semantic.</para>
        /// <para>It multiple parameters in the effect have this semantic, an arbitrary one of them will be returned.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect. CG_INVALID_PARAMETER_ERROR is generated if semantic is NULL or the empty string.</para>
        /// <para>VERSION: cgGetEffectParameterBySemantic was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the param.</param>
        /// <param name="semantic">The name of the semantic.</param>
        /// <returns>Returns the CgParameter object in effect that has the given semantic. Returns NULL if effect is invalid or does not have any parameters with the given semantic.</returns>
        public static CgParameter GetEffectParameterBySemantic(CgEffect effect, [In]string semantic)
        { return cgGetEffectParameterBySemantic(effect, semantic); }

        /// <summary>
        /// <para>cgGetEnum returns the enumerant assigned to an enum name.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if enum_string is NULL.</para>
        /// <para>VERSION: cgGetEnum was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="enum_string">A string containing the case-sensitive enum name.</param>
        /// <returns>Returns the enumerant for enum_string. Returns CG_UNKNOWN if no such enumerant exists</returns>
        public static CgEnum GetEnum([In]string enum_string)
        { return cgGetEnum(enum_string); }

        /// <summary>
        /// <para>cgGetEnumString returns the name string associated with an enumerant.</para>
        /// <para>It's primary use to print debugging information.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetEnumString was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="en">The enumerant.</param>
        /// <returns>Returns the string representation of the enumerant enum. Returns NULL if enum is not a valid Cg enumerant.</returns>
        public static string GetEnumString(CgEnum en)
        { return Marshal.PtrToStringAnsi(cgGetEnumString(en)); }

        /// <summary>
        /// <para>cgGetError returns the last error condition that has occured.</para>
        /// <para>The error condition is reset after cgGetError is called.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetError was introduced in Cg 1.1.</para>
        /// </summary>
        /// <returns>Returns the last error condition that has occured. Returns CG_NO_ERROR if no error has occurred.</returns>
        public static CgError GetError()
        { return cgGetError(); }

        /// <summary>
        /// <para>cgGetErrorCallback returns the current error callback function.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetErrorCallback was introduced in Cg 1.1.</para>
        /// </summary>
        /// <returns>Returns the currently set error callback function. Returns NULL if no callback function has been set.</returns>
        public static void GetErrorCallback()
        { cgGetErrorCallback(); }

        /// <summary>
        /// <para>cgGetErrorHandler returns the current error handler callback function and application provided data pointer.</para>
        /// <para>VERSION: cgGetErrorHandler was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="appdataptr">A pointer for an application provided data pointer.</param>
        /// <returns>Returns the current error handler callback function. Returns NULL if no callback function is set. If appdataptr is not NULL then the current appdata pointer will be copied into the location pointed to by appdataptr.</returns>
        public static void GetErrorHandler(out IntPtr appdataptr)
        { cgGetErrorHandler(out appdataptr); }

        /// <summary>
        /// <para>cgGetErrorString returns a human readable error string for the given error condition.</para>
        /// <para>VERSION: cgGetErrorString was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="error">The error condition.</param>
        /// <returns>Returns a human readable error string for the given error condition.</returns>
        public static string GetErrorString(CgError error)
        { return Marshal.PtrToStringAnsi(cgGetErrorString(error)); }

        /// <summary>
        /// <para>cgGetFirstDependentParameter returns the first  member dependent param associated with a given param.</para>
        /// <para>The rest of the members may be retrieved from the first member by iterating with cgGetNextParameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetFirstDependentParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The  parameter.</param>
        /// <returns>Returns a handle to the first member param. Returns NULL if param is not a struct or if some other error occurs.</returns>
        public static CgParameter GetFirstDependentParameter(CgParameter param)
        { return cgGetFirstDependentParameter(param); }

        /// <summary>
        /// <para>cgGetFirstEffect is used to begin iteration over all of the effects contained by a context.</para>
        /// <para>See cgGetNextEffect for more information.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetFirstEffect was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context from which to retrieve the first effect.</param>
        /// <returns>Returns the first CGeffect object in context. Returns NULL if context contains no effects.</returns>
        public static CgEffect GetFirstEffect(CgContext context)
        { return cgGetFirstEffect(context); }

        /// <summary>
        /// <para>The first annotation associated with an effect can be retrieved using cgGetFirstEffectAnnotation.</para>
        /// <para>The rest of the effect's annotations can be discovered by iterating through them using cgGetNextAnnotation.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetFirstEffectAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the first annotation.</param>
        /// <returns>Returns the first annotation in an effect. Returns NULL if the effect has no annotations.</returns>
        public static CgAnnotation GetFirstEffectAnnotation(CgEffect effect)
        { return cgGetFirstEffectAnnotation(effect); }

        /// <summary>
        /// <para>The first top-level param in an effect can be retrieved using cgGetFirstEffectParameter.</para>
        /// <para>The rest of the effect's parameters can be discovered by iterating through them using cgGetNextParameter.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetFirstEffectParameter was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the first param.</param>
        /// <returns>Returns the first CgParameter object in effect. Returns NULL if effect is invalid or if effect does not have any parameters.</returns>
        public static CgParameter GetFirstEffectParameter(CgEffect effect)
        { return cgGetFirstEffectParameter(effect); }

        /// <summary>
        /// <para>cgGetFirstError returns the first error condition that has occured since cgGetFirstError was previously called.</para>
        /// <para>VERSION: cgGetFirstError was introduced in Cg 1.4.</para>
        /// </summary>
        /// <returns>Returns the first error condition that has occured since cgGetFirstError was last called. Returns CG_NO_ERROR if no error has occurred.</returns>
        public static void GetFirstError()
        { cgGetFirstError(); }

        /// <summary>
        /// <para>cgGetFirstLeafEffectParameter returns the first leaf param in an effect.</para>
        /// <para>The combination of cgGetFirstLeafEffectParameter and cgGetNextLeafParameter allows the iteration through all of the parameters of basic data types (not structs or arrays) without recursion.</para>
        /// <para>See cgGetNextLeafParameter for more information.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetFirstLeafEffectParameter was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the first leaf param.</param>
        /// <returns>Returns the first leaf CgParameter object in effect. Returns NULL if effect is invalid or if effect does not have any parameters.</returns>
        public static CgParameter GetFirstLeafEffectParameter(CgEffect effect)
        { return cgGetFirstLeafEffectParameter(effect); }

        /// <summary>
        /// <para>cgGetFirstLeafParameter returns the first leaf param in a program.</para>
        /// <para>The combination of cgGetFirstLeafParameter and cgGetNextLeafParameter allow the iteration through all of the parameters of basic data types (not structs or arrays) without recursion.</para>
        /// <para>See cgGetNextLeafParameter for more information.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_ENUMERANT_ERROR is generated if name_space is not CG_PROGRAM or CG_GLOBAL.</para>
        /// <para>VERSION: cgGetFirstLeafParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program from which to retrieve the first leaf param.</param>
        /// <param name="name_space">Specifies the param namespace through which to iterate.  Currently CG_PROGRAM and CG_GLOBAL are supported.</param>
        /// <returns>Returns the first leaf CgParameter object in program. Returns NULL if program is invalid or if program does not have any parameters.</returns>
        public static CgParameter GetFirstLeafParameter(CgProgram program, CgEnum name_space)
        { return cgGetFirstLeafParameter(program, name_space); }

        /// <summary>
        /// <para>cgGetFirstParameter returns the first top-level param  in a program.</para>
        /// <para>cgGetFirstParameter is used for recursing through all parameters in a program.</para>
        /// <para>See cgGetNextParameter for more information on param traversal.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_ENUMERANT_ERROR is generated if name_space is not CG_PROGRAM or CG_GLOBAL.</para>
        /// <para>VERSION: cgGetFirstParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program from which to retrieve the first param.</param>
        /// <param name="name_space">Specifies the param namespace through which to iterate.  Currently CG_PROGRAM and CG_GLOBAL are supported.</param>
        /// <returns>Returns the first CgParameter object in program. Returns zero if program is invalid or if program does not have any parameters. Also returns zero if program is a combined program. To access the parameters of a combined program, use cgGetProgramDomainProgram to get each domain program and then call cgGetFirstParameter on each domain program.</returns>
        public static CgParameter GetFirstParameter(CgProgram program, CgEnum name_space)
        { return cgGetFirstParameter(program, name_space); }

        /// <summary>
        /// <para>The annotations associated with a param can be retrieved with cgGetFirstParameterAnnotation.</para>
        /// <para>Use cgGetNextAnnotation to iterate through the remainder of the param's annotations.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetFirstParameterAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The parameter from which to retrieve the annotation.</param>
        /// <returns>Returns the first annotation for the given param. Returns NULL if the param has no annotations or an error occurs.</returns>
        public static CgAnnotation GetFirstParameterAnnotation(CgParameter param)
        { return cgGetFirstParameterAnnotation(param); }

        /// <summary>
        /// <para>cgGetFirstPass is used to begin iteration over all of the passes contained within a technique.</para>
        /// <para>See cgGetNextPass for more information.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgGetFirstPass was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique from which to retrieve the first pass.</param>
        /// <returns>Returns the first CGpass object in tech. Returns NULL if tech contains no passes.</returns>
        public static CgPass GetFirstPass(CgTechnique tech)
        { return cgGetFirstPass(tech); }

        /// <summary>
        /// <para>The annotations associated with a pass can be retrieved using cgGetFirstPassAnnotation.</para>
        /// <para>The remainder of the pass's annotations can be discovered by iterating through the parameters, calling cgGetNextAnnotation to get to the next one.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass.</para>
        /// <para>VERSION: cgGetFirstPassAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass from which to retrieve the annotation.</param>
        /// <returns>Returns the first annotation from the given pass. Returns NULL if the pass has no annotations or an error occurs.</returns>
        public static CgAnnotation GetFirstPassAnnotation(CgPass pass)
        { return cgGetFirstPassAnnotation(pass); }

        /// <summary>
        /// <para>cgGetFirstProgram is used to begin iteration over all of the programs contained within a context.</para>
        /// <para>See cgGetNextProgram for more information.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetFirstProgram was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="context">The context from which to retrieve the first program.</param>
        /// <returns>Returns the first CgProgram object in context. Returns NULL if context contains no programs or an error occurs.</returns>
        public static CgProgram GetFirstProgram(CgContext context)
        { return cgGetFirstProgram(context); }

        /// <summary>
        /// <para>The annotations associated with a program can be retrieved using cgGetFirstProgramAnnotation.</para>
        /// <para>The remainder of the program's annotations can be discovered by iterating through the parameters, calling cgGetNextAnnotation to get to the next one.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetFirstProgramAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="program">The program from which to retrieve the annotation.</param>
        /// <returns>Returns the first annotation from the given program. Returns NULL if the program has no annotations.</returns>
        public static CgAnnotation GetFirstProgramAnnotation(CgProgram program)
        { return cgGetFirstProgramAnnotation(program); }

        /// <summary>
        /// <para>cgGetFirstSamplerState is used to begin iteration over all of the sampler state definitions contained within a context.</para>
        /// <para>See cgGetNextState for more information.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetFirstSamplerState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context from which to retrieve the first sampler state definition.</param>
        /// <returns>Returns the first CGstate object in context. Returns NULL if context contains no programs or an error occurs.</returns>
        public static CgState GetFirstSamplerState(CgContext context)
        { return cgGetFirstSamplerState(context); }

        /// <summary>
        /// <para>cgGetFirstSamplerStateAssignment is used to begin iteration over all of the state assignments contained within a sampler_state block assigned to a param in an effect file.</para>
        /// <para>See cgGetNextStateAssignment for more information.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetFirstSamplerStateAssignment was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The sampler parameter from which to retrieve the first state assignment.</param>
        /// <returns>Returns the first CGstateassignment object assigned to param. Returns NULL if param has no sampler_state block or an error occurs.</returns>
        public static CgStateAssignment GetFirstSamplerStateAssignment(CgParameter param)
        { return cgGetFirstSamplerStateAssignment(param); }

        /// <summary>
        /// <para>cgGetFirstState is used to begin iteration over all of the state definitions contained within a context.</para>
        /// <para>See cgGetNextState for more information.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetFirstState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context from which to retrieve the first state definition.</param>
        /// <returns>Returns the first CGstate object in context. Returns NULL if context contains no state definitions or an error occurs.</returns>
        public static CgState GetFirstState(CgContext context)
        { return cgGetFirstState(context); }

        /// <summary>
        /// <para>cgGetFirstStateAssignment is used to begin iteration over all of the state assignment contained within a pass.</para>
        /// <para>See cgGetNextStateAssignment for more information.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass.</para>
        /// <para>VERSION: cgGetFirstStateAssignment was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass from which to retrieve the first state assignment.</param>
        /// <returns>Returns the first CGstateassignment object in pass. Returns NULL if pass contains no state assignments or an error occurs.</returns>
        public static CgStateAssignment GetFirstStateAssignment(CgPass pass)
        { return cgGetFirstStateAssignment(pass); }

        /// <summary>
        /// <para>cgGetFirstStructParameter returns the first member param of a struct param.</para>
        /// <para>The rest of the members may be retrieved from the first member by iterating with cgGetNextParameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_TYPE_ERROR is generated if param is not a struct param.</para>
        /// <para>VERSION: cgGetFirstStructParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">Specifies the struct parameter. This param must be of type CG_STRUCT (returned by cgGetParameterType).</param>
        /// <returns>Returns a handle to the first member param. Returns NULL if param is not a struct or if some other error occurs.</returns>
        public static CgParameter GetFirstStructParameter(CgParameter param)
        { return cgGetFirstStructParameter(param); }

        /// <summary>
        /// <para>cgGetFirstTechnique is used to begin iteration over all of the techniques contained within a effect.</para>
        /// <para>See cgGetNextTechnique for more information.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetFirstTechnique was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the first technique.</param>
        /// <returns>Returns the first CGtechnique object in effect. Returns NULL if effect contains no techniques or an error occurs.</returns>
        public static CgTechnique GetFirstTechnique(CgEffect effect)
        { return cgGetFirstTechnique(effect); }

        /// <summary>
        /// <para>The annotations associated with a technique can be retrieved   using cgGetFirstTechniqueAnnotation.</para>
        /// <para>The remainder of the technique's annotations can be discovered by iterating through the parameters, calling cgGetNextAnnotation to get to the next one.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgGetFirstTechniqueAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique from which to retrieve the annotation.</param>
        /// <returns>Returns the first annotation in the given technique. Returns NULL if the technique has no annotations or an error occurs.</returns>
        public static CgAnnotation GetFirstTechniqueAnnotation(CgTechnique tech)
        { return cgGetFirstTechniqueAnnotation(tech); }

        /// <summary>
        /// <para>cgGetFloatAnnotationValues allows the application to  retrieve the value(s) of a floating-point typed annotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL.</para>
        /// <para>VERSION: cgGetFloatAnnotationValues was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation from which the values will be retrieved.</param>
        /// <param name="nvalues">Pointer to integer where the number of returned values will be stored.</param>
        /// <returns>Returns a pointer to an array of float values. The number of values in the array is returned via the nvalues param. Returns NULL if no values are available. nvalues will be 0.</returns>
        public static float[] GetFloatAnnotationValues(CgAnnotation ann, out int nvalues)
        { return cgGetFloatAnnotationValues(ann, out nvalues); }

        /// <summary>
        /// <para>cgGetFloatStateAssignmentValues allows the application to  retrieve the value(s) of a floating-point typed state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a float type.</para>
        /// <para>VERSION: cgGetFloatStateAssignmentValues was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment from which the values will be retrieved.</param>
        /// <param name="nvalues">Pointer to integer where the number of returned values will be stored.</param>
        /// <returns>Returns a pointer to an array of float values. The number of values in the array is returned via the nvalues param. Returns NULL if an error occurs or if no values are available. nvalues will be 0 in the latter case.</returns>
        public static float[] GetFloatStateAssignmentValues(CgStateAssignment sa, int[] nvalues)
        { return cgGetFloatStateAssignmentValues(sa, nvalues); }

        /// <summary>
        /// <para>cgGetIntAnnotationValues allows the application to  retrieve the value(s) of an int typed annotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL.</para>
        /// <para>VERSION: cgGetIntAnnotationValues was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation from which the values will be retrieved.</param>
        /// <param name="nvalues">Pointer to integer where the number of returned values will be stored.</param>
        /// <returns>Returns a pointer to an array of int values. The number of values in the array is returned via the nvalues param. Returns NULL if no values are available. nvalues will be 0.</returns>
        public static int[] GetIntAnnotationValues(CgAnnotation ann, out int nvalues)
        { return cgGetIntAnnotationValues(ann, out nvalues); }

        /// <summary>
        /// <para>cgGetIntStateAssignmentValues allows the application to  retrieve the value(s) of an integer typed state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of an integer type.</para>
        /// <para>VERSION: cgGetIntStateAssignmentValues was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment from which the values will be retrieved.</param>
        /// <param name="nvalues">Pointer to integer where the number of values returned will be stored.</param>
        /// <returns>Returns a pointer to an array of int values. The number of values in the array is returned via the nvalues param. Returns NULL if an error occurs or if no values are available. nvalues will be 0 in the latter case.</returns>
        public static int[] GetIntStateAssignmentValues(CgStateAssignment sa, int[] nvalues)
        { return cgGetIntStateAssignmentValues(sa, nvalues); }

        /// <summary>
        /// <para>cgGetLastErrorString returns the current error condition and error condition string.</para>
        /// <para>It's similar to calling cgGetErrorString with the result of cgGetError.</para>
        /// <para>However in certain cases the error string may contain more information about the specific error that  last ocurred than what cgGetErrorString would return.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetLastErrorString was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="error">A pointer to a CGerror variable for returning the last error source.</param>
        /// <returns>Returns the last error string. Returns NULL if there was no error. If error is not NULL, the last error source will be returned in the location specified by error. This is the same value that would be returned by cgGetError.</returns>
        public static string GetLastErrorString(out CgError error)
        { return Marshal.PtrToStringAnsi(cgGetLastErrorString(out error)); }

        /// <summary>
        /// <para>Each Cg context maintains a NULL-terminated string containing warning and error messages generated by the Cg compiler, state managers and the like.</para>
        /// <para>cgGetLastListing allows applications and custom state managers to query the listing text.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetLastListing was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="context">The context handle.</param>
        /// <returns>Returns a NULL-terminated string containing the current listing text. Returns NULL if no listing text is available, or the listing text string is empty. In all cases, the pointer returned by cgGetLastListing is only guaranteed to be valid until the next Cg entry point not related to error reporting is called. For example, calls to cgCreateProgram, cgCompileProgram, cgCreateEffect, or cgValidateTechnique will invalidate any previously-returned listing pointer.</returns>
        public static string GetLastListing(CgContext context)
        { return Marshal.PtrToStringAnsi(cgGetLastListing(context)); }

        /// <summary>
        /// <para>cgGetLockingPolicy returns an enumerant indicating the current locking policy for the library.</para>
        /// <para>See cgSetLockingPolicy for more information.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetLockingPolicy was introduced in Cg 2.0.</para>
        /// </summary>
        /// <returns>Returns an enumerant indicating the current locking policy.</returns>
        public static CgEnum GetLockingPolicy()
        { return cgGetLockingPolicy(); }

        /// <summary>
        /// <para>cgGetMatrixParameterOrder returns the row or column order of a matrix parameter.</para>
        /// <para>The Cg compiler supports #pragma pack_matrix(row_major) or #pragma pack_matrix(column_major) for specifying the order of matrix parameters. Row-major order is the Cg default.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid parameter or param is not a matrix parameter.</para>
        /// <para>VERSION: cgGetMatrixParameterOrder was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns CG_ROW_MAJOR for a row-major matrix parameter. Returns CG_COLUMN_MAJOR for a column-major matrix parameter. Returns CG_UNKNOWN for a parameter that is not a matrix.</returns>
        public static CgEnum GetMatrixParameterOrder(CgParameter param)
        { return cgGetMatrixParameterOrder(param); }

        /// <summary>
        /// <para>cgGetMatrixParameterdc retrieves the values of the given matrix param using column-major ordering.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetMatrixParameterdc was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param from which the values will be returned.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be written. The array must have size equal to the number of rows in the matrix times the number of columns in the matrix.</param>
        public static void GetMatrixParameterdc(CgParameter param, double[] matrix)
        { cgGetMatrixParameterdc(param, matrix); }

        /// <summary>
        /// <para>cgGetMatrixParameterdr retrieves the values of the given matrix param using row-major ordering.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetMatrixParameterdr was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param from which the values will be returned.</param>
        /// <param name="matrix">An array of doubles into which the matrix values will be written. The array must have size equal to the number of rows in the matrix times the number of columns in the matrix.</param>
        public static void GetMatrixParameterdr(CgParameter param, double[] matrix)
        { cgGetMatrixParameterdr(param, matrix); }

        /// <summary>
        /// <para>cgGetMatrixParameterfc retrieves the values of the given matrix param using column-major ordering.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetMatrixParameterfc was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param from which the values will be returned.</param>
        /// <param name="matrix">An array of floats into which the matrix values will be written. The array must have size equal to the number of rows in the matrix times the number of columns in the matrix.</param>
        public static void GetMatrixParameterfc(CgParameter param, float[] matrix)
        { cgGetMatrixParameterfc(param, matrix); }

        /// <summary>
        /// <para>cgGetMatrixParameterfr retrieves the values of the given matrix param using row-major ordering.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetMatrixParameterfr was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param from which the values will be returned.</param>
        /// <param name="matrix">An array of floats into which the matrix values will be written. The array must have size equal to the number of rows in the matrix times the number of columns in the matrix.</param>
        public static void GetMatrixParameterfr(CgParameter param, float[] matrix)
        { cgGetMatrixParameterfr(param, matrix); }

        /// <summary>
        /// <para>cgGetMatrixParameteric retrieves the values of the given matrix param using column-major ordering.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetMatrixParameteric was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param from which the values will be returned.</param>
        /// <param name="matrix">An array of ints into which the matrix values will be written. The array must have size equal to the number of rows in the matrix times the number of columns in the matrix.</param>
        public static void GetMatrixParameteric(CgParameter param, int[] matrix)
        { cgGetMatrixParameteric(param, matrix); }

        /// <summary>
        /// <para>cgGetMatrixParameterir retrieves the values of the given matrix param using row-major ordering.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetMatrixParameterir was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param from which the values will be returned.</param>
        /// <param name="matrix">An array of ints into which the matrix values will be written. The array must have size equal to the number of rows in the matrix times the number of columns in the matrix.</param>
        public static void GetMatrixParameterir(CgParameter param, int[] matrix)
        { cgGetMatrixParameterir(param, matrix); }

        /// <summary>
        /// <para>cgGetMatrixSize writes the number of rows and columns contained by the specified matrix type into nrows and ncols locations respectively.</para>
        /// <para>If type is not a matrix enumerant type, 0 is written as both the rows and columns size.</para>
        /// <para>VERSION: cgGetMatrixSize was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="type">The type enumerant.</param>
        /// <param name="nrows">A pointer to the location where the number of rows that type has will be written.</param>
        /// <param name="ncols">A pointer to the location where the number of columns that type has will be written.</param>
        public static void GetMatrixSize(CgType type, out int nrows, out int ncols)
        { cgGetMatrixSize(type, out nrows, out ncols); }

        /// <summary>
        /// <para>The effects in a context can be retrieved directly by name using cgGetNamedEffect.</para>
        /// <para>The effect names can be discovered by iterating through the context's effects (see cgGetFirstEffect and cgGetNextEffect) and calling cgGetEffectName for each.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetNamedEffect was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="context">The context from which to retrieve the effect.</param>
        /// <param name="name">The name of the effect to retrieve.</param>
        /// <returns>Returns the named effect if found. Returns NULL if context has no effect corresponding to name or if an error occurs.</returns>
        public static CgEffect GetNamedEffect(CgContext context, [In]string name)
        { return cgGetNamedEffect(context, name); }

        /// <summary>
        /// <para>The annotations associated with an effect can be retrieved directly by name using cgGetNamedEffectAnnotation.</para>
        /// <para>The names of a effect's annotations can be  discovered by iterating through the annotations (see cgGetFirstEffectAnnotation and cgGetNextAnnotation), calling cgGetAnnotationName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect. CG_INVALID_POINTER_ERROR is generated if name is NULL.</para>
        /// <para>VERSION: cgGetNamedEffectAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the annotation.</param>
        /// <param name="name">The name of the annotation to retrieve.</param>
        /// <returns>Returns the named annotation. Returns NULL if the effect has no annotation corresponding to name.</returns>
        public static CgAnnotation GetNamedEffectAnnotation(CgEffect effect, [In]string name)
        { return cgGetNamedEffectAnnotation(effect, name); }

        /// <summary>
        /// <para>The parameters of a effect can be retrieved directly by name using cgGetNamedEffectParameter.</para>
        /// <para>The names of the parameters in a effect can be discovered by iterating through the effect's parameters (see cgGetFirstEffectParameter and cgGetNextParameter), calling cgGetParameterName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetNamedEffectParameter was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the param.</param>
        /// <param name="name">The name of the param to retrieve.</param>
        /// <returns>Returns the named param from the effect. Returns NULL if the effect has no param corresponding to name.</returns>
        public static CgParameter GetNamedEffectParameter(CgEffect effect, [In]string name)
        { return cgGetNamedEffectParameter(effect, name); }

        /// <summary>
        /// <para>The parameters of a program can be retrieved directly by name using cgGetNamedParameter.</para>
        /// <para>The names of the parameters in a program can be discovered by iterating through the program's parameters (see cgGetNextParameter), calling cgGetParameterName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetNamedParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program from which to retrieve the param.</param>
        /// <param name="name">The name of the param to retrieve.</param>
        /// <returns>Returns the named param from the program. Returns NULL if the program has no param corresponding to name.</returns>
        public static CgParameter GetNamedParameter(CgProgram program, [In]string name)
        { return cgGetNamedParameter(program, name); }

        /// <summary>
        /// <para>The annotations associated with a param can be retrieved directly by name using cgGetNamedParameterAnnotation.</para>
        /// <para>The names of a param's annotations can be discovered by iterating through the annotations (see cgGetFirstParameterAnnotation and cgGetNextAnnotation), calling cgGetAnnotationName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetNamedParameterAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param from which to retrieve the annotation.</param>
        /// <param name="name">The name of the annotation to retrieve.</param>
        /// <returns>Returns the named annotation. Returns NULL if the param has no annotation corresponding to name.</returns>
        public static CgAnnotation GetNamedParameterAnnotation(CgParameter param, [In]string name)
        { return cgGetNamedParameterAnnotation(param, name); }

        /// <summary>
        /// <para>The passes of a technique can be retrieved directly by name using cgGetNamedPass.</para>
        /// <para>The names of the passes in a technique can be discovered by iterating through the technique's passes (see cgGetFirstPass and cgGetNextPass), calling cgGetPassName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgGetNamedPass was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique from which to retrieve the pass.</param>
        /// <param name="name">The name of the pass to retrieve.</param>
        /// <returns>Returns the named pass from the technique. Returns NULL if the technique has no pass corresponding to name.</returns>
        public static CgPass GetNamedPass(CgTechnique tech, [In]string name)
        { return cgGetNamedPass(tech, name); }

        /// <summary>
        /// <para>The annotations associated with a pass can be retrieved directly by name using cgGetNamedPassAnnotation.</para>
        /// <para>The names of a pass's annotations can be discovered by iterating through the annotations (see cgGetFirstPassAnnotation and cgGetNextAnnotation), calling cgGetAnnotationName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass.</para>
        /// <para>VERSION: cgGetNamedPassAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass from which to retrieve the annotation.</param>
        /// <param name="name">The name of the annotation to retrieve.</param>
        /// <returns>Returns the named annotation. Returns NULL if the pass has no annotation corresponding to name.</returns>
        public static CgAnnotation GetNamedPassAnnotation(CgPass pass, [In]string name)
        { return cgGetNamedPassAnnotation(pass, name); }

        /// <summary>
        /// <para>The annotations associated with a program can be retrieved directly by name using cgGetNamedProgramAnnotation.</para>
        /// <para>The names of a program's annotations can be discovered by iterating through the annotations (see cgGetFirstProgramAnnotation and cgGetNextAnnotation), calling cgGetAnnotationName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetNamedProgramAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="program">The program from which to retrieve the annotation.</param>
        /// <param name="name">The name of the annotation to retrieve.</param>
        /// <returns>Returns the named annotation. Returns NULL if the program has no annotation corresponding to name.</returns>
        public static CgAnnotation GetNamedProgramAnnotation(CgProgram prog, [In]string name)
        { return cgGetNamedProgramAnnotation(prog, name); }

        /// <summary>
        /// <para>cgGetNamedProgramParameter is essentially identical to  cgGetNamedParameter except it limits the search of the param to the name space specified by name_space.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetNamedProgramParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="program">The program from which to retrieve the param.</param>
        /// <param name="name_space">Specifies the namespace of the param to iterate through.  Currently CG_PROGRAM and CG_GLOBAL are supported.</param>
        /// <param name="name">Specifies the name of the param to retrieve.</param>
        /// <returns>Returns the named param from the program. Returns NULL if the program has no param corresponding to name.</returns>
        public static CgParameter GetNamedProgramParameter(CgProgram program, CgEnum name_space, string name)
        { return cgGetNamedProgramParameter(program, name_space, name); }

        /// <summary>
        /// <para>The sampler states associated with a context, as specified with a sampler_state block in an effect file, can be retrieved directly by name using cgGetNamedSamplerState.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_PARAMETER_ERROR is generated if name is NULL.</para>
        /// <para>VERSION: cgGetNamedSamplerState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context from which to retrieve the named sampler state.</param>
        /// <param name="name">The name of the state to retrieve.</param>
        /// <returns>Returns the named sampler state. Returns NULL if context is invalid or if context has no sampler states corresponding to name.</returns>
        public static CgState GetNamedSamplerState(CgContext context, [In]string name)
        { return cgGetNamedSamplerState(context, name); }

        /// <summary>
        /// <para>The sampler state assignments associated with a sampler param, as specified with a sampler_state block in an effect file, can be retrieved directly by name using cgGetNamedSamplerStateAssignment.</para>
        /// <para>The names of the sampler state assignments can be discovered by iterating through the sampler's state assignments (see cgGetFirstSamplerStateAssignment and cgGetNextStateAssignment), calling cgGetSamplerStateAssignmentState then cgGetStateName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetNamedSamplerStateAssignment was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The sampler param from which to retrieve the sampler state assignment.</param>
        /// <param name="name">The name of the state assignment to retrieve.</param>
        /// <returns>Returns the named sampler state assignment. Returns NULL if the pass has no sampler state assignment corresponding to name.</returns>
        public static CgStateAssignment GetNamedSamplerStateAssignment(CgParameter param, [In]string name)
        { return cgGetNamedSamplerStateAssignment(param, name); }

        /// <summary>
        /// <para>The states of a context can be retrieved directly by name using cgGetNamedState.</para>
        /// <para>The names of the states in a context can be discovered by iterating through the context's states (see cgGetFirstState and cgGetNextState), calling cgGetStateName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if name is NULL.</para>
        /// <para>VERSION: cgGetNamedState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="context">The context from which to retrieve the state.</param>
        /// <param name="name">The name of the state to retrieve.</param>
        /// <returns>Returns the named state from the context. Returns NULL if the context has no state corresponding to name.</returns>
        public static CgState GetNamedState(CgContext context, [In]string name)
        { return cgGetNamedState(context, name); }

        /// <summary>
        /// <para>The state assignments of a pass can be retrieved directly by name using cgGetNamedStateAssignment.</para>
        /// <para>The names of the state assignments in a pass can be discovered by iterating through the pass's state assignments (see cgGetFirstStateAssignment and cgGetNextStateAssignment), calling cgGetStateAssignmentState then cgGetStateName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass.</para>
        /// <para>VERSION: cgGetNamedStateAssignment was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass from which to retrieve the state assignment.</param>
        /// <param name="name">The name of the state assignment to retrieve.</param>
        /// <returns>Returns the named state assignment from the pass. Returns NULL if the pass has no state assignment corresponding to name.</returns>
        public static CgStateAssignment GetNamedStateAssignment(CgPass pass, [In]string name)
        { return cgGetNamedStateAssignment(pass, name); }

        /// <summary>
        /// <para>The member parameters of a struct param may be retrieved directly by name using cgGetNamedStructParameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_TYPE_ERROR is generated if param is not a struct param.</para>
        /// <para>VERSION: cgGetNamedStructParameter was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The struct param from which to retrieve the member param.</param>
        /// <param name="name">The name of the member param to retrieve.</param>
        /// <returns>Returns the member param from the given struct. Returns NULL if the struct has no member param corresponding to name.</returns>
        public static CgParameter GetNamedStructParameter(CgParameter param, [In]string name)
        { return cgGetNamedStructParameter(param, name); }

        /// <summary>
        /// <para>cgGetNamedSubParameter is a generalized param getter function that will retrieve parameters, including deep parameters, of an aggregate param type such as a structure or an array.</para>
        /// <para>VERSION: cgGetNamedSubParameter was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="param">Aggregate param.</param>
        /// <param name="name">Name of the param inside the aggregate param (param) being requested.</param>
        /// <returns>Returns the named param. Returns NULL if param has no param corresponding to name.</returns>
        public static CgParameter GetNamedSubParameter(CgParameter param, string name)
        { return cgGetNamedSubParameter(param, name); }

        /// <summary>
        /// <para>The techniques of an effect can be retrieved directly by name using cgGetNamedTechnique.</para>
        /// <para>The names of the techniques in a effect can be discovered by iterating through the effect's techniques (see cgGetFirstTechnique and cgGetNextTechnique), calling cgGetTechniqueName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetNamedTechnique was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect from which to retrieve the technique.</param>
        /// <param name="name">The name of the technique to retrieve.</param>
        /// <returns>Returns the named technique from the effect. Returns NULL if the effect has no technique corresponding to name.</returns>
        public static CgTechnique GetNamedTechnique(CgEffect effect, [In]string name)
        { return cgGetNamedTechnique(effect, name); }

        /// <summary>
        /// <para>The annotations associated with a technique can be retrieved directly by name using cgGetNamedTechniqueAnnotation.</para>
        /// <para>The names of a technique's annotations can be discovered by iterating through the annotations (see cgGetFirstTechniqueAnnotation and cgGetNextAnnotation), calling cgGetAnnotationName for each one in turn.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgGetNamedTechniqueAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique from which to retrieve the annotation.</param>
        /// <param name="name">The name of the annotation to retrieve.</param>
        /// <returns>Returns the named annotation. Returns NULL if the technique has no annotation corresponding to name.</returns>
        public static CgAnnotation GetNamedTechniqueAnnotation(CgTechnique tech, [In]string name)
        { return cgGetNamedTechniqueAnnotation(tech, name); }

        /// <summary>
        /// <para>cgGetNamedUserType returns the enumerant associated with the named type defined in the constuct associated with handle, which may be a CgProgram or CGeffect.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if handle is not a valid program or effect.</para>
        /// <para>VERSION: cgGetNamedUserType was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="handle">The CgProgram or CGeffect in which the type is defined.</param>
        /// <param name="name">A string containing the case-sensitive type name.</param>
        /// <returns>Returns the type enumerant associated with name. Returns CG_UNKNOWN_TYPE if no such type exists.</returns>
        public static CgType GetNamedUserType(CgHandle handle, [In]string name)
        { return cgGetNamedUserType(handle, name); }

        /// <summary>
        /// <para>The annotations associated with a param, pass, technique, or program can be iterated over by using cgGetNextAnnotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation.</para>
        /// <para>VERSION: cgGetNextAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The current annotation.</param>
        /// <returns>Returns the next annotation in the sequence of annotations associated with the annotated object. Returns NULL when ann is the last annotation.</returns>
        public static CgAnnotation GetNextAnnotation(CgAnnotation ann)
        { return cgGetNextAnnotation(ann); }

        /// <summary>
        /// <para>The effects within a context can be iterated over with cgGetNextEffect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgGetNextEffect was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The current effect.</param>
        /// <returns>Returns the next effect in the context's internal sequence of effects. Returns NULL when effect is the last effect in the context.</returns>
        public static CgEffect GetNextEffect(CgEffect effect)
        { return cgGetNextEffect(effect); }

        /// <summary>
        /// <para>cgGetNextLeafParameter returns the next leaf param (not struct or array parameters) following a given leaf param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetNextLeafParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The current leaf parameter.</param>
        /// <returns>Returns the next leaf CgParameter object. Returns NULL if param is invalid or if the program or effect from which the iteration started does not have any more leaf parameters.</returns>
        public static CgParameter GetNextLeafParameter(CgParameter param)
        { return cgGetNextLeafParameter(param); }

        /// <summary>
        /// <para>The parameters of a program or effect can be iterated over using cgGetNextParameter with cgGetFirstParameter, or cgGetArrayParameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetNextParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="current">The current param.</param>
        /// <returns>Returns the next param in the program or effect's internal sequence of parameters. Returns NULL when current is the last param in the program or effect.</returns>
        public static CgParameter GetNextParameter(CgParameter current)
        { return cgGetNextParameter(current); }

        /// <summary>
        /// <para>The passes within a technique can be iterated over using cgGetNextPass.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass.</para>
        /// <para>VERSION: cgGetNextPass was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The current pass.</param>
        /// <returns>Returns the next pass in the technique's internal sequence of passes. Returns NULL when pass is the last pass in the technique.</returns>
        public static CgPass GetNextPass(CgPass pass)
        { return cgGetNextPass(pass); }

        /// <summary>
        /// <para>The programs within a context can be iterated over by using cgGetNextProgram.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetNextProgram was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The current program.</param>
        /// <returns>Returns the next program in the context's internal sequence of programs. Returns NULL when program is the last program in the context.</returns>
        public static CgProgram GetNextProgram(CgProgram program)
        { return cgGetNextProgram(program); }

        /// <summary>
        /// <para>The states within a context can be iterated over using cgGetNextState.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetNextState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <returns>Returns the next state in the context's internal sequence of states. Returns NULL when state is the last state in the context.</returns>
        public static CgState GetNextState(CgState state)
        { return cgGetNextState(state); }

        /// <summary>
        /// <para>The state assignments within a pass can be iterated over by using cgGetNextStateAssignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetNextStateAssignment was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The current state assignment.</param>
        /// <returns>Returns the next state assignment in the pass' internal sequence of state assignments. Returns NULL when prog is the last state assignment in the pass.</returns>
        public static CgStateAssignment GetNextStateAssignment(CgStateAssignment sa)
        { return cgGetNextStateAssignment(sa); }

        /// <summary>
        /// <para>The techniques within a effect can be iterated over using cgGetNextTechnique.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgGetNextTechnique was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The current technique.</param>
        /// <returns>Returns the next technique in the effect's internal sequence of techniques. Returns NULL when tech is the last technique in the effect.</returns>
        public static CgTechnique GetNextTechnique(CgTechnique tech)
        { return cgGetNextTechnique(tech); }

        /// <summary>
        /// <para>cgGetNumConnectedToParameters returns the number  of destination parameters connected to the source param param.</para>
        /// <para>It's primarily used with cgGetConnectedToParameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetNumConnectedToParameters was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The source param.</param>
        /// <returns>Returns the number of destination parameters connected to param. Returns 0 if an error occurs.</returns>
        public static int GetNumConnectedToParameters(CgParameter param)
        { return cgGetNumConnectedToParameters(param); }

        /// <summary>
        /// <para>Annotations in CgFX files may include references to one or more effect parameters on the right hand side of the annotation that are used for computing the annotation's value.</para>
        /// <para>cgGetNumDependentAnnotationParameters returns the total number of such parameters.</para>
        /// <para>cgGetDependentAnnotationParameter can then be used to iterate over these parameters.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation.</para>
        /// <para>VERSION: cgGetNumDependentAnnotationParameters was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation handle.</param>
        /// <returns>Returns the number of parameters on which ann depends.</returns>
        public static int GetNumDependentAnnotationParameters(CgAnnotation ann)
        { return cgGetNumDependentAnnotationParameters(ann); }

        /// <summary>
        /// <para>State assignments in CgFX files may include references to an array indexed by an effect param (or expression) on the right hand side of the state assignment  that is used for computing the state assignment's value.</para>
        /// <para>Usually this array holds the compile statements of shader programs and by changing the index of the shader array, it's possible to switch to a different program or profile on-the-fly.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetNumDependentProgramArrayStateAssignmentParameters was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <returns>Returns the number of parameters on which sa depends. Returns 0 if sa is not a program state assignment or an error occurs.</returns>
        public static int GetNumDependentProgramArrayStateAssignmentParameters(CgStateAssignment sa)
        { return cgGetNumDependentProgramArrayStateAssignmentParameters(sa); }

        /// <summary>
        /// <para>State assignments in CgFX passes may include references to one or more effect parameters on the right hand side of the state assignment that are used for computing the state assignment's value.</para>
        /// <para>cgGetNumDependentStateAssignmentParameters returns the total number of such parameters.</para>
        /// <para>cgGetDependentStateAssignmentParameter can then be used to iterate over these parameters.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetNumDependentStateAssignmentParameters was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <returns>Returns the number of parameters on which sa depends.</returns>
        public static int GetNumDependentStateAssignmentParameters(CgStateAssignment sa)
        { return cgGetNumDependentStateAssignmentParameters(sa); }

        /// <summary>
        /// <para>cgGetNumParentTypes returns the number of parents from which type inherits.</para>
        /// <para>VERSION: cgGetNumParentTypes was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="type">The child type.</param>
        /// <returns>Returns the number of parent types. Returns 0 if there are no parents.</returns>
        public static int GetNumParentTypes(CgType type)
        { return cgGetNumParentTypes(type); }

        /// <summary>
        /// <para>cgGetNumProgramDomains returns the number of domains in a combined program.</para>
        /// <para>For example, if the combined program contained a vertex program and a fragment program, cgGetNumProgramDomains will return 2.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetNumProgramDomains was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="program">The combined program object to be queried.</param>
        /// <returns>Returns the number of domains in the combined program program. Returns 0 if an error occurs.</returns>
        public static int GetNumProgramDomains(CgProgram program)
        { return cgGetNumProgramDomains(program); }

        /// <summary>
        /// <para>cgGetNumStateEnumerants returns the number of enumerants associated with a given CGstate.</para>
        /// <para>Enumerants can be added to a CGstate using cgAddStateEnumerant.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetNumStateEnumerants was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve the number of associated enumerants.</param>
        /// <returns>Returns the number of enumerants associated with state. Returns 0 if an error occurs.</returns>
        public static int GetNumStateEnumerants(CgState state)
        { return cgGetNumStateEnumerants(state); }

        /// <summary>
        /// <para>cgGetNumSupportedProfiles provides the number of profiles which are supported by this version of the Cg library.</para>
        /// <para>VERSION: cgGetNumSupportedProfiles was introduced in Cg 2.2.</para>
        /// </summary>
        /// <returns>Returns the number of profiles supported by this version of Cg.</returns>
        public static int GetNumSupportedProfiles()
        { return cgGetNumSupportedProfiles(); }

        /// <summary>
        /// <para>cgGetNumUserTypes returns the number of user-defined types in a given CgProgram or CGeffect.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if handle is not a valid program or effect handle.</para>
        /// <para>VERSION: cgGetNumUserTypes was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="handle">The CgProgram or CGeffect in which the types are defined.</param>
        /// <returns>Returns the number of user defined types.</returns>
        public static int GetNumUserTypes(CgHandle handle)
        { return cgGetNumUserTypes(handle); }

        /// <summary>
        /// <para>cgGetParameterBaseResource allows the application to  retrieve the base resource for a param in a Cg program.</para>
        /// <para>The base resource is the first resource in a set of sequential resources.</para>
        /// <para>For example, if a given param has a resource of CG_ATTR7, it's base resource would be CG_ATTR0.</para>
        /// <para>Only  parameters with resources whose name ends with a number will have a base resource.</para>
        /// <para>For all other parameters the undefined resource CG_UNDEFINED will be returned.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is not a leaf node.</para>
        /// <para>VERSION: cgGetParameterBaseResource was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>Returns the base resource of param. Returns CG_UNDEFINED if no base resource exists for the given param.</returns>
        public static CgResource GetParameterBaseResource(CgParameter param)
        { return cgGetParameterBaseResource(param); }

        /// <summary>
        /// <para>cgGetParameterBaseType allows the application to retrieve the base type of a param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterBaseType was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>Returns the base type enumerant of param. Returns CG_UNKNOWN_TYPE if an error occurs.</returns>
        public static CgType GetParameterBaseType(CgParameter param)
        { return cgGetParameterBaseType(param); }

        /// <summary>
        /// <para>cgGetParameterBufferIndex returns the index for the buffer to which a param belongs.</para>
        /// <para>If param does not belong to a buffer, then -1 is returned.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterBufferIndex was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="param">The param for which the associated buffer index will be retrieved.</param>
        /// <returns>Returns the index for the buffer to which param belongs. Returns -1 if param does not belong to a buffer or an error occurs.</returns>
        public static int GetParameterBufferIndex(CgParameter param)
        { return cgGetParameterBufferIndex(param); }

        /// <summary>
        /// <para>cgGetParameterBufferOffset returns the buffer offset associated with a param.</para>
        /// <para>If param does not belong to a buffer, then -1 is returned.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterBufferOffset was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="param">The parameter for which the buffer offset will be retrieved.</param>
        /// <returns>Returns the buffer offset for param. Returns -1 if param does not belong to a buffer or an error occurs.</returns>
        public static int GetParameterBufferOffset(CgParameter param)
        { return cgGetParameterBufferOffset(param); }

        /// <summary>
        /// <para>cgGetParameterClass allows the application to retrieve the class of a parameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterClass was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the param class enumerant of param. Returns CG_PARAMETERCLASS_UNKNOWN if an error occurs.</returns>
        public static CgParameterClass GetParameterClass(CgParameter param)
        { return cgGetParameterClass(param); }

        /// <summary>
        /// <para>cgGetParameterClassEnum returns the enumerant associated with a param class name.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if pString is NULL.</para>
        /// <para>VERSION: cgGetParameterClassEnum was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="pString">A string containing the case-sensitive param class name.</param>
        /// <returns>Returns the param class enumerant associated with pString. Returns CG_PARAMETERCLASS_UNKNOWN if the given param class does not exist.</returns>
        public static CgParameterClass GetParameterClassEnum(string pString)
        { return cgGetParameterClassEnum(pString); }

        /// <summary>
        /// <para>cgGetParameterClassString returns the name associated with a param class enumerant.</para>
        /// <para>ERROR: None.</para>
        /// <para>VERSION: cgGetParameterClassString was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="parameterclass">The param class enumerant.</param>
        /// <returns>Returns the name associated with parameterclass. Returns "unknown" if parameterclass is not a valid param class.</returns>
        public static string GetParameterClassString(CgParameterClass parameterclass)
        { return Marshal.PtrToStringAnsi(cgGetParameterClassString(parameterclass)); }

        /// <summary>
        /// <para>cgGetParameterColumns return the number of columns associated with the given param's type.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterColumns was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>Returns the number of columns associated with the type if param is a numeric type or an array of numeric types. Returns 0 otherwise.</returns>
        public static int GetParameterColumns(CgParameter param)
        { return cgGetParameterColumns(param); }

        /// <summary>
        /// <para>cgGetParameterContext allows the application to  retrieve a handle to the context to which a given param belongs.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterContext was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns a CgContext handle to the parent context. Returns NULL if an error occurs.</returns>
        public static CgContext GetParameterContext(CgParameter param)
        { return cgGetParameterContext(param); }

        /// <summary>
        /// <para>cgGetParameterDefaultValuedc allows the application to get the default values from any numeric param or param array.</para>
        /// <para>The default values are returned as doubles in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterDefaultValuedc was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="param">The param whose default values will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param default values will be written.</param>
        /// <returns>Returns the total number of default values written to v.</returns>
        public static int GetParameterDefaultValuedc(CgParameter param, int nelements, double[] v)
        { return cgGetParameterDefaultValuedc(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterDefaultValuedr allows the application to get the default values from any numeric param or param array.</para>
        /// <para>The default values are returned as doubles in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterDefaultValuedr was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="param">The param whose default values will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param default values will be written.</param>
        /// <returns>Returns the total number of default values written to v.</returns>
        public static int GetParameterDefaultValuedr(CgParameter param, int nelements, double[] v)
        { return cgGetParameterDefaultValuedr(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterDefaultValuefc allows the application to get the default values from any numeric param or param array.</para>
        /// <para>The default values are returned as floats in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterDefaultValuefc was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="param">The param whose default values will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param default values will be written.</param>
        /// <returns>Returns the total number of default values written to v.</returns>
        public static int GetParameterDefaultValuefc(CgParameter param, int nelements, float[] v)
        { return cgGetParameterDefaultValuefc(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterDefaultValuefr allows the application to get the default values from any numeric param or param array.</para>
        /// <para>The default values are returned as floats in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterDefaultValuefr was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="param">The param whose default values will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param default values will be written.</param>
        /// <returns>Returns the total number of default values written to v.</returns>
        public static int GetParameterDefaultValuefr(CgParameter param, int nelements, float[] v)
        { return cgGetParameterDefaultValuefr(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterDefaultValueic allows the application to get the default values from any numeric param or param array.</para>
        /// <para>The default values are returned as ints in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterDefaultValueic was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="param">The param whose default values will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param default values will be written.</param>
        /// <returns>Returns the total number of default values written to v.</returns>
        public static int GetParameterDefaultValueic(CgParameter param, int nelements, int[] v)
        { return cgGetParameterDefaultValueic(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterDefaultValueir allows the application to get the default values from any numeric param or param array.</para>
        /// <para>The default values are returned as ints in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterDefaultValueir was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="param">The param whose default values will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param default values will be written.</param>
        /// <returns>Returns the total number of default values written to v.</returns>
        public static int GetParameterDefaultValueir(CgParameter param, int nelements, int[] v)
        { return cgGetParameterDefaultValueir(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterDirection allows the application to distinguish program input parameters from program output parameters.</para>
        /// <para>This information is necessary for the application to properly supply the program inputs and use the program outputs.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterDirection was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The program parameter.</param>
        /// <returns>Returns the direction of param. Returns CG_ERROR if an error occurs.</returns>
        public static CgEnum GetParameterDirection(CgParameter param)
        { return cgGetParameterDirection(param); }

        /// <summary>
        /// <para>cgGetParameterEffect allows the application to  retrieve a handle to the effect to which a given param belongs.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterEffect was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns a CGeffect handle to the parent effect. Returns NULL if the param is not a child of an effect or if an error occurs.</returns>
        public static CgEffect GetParameterEffect(CgParameter param)
        { return cgGetParameterEffect(param); }

        /// <summary>
        /// <para>cgGetParameterIndex returns the integer index of an array param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_ARRAY_PARAM_ERROR is generated if param is not an array param.</para>
        /// <para>VERSION: cgGetParameterIndex was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the index associated with an array member param. Returns -1 if the param is not in an array.</returns>
        public static int GetParameterIndex(CgParameter param)
        { return cgGetParameterIndex(param); }

        /// <summary>
        /// <para>cgGetParameterName allows the application to retrieve the name of a param in a Cg program.</para>
        /// <para>This name can be used later to retrieve the param from the program using cgGetNamedParameter.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterName was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The program parameter.</param>
        /// <returns>Returns the NULL-terminated name string for the param. Returns NULL if param is invalid.</returns>
        public static string GetParameterName(CgParameter param)
        { return Marshal.PtrToStringAnsi(cgGetParameterName(param)); }

        /// <summary>
        /// <para>cgGetParameterNamedType returns the type of param similarly to cgGetParameterType.</para>
        /// <para>However, if the type is a user defined struct it will return the unique enumerant associated with the user defined type instead of CG_STRUCT.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterNamedType was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the type of param.</returns>
        public static int GetParameterNamedType(CgParameter param)
        { return cgGetParameterNamedType(param); }

        /// <summary>
        /// <para>cgGetParameterOrdinalNumber returns an integer  that represents the order in which the param was declared within the Cg program.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterOrdinalNumber was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The program parameter.</param>
        /// <returns>Returns the ordinal number associated with a param. If the param is a constant (cgGetParameterVariability returns CG_CONSTANT) then 0 is returned and no error is generated. When cgGetParameterOrdinalNumber is passed an array, the ordinal number of the first array element is returned. When passed a struct, the ordinal number of first struct data member is returned.</returns>
        public static int GetParameterOrdinalNumber(CgParameter param)
        { return cgGetParameterOrdinalNumber(param); }

        /// <summary>
        /// <para>cgGetParameterProgram allows the application to  retrieve a handle to the program to which a given param belongs.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterProgram was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns a CgProgram handle to the parent program. Returns NULL if the param is not a child of a program or an error occurs.</returns>
        public static CgProgram GetParameterProgram(CgParameter param)
        { return cgGetParameterProgram(param); }

        /// <summary>
        /// <para>cgGetParameterResource allows the application to  retrieve the resource for a param in a Cg program.</para>
        /// <para>This resource is necessary for the application to be able to supply the program's inputs and use the program's outputs.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is not a leaf node.</para>
        /// <para>VERSION: cgGetParameterResource was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The program param.</param>
        /// <returns>Returns the resource of param.</returns>
        public static CgResource GetParameterResource(CgParameter param)
        { return cgGetParameterResource(param); }

        /// <summary>
        /// <para>cgGetParameterResourceIndex allows the application to retrieve the resource index for a param in a Cg program.</para>
        /// <para>This index value is  only used with resources that are linearly addressable.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is not a leaf node.</para>
        /// <para>VERSION: cgGetParameterResourceIndex was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The program param.</param>
        /// <returns>Returns the resource index of param. Returns 0 if an error occurs.</returns>
        public static int GetParameterResourceIndex(CgParameter param)
        { return cgGetParameterResourceIndex(param); }

        /// <summary>
        /// <para>cgGetParameterResourceName allows the application to retrieve the resource name for a param in a Cg program.</para>
        /// <para>For translated profiles this name will most likely be different from the string returned by cgGetResourceString.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is not a leaf node.</para>
        /// <para>VERSION: cgGetParameterResourceName was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="param">The program param.</param>
        /// <returns>Returns the resource name of param. Returns NULL if an error occurs.</returns>
        public static string GetParameterResourceName(CgParameter param)
        { return cgGetParameterResourceName(param); }

        /// <summary>
        /// <para>cgGetParameterResourceSize returns the size in bytes of the resource corresponding to a param if the param belongs to a Cg buffer resource.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterResourceSize was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="param">The param for which the associated resource size will be retrieved.</param>
        /// <returns>Returns the size on the GPU of the resource associated with param. Returns -1 if an error occurs.</returns>
        public static int GetParameterResourceSize(CgParameter param)
        { return cgGetParameterResourceSize(param); }

        /// <summary>
        /// <para>cgGetParameterResourceType allows the application to retrieve the resource type for a param in a Cg program.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterResourceType was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="param">The param for which the resource type will be returned.</param>
        /// <returns>Returns the resource type of param. Returns CG_UNKNOWN_TYPE if the param does not belong to a program, if the program is not compiled, or if an error occurs.</returns>
        public static CgType GetParameterResourceType(CgParameter param)
        { return cgGetParameterResourceType(param); }

        /// <summary>
        /// <para>cgGetParameterRows return the number of rows associated with the given param's type.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterRows was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>Returns the number of rows associated with the type if param is a numeric type or an array of numeric types. Returns 0 otherwise.</returns>
        public static int GetParameterRows(CgParameter param)
        { return cgGetParameterRows(param); }

        /// <summary>
        /// <para>cgGetParameterSemantic allows the application to retrieve the semantic of a param in a Cg program.</para>
        /// <para>If a uniform param does not have a user-assigned semantic, an empty string  will be returned.</para>
        /// <para>If a varying param does not have a user-assigned semantic, the semantic string corresponding to the compiler-assigned resource for that varying will be returned.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterSemantic was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>Returns the NULL-terminated semantic string for the param. Returns NULL if an error occurs.</returns>
        public static string GetParameterSemantic(CgParameter param)
        { return Marshal.PtrToStringAnsi(cgGetParameterSemantic(param)); }

        /// <summary>
        /// <para>cgGetParameterSettingMode returns the current param setting mode enumerant for context.</para>
        /// <para>See cgSetParameterSettingMode for more information.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgGetParameterSettingMode was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="context">The context from which the param setting mode will be retrieved.</param>
        /// <returns>Returns the param setting mode enumerant for context. Returns CG_UNKNOWN if an error occurs.</returns>
        public static CgEnum GetParameterSettingMode(CgContext context)
        { return cgGetParameterSettingMode(context); }

        /// <summary>
        /// <para>cgGetParameterType allows the application to retrieve the type of a param in a Cg program.</para>
        /// <para>This type is necessary for the application to be able to supply the program's inputs and use the program's outputs.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterType was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>Returns the type enumerant of param. Returns CG_UNKNOWN_TYPE if an error occurs.</returns>
        public static CgType GetParameterType(CgParameter param)
        { return cgGetParameterType(param); }

        /// <summary>
        /// <para>cgGetParameterValuedc allows the application to get the value(s) from any numeric param or param array.</para>
        /// <para>The value(s) are returned as doubles in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterValuedc was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param values will be written.</param>
        /// <returns>Returns the total number of values written to v.</returns>
        public static int GetParameterValuedc(CgParameter param, int nelements, double[] v)
        { return cgGetParameterValuedc(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterValuedr allows the application to get the value(s) from any numeric param or param array.</para>
        /// <para>The value(s) are returned as doubles in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterValuedr was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param values will be written.</param>
        /// <returns>Returns the total number of values written to v.</returns>
        public static int GetParameterValuedr(CgParameter param, int nelements, double[] v)
        { return cgGetParameterValuedr(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterValuefc allows the application to get the value(s) from any numeric param or param array.</para>
        /// <para>The value(s) are returned as floats in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterValuefc was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param values will be written.</param>
        /// <returns>Returns the total number of values written to v.</returns>
        public static int GetParameterValuefc(CgParameter param, int nelements, float[] v)
        { return cgGetParameterValuefc(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterValuefr allows the application to get the value(s) from any numeric param or param array.</para>
        /// <para>The value(s) are returned as floats in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterValuefr was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param values will be written.</param>
        /// <returns>Returns the total number of values written to v.</returns>
        public static int GetParameterValuefr(CgParameter param, int nelements, float[] v)
        { return cgGetParameterValuefr(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterValueic allows the application to get the value(s) from any numeric param or param array.</para>
        /// <para>The value(s) are returned as ints in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterValueic was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param values will be written.</param>
        /// <returns>Returns the total number of values written to v.</returns>
        public static int GetParameterValueic(CgParameter param, int nelements, int[] v)
        { return cgGetParameterValueic(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterValueir allows the application to get the value(s) from any numeric param or param array.</para>
        /// <para>The value(s) are returned as ints in v.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgGetParameterValueir was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be retrieved.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Destination buffer into which the param values will be written.</param>
        /// <returns>Returns the total number of values written to v.</returns>
        public static int GetParameterValueir(CgParameter param, int nelements, int[] v)
        { return cgGetParameterValueir(param, nelements, v); }

        /// <summary>
        /// <para>cgGetParameterValues is deprecated.</para>
        /// <para>Use a variation of cgGetParameterValue or cgGetParameterDefaultValue instead.</para>
        /// </summary>
        [Obsolete("Use a variation of cgGetParameterValue or cgGetParameterDefaultValue instead.")]
        public unsafe static double* GetParameterValues(CgParameter param, CgEnum value_type, int* nvalues)
        { return cgGetParameterValues(param, value_type, nvalues); }

        /// <summary>
        /// <para>cgGetParameterValues is deprecated.</para>
        /// <para>Use a variation of cgGetParameterValue or cgGetParameterDefaultValue instead.</para>
        /// </summary>
        [Obsolete("Use a variation of cgGetParameterValue or cgGetParameterDefaultValue instead.")]
        public static double[] GetParameterValues(CgParameter param, CgEnum value_type, [In]int[] nvalues)
        { return cgGetParameterValues(param, value_type, nvalues); }

        /// <summary>
        /// <para>cgGetParameterVariability allows the application to retrieve the variability of a param in a Cg program.</para>
        /// <para>This variability is necessary for the application to be able to supply the program's inputs and use the program's outputs.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgGetParameterVariability was introduced in Cg 1.1.</para>
        /// </summary>
        /// <returns>Returns the variability of param. Returns CG_ERROR if an error occurs.</returns>
        public static CgEnum GetParameterVariability(CgParameter param)
        { return cgGetParameterVariability(param); }

        /// <summary>
        /// <para>cgGetParentType returns a parent type of type.</para>
        /// <para>ERROR: CG_OUT_OF_ARRAY_BOUNDS_ERROR is generated if index is outside the proper range.</para>
        /// <para>VERSION: cgGetParentType was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="type">The child type.</param>
        /// <param name="index">The index of the parent type.  index must be greater than or equal to 0 and less than the value returned by cgGetNumParentTypes.</param>
        /// <returns>Returns the number of parent types. Returns NULL if there are no parents. Returns CG_UNKNOWN_TYPE if type is a built-in type or an error is thrown.</returns>
        public static CgType GetParentType(CgType type, int index)
        { return cgGetParentType(type, index); }

        /// <summary>
        /// <para>cgGetPassName allows the application to retrieve the name of a pass in a Cg program.</para>
        /// <para>This name can be used later to retrieve the pass from the program using cgGetNamedPass.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass.</para>
        /// <para>VERSION: cgGetPassName was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <returns>Returns the NULL-terminated name string for the pass. Returns NULL if pass is invalid.</returns>
        public static string GetPassName(CgPass pass)
        { return cgGetPassName(pass); }

        /// <summary>
        /// <para>cgGetPassProgram allows the application to retrieve the program associated with a specific domain from a pass.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass. CG_INVALID_ENUMERANT_ERROR is generated if domain is not CG_VERTEX_DOMAIN, CG_FRAGMENT_DOMAIN, or CG_GEOMETRY_DOMAIN.</para>
        /// <para>VERSION: cgGetPassProgram was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="pass">The pass from which to get a program.</param>
        /// <param name="domain">The domain for which a program will be retrieved.</param>
        /// <returns>Returns the program associated with a specified domain from the given pass. Returns NULL if pass or domain is invalid.</returns>
        public static CgProgram GetPassProgram(CgPass pass, CgDomain domain)
        { return cgGetPassProgram(pass, domain); }

        /// <summary>
        /// <para>cgGetPassTechnique allows the application to retrieve a handle to the technique to which a given pass belongs.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass.</para>
        /// <para>VERSION: cgGetPassTechnique was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <returns>Returns a CGtechnique handle to the technique. Returns NULL if an error occurs.</returns>
        public static CgTechnique GetPassTechnique(CgPass pass)
        { return cgGetPassTechnique(pass); }

        /// <summary>
        /// <para>cgGetProfileDomain returns which type of domain the given profile belongs to.</para>
        /// <para>VERSION: cgGetProfileDomain was introduced in Cg 1.5. CG_GEOMETRY_DOMAIN was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="profile">The profile enumerant.</param>
        /// <returns>Returns: CG_UNKNOWN_DOMAIN CG_VERTEX_DOMAIN CG_FRAGMENT_DOMAIN CG_GEOMETRY_DOMAIN</returns>
        public static CgDomain GetProfileDomain(CgProfile profile)
        { return cgGetProfileDomain(profile); }

        /// <summary>
        /// <para>cgGetProfileProperty returns property information about the given profile.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if profile is not supported by this version of the Cg library. CG_INVALID_ENUMERANT_ERROR is generated if query is not CG_IS_OPENGL_PROFILE, CG_IS_DIRECT3D_PROFILE, CG_IS_DIRECT3D_8_PROFILE, CG_IS_DIRECT3D_9_PROFILE, CG_IS_DIRECT3D_10_PROFILE, CG_IS_DIRECT3D_11_PROFILE, CG_IS_VERTEX_PROFILE, CG_IS_FRAGMENT_PROFILE, CG_IS_GEOMETRY_PROFILE, CG_IS_TESSELLATION_CONTROL_PROFILE, CG_IS_TESSELLATION_EVALUATION_PROFILE, CG_IS_TRANSLATION_PROFILE, CG_IS_HLSL_PROFILE, or CG_IS_GLSL_PROFILE</para>
        /// <para>VERSION: cgGetProfileProperty was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="profile">The profile to query.</param>
        /// <param name="query">An enumerant describing the property to be queried. The following enumerants are allowed:</param>
        /// <returns>Returns CG_TRUE if profile holds the property expressed by query. Returns CG_FALSE otherwise.</returns>
        public static CgBool GetProfileProperty(CgProfile profile, CgEnum query)
        { return cgGetProfileProperty(profile, query); }

        /// <summary>
        /// <para>cgGetProfileString returns the profile name associated with a profile enumerant.</para>
        /// <para>VERSION: cgGetProfileString was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="profile">The profile enumerant.</param>
        /// <returns>Returns the profile string of the enumerant profile. Returns NULL if profile is not a valid profile.</returns>
        public static string GetProfileString(CgProfile profile)
        { return cgGetProfileString(profile); }

        /// <summary>
        /// <para>cgGetProgramBuffer returns the buffer handle associated with a given buffer index from program.</para>
        /// <para>The returned value can be NULL if no buffer is associated with this index or if an error occurs.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_BUFFER_INDEX_OUT_OF_RANGE_ERROR is generated if bufferIndex is not within the valid range of buffer indices for program.</para>
        /// <para>VERSION: cgGetProgramBuffer was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="program">The program from which the associated buffer will be retrieved.</param>
        /// <param name="bufferIndex">The buffer index for which the associated buffer will be retrieved.</param>
        /// <returns>Returns a buffer handle on success. Returns NULL if an error occurs.</returns>
        public static CgBuffer GetProgramBuffer(CgProgram program, int bufferIndex)
        { return cgGetProgramBuffer(program, bufferIndex); }

        /// <summary>
        /// <para>cgGetProgramBufferMaxIndex returns the maximum buffer index for a profile.</para>
        /// <para>cgGetProgramBufferMaxIndex will return 0 if an invalid profile is passed.</para>
        /// <para>VERSION: cgGetProgramBufferMaxIndex was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="profile">The target for determining the maximum buffer index.</param>
        /// <returns>Returns the maximum buffer index for a given profile. Returns 0 if an error occurs.</returns>
        public static int GetProgramBufferMaxIndex(CgProfile profile)
        { return cgGetProgramBufferMaxIndex(profile); }

        /// <summary>
        /// <para>cgGetProgramBufferMaxSize returns the maximum size of a buffer for a profile in bytes.</para>
        /// <para>cgGetProgramBufferMaxSize will return 0 if an invalid profile is passed.</para>
        /// <para>VERSION: cgGetProgramBufferMaxSize was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="profile">The target for determining the maximum buffer size.</param>
        /// <returns>Returns the size of a buffer for the given profile in bytes. Returns 0 if an error occurs.</returns>
        public static int GetProgramBufferMaxSize(CgProfile profile)
        { return cgGetProgramBufferMaxSize(profile); }

        /// <summary>
        /// <para>cgGetProgramContext allows the application to retrieve a handle to the context to which a given program belongs.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetProgramContext was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns>Returns a CgContext handle to the parent context. Returns NULL if an error occurs.</returns>
        public static CgContext GetProgramContext(CgProgram program)
        { return cgGetProgramContext(program); }

        /// <summary>
        /// <para>cgGetProgramDomain retrieves the domain enumerant currently associated with a program.</para>
        /// <para>This is a convenience routine which essentially calls cgGetProgramProfile followed by cgGetProfileDomain.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetProgramDomain was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns>Returns the domain enumerant associated with program.</returns>
        public static CgDomain GetProgramDomain(CgProgram program)
        { return cgGetProgramDomain(program); }

        /// <summary>
        /// <para>cgGetProgramDomainProfile gets the profile of the passed combined program using the index to select which domain to choose.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_PARAMETER_ERROR is generated if index is less than 0 or greater than or equal to the number of domains in program.</para>
        /// <para>VERSION: cgGetProgramDomainProfile was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="program">The handle of the combined program object.</param>
        /// <param name="index">The index of the program's domain to be queried.</param>
        /// <returns>Returns the profile enumerant for the program with the given domain index, specifically one of: CG_UNKNOWN_DOMAIN CG_VERTEX_DOMAIN CG_FRAGMENT_DOMAIN CG_GEOMETRY_DOMAIN Returns CG_PROFILE_UNKNOWN if an error occurs.</returns>
        public static CgProfile GetProgramDomainProfile(CgProgram program, int index)
        { return cgGetProgramDomainProfile(program, index); }

        /// <summary>
        /// <para>A combined program consists of multiple domain programs.</para>
        /// <para>For example, a combined program may contain a vertex domain program and a fragment domain program.</para>
        /// <para>cgGetProgramDomainProgram gets the indexed domain program of the specified combined program.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_PARAMETER_ERROR is generated if index is less than 0 or greater than or equal to the number of domains in program.</para>
        /// <para>VERSION: cgGetProgramDomainProgram was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="program">The handle of the combined program object.</param>
        /// <param name="index">The index of the program's domain program to be queried.</param>
        /// <returns>Returns the program handle for the program with the given domain index. Returns 0 if an error occurs.</returns>
        public static CgProgram GetProgramDomainProgram(CgProgram program, int index)
        { return cgGetProgramDomainProgram(program, index); }

        /// <summary>
        /// <para>cgGetProgramInput returns the program input enumerant.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not valid program handle.</para>
        /// <para>VERSION: cgGetProgramInput was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="program">A program handle.</param>
        /// <returns>Returns a program input enumerant. If the program is a vertex or fragment program, it returns CG_VERTEX or CG_FRAGMENT, respectively. For geometry programs the input is one of: CG_POINT, CG_LINE, CG_LINE_ADJ, CG_TRIANGLE, or CG_TRIANGLE_ADJ. For tessellation control and evaluation programs the input is CG_PATCH. Returns CG_UNKNOWN if the input is unknown.</returns>
        public static CgEnum GetProgramInput(CgProgram program)
        { return cgGetProgramInput(program); }

        /// <summary>
        /// <para>cgGetProgramOptions allows the application to retrieve the set of options used to compile the program.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetProgramOptions was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="program">The Cg program to query.</param>
        /// <returns>Returns the options used to compile the program as an array of NULL-terminated strings. Returns NULL if no options exist, or if an error occurs.</returns>
        public static string[] GetProgramOptions(CgProgram program)
        {
            IntPtr ptr = cgGetProgramOptions(program);

            if (ptr == IntPtr.Zero) return null;

            unsafe
            {
                var byteArray = (byte**)ptr;
                var lines = new List<string>();
                var buffer = new List<byte>();

                for (; ; )
                {
                    byte* b = *byteArray;
                    for (; ; )
                    {
                        if (b == null || *b == '\0')
                        {
                            if (buffer.Count > 0)
                            {
                                char[] cc = Encoding.ASCII.GetChars(buffer.ToArray());
                                lines.Add(new string(cc));
                                buffer.Clear();
                            }
                            break;
                        }
                        buffer.Add(*b);
                        b++;
                    }
                    byteArray++;

                    if (b == null) break;
                }
                return lines.Count == 0 ? null : lines.ToArray();
            }
        }

        /// <summary>
        /// <para>cgGetProgramOutput returns the program output enumerant.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetProgramOutput was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="program">A program handle.</param>
        /// <returns>Returns a program output enumerant. If the program is a vertex or fragment program, it returns CG_VERTEX or CG_FRAGMENT, respectively. For geometry programs the output is one of: CG_POINT_OUT, CG_LINE_OUT, or CG_TRIANGLE_OUT. For tessellation control programs the output is CG_PATCH. Returns CG_UNKNOWN if the output is unknown.</returns>
        public static CgEnum GetProgramOutput(CgProgram program)
        { return cgGetProgramOutput(program); }

        /// <summary>
        /// <para>cgGetProgramProfile retrieves the profile enumerant currently associated with a program.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgGetProgramProfile was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns>Returns the profile enumerant associated with program.</returns>
        public static CgProfile GetProgramProfile(CgProgram program)
        { return cgGetProgramProfile(program); }

        /// <summary>
        /// <para>cgGetProgramStateAssignmentValue allows the application to  retrieve the value(s) of a state assignment that stores a CgProgram.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a program type.</para>
        /// <para>VERSION: cgGetProgramStateAssignmentValue was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <returns>Returns a CgProgram handle. Returns NULL if an error occurs or no program is available.</returns>
        public static CgProgram GetProgramStateAssignmentValue(CgStateAssignment sa)
        { return cgGetProgramStateAssignmentValue(sa); }

        /// <summary>
        /// <para>cgGetProgramString allows the application to retrieve program strings that have been set via functions that modify program state.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_ENUMERANT_ERROR is generated if enum is not CG_PROGRAM_SOURCE, CG_PROGRAM_ENTRY, CG_PROGRAM_PROFILE, or CG_COMPILED_PROGRAM.</para>
        /// <para>VERSION: cgGetProgramString was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program to query.</param>
        /// <param name="sourceType">Specifies the string to retrieve. enum can be one of CG_PROGRAM_SOURCE, CG_PROGRAM_ENTRY, CG_PROGRAM_PROFILE, or CG_COMPILED_PROGRAM.</param>
        /// <returns>Returns a NULL-terminated string based on the value of enum. Returns an empty string if an error occurs.</returns>
        public static string GetProgramString(CgProgram program, CgEnum sourceType)
        { return Marshal.PtrToStringAnsi(cgGetProgramString(program, sourceType)); }

        /// <summary>
        /// <para>cgGetResource returns the enumerant assigned to a resource name.</para>
        /// <para>VERSION: cgGetResource was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="resource_string">A string containing the resource name.</param>
        /// <returns>Returns the resource enumerant of resource_string. Returns CG_UNKNOWN if no such resource exists.</returns>
        public static CgResource GetResource([In]string resource_string)
        { return cgGetResource(resource_string); }

        /// <summary>
        /// <para>cgGetResourceString returns the resource name associated with a resource enumerant.</para>
        /// <para>VERSION: cgGetResourceString was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="resource">The resource enumerant.</param>
        /// <returns>Returns the NULL-terminated resource string of the enumerant resource.</returns>
        public static string GetResourceString(CgResource resource)
        { return Marshal.PtrToStringAnsi(cgGetResourceString(resource)); }

        /// <summary>
        /// <para>Given the handle to a state assignment in a sampler_state block in an effect file, cgGetSamplerStateAssignmentParameter returns a handle to the sampler param being initialized.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetSamplerStateAssignmentParameter was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment in a sampler_state block</param>
        /// <returns>Returns a handle to a param. Returns NULL if sa is not a state assignment in a sampler_state block.</returns>
        public static CgParameter GetSamplerStateAssignmentParameter(CgStateAssignment sa)
        { return cgGetSamplerStateAssignmentParameter(sa); }

        /// <summary>
        /// <para>cgGetSamplerStateAssignmentState allows the application to  retrieve the state of a state assignment that stores a sampler.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetSamplerStateAssignmentState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <returns>Returns a CGstate handle for the state. Returns NULL if the handle sa is invalid.</returns>
        public static CgState GetSamplerStateAssignmentState(CgStateAssignment sa)
        { return cgGetSamplerStateAssignmentState(sa); }

        /// <summary>
        /// <para>cgGetSamplerStateAssignmentValue allows the application to  retrieve the value(s) of a state assignment that stores a sampler.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a sampler type.</para>
        /// <para>VERSION: cgGetSamplerStateAssignmentValue was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <returns>Returns a CgParameter handle for the sampler. Returns NULL if an error occurs.</returns>
        public static CgParameter GetSamplerStateAssignmentValue(CgStateAssignment sa)
        { return cgGetSamplerStateAssignmentValue(sa); }

        /// <summary>
        /// <para>cgGetSemanticCasePolicy returns an enumerant indicating the current semantic case policy for the library.</para>
        /// <para>See cgSetSemanticCasePolicy for more information.</para>
        /// <para>VERSION: cgGetSemanticCasePolicy was introduced in Cg 2.0.</para>
        /// </summary>
        /// <returns>Returns an enumerant indicating the current semantic case policy.</returns>
        public static CgEnum GetSemanticCasePolicy()
        { return cgGetSemanticCasePolicy(); }

        /// <summary>
        /// <para>cgGetStateAssignmentIndex returns the array index of a state assignment if the state it is based on is an array type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetStateAssignmentIndex was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <returns>Returns an integer index value. Returns 0 if the CGstate for this state assignment is not an array type.</returns>
        public static int GetStateAssignmentIndex(CgStateAssignment sa)
        { return cgGetStateAssignmentIndex(sa); }

        /// <summary>
        /// <para>cgGetStateAssignmentPass allows the application to retrieve a handle to the pass to which a given sa belongs.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment.</para>
        /// <para>VERSION: cgGetStateAssignmentPass was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <returns>Returns a CGpass handle to the pass. Returns NULL if an error occurs.</returns>
        public static CgPass GetStateAssignmentPass(CgStateAssignment sa)
        { return cgGetStateAssignmentPass(sa); }

        /// <summary>
        /// <para>cgGetStateAssignmentState returns the CGstate object that corresponds to a particular state assignment in a pass.</para>
        /// <para>This object can then be queried to find out its type, giving the type of the state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_INVALID_STATE_HANDLE_ERROR is generated if the effect doesn't contain a state matching the given state assignment.</para>
        /// <para>VERSION: cgGetStateAssignmentState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle.</param>
        /// <returns>Returns the state corresponding to the given state assignment. Returns NULL if an error occurs.</returns>
        public static CgState GetStateAssignmentState(CgStateAssignment sa)
        { return cgGetStateAssignmentState(sa); }

        /// <summary>
        /// <para>cgGetStateContext allows the application to retrieve the context of a state. This is the context used to create the state with cgCreateState.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetStateContext was introduced in Cg 1.5. </para>
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>Returns the context to which state belongs. Returns NULL if an error occurs.</returns>
        public static CgContext GetStateContext(CgState state)
        { return cgGetStateContext(state); }

        /// <summary>
        /// <para>cgGetStateEnumerant allows the application to retrieve the enumerant name and value associated with a CGstate at a specified index location.</para>
        /// <para>The number of enumerants assocated with a state can be discovered using cgGetNumStateEnumerants.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state. CG_INVALID_POINTER_ERROR is generated if value is NULL. CG_INVALID_PARAMETER_ERROR is generated if index is less than 0 or index is greater than or equal to the number of enumerants associated with state.</para>
        /// <para>VERSION: cgGetStateEnumerant was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve an enumerant name and value.</param>
        /// <param name="index">The index for the enumerant in state.</param>
        /// <param name="value">Pointer to integer where the enumerant value will be stored.</param>
        /// <returns>Returns the NULL-terminated enumerant name string associated with state at position index. The enumerant value is returned via the value param. Returns NULL if an error occurs. value will be 0.</returns>
        public static string GetStateEnumerant(CgState state, int index, out int value)
        { return cgGetStateEnumerant(state, index, out value); }

        /// <summary>
        /// <para>cgGetStateEnumerantName returns the enumerant name associated with a given enumerant value from a specified state.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state. CG_INVALID_PARAMETER_ERROR is generated if state does not contain an enumerant defined for value.</para>
        /// <para>VERSION: cgGetStateEnumerantName was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve an enumerant name.</param>
        /// <param name="value">The enumerant value for which to retrieve the associated name.</param>
        /// <returns>Returns the NULL-terminated enumerant name string associated with the given enumerant value in state. Returns NULL if an error occurs.</returns>
        public static string GetStateEnumerantName(CgState state, int value)
        { return cgGetStateEnumerantName(state, value); }

        /// <summary>
        /// <para>cgGetStateEnumerantValue retrieves the enumerant value associated with a given enumerant name from the specified state.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state. CG_INVALID_PARAMETER_ERROR is generated if state does not contain name, if name is NULL, or if name points to an empty string.</para>
        /// <para>VERSION: cgGetStateEnumerantValue was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve the value associated with name.</param>
        /// <param name="name">The enumerant name for which to retrieve the associated value from state.</param>
        /// <returns>Returns the enumerant value associated with name. Returns -1 if an error occurs.</returns>
        public static int GetStateEnumerantValue(CgState state, [In]string name)
        { return cgGetStateEnumerantValue(state, name); }

        /// <summary>
        /// <para>cgGetStateLatestProfile gets the specified state's designated latest profile for states of type CG_PROGRAM_TYPE.</para>
        /// <para>This profile is used to compile the program for a state assignment for the state where the profile in the compile statement is the identifier latest.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetStateLatestProfile was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="state">The state handle.</param>
        /// <returns>Returns the designated latest profile if state is of type CG_PROGRAM_TYPE. Returns CG_PROFILE_UNKNOWN otherwise.</returns>
        public static CgProfile GetStateLatestProfile(CgState state)
        { return cgGetStateLatestProfile(state); }

        /// <summary>
        /// <para>cgGetStateName allows the application to retrieve the name of a state defined in a Cg context.</para>
        /// <para>This name can be used later to retrieve the state from the context using cgGetNamedState.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetStateName was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>Returns the NULL-terminated name string for the state. Returns NULL if state is invalid.</returns>
        public static string GetStateName(CgState state)
        { return cgGetStateName(state); }

        /// <summary>
        /// <para>cgGetStateResetCallback returns the callback function used for resetting the state when the given state is encountered in a pass in a technique.</para>
        /// <para>See cgSetStateCallbacks for more information.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetStateResetCallback was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve the callback.</param>
        /// <returns>Returns a pointer to the state resetting callback function. Returns NULL if state is not a valid state or if it has no callback.</returns>
        public static StateCallbackDelegate GetStateResetCallback(CgState state)
        { return cgGetStateResetCallback(state); }

        /// <summary>
        /// <para>cgGetStateSetCallback returns the callback function used for setting the state when the given state is encountered in a pass in a technique.</para>
        /// <para>See cgSetStateCallbacks for more information.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetStateSetCallback was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve the callback.</param>
        /// <returns>Returns a pointer to the state setting callback function. Returns NULL if state is not a valid state or if it has no callback.</returns>
        public static StateCallbackDelegate GetStateSetCallback(CgState state)
        { return cgGetStateSetCallback(state); }

        /// <summary>
        /// <para>cgGetStateType returns the type of a state that was previously defined via cgCreateState, cgCreateArrayState, cgCreateSamplerState, or cgCreateArraySamplerState.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetStateType was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve the type.</param>
        /// <returns>Returns the CGtype of the given state.</returns>
        public static CgType GetStateType(CgState state)
        { return cgGetStateType(state); }

        /// <summary>
        /// <para>cgGetStateValidateCallback returns the callback function used for validating the state when the given state is encountered in a pass in a technique.</para>
        /// <para>See cgSetStateCallbacks and cgCallStateValidateCallback for more information.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgGetStateValidateCallback was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state from which to retrieve the callback.</param>
        /// <returns>Returns a pointer to the state validateting callback function. Returns NULL if state is not a valid state or if it has no callback.</returns>
        public static StateCallbackDelegate GetStateValidateCallback(CgState state)
        { return cgGetStateValidateCallback(state); }

        /// <summary>
        /// <para>cgGetString returns an informative string depending on the enum.</para>
        /// <para>Currently there is only one valid enumerant that may be passed in. CG_VERSION - Returns the version string of the Cg runtime and compiler.</para>
        /// <para>ERROR: CG_INVALID_ENUMERANT_ERROR is generated if enum is not CG_VERSION.</para>
        /// <para>VERSION: cgGetString was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="sname">An enumerant describing the string to be returned.</param>
        /// <returns>Returns the string associtated with enum. Returns NULL in the event of an error.</returns>
        public static string GetString(CgEnum sname)
        { return Marshal.PtrToStringAnsi(cgGetString(sname)); }

        /// <summary>
        /// <para>cgGetStringAnnotationValue allows the application to  retrieve the value of a string typed annotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation.</para>
        /// <para>VERSION: cgGetStringAnnotationValue was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation.</param>
        /// <returns>Returns a pointer to a string contained by ann. Returns NULL if no value is available.</returns>
        public static string GetStringAnnotationValue(CgAnnotation ann)
        { return cgGetStringAnnotationValue(ann); }

        /// <summary>
        /// <para>cgGetStringAnnotationValues allows the application to retrieve the value(s) of a string typed annotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL.</para>
        /// <para>VERSION: cgGetStringAnnotationValues was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="ann">The annotation from which the values will be retrieved.</param>
        /// <param name="nvalues">Pointer to integer where the number of returned values will be stored.</param>
        /// <returns>Returns a pointer to an array of string values. The number of values in the array is returned via the nvalues param. Returns NULL if no values are available, ann is not string-typed, or an error occurs. nvalues will be 0.</returns>
        public static string[] GetStringAnnotationValues(CgAnnotation ann, out int nvalues)
        {
            var ptr = cgGetStringAnnotationValues(ann, out nvalues);

            if (nvalues == 0) return null;

            unsafe
            {
                var byteArray = (byte**)ptr;
                var lines = new List<string>();
                var buffer = new List<byte>();

                for (int i = 0; i < nvalues; i++)
                {
                    byte* b = *byteArray;
                    for (; ; )
                    {
                        if (*b == '\0')
                        {
                            char[] cc = Encoding.ASCII.GetChars(buffer.ToArray());
                            lines.Add(new string(cc));
                            buffer.Clear();
                            break;
                        }
                        buffer.Add(*b);
                        b++;
                    }
                    byteArray++;
                }
                return lines.ToArray();
            }
        }

        /// <summary>
        /// <para>cgGetStringAnnotationValues allows the application to retrieve the value(s) of a string typed annotation.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_ERROR is generated if nvalues is NULL.</para>
        /// <para>VERSION: cgGetStringAnnotationValues was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="ann">The annotation from which the values will be retrieved.</param>
        /// <returns>Returns a pointer to an array of string values. The number of values in the array is returned via the nvalues param. Returns NULL if no values are available, ann is not string-typed, or an error occurs. nvalues will be 0.</returns>
        public static string[] GetStringAnnotationValues(CgAnnotation ann)
        {
            int nvalues;
            var ptr = cgGetStringAnnotationValues(ann, out nvalues);

            if (nvalues == 0) return null;

            unsafe
            {
                var byteArray = (byte**)ptr;
                var lines = new List<string>();
                var buffer = new List<byte>();

                for (int i = 0; i < nvalues; i++)
                {
                    byte* b = *byteArray;
                    for (; ; )
                    {
                        if (*b == '\0')
                        {
                            char[] cc = Encoding.ASCII.GetChars(buffer.ToArray());
                            lines.Add(new string(cc));
                            buffer.Clear();
                            break;
                        }
                        buffer.Add(*b);
                        b++;
                    }
                    byteArray++;
                }
                return lines.ToArray();
            }
        }

        /// <summary>
        /// <para>cgGetStringParameterValue allows the application to  get the value of a string param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_TYPE_ERROR is generated if param is not string-typed.</para>
        /// <para>VERSION: cgGetStringParameterValue was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be retrieved.</param>
        /// <returns>Returns a pointer to the string contained by a string param. Returns NULL if the param does not contain a valid string value.</returns>
        public static string GetStringParameterValue(CgParameter param)
        { return cgGetStringParameterValue(param); }

        /// <summary>
        /// <para>cgGetStringStateAssignmentValue allows the application to  retrieve the value(s) of a string typed state assignment.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a string type.</para>
        /// <para>VERSION: cgGetStringStateAssignmentValue was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <returns>Returns a pointer to a string. Returns NULL if an error occurs.</returns>
        public static string GetStringStateAssignmentValue(CgStateAssignment sa)
        { return cgGetStringStateAssignmentValue(sa); }

        /// <summary>
        /// <para>cgGetSupportedProfile retrieves by index a profile supported by this version of the Cg library.</para>
        /// <para>The number of supported profiles can be found using cgGetNumSupportedProfiles.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if index is less than 0 or greater than or equal to the number of supported profiles returned by cgGetNumSupportedProfiles.</para>
        /// <para>VERSION: cgGetSupportedProfile was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="index">The index for the supported profile.</param>
        /// <returns>Returns the supported CgProfile at position index. Returns the CG_PROFILE_UNKNOWN if an error occurs.</returns>
        public static CgProfile GetSupportedProfile(int index)
        { return cgGetSupportedProfile(index); }

        /// <summary>
        /// <para>cgGetTechniqueEffect allows the application to retrieve a handle to the effect to which a given technique belongs.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgGetTechniqueEffect was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique.</param>
        /// <returns>Returns a CGeffect handle to the effect. Returns NULL if an error occurs.</returns>
        public static CgEffect GetTechniqueEffect(CgTechnique tech)
        { return cgGetTechniqueEffect(tech); }

        /// <summary>
        /// <para>cgGetTechniqueName allows the application to retrieve the name of a technique in a Cg effect.</para>
        /// <para>This name can be used later to retrieve the technique from the effect using cgGetNamedTechnique.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgGetTechniqueName was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique.</param>
        /// <returns>Returns the NULL-terminated name string for the technique. Returns NULL if tech is invalid.</returns>
        public static string GetTechniqueName(CgTechnique tech)
        { return cgGetTechniqueName(tech); }

        /// <summary>
        /// <para>cgGetTextureStateAssignmentValue allows the application to  retrieve the value(s) of a state assignment that stores a texture param.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a texture type.</para>
        /// <para>VERSION: cgGetTextureStateAssignmentValue was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment.</param>
        /// <returns>Returns a handle to the texture param associated with this state assignment. Returns NULL if an error occurs.</returns>
        public static CgParameter GetTextureStateAssignmentValue(CgStateAssignment sa)
        { return cgGetTextureStateAssignmentValue(sa); }

        /// <summary>
        /// <para>cgGetType returns the enumerant assigned to a type name.</para>
        /// <para>VERSION: cgGetType was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="type_string">A string containing the case-sensitive type name.</param>
        /// <returns>Returns the type enumerant of type_string. Returns CG_UNKNOWN_TYPE if no such type exists.</returns>
        public static CgType GetType([In]string type_string)
        { return cgGetType(type_string); }

        /// <summary>
        /// <para>cgGetTypeBase returns the base (scalar) type associated with a type enumerant.</para>
        /// <para>For example, cgGetTypeBase(CG_FLOAT3x4) returns CG_FLOAT.</para>
        /// <para>The base type for a non-numeric type such as CG_STRING, CG_STRUCT, CG_SAMPLER2D, or user-defined types is simply the type itself.</para>
        /// <para>VERSION: cgGetTypeBase was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="type">The type enumerant.</param>
        /// <returns>Returns the scalar base type of the enumerant type.</returns>
        public static CgType GetTypeBase(CgType type)
        { return cgGetTypeBase(type); }

        /// <summary>
        /// <para>cgGetTypeClass returns the param class associated with a type enumerant.</para>
        /// <para>For example, cgGetTypeClass(CG_FLOAT3x4) returns CG_PARAMETERCLASS_MATRIX while cgGetTypeClass(CG_HALF) returns CG_PARAMETERCLASS_SCALAR and cgGetTypeClass(CG_BOOL3) returns CG_PARAMETERCLASS_VECTOR.</para>
        /// <para>VERSION: cgGetTypeClass was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="type">The type enumerant.</param>
        /// <returns>Returns the param class of the enumerant type. Possible return values are: CG_PARAMETERCLASS_UNKNOWN CG_PARAMETERCLASS_SCALAR CG_PARAMETERCLASS_VECTOR CG_PARAMETERCLASS_MATRIX CG_PARAMETERCLASS_STRUCT CG_PARAMETERCLASS_ARRAY CG_PARAMETERCLASS_SAMPLER CG_PARAMETERCLASS_OBJECT</returns>
        public static CgParameterClass GetTypeClass(CgType type)
        { return cgGetTypeClass(type); }

        /// <summary>
        /// <para>cgGetTypeSizes returns the number of rows and columns for enumerant type in the locations specified by nrows and ncols respectively.</para>
        /// <para>VERSION: cgGetTypeSizes was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="type">The type enumerant.</param>
        /// <param name="nrows">The location where the number of rows will be written.</param>
        /// <param name="ncols">The location where the number of columns will be written.</param>
        /// <returns>Returns CG_TRUE if the type enumerant is for a matrix. Returns CG_FALSE otherwise.</returns>
        public static CgBool GetTypeSizes(CgType type, out int nrows, out int ncols)
        { return cgGetTypeSizes(type, out nrows, out ncols); }

        /// <summary>
        /// <para>cgGetTypeString returns the type name associated with a type enumerant.</para>
        /// <para>VERSION: cgGetTypeString was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="type">The type enumerant.</param>
        /// <returns>Returns the type string of the enumerant type.</returns>
        public static string GetTypeString(CgType type)
        { return Marshal.PtrToStringAnsi(cgGetTypeString(type)); }

        /// <summary>
        /// <para>cgGetUserType returns the enumerant associated with the user-defined type with the given index in the given CgProgram or CGeffect.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if handle is not a valid program or effect handle. CG_OUT_OF_ARRAY_BOUNDS_ERROR is generated if index is outside the proper range.</para>
        /// <para>VERSION: cgGetUserType was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="handle">The CgProgram or CGeffect in which the type is defined.</param>
        /// <param name="index">The index of the user-defined type.  index must be greater than or equal to 0 and less than the value returned by cgGetNumUserTypes.</param>
        /// <returns>Returns the type enumerant associated with the type with the given index.</returns>
        public static CgType GetUserType(CgHandle handle, int index)
        { return cgGetUserType(handle, index); }

        /// <summary>
        /// <para>cgIsAnnotation returns CG_TRUE if ann references a valid annotation, CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsAnnotation was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="ann">The annotation handle to check.</param>
        /// <returns>Returns CG_TRUE if ann references a valid annotation. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsAnnotation(CgAnnotation ann)
        { return cgIsAnnotation(ann); }

        /// <summary>
        /// <para>cgIsContext returns CG_TRUE if context references a valid context, CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsContext was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="context">The context handle to check.</param>
        /// <returns>Returns CG_TRUE if context references a valid context. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsContext(CgContext context)
        { return cgIsContext(context); }

        /// <summary>
        /// <para>cgIsEffect returns CG_TRUE if effect references a valid effect, CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsEffect was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="effect">The effect handle to check.</param>
        /// <returns>Returns CG_TRUE if effect references a valid effect. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsEffect(CgEffect effect)
        { return cgIsEffect(effect); }

        /// <summary>
        /// <para>cgIsInterfaceType returns CG_TRUE if type is  an interface (not just a struct), CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsInterfaceType was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="type">The type being evaluated.</param>
        /// <returns>Returns CG_TRUE if type is an interface (not just a struct). Returns CG_FALSE otherwise.</returns>
        public static CgBool IsInterfaceType(CgType type)
        { return cgIsInterfaceType(type); }

        /// <summary>
        /// <para>cgIsParameter returns CG_TRUE if param references a valid param object.</para>
        /// <para>cgIsParameter is typically used for iterating through the parameters of an object.</para>
        /// <para>It can also be used as a consistency check when the application caches CgParameter handles.</para>
        /// <para>Certain program operations like deleting the program or context object that the param is contained in will cause a param object to become invalid.</para>
        /// <para>VERSION: cgIsParameter was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The param handle to check.</param>
        /// <returns>Returns CG_TRUE if param references a valid param object. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsParameter(CgParameter param)
        { return cgIsParameter(param); }

        /// <summary>
        /// <para>cgIsParameterGlobal returns CG_TRUE if param is a global param and  CG_FALSE otherwise.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgIsParameterGlobal was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param handle to check.</param>
        /// <returns>Returns CG_TRUE if param is global. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsParameterGlobal(CgParameter param)
        { return cgIsParameterGlobal(param); }

        /// <summary>
        /// <para>cgIsParameterReferenced returns CG_TRUE if param is a program param, and is potentially referenced (used) within the program.</para>
        /// <para>It otherwise returns CG_FALSE.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgIsParameterReferenced was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="param">The handle of the param to check.</param>
        /// <returns>Returns CG_TRUE if param is a program param and is potentially referenced by the program. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsParameterReferenced(CgParameter param)
        { return cgIsParameterReferenced(param); }

        /// <summary>
        /// <para>cgIsParameterUsed returns CG_TRUE if param is potentially used by the given container.</para>
        /// <para>If param is a struct or array, CG_TRUE is returned if any of its children are potentially used by container.</para>
        /// <para>It otherwise returns CG_FALSE.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param, or if container is not the handle of a valid container.</para>
        /// <para>VERSION: cgIsParameterUsed was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param to check.</param>
        /// <param name="container">Specifies the CGeffect, CGtechnique, CGpass, CGstateassignment, or CgProgram that may potentially use param.</param>
        /// <returns>Returns CG_TRUE if param is potentially used by container. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsParameterUsed(CgParameter param, CgHandle handle)
        { return cgIsParameterUsed(param, handle); }

        /// <summary>
        /// <para>cgIsParentType returns CG_TRUE if parent is  a parent type of child.</para>
        /// <para>Otherwise CG_FALSE is returned.</para>
        /// <para>VERSION: cgIsParentType was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="parent">The parent type.</param>
        /// <param name="child">The child type.</param>
        /// <returns>Returns CG_TRUE if parent is a parent type of child. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsParentType(CgType parent, CgType child)
        { return cgIsParentType(parent, child); }

        /// <summary>
        /// <para>cgIsPass returns CG_TRUE if pass references a valid pass, CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsPass was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass handle to check.</param>
        /// <returns>Returns CG_TRUE if pass references a valid pass. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsPass(CgPass pass)
        { return cgIsPass(pass); }

        /// <summary>
        /// <para>cgIsProfileSupported checks whether profile is supported by this version of the Cg library.</para>
        /// <para>VERSION: cgIsProfileSupported was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="profile">The profile enumerant to test.</param>
        /// <returns>Returns CG_TRUE if profile is supported.</returns>
        public static CgBool IsProfileSupported(CgProfile profile)
        { return cgIsProfileSupported(profile); }

        /// <summary>
        /// <para>cgIsProgram return CG_TRUE if program references a valid program object.</para>
        /// <para>Note that this does not imply that the program has been successfully compiled.</para>
        /// <para>VERSION: cgIsProgram was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program handle to check.</param>
        /// <returns>Returns CG_TRUE if program references a valid program object. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsProgram(CgProgram program)
        { return cgIsProgram(program); }

        /// <summary>
        /// <para>cgIsProgramCompiled returns CG_TRUE if program has been compiled and CG_FALSE otherwise.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgIsProgramCompiled was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns>Returns CG_TRUE if program has been compiled. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsProgramCompiled(CgProgram program)
        { return cgIsProgramCompiled(program); }

        /// <summary>
        /// <para>cgIsState returns CG_TRUE if state references a valid state, CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state handle to check.</param>
        /// <returns>Returns CG_TRUE if state references a valid state. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsState(CgState state)
        { return cgIsState(state); }

        /// <summary>
        /// <para>cgIsStateAssignment returns CG_TRUE if sa references a valid state assignment, CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsStateAssignment was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="sa">The state assignment handle to check.</param>
        /// <returns>Returns CG_TRUE if sa references a valid state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsStateAssignment(CgStateAssignment sa)
        { return cgIsStateAssignment(sa); }

        /// <summary>
        /// <para>cgIsTechnique returns CG_TRUE if tech references a valid technique, CG_FALSE otherwise.</para>
        /// <para>VERSION: cgIsTechnique was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique handle to check.</param>
        /// <returns>Returns CG_TRUE if tech references a valid technique. Returns CG_FALSE otherwise.</returns>
        public static CgBool IsTechnique(CgTechnique tech)
        { return cgIsTechnique(tech); }

        /// <summary>
        /// <para>cgIsTechniqueValidated returns CG_TRUE if the technique has previously passes validation via a call to cgValidateTechnique.</para>
        /// <para>CG_FALSE is returned both if validation hasn't been attempted as well as if the technique has failed a validation attempt.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgIsTechniqueValidated was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique handle.</param>
        /// <returns>Returns CG_TRUE if the technique has previously passes validation via a call to cgValidateTechnique. Returns CG_FALSE if validation hasn't been attempted or the technique has failed a validation attempt.</returns>
        public static CgBool IsTechniqueValidated(CgTechnique tech)
        { return cgIsTechniqueValidated(tech); }

        /// <summary>
        /// <para>cgMapBuffer maps a buffer into the application's address space for memory-mapped updating of the buffer's data.</para>
        /// <para>The application should call cgUnmapBuffer|cgUnmapBuffer when it's done updating or querying the buffer.</para>
        /// <para>ERROR: CG_INVALID_BUFFER_HANDLE_ERROR is generated if buffer is not a valid buffer. CG_INVALID_ENUMERANT_ERROR is generated if access is not CG_READ_ONLY, CG_WRITE_ONLY, or CG_READ_WRITE. CG_BUFFER_ALREADY_MAPPED_ERROR is generated if buffer is already mapped.</para>
        /// <para>VERSION: cgMapBuffer was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="buffer">The buffer which will be mapped into the application's address space.</param>
        /// <param name="access">An enumerant indicating the operations the client may perform on the data store through the pointer while the buffer data is mapped.</param>
        /// <returns>Returns a pointer through which the application can read or write the buffer's data store. Returns NULL if an error occurs.</returns>
        public static IntPtr MapBuffer(CgBuffer buffer, CgBufferAccess access)
        { return cgMapBuffer(buffer, access); }

        /// <summary>
        /// <para>cgResetPassState resets all of the graphics state defined in a pass by calling the state resetting callbacks for all of the state assignments in the pass.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass. CG_INVALID_TECHNIQUE_ERROR is generated if the technique of which pass is a part has failed validation.</para>
        /// <para>VERSION: cgResetPassState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass handle.</param>
        public static void ResetPassState(CgPass pass)
        { cgResetPassState(pass); }

        /// <summary>
        /// <para>cgSetArraySize sets the size of a resiable array param param to size.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_ARRAY_PARAM_ERROR is generated if param is not an array param. CG_ARRAY_HAS_WRONG_DIMENSION_ERROR is generated if the dimension of the array param param is not 1. CG_PARAMETER_IS_NOT_RESIZABLE_ARRAY_ERROR is generated if param is not a resizable array. CG_INVALID_PARAMETER_ERROR is generated if size is less than 0.</para>
        /// <para>VERSION: cgSetArraySize was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The array param handle.</param>
        /// <param name="size">The new size of the array.</param>
        public static void SetArraySize(CgParameter param, int size)
        { cgSetArraySize(param, size); }

        /// <summary>
        /// <para>cgSetAutoCompile sets the auto compile mode for a given  context.</para>
        /// <para>By default, programs are immediately recompiled when they enter an uncompiled state.</para>
        /// <para>This may happen for a variety of reasons including :.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_ENUMERANT_ERROR is generated if autoCompileMode is not CG_COMPILE_MANUAL, CG_COMPILE_IMMEDIATE, or CG_COMPILE_LAZY.</para>
        /// <para>VERSION: cgSetAutoCompile was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="autoCompileMode">The auto-compile mode to which to set context.  Must be one of the following :</param>
        public static void SetAutoCompile(CgContext context, CgEnum autoCompileMode)
        { cgSetAutoCompile(context, autoCompileMode); }

        /// <summary>
        /// <para>cgSetBoolAnnotation sets the value of an annotation of bool type.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_TYPE_ERROR is generated if ann is not an annotation of bool type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if ann is not a scalar.</para>
        /// <para>VERSION: cgSetBoolAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="ann">The annotation that will be set.</param>
        /// <param name="value">The value to which ann will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the annotation. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetBoolAnnotation(CgAnnotation ann, CgBool value)
        { return cgSetBoolAnnotation(ann, value); }

        /// <summary>
        /// <para>cgSetBoolArrayStateAssignment sets the value of a state assignment of bool array type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a bool type.</para>
        /// <para>VERSION: cgSetBoolArrayStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment array of type CG_BOOL.</param>
        /// <param name="vals">The values which will be used to set sa.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetBoolArrayStateAssignment(CgStateAssignment sa, [In]CgBool[] vals)
        { return cgSetBoolArrayStateAssignment(sa, vals); }

        /// <summary>
        /// <para>cgSetBoolStateAssignment sets the value of a state assignment of bool type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a bool type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if sa is an array and not a scalar.</para>
        /// <para>VERSION: cgSetBoolStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment of type CG_BOOL.</param>
        /// <param name="value">The value to which sa will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetBoolStateAssignment(CgStateAssignment sa, CgBool value)
        { return cgSetBoolStateAssignment(sa, value); }

        /// <summary>
        /// <para>cgSetBufferData resizes and completely updates an existing buffer object.</para>
        /// <para>ERROR: CG_INVALID_BUFFER_HANDLE_ERROR is generated if buffer is not a valid buffer. CG_BUFFER_UPDATE_NOT_ALLOWED_ERROR is generated if buffer is currently mapped.</para>
        /// <para>VERSION: cgSetBufferData was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="buffer">The buffer which will be updated.</param>
        /// <param name="size">Specifies a new size for the buffer object. Zero for size means use the existing size of the buffer as the effective size.</param>
        /// <param name="data">Pointer to the data to copy into the buffer. The number of bytes to copy is determined by the size param.</param>
        public static void SetBufferData(CgBuffer buffer, int size, [In]IntPtr data)
        { cgSetBufferData(buffer, size, data); }

        /// <summary>
        /// <para>cgSetBufferSubData resizes and partially updates an existing buffer object.</para>
        /// <para>ERROR: CG_INVALID_BUFFER_HANDLE_ERROR is generated if buffer is not a valid buffer. CG_BUFFER_UPDATE_NOT_ALLOWED_ERROR is generated if buffer is currently mapped. CG_BUFFER_INDEX_OUT_OF_RANGE_ERROR is generated if offset or size is out of range.</para>
        /// <para>VERSION: cgSetBufferSubData was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="buffer">Buffer being updated.</param>
        /// <param name="offset">Buffer offset in bytes of the beginning of the partial update.</param>
        /// <param name="size">Number of buffer bytes to be updated. Zero means no update.</param>
        /// <param name="data">Pointer to the start of the data being copied into the buffer.</param>
        public static void SetBufferSubData(CgBuffer buffer, int offset, int size, [In]IntPtr data)
        { cgSetBufferSubData(buffer, offset, size, data); }

        /// <summary>
        /// <para>cgSetCompilerIncludeCallback sets a callback function used for handing include statements.</para>
        /// <para>Each Cg runtime context maintains a virtual file system of shader source source for inclusion by the compiler.</para>
        /// <para>Source source is populated into the virtual filesystem using cgSetCompilerIncludeString and cgSetCompilerIncludeFile.</para>
        /// <para>When the compiler encounters an include, firstly the virtual file system is searched for a match.</para>
        /// <para>Secondly the include callback function is called, providing an opportunity for populating shader source via cgSetCompilerIncludeString and cgSetCompilerIncludeFile.</para>
        /// <para>The callback function is passed the context and the requested name.</para>
        /// <para>Thirdly, the filesystem is searched in the usual manner.</para>
        /// <para>Fourthly, an error is raised by the compiler that the include can not be satisfied.</para>
        /// <para>NULL is passed to cgSetCompilerIncludeCallback to disable the callback.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgSetCompilerIncludeCallback was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="context">The context for which the include callback will be used.</param>
        /// <param name="func">A pointer to the include callback function.</param>
        public static void SetCompilerIncludeCallback(CgContext context, IncludeCallbackFuncDelegate func)
        { cgSetCompilerIncludeCallback(context, func); }

        /// <summary>
        /// <para>Each Cg runtime context maintains a virtual filesystem of shader source source for inclusion by the compiler.</para>
        /// <para>cgSetCompilerIncludeFile populates source source into the virtual filesystem from a file.</para>
        /// <para>A name is removed from the virtual filesystem by using NULL for the filename.</para>
        /// <para>The virtual filesystem is completely cleared by using NULL for the name.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_FILE_READ_ERROR is generated if the file filename can not be opened for input.</para>
        /// <para>VERSION: cgSetCompilerIncludeFile was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="context">The context in which to add the source source for inclusion by the compiler.</param>
        /// <param name="name">The virtual file system name of the shader source.</param>
        /// <param name="filename">System file system name of shader source file.</param>
        public static void SetCompilerIncludeFile(CgContext context, string name, string filename)
        { cgSetCompilerIncludeFile(context, name, filename); }

        /// <summary>
        /// <para>Each Cg runtime context maintains a virtual file system of shader source source for inclusion by the compiler.</para>
        /// <para>cgSetCompilerIncludeString populates source source into the virtual filesystem.</para>
        /// <para>A name is removed from the virtual filesystem by using NULL for the source.</para>
        /// <para>The virtual filesystem is completely cleared by using NULL for the name.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context.</para>
        /// <para>VERSION: cgSetCompilerIncludeString was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="context">The context in which to add the source source for inclusion by the compiler.</param>
        /// <param name="name">The virtual file system name of the shader source.</param>
        /// <param name="source">Shader source source string.</param>
        public static void SetCompilerIncludeString(CgContext context, string name, string source)
        { cgSetCompilerIncludeString(context, name, source); }

        /// <summary>
        /// <para>Each new version of Cg is supposed to be completely backwards compatible with previous versions, providing bug fixes and/or new capabilities while maintaining the behavior which applications were written against.</para>
        /// <para>The intent is to allow Cg to be updated and have existing applications continue to work exactly as designed.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_ENUMERANT_ERROR is generated if behavior is not CG_BEHAVIOR_3000, CG_BEHAVIOR_2200, CG_BEHAVIOR_CURRENT, or CG_BEHAVIOR_LATEST.</para>
        /// <para>VERSION: cgSetContextBehavior was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="context">The context for which the behavior will be set.</param>
        /// <param name="behavior">An enumerant which defines the behavior that will be exhibited by context.  The following enumerants are allowed:</param>
        public static void SetContextBehavior(CgContext context, CgBehavior behavior)
        { cgSetContextBehavior(context, behavior); }

        /// <summary>
        /// <para>cgSetEffectName allows the application to set the name of an effect.</para>
        /// <para>ERROR: CG_INVALID_EFFECT_HANDLE_ERROR is generated if effect is not a valid effect.</para>
        /// <para>VERSION: cgSetEffectName was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="effect">The effect in which the name will be set.</param>
        /// <param name="name">The new name for effect.</param>
        /// <returns>Returns CG_TRUE if it succeeds. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetEffectName(CgEffect effect, [In]string name)
        { return cgSetEffectName(effect, name); }

        /// <summary>
        /// <para>The cgSetEffectParameterBuffer allows the application with a single API call to set a Cg buffer to an effect parameter using the BUFFER semantic for each program in the effect that uses this effect parameter.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if param is not a valid parameter.</para>
        /// <para>VERSION: cgSetEffectParameterBuffer was introduced in Cg 3.0.</para>
        /// </summary>
        /// <param name="param">The effect parameter used by programs in the effect as a buffer parameter.</param>
        /// <param name="buffer">The Cg buffer being set to param for each program in the effect that uses param.</param>
        public static void SetEffectParameterBuffer(CgParameter param, CgBuffer buffer)
        { cgSetEffectParameterBuffer(param, buffer); }

        /// <summary>
        /// <para>cgSetErrorCallback sets a callback function that will be called every time an error occurrs.</para>
        /// <para>The callback function is not passed any parameters.</para>
        /// <para>It is assumed that the callback function  will call cgGetError to obtain the current error.</para>
        /// <para>To disable the callback function, cgSetErrorCallback may be called with NULL.</para>
        /// <para>VERSION: cgSetErrorCallback was introduced in Cg 1.1.</para>
        /// </summary>
        /// <param name="func">A function pointer to the error callback function.</param>
        public static void SetErrorCallback(ErrorCallbackFuncDelegate func)
        { cgSetErrorCallback(func); }

        /// <summary>
        /// <para>cgSetErrorHandler specifies an error handler function that will be called every time a Cg runtime error occurrs.</para>
        /// <para>The callback function is passed:</para>
        /// <para>Context - The context in which the error occured. If the context cannot be determined, NULL is used.</para>
        /// <para>Error - The enumerant of the error triggering the callback.</para>
        /// <para>Appdata - The value of the pointer passed to cgSetErrorHandler. This pointer can be used to make arbitrary application-side information available to the error handler.</para>
        /// <para>VERSION: cgSetErrorHandler was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="func">A pointer to the error handler callback function.</param>
        /// <param name="data">A pointer to arbitrary application-provided data.</param>
        public static void SetErrorHandler(ErrorHandlerFuncDelegate func, IntPtr data)
        { cgSetErrorHandler(func, data); }

        /// <summary>
        /// <para>cgSetFloatAnnotation sets the value of an annotation of float type.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_TYPE_ERROR is generated if ann is not an annotation of float type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if ann is not a scalar.</para>
        /// <para>VERSION: cgSetFloatAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="ann">The annotation that will be set.</param>
        /// <param name="value">The value to which ann will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the annotation. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetFloatAnnotation(CgAnnotation ann, float value)
        { return cgSetFloatAnnotation(ann, value); }

        /// <summary>
        /// <para>cgSetFloatArrayStateAssignment sets the value of a state assignment of float array type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a float type.</para>
        /// <para>VERSION: cgSetFloatArrayStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment array of type CG_FLOAT, CG_FIXED, CG_HALF.</param>
        /// <param name="vals">The values which will be used to set sa.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetFloatArrayStateAssignment(CgStateAssignment sa, [In]float[] vals)
        { return cgSetFloatArrayStateAssignment(sa, vals); }

        /// <summary>
        /// <para>cgSetFloatStateAssignment sets the value of a state assignment of float type.</para>
        /// <para>ERROR: CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a float type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if sa is an array and not a scalar.</para>
        /// <para>VERSION: cgSetFloatStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment of type CG_FLOAT, CG_FIXED, or CG_HALF.</param>
        /// <param name="value">The value to which sa will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetFloatStateAssignment(CgStateAssignment sa, float value)
        { return cgSetFloatStateAssignment(sa, value); }

        /// <summary>
        /// <para>cgSetIntAnnotation sets the value of an annotation of int type.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_TYPE_ERROR is generated if ann is not an annotation of int type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if ann is not a scalar.</para>
        /// <para>VERSION: cgSetIntAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="ann">The annotation that will be set.</param>
        /// <param name="value">The value to which ann will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the annotation. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetIntAnnotation(CgAnnotation ann, int value)
        { return cgSetIntAnnotation(ann, value); }

        /// <summary>
        /// <para>cgSetIntArrayStateAssignment sets the value of a state assignment of int array type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of an int type.</para>
        /// <para>VERSION: cgSetIntArrayStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment array of type CG_INT.</param>
        /// <param name="vals">The values which will be used to set sa.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetIntArrayStateAssignment(CgStateAssignment sa, [In]int[] vals)
        { return cgSetIntArrayStateAssignment(sa, vals); }

        /// <summary>
        /// <para>cgSetIntStateAssignment sets the value of a state assignment of int type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of an int type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if sa is an array and not a scalar.</para>
        /// <para>VERSION: cgSetIntStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment of type CG_INT.</param>
        /// <param name="value">The value to which sa will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetIntStateAssignment(CgStateAssignment sa, int value)
        { return cgSetIntStateAssignment(sa, value); }

        /// <summary>
        /// <para>Each Cg context maintains a NULL-terminated string containing warning and error messages generated by the Cg compiler, state managers and the like.</para>
        /// <para>cgSetLastListing allows applications and custom state managers to set the listing text.</para>
        /// <para>ERROR: CG_INVALID_PARAMETER_ERROR is generated if handle is invalid.</para>
        /// <para>VERSION: cgSetLastListing was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="handle">A CgContext, CGstateassignment, CGeffect, CGpass, or CGtechnique belonging to the context whose listing text is to be set.</param>
        /// <param name="listing">The new listing text.</param>
        public static void SetLastListing(CgHandle handle, string listing)
        { cgSetLastListing(handle, listing); }

        /// <summary>
        /// <para>cgSetLockingPolicy allows an application to change the locking policy used by the Cg library.</para>
        /// <para>The default policy is CG_THREAD_SAFE_POLICY, meaning a lock is used to serialize access to the library by mulitiple threads.</para>
        /// <para>Single threaded applications can change this policy to CG_NO_LOCKS_POLICY to avoid the overhead associated with this lock.</para>
        /// <para>Multithreaded applications should never change this policy.</para>
        /// <para>ERROR: CG_INVALID_ENUMERANT_ERROR is generated if lockingPolicy is not CG_NO_LOCKS_POLICY or CG_THREAD_SAFE_POLICY.</para>
        /// <para>VERSION: cgSetLockingPolicy was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="lockingPolicy">An enumerant describing the desired locking policy for the library. The following enumerants are allowed: CG_THREAD_SAFE_POLICY, CG_NO_LOCKS_POLICY</param>
        /// <returns>Returns the previous locking policy, or CG_UNKNOWN if an error occurs.</returns>
        public static CgEnum SetLockingPolicy(CgEnum lockingPolicy)
        { return cgSetLockingPolicy(lockingPolicy); }

        /// <summary>
        /// <para>cgSetMatrixParameterdc sets the value of a given matrix param from an array of doubles laid out in column-major order.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if the param fails to set for any other reason.</para>
        /// <para>VERSION: cgSetMatrixParameterdc was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix param.  The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterdc(CgParameter param, double[] matrix)
        { cgSetMatrixParameterdc(param, matrix); }

        /// <summary>
        /// <para>cgSetMatrixParameterdr sets the value of a given matrix param from an array of doubles laid out in row-major order.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if the param fails to set for any other reason.</para>
        /// <para>VERSION: cgSetMatrixParameterdr was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix param.  The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterdr(CgParameter param, double[] matrix)
        { cgSetMatrixParameterdr(param, matrix); }

        /// <summary>
        /// <para>cgSetMatrixParameterfc sets the value of a given matrix param from an array of floats laid out in column-major order.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if the param fails to set for any other reason.</para>
        /// <para>VERSION: cgSetMatrixParameterfc was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix param.  The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterfc(CgParameter param, float[] matrix)
        { cgSetMatrixParameterfc(param, matrix); }

        /// <summary>
        /// <para>cgSetMatrixParameterfr sets the value of a given matrix param from an array of floats laid out in row-major order.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if the param fails to set for any other reason.</para>
        /// <para>VERSION: cgSetMatrixParameterfr was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix param.  The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterfr(CgParameter param, float[] matrix)
        { cgSetMatrixParameterfr(param, matrix); }

        /// <summary>
        /// <para>cgSetMatrixParameteric sets the value of a given matrix param from an array of ints laid out in column-major order.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if the param fails to set for any other reason.</para>
        /// <para>VERSION: cgSetMatrixParameteric was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix param.  The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameteric(CgParameter param, int[] matrix)
        { cgSetMatrixParameteric(param, matrix); }

        /// <summary>
        /// <para>cgSetMatrixParameterir sets the value of a given matrix param from an array of ints laid out in row-major order.</para>
        /// <para>ERROR: CG_NOT_MATRIX_PARAM_ERROR is generated if param is not a matrix param. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if the param fails to set for any other reason.</para>
        /// <para>VERSION: cgSetMatrixParameterir was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="matrix">An array of values used to set the matrix param.  The array must be the number of rows times the number of columns in size.</param>
        public static void SetMatrixParameterir(CgParameter param, int[] matrix)
        { cgSetMatrixParameterir(param, matrix); }

        /// <summary>
        /// <para>cgSetMultiDimArraySize sets the size of each dimension of resizable multi-dimensional array param param.</para>
        /// <para>sizes must be an array that has N number of elements where N is equal to the result of cgGetArrayDimension.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_ARRAY_PARAM_ERROR is generated if param is not an array param. CG_INVALID_POINTER_ERROR is generated if sizes is NULL. CG_INVALID_PARAMETER_ERROR is generated if any value in sizes is less than or equal to 0. CG_PARAMETER_IS_NOT_RESIZABLE_ARRAY_ERROR is generated if param is not a resizable array.</para>
        /// <para>VERSION: cgSetMultiDimArraySize was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The array param handle.</param>
        /// <param name="sizes">An array of sizes for each dimension of the array.</param>
        public static void SetMultiDimArraySize(CgParameter param, int[] sizes)
        { cgSetMultiDimArraySize(param, sizes); }

        /// <summary>
        /// <para>cgSetParameter1d sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter1d was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        public static void SetParameter1d(CgParameter param, double x)
        { cgSetParameter1d(param, x); }

        /// <summary>
        /// <para>cgSetParameter1dv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter1dv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter1dv(CgParameter param, double[] v)
        { cgSetParameter1dv(param, v); }

        /// <summary>
        /// <para>cgSetParameter1f sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter1f was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        public static void SetParameter1f(CgParameter param, float x)
        { cgSetParameter1f(param, x); }

        /// <summary>
        /// <para>cgSetParameter1fv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter1fv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter1fv(CgParameter param, float[] v)
        { cgSetParameter1fv(param, v); }

        /// <summary>
        /// <para>cgSetParameter1i sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter1i was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x">The value to which param will be set.</param>
        public static void SetParameter1i(CgParameter param, int x)
        { cgSetParameter1i(param, x); }

        /// <summary>
        /// <para>cgSetParameter1iv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter1iv was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter1iv(CgParameter param, int[] v)
        { cgSetParameter1iv(param, v); }

        /// <summary>
        /// <para>cgSetParameter2d sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter2d was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x">The values used to set param.</param>
        /// <param name="y">The values used to set param.</param>
        public static void SetParameter2d(CgParameter param, double x, double y)
        { cgSetParameter2d(param, x, y); }

        /// <summary>
        /// <para>cgSetParameter2dv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter2dv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter2dv(CgParameter param, double[] v)
        { cgSetParameter2dv(param, v); }

        /// <summary>
        /// <para>cgSetParameter2f sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter2f was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y">The values used to set param.</param>
        public static void SetParameter2f(CgParameter param, float x, float y)
        { cgSetParameter2f(param, x, y); }

        /// <summary>
        /// <para>cgSetParameter2fv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter2fv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter2fv(CgParameter param, float[] v)
        { cgSetParameter2fv(param, v); }

        /// <summary>
        /// <para>cgSetParameter2i sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter2i was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y">The values used to set param.</param>
        public static void SetParameter2i(CgParameter param, int x, int y)
        { cgSetParameter2i(param, x, y); }

        /// <summary>
        /// <para>cgSetParameter2iv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter2iv was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter2iv(CgParameter param, int[] v)
        { cgSetParameter2iv(param, v); }

        /// <summary>
        /// <para>cgSetParameter3d sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter3d was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y, z">The values used to set param.</param>
        public static void SetParameter3d(CgParameter param, double x, double y, double z)
        { cgSetParameter3d(param, x, y, z); }

        /// <summary>
        /// <para>cgSetParameter3dv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter3dv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter3dv(CgParameter param, double[] v)
        { cgSetParameter3dv(param, v); }

        /// <summary>
        /// <para>cgSetParameter3f sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter3f was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y, z">The values used to set param.</param>
        public static void SetParameter3f(CgParameter param, float x, float y, float z)
        { cgSetParameter3f(param, x, y, z); }

        /// <summary>
        /// <para>cgSetParameter3fv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter3fv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter3fv(CgParameter param, float[] v)
        { cgSetParameter3fv(param, v); }

        /// <summary>
        /// <para>cgSetParameter3i sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter3i was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y, z">The values used to set param.</param>
        public static void SetParameter3i(CgParameter param, int x, int y, int z)
        { cgSetParameter3i(param, x, y, z); }

        /// <summary>
        /// <para>cgSetParameter3iv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter3iv was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter3iv(CgParameter param, int[] v)
        { cgSetParameter3iv(param, v); }

        /// <summary>
        /// <para>cgSetParameter4d sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter4d was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y, z, w">The values used to set param.</param>
        public static void SetParameter4d(CgParameter param, double x, double y, double z, double w)
        { cgSetParameter4d(param, x, y, z, w); }

        /// <summary>
        /// <para>cgSetParameter4dv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter4dv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter4dv(CgParameter param, double[] v)
        { cgSetParameter4dv(param, v); }

        /// <summary>
        /// <para>cgSetParameter4f sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter4f was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y, z, w">The values used to set param.</param>
        public static void SetParameter4f(CgParameter param, float x, float y, float z, float w)
        { cgSetParameter4f(param, x, y, z, w); }

        /// <summary>
        /// <para>cgSetParameter4fv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter4fv was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter4fv(CgParameter param, float[] v)
        { cgSetParameter4fv(param, v); }

        /// <summary>
        /// <para>cgSetParameter4i sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter4i was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="x, y, z, w">The values used to set param.</param>
        public static void SetParameter4i(CgParameter param, int x, int y, int z, int w)
        { cgSetParameter4i(param, x, y, z, w); }

        /// <summary>
        /// <para>cgSetParameter4iv sets the value of a given scalar or vector param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program.</para>
        /// <para>VERSION: cgSetParameter4iv was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param that will be set.</param>
        /// <param name="v">Array of values to use to set param.</param>
        public static void SetParameter4iv(CgParameter param, int[] v)
        { cgSetParameter4iv(param, v); }

        /// <summary>
        /// <para>cgSetParameterSemantic allows the application to set the semantic of a param in a Cg program.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is not a leaf node, or if the semantic string is NULL.</para>
        /// <para>VERSION: cgSetParameterSemantic was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The program param.</param>
        /// <param name="semantic">The semantic.</param>
        public static void SetParameterSemantic(CgParameter param, string semantic)
        { cgSetParameterSemantic(param, semantic); }

        /// <summary>
        /// <para>cgSetParameterSettingMode controls the behavior of the context when setting parameters.</para>
        /// <para>With deferred param setting, the corresponding 3D API param is not immediately updated by cgSetParameter commands.</para>
        /// <para>If the application does not need to access these 3D API param values, then this mode allows improved performance by avoiding unnecessary 3D API calls.</para>
        /// <para>ERROR: CG_INVALID_CONTEXT_HANDLE_ERROR is generated if context is not a valid context. CG_INVALID_ENUMERANT_ERROR is generated if parameterSettingMode is not CG_IMMEDIATE_PARAMETER_SETTING or CG_DEFERRED_PARAMETER_SETTING.</para>
        /// <para>VERSION: cgSetParameterSettingMode was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="context">The context in which to set the param setting mode.</param>
        /// <param name="parameterSettingMode">The mode to which context will be set.  Must be one of the following :</param>
        public static void SetParameterSettingMode(CgContext context, CgEnum parameterSettingMode)
        { cgSetParameterSettingMode(context, parameterSettingMode); }

        /// <summary>
        /// <para>cgSetParameterValuedc allows the application to set the value of any numeric param or param array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgSetParameterValuedc was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The program param whose value will be set.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Source buffer from which the param values will be read.</param>
        public static void SetParameterValuedc(CgParameter param, int nelements, double[] v)
        { cgSetParameterValuedc(param, nelements, v); }

        /// <summary>
        /// <para>cgSetParameterValuedr allows the application to set the value of any numeric param or param array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgSetParameterValuedr was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The program param whose value will be set.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Source buffer from which the param values will be read.</param>
        public static void SetParameterValuedr(CgParameter param, int nelements, double[] v)
        { cgSetParameterValuedr(param, nelements, v); }

        /// <summary>
        /// <para>cgSetParameterValuefc allows the application to set the value of any numeric param or param array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgSetParameterValuefc was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The program param whose value will be set.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Source buffer from which the param values will be read.</param>
        public static void SetParameterValuefc(CgParameter param, int nelements, float[] v)
        { cgSetParameterValuefc(param, nelements, v); }

        /// <summary>
        /// <para>cgSetParameterValuefr allows the application to set the value of any numeric param or param array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgSetParameterValuefr was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The program param whose value will be set.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Source buffer from which the param values will be read.</param>
        public static void SetParameterValuefr(CgParameter param, int nelements, float[] v)
        { cgSetParameterValuefr(param, nelements, v); }

        /// <summary>
        /// <para>cgSetParameterValueic allows the application to set the value of any numeric param or param array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgSetParameterValueic was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The program param whose value will be set.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Source buffer from which the param values will be read.</param>
        public static void SetParameterValueic(CgParameter param, int nelements, int[] v)
        { cgSetParameterValueic(param, nelements, v); }

        /// <summary>
        /// <para>cgSetParameterValueir allows the application to set the value of any numeric param or param array.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_ERROR is generated if param is a varying input to a fragment program. CG_INVALID_POINTER_ERROR is generated if v is NULL. CG_NOT_ENOUGH_DATA_ERROR is generated if nelements is less than the total size of param. CG_NON_NUMERIC_PARAMETER_ERROR is generated if param is of a non-numeric type.</para>
        /// <para>VERSION: cgSetParameterValueir was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The program param whose value will be set.</param>
        /// <param name="nelements">The number of elements in array v.</param>
        /// <param name="v">Source buffer from which the param values will be read.</param>
        public static void SetParameterValueir(CgParameter param, int nelements, int[] v)
        { cgSetParameterValueir(param, nelements, v); }

        /// <summary>
        /// <para>cgSetParameterVariability allows the  application to change the variability of a param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_ENUMERANT_ERROR is generated if vary is not CG_UNIFORM, CG_LITERAL, or CG_DEFAULT. CG_INVALID_PARAMETER_VARIABILITY_ERROR is generated if the param could not be changed to the variability indicated by vary. CG_INVALID_PARAMETER_TYPE_ERROR is generated if vary is CG_LITERAL and param is a not a numeric param.</para>
        /// <para>VERSION: cgSetParameterVariability was introduced in Cg 1.2.</para>
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="vary">The variability to which param will be set.</param>
        public static void SetParameterVariability(CgParameter param, CgEnum vary)
        { cgSetParameterVariability(param, vary); }

        /// <summary>
        /// <para>Given the handle to a program specified in a pass in a CgFX file, cgSetPassProgramParameters sets the values of the program's uniform parameters given the expressions in the compile statement in the CgFX file.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgSetPassProgramParameters was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="program">The program.</param>
        public static void SetPassProgramParameters(CgProgram program)
        { cgSetPassProgramParameters(program); }

        /// <summary>
        /// <para>cgSetPassState sets all of the graphics state defined in a pass by calling the state setting callbacks for all of the state assignments in the pass.</para>
        /// <para>ERROR: CG_INVALID_PASS_HANDLE_ERROR is generated if pass is not a valid pass. CG_INVALID_TECHNIQUE_ERROR is generated if the technique of which pass is a part has failed validation.</para>
        /// <para>VERSION: cgSetPassState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="pass">The pass handle.</param>
        public static void SetPassState(CgPass pass)
        { cgSetPassState(pass); }

        /// <summary>
        /// <para>cgSetProgramBuffer sets the buffer for a given buffer index of a program.</para>
        /// <para>A NULL buffer handle means the given buffer index should not be bound to a buffer.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_BUFFER_HANDLE_ERROR is generated if buffer is not a valid buffer. CG_BUFFER_INDEX_OUT_OF_RANGE_ERROR is generated if bufferIndex is not within the valid range of buffer indices for program.</para>
        /// <para>VERSION: cgSetProgramBuffer was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="program">The program for which the buffer will be set.</param>
        /// <param name="bufferIndex">The buffer index of program to which buffer will be bound.</param>
        /// <param name="buffer">The buffer to be bound.</param>
        public static void SetProgramBuffer(CgProgram program, int bufferIndex, CgBuffer buffer)
        { cgSetProgramBuffer(program, bufferIndex, buffer); }

        /// <summary>
        /// <para>cgSetProgramProfile allows the application to specify the profile to be used when compiling the given program.</para>
        /// <para>When called, the program will be unloaded if it is currently loaded, and marked as uncompiled.</para>
        /// <para>When the program is next compiled (see cgSetAutoCompile), the given profile will be used.</para>
        /// <para>cgSetProgramProfile can be used to override the profile specified in a CgFX compile statement, or to change the profile associated with a program created by a call to cgCreateProgram.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle. CG_INVALID_PROFILE_ERROR is generated if profile is not a valid profile enumerant.</para>
        /// <para>VERSION: cgSetProgramProfile was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="program">The program.</param>
        /// <param name="profile">The profile to be used when compiling the program.</param>
        public static void SetProgramProfile(CgProgram program, CgProfile profile)
        { cgSetProgramProfile(program, profile); }

        /// <summary>
        /// <para>cgSetProgramStateAssignment sets the value of a state assignment of program type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a program type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if sa is an array and not a scalar. CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgSetProgramStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment of type CG_PROGRAM_TYPE.</param>
        /// <param name="program">The program object to which sa will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetProgramStateAssignment(CgStateAssignment sa, CgProgram program)
        { return cgSetProgramStateAssignment(sa, program); }

        /// <summary>
        /// <para>cgSetSamplerState sets the sampler state for a sampler param that was specified via a sampler_state block in a CgFX file.</para>
        /// <para>The corresponding sampler should be bound via the graphics API before this call is made.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgSetSamplerState was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param handle.</param>
        public static void SetSamplerState(CgParameter param)
        { cgSetSamplerState(param); }

        /// <summary>
        /// <para>cgSetSamplerStateAssignment sets a state assignment of a sampler type to an effect param of the same sampler type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a sampler type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if sa is an array and not a scalar. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgSetSamplerStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A state assignment of a sampler type (one of CG_SAMPLER1D, CG_SAMPLER2D, CG_SAMPLER3D, CG_SAMPLERCUBE, or CG_SAMPLERRECT).</param>
        /// <param name="param">An effect param of a sampler type.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetSamplerStateAssignment(CgStateAssignment sa, CgParameter param)
        { return cgSetSamplerStateAssignment(sa, param); }

        /// <summary>
        /// <para>cgSetSemanticCasePolicy allows an application to change the semantic case policy used by the Cg library.</para>
        /// <para>A policy of CG_FORCE_UPPER_CASE_POLICY means that semantic strings returned by cgGetParameterSemantic will have been converted to all upper-case letters.</para>
        /// <para>This is the default policy for the library.</para>
        /// <para>If the policy is changed to CG_UNCHANGED_CASE_POLICY no case coversion will be done to the semantic strings.</para>
        /// <para>ERROR: CG_INVALID_ENUMERANT_ERROR is generated if casePolicy is not CG_FORCE_UPPER_CASE_POLICY or CG_UNCHANGED_CASE_POLICY.</para>
        /// <para>VERSION: cgSetSemanticCasePolicy was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="casePolicy">n enumerant describing the desired semantic case policy for the library. The following enumerants are allowed: CG_FORCE_UPPER_CASE_POLICY, CG_UNCHANGED_CASE_POLICY</param>
        /// <returns>Returns the previous semantic case policy, or CG_UNKNOWN if an error occurs.</returns>
        public static CgEnum SetSemanticCasePolicy(CgEnum casePolicy)
        { return cgSetSemanticCasePolicy(casePolicy); }

        /// <summary>
        /// <para>cgSetStateCallbacks sets the three callback functions for a state definition.</para>
        /// <para>These functions are later called when the state a particular state assignment based on this state must be set, reset, or validated.</para>
        /// <para>Any of the callback functions may be specified as NULL.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state.</para>
        /// <para>VERSION: cgSetStateCallbacks was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="state">The state handle.</param>
        /// <param name="set">The pointer to the callback function to call for setting the state of state assignments based on state.  This may be a NULL pointer.</param>
        /// <param name="reset">The pointer to the callback function to call for resetting the state of state assignments based on state.  This may be a NULL pointer.</param>
        /// <param name="validate">The pointer to the callback function to call for validating the state of state assignments based on state.  This may be a NULL pointer.</param>
        public static void SetStateCallbacks(CgState state, StateCallbackDelegate set, StateCallbackDelegate reset, StateCallbackDelegate validate)
        { cgSetStateCallbacks(state, set, reset, validate); }

        /// <summary>
        /// <para>cgSetStateLatestProfile sets the specified state's designated latest profile for states of type CG_PROGRAM_TYPE.</para>
        /// <para>ERROR: CG_INVALID_STATE_HANDLE_ERROR is generated if state is not a valid state. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if the type of state is not CG_PROGRAM_TYPE.</para>
        /// <para>VERSION: cgSetStateLatestProfile was introduced in Cg 2.2.</para>
        /// </summary>
        /// <param name="state">The state handle.</param>
        /// <param name="profile">The profile to designate as the state's latest profile.</param>
        public static void SetStateLatestProfile(CgState state, CgProfile profile)
        { cgSetStateLatestProfile(state, profile); }

        /// <summary>
        /// <para>cgSetStringAnnotation sets the value of an annotation of string type.</para>
        /// <para>ERROR: CG_INVALID_ANNOTATION_HANDLE_ERROR is generated if ann is not a valid annotation. CG_INVALID_PARAMETER_TYPE_ERROR is generated if ann is not an annotation of string type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if ann is not a scalar.</para>
        /// <para>VERSION: cgSetStringAnnotation was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="ann">The annotation that will be set.</param>
        /// <param name="value">The value to which ann will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the annotation. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetStringAnnotation(CgAnnotation ann, [In]string value)
        { return cgSetStringAnnotation(ann, value); }

        /// <summary>
        /// <para>cgSetStringParameterValue allows the application to  set the value of a string param.</para>
        /// <para>ERROR: CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param. CG_INVALID_PARAMETER_TYPE_ERROR is generated if param is not string-typed. CG_INVALID_PARAMETER_ERROR is generated if value is NULL.</para>
        /// <para>VERSION: cgSetStringParameterValue was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="param">The param whose value will be set.</param>
        /// <param name="value">The string to set the param's value as.</param>
        public static void SetStringParameterValue(CgParameter param, string value)
        { cgSetStringParameterValue(param, value); }

        /// <summary>
        /// <para>cgSetStringStateAssignment sets the value of a state assignment of string type.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of a string type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if sa is an array and not a scalar.</para>
        /// <para>VERSION: cgSetStringStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A handle to a state assignment of type CG_STRING.</param>
        /// <param name="value">The value to which sa will be set.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetStringStateAssignment(CgStateAssignment sa, [In]string value)
        { return cgSetStringStateAssignment(sa, value); }

        /// <summary>
        /// <para>cgSetTextureStateAssignment sets the value of a state assignment of texture type to an effect param of type CG_TEXTURE.</para>
        /// <para>ERROR: CG_INVALID_STATE_ASSIGNMENT_HANDLE_ERROR is generated if sa is not a valid state assignment. CG_STATE_ASSIGNMENT_TYPE_MISMATCH_ERROR is generated if sa is not a state assignment of texture type. CG_ARRAY_SIZE_MISMATCH_ERROR is generated if sa is an array and not a scalar. CG_INVALID_PARAM_HANDLE_ERROR is generated if param is not a valid param.</para>
        /// <para>VERSION: cgSetTextureStateAssignment was introduced in Cg 1.5.</para>
        /// </summary>
        /// <param name="sa">A state assignment of type CG_TEXTURE.</param>
        /// <param name="param">An effect param of type CG_TEXTURE.</param>
        /// <returns>Returns CG_TRUE if it succeeds in setting the state assignment. Returns CG_FALSE otherwise.</returns>
        public static CgBool SetTextureStateAssignment(CgStateAssignment sa, CgParameter param)
        { return cgSetTextureStateAssignment(sa, param); }

        /// <summary>
        /// <para>cgUnmapBuffer unmaps a buffer from the application's address space.</para>
        /// <para>ERROR: CG_INVALID_BUFFER_HANDLE_ERROR is generated if buffer is not a valid buffer.</para>
        /// <para>VERSION: cgUnmapBuffer was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="buffer">The buffer which will be unmapped from the application's address space.</param>
        public static void UnmapBuffer(CgBuffer buffer)
        { cgUnmapBuffer(buffer); }

        /// <summary>
        /// <para>cgUpdatePassParameters is a convenience routine which calls cgUpdateProgramParameters for all programs of a pass.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if pass is not a valid pass handle.</para>
        /// <para>VERSION: cgUpdatePassParameters was introduced in Cg 2.1.</para>
        /// </summary>
        /// <param name="pass">The pass for which deferred parameters will be updated.</param>
        public static void UpdatePassParameters(CgPass pass)
        { cgUpdatePassParameters(pass); }

        /// <summary>
        /// <para>cgUpdateProgramParameters performs the appropriate 3D API commands to set the 3D API resources for all of program's parameters that are marked update deferred and clears the update deferred state of these parameters.</para>
        /// <para>cgUpdateProgramParameters does nothing when none of program's parameters are marked update deferred.</para>
        /// <para>ERROR: CG_INVALID_PROGRAM_HANDLE_ERROR is generated if program is not a valid program handle.</para>
        /// <para>VERSION: cgUpdateProgramParameters was introduced in Cg 2.0.</para>
        /// </summary>
        /// <param name="program">The program for which deferred parameters will be sent to the corresponding 3D API parameters.</param>
        public static void UpdateProgramParameters(CgProgram program)
        { cgUpdateProgramParameters(program); }

        /// <summary>
        /// <para>cgValidateTechnique iterates over all of the passes of a technique and tests to see if every state assignment in the pass passes validation.</para>
        /// <para>ERROR: CG_INVALID_TECHNIQUE_HANDLE_ERROR is generated if tech is not a valid technique.</para>
        /// <para>VERSION: cgValidateTechnique was introduced in Cg 1.4.</para>
        /// </summary>
        /// <param name="tech">The technique handle to validate.</param>
        /// <returns>Returns CG_TRUE if all of the state assignments in all of the passes in tech are valid and can be used on the current hardware. Returns CG_FALSE if any state assignment fails validation, or if an error occurs.</returns>
        public static CgBool ValidateTechnique(CgTechnique tech)
        { return cgValidateTechnique(tech); }

        #endregion

        #region Internal Static Methods

        internal static bool[] IntPtrToBoolArray(IntPtr values, int count)
        {
            if (count > 0)
            {
                var retValue = new bool[count];
                unsafe
                {
                    var ii = (int*)values;
                    for (int i = 0; i < count; i++)
                    {
                        retValue[i] = ii[i] == True;
                    }
                }
                return retValue;
            }
            return null;
        }

        internal static unsafe string[] IntPtrToStringArray(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            var byteArray = (byte**)ptr;
            var lines = new List<string>();
            var buffer = new List<byte>();

            for (; ; )
            {
                byte* b = *byteArray;
                for (; ; )
                {
                    if (b == null || *b == '\0')
                    {
                        if (buffer.Count > 0)
                        {
                            char[] cc = Encoding.ASCII.GetChars(buffer.ToArray());
                            lines.Add(new string(cc));
                            buffer.Clear();
                        }
                        break;
                    }

                    buffer.Add(*b);
                    b++;
                }

                byteArray++;

                if (b == null)
                {
                    break;
                }
            }
            return lines.Count == 0 ? null : lines.ToArray();
        }

        #endregion Internal Static Methods
    }
}