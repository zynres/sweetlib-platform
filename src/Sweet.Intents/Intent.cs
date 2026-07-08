using Sweet.Collections.Unsafe.List;
using Sweet.Intents.Generated;
using Sweet.Intents.Actions;
using Sweet.Intents.Axes;
using System.Numerics;
using Silk.NET.GLFW;
using Silk.NET.Core.Native;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Sweet.Intents;

public unsafe static class Intent
{
    private static IntentBuilder builder;
    private static WindowHandle* window;
    private static Glfw glfw;

    public static void Init(WindowHandle* _window, Glfw _glfw)
    {
        window = _window;
        glfw = _glfw;

        builder = new();

        EditorCameraIntents.Build(ref builder);

        glfw.SetKeyCallback(window, KeyCallback);
    }

    private static void KeyCallback(WindowHandle* window, Keys key, int scanCode, InputAction inputAction, KeyModifiers mods)
    {
        ref UnsafeList<Actions.Action> actions = ref builder.Keys[(uint)key];

        if (inputAction == InputAction.Press)
        {
            for (uint i = 0; i < actions.Length; i++)
            {
                ref Actions.Action action = ref actions[i];

                if (!action.IsDowned)
                {
                    ref Clause clause = ref action.State->Clauses[action.ClauseIndex];

                    action.IsDowned = true;

                    bool wasActive = clause.IsSatisfied;

                    clause.Current++;

                    if (!wasActive && clause.IsSatisfied)
                        action.State->SatisfiedClauses++;

                    Console.WriteLine();
                    Console.WriteLine($"Downed Key: {key}");
                    Console.WriteLine($"current: {clause.Current}");
                    Console.WriteLine($"active: {action.State->SatisfiedClauses}");
                }
            }
        }
        else if (inputAction == InputAction.Release)
        {
            for (uint i = 0; i < actions.Length; i++)
            {
                ref Actions.Action action = ref actions[i];

                if (action.IsDowned)
                {
                    ref Clause clause = ref action.State->Clauses[action.ClauseIndex];

                    action.IsDowned = false;

                    bool wasActive = clause.IsSatisfied;

                    clause.Current--;

                    if (wasActive && !clause.IsSatisfied)
                        action.State->SatisfiedClauses--;

                    Console.WriteLine();
                    Console.WriteLine($"Upped Key: {key}");
                    Console.WriteLine($"current: {clause.Current}");
                    Console.WriteLine($"active: {action.State->SatisfiedClauses}");
                }
            }
        }
        else if (inputAction == InputAction.Repeat)
        {

        }
        else
        {

        }
    }

    public static bool IsDown(Keys key)
    {
        return InputAction.Press == (InputAction)glfw.GetKey(window, key);
    }

    public static bool IsUp(Keys key)
    {
        return InputAction.Release == (InputAction)glfw.GetKey(window, key);
    }

    public static bool IsPressed(Keys key)
    {
        var state = (InputAction)glfw.GetKey(window, key);

        return InputAction.Press == state || InputAction.Repeat == state;
    }

    private static void MouseCallback()
    {

    }

    public static ref Vector2 GetAxis(AxisState* axisState)
    {
        return ref axisState->Value;
    }

    public static bool IsDown(ActionState* actionState)
    {
        return actionState->IsDown;
    }

    /*public static bool IsUp(ActionState* actionState)
    {
        return actionState->Up.IsKey;
    }

    public static bool IsPressed(ActionState* actionState)
    {
        return actionState->Pressed.IsKey;
    }*/

    public static bool IsMouse(MouseButton button)
    {
        var state = (InputAction)glfw.GetMouseButton(window, (int)button);

        return InputAction.Press == state || InputAction.Repeat == state;
    }

    public static void Dispose()
    {
        builder.Dispose();
    }
}