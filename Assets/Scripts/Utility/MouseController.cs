using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    public InputActionReference mouseMovementInput;
    public InputActionReference mouseLeftClickInput;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        mouseMovementInput.action.Enable();
        mouseLeftClickInput.action.Enable();
    }

    private void Update()
    {
        Color rayColor = Color.red;
        Ray ray = _mainCamera.ScreenPointToRay(mouseMovementInput.action.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out var hitInfo) && hitInfo.collider.GetComponent<DetectableObject>() is
            { } foundDetectableObject)
        {
            if (mouseLeftClickInput.action.ReadValue<float>() > 0 && mouseLeftClickInput.action.triggered)
                foundDetectableObject.onDetectionTrigger?.Invoke(gameObject);
            rayColor = Color.green;
        }

        Debug.DrawRay(ray.origin, ray.direction * (_mainCamera.farClipPlane + 1), rayColor);
    }

    private void OnDisable()
    {
        mouseMovementInput.action.Disable();
        mouseLeftClickInput.action.Disable();
    }
}