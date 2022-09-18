using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodHub.Core.Runtime
{
    public class TouchInput : GlobalSingleton<TouchInput>
    {
        public struct TouchData
        {
            public Vector2 DownPosition;
            public Vector2 CurrentPosition;
            public Vector2 UpPosition;

            public bool IsDragging;
            public float CurrentDragDistance;

            public bool DownOverUI;
            public bool CurrentlyOverUI;
            public bool UpOverUI;
        }

        public delegate void TouchDelegate(TouchData touchData);
        
        public static event TouchDelegate OnTouchDown;
        public static event TouchDelegate OnTouchUp;
        public static event TouchDelegate OnTouchClick;

        public static event TouchDelegate OnTouchDragEnter;
        public static event TouchDelegate OnTouchDragStay;
        public static event TouchDelegate OnTouchDragEnd;

        public float _relativeDistanceToDrag = 0.04f;
        private float _pixelDistanceToDrag;

        private static TouchData _frameTouchData;
        public static TouchData FrameTouchData => _frameTouchData;

        public bool debugEvents;

        private void Awake()
        {
            _pixelDistanceToDrag = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2)) * _relativeDistanceToDrag;
        }

        private void Update()
        {
            _frameTouchData.CurrentPosition = Input.mousePosition;
            _frameTouchData.CurrentlyOverUI = CameraHelper.IsMouseOverUI();

            if (Input.GetMouseButtonDown(0))
            {
                TriggerTouchDown();
            }

            if (Input.GetMouseButton(0))
            {
                _frameTouchData.CurrentDragDistance = Vector2.Distance(_frameTouchData.DownPosition, _frameTouchData.CurrentPosition);

                if (_frameTouchData.IsDragging || _frameTouchData.CurrentDragDistance >= _pixelDistanceToDrag)
                {
                    if (!_frameTouchData.IsDragging)
                    {
                        TriggerTouchDragEnter();
                    }

                    TriggerTouchDragStay();
                }
            }
            else
            {
                if (_frameTouchData.IsDragging)
                {
                    TriggerTouchDragExit();
                }

                _frameTouchData.CurrentDragDistance = 0;
            }

            if (Input.GetMouseButtonUp(0))
            {
                TriggerTouchUp();

                if (_frameTouchData.CurrentDragDistance < _pixelDistanceToDrag)
                {
                    TriggerTouchClick();
                }
            }
        }

        private void TriggerTouchDown()
        {
            _frameTouchData.DownPosition = _frameTouchData.CurrentPosition;
            _frameTouchData.DownOverUI = _frameTouchData.CurrentlyOverUI;

            OnTouchDown?.Invoke(_frameTouchData);

            if (debugEvents)
                Debug.Log("[DragCameraController]: Touch Down Triggered");
        }

        private void TriggerTouchUp()
        {
            _frameTouchData.UpPosition = _frameTouchData.CurrentPosition;
            _frameTouchData.UpOverUI = _frameTouchData.CurrentlyOverUI;

            OnTouchUp?.Invoke(_frameTouchData);

            if (debugEvents)
                Debug.Log("[DragCameraController]: Touch Up Triggered");
        }

        private void TriggerTouchClick()
        {
            OnTouchClick?.Invoke(_frameTouchData);

            if (debugEvents)
                Debug.Log("[DragCameraController]: Touch Click Triggered");
        }

        private void TriggerTouchDragEnter()
        {
            _frameTouchData.IsDragging = true;

            OnTouchDragEnter?.Invoke(_frameTouchData);

            if (debugEvents)
                Debug.Log("[DragCameraController]: Touch Drag Enter Triggered");
        }

        private void TriggerTouchDragStay()
        {
            _frameTouchData.CurrentPosition = Input.mousePosition;
            _frameTouchData.CurrentlyOverUI = EventSystem.current.IsPointerOverGameObject();

            OnTouchDragStay?.Invoke(_frameTouchData);

            if (debugEvents)
                Debug.Log("[DragCameraController]: Touch Drag Stay Triggered");
        }

        private void TriggerTouchDragExit()
        {
            _frameTouchData.IsDragging = false;

            OnTouchDragEnd?.Invoke(_frameTouchData);

            if (debugEvents)
                Debug.Log("[DragCameraController]: Touch Drag Exit Triggered");
        }
    }
}