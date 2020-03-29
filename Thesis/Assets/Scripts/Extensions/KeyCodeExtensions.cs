using UnityEngine;

public static class KeyCodeExtensions
{
    public static bool IsKeyboardKey(this KeyCode key)
    {
        return key > KeyCode.None && key < KeyCode.Mouse0;
    }

    public static bool IsMouseKey(this KeyCode key)
    {
        return key >= KeyCode.Mouse0 && key <= KeyCode.Mouse6;
    }
}
