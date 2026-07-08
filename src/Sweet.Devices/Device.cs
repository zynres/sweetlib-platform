using System.Runtime.InteropServices;
using System.Numerics;
using Silk.NET.GLFW;

namespace Sweet.Devices;

public unsafe static class Device
{
    public static WindowDevice* Window { get; private set; }
    public static MouseDevice* Mouse { get; private set; }

    private static WindowHandle* window;
    private static Glfw glfw;

    public static void Init(WindowHandle* _window, Glfw _glfw)
    {
        window = _window;
        glfw = _glfw;

        Window = (WindowDevice*)NativeMemory.Alloc((nuint)sizeof(WindowDevice));
        *Window = new WindowDevice();

        Window->Init(glfw, window);

        Mouse = (MouseDevice*)NativeMemory.Alloc((nuint)sizeof(MouseDevice));
        *Mouse = new MouseDevice();

        Mouse->Init(glfw, window, in Window->Size);
    }

    public static void Update(bool isMouseRight)
    {
        Window->UpdateWindowSize(glfw, window);

        Mouse->WrapCursor(glfw, window, isMouseRight, in Window->Size);
    }

    public static void Dispose()
    {
        if (Window != null)
        {
            NativeMemory.Free(Window);
            Window = null;
        }

        if (Mouse != null)
        {
            NativeMemory.Free(Mouse);
            Mouse = null;
        }
    }
}