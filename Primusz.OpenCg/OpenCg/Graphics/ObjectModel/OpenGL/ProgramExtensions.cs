using OpenCg.Graphics.OpenGL;

namespace OpenCg.Graphics.ObjectModel.OpenGL
{
    public static class ProgramExtensions
    {
        public static void Bind(this Program program)
        {
            CgGL.BindProgram(program.Handle);
        }

        public static void EnableProfiles(this Program program)
        {
            CgGL.EnableProgramProfiles(program.Handle);
        }

        public static void DisableProfiles(this Program program)
        {
            CgGL.DisableProgramProfiles(program.Handle);
        }

        public static int GetProgramId(this Program program)
        {
            return CgGL.GetProgramID(program.Handle);
        }

        public static bool IsLoaded(this Program program)
        {
            return CgGL.IsProgramLoaded(program.Handle);
        }

        public static void Load(this Program program)
        {
            CgGL.LoadProgram(program.Handle);
        }

        public static void Unload(this Program program)
        {
            CgGL.UnloadProgram(program.Handle);
        }

        public static void Unbind(CgProfile profile)
        {
            CgGL.UnbindProgram(profile);
        }
    }
}
