using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public static class InputExtension
    {
        public static Vector2 GetPosition(this InputActionReference reference)
        {
            return reference && reference.action.name.ToLower().Contains("position")
                ? reference.action.ReadValue<Vector2>()
                : Vector2.zero;
        }

        public static bool GetButton(this InputActionReference reference)
        {
            return reference && reference.action.ReadValue<float>() > 0;
        }

        public static bool GetButtonDown(this InputActionReference reference)
        {
            return GetButton(reference) && reference && reference.action.triggered;
        }


        private static Camera _mainCamera;

        private static Camera GetMainCamera()
        {
            if (!_mainCamera)
                _mainCamera = Camera.main;

            return _mainCamera;
        }


        public static void Enable(this InputActionReference reference)
        {
            if (reference)
                reference.action.Enable();
        }

        public static void Disable(this InputActionReference reference)
        {
            if (reference)
                reference.action.Disable();
        }
    }


    public enum InputType
    {
        Mouse,
        Keyboard
    }
}