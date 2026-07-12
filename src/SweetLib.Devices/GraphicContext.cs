using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace SweetLib.Devices;

public unsafe struct GraphicContext
{
    public WindowHandle* Window;
    public Glfw Glfw; 
    public GL GL;
}