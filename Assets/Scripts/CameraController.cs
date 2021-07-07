using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class CameraController : MonoBehaviour
    {
        private Camera _mainCam;


        private Vector3 _basePosition;
        private Vector2 _dragCameraPosition;
        private Vector2 _previousDragCameraPosition;
        private bool _dragCameraButton;
        private bool _resetCameraInput;

        private bool _drag = false;

        
        public float dragSpeed;
        [Space] public InputActionReference cameraDragPositionReference;
        public InputActionReference cameraDragInputReference;
        public InputActionReference cameraResetReference;

        

        private void Awake()
        {
            _mainCam = Camera.main;
            _basePosition = _mainCam.transform.position;
        }

        private void OnEnable()
        {
            cameraDragPositionReference.Enable();
            cameraDragInputReference.Enable();
            cameraResetReference.Enable();
        }

        private void OnDisable()
        {
            cameraDragPositionReference.Disable();
            cameraDragInputReference.Disable();
            cameraResetReference.Disable();
        }


        private void Update()
        {
            _dragCameraPosition = cameraDragPositionReference.GetPosition();
            _dragCameraButton = cameraDragInputReference.GetButton();
            _resetCameraInput = cameraResetReference.GetButtonDown();
        }


        private void LateUpdate()
        {
            MoveCamera(_dragCameraPosition, _dragCameraButton, _resetCameraInput);
        }


        //Based of Paul Jang's Camera Controller: https://blog.naver.com/paulj2000/220868759391
        private void MoveCamera(Vector2 dragPos, bool dragCameraInput, bool resetCameraInput)
        {
            if (dragCameraInput)
            {
                if (_previousDragCameraPosition == Vector2.zero)
                {
                    _previousDragCameraPosition = dragPos;
                    return;
                }

                Vector2 delta = dragPos - _previousDragCameraPosition;
                Vector2 dir = delta.normalized;


                var position = _mainCam.transform.position;
                position = Vector3.Lerp(position,
                    position - new Vector3(dir.x, dir.y, 0) * (delta.magnitude * Time.deltaTime), 1f);
                _mainCam.transform.position = position;
                _previousDragCameraPosition = dragPos;
            }
            else
            {
                _previousDragCameraPosition = Vector2.zero;
            }
        }
    }
}