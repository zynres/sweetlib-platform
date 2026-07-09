using System.Numerics;
using Silk.NET.GLFW;
using Sweet.Intents;
using Sweet.Intents.Generated;

namespace Sweet.Devices;

public unsafe struct MouseDevice
{
    public Vector2 Position;
    public Vector2 Delta;

    internal Vector2 _lastPosition;

    internal void Init(Glfw glfw, WindowHandle* window, in Vector2 Size)
    {
        glfw.SetInputMode(window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
        SetMousePosition(glfw, window, new Vector2(Size.X / 2, Size.Y / 2));
    }

    internal void WrapCursor(Glfw glfw, WindowHandle* window, in Vector2 Size)
    {
        glfw.GetCursorPos(window, out double x, out double y);

        if (Intent.IsHeld(MouseButton.Right) && (x < 0 || x > Size.X || y < 0 || y > Size.Y))
        {
            if (x < 0)
                x = Size.X;
            if (x > Size.X)
                x = 0;

            if (y < 0)
                y = Size.Y;
            if (y > Size.Y)
                y = 0;

            glfw.SetCursorPos(window, x, y);

            _lastPosition = new Vector2((float)x, (float)y);
            Position = _lastPosition;
            Delta = Vector2.Zero;

            return;
        }

        Position = new Vector2((float)x, (float)y);
        Delta = Position - _lastPosition;
        _lastPosition = Position;
    }

    internal void SetMousePosition(Glfw glfw, WindowHandle* window, in Vector2 position)
    {
        glfw.SetCursorPos(window, position.X, position.Y);

        _lastPosition = position;
        Position = position;
        Delta = Vector2.Zero;
    }
}