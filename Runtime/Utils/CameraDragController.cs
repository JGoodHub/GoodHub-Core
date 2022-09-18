using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime
{
    public class CameraDragController : SceneSingleton<CameraDragController>
    {
        private Vector3 _dragStartPosition;
        private Vector3 _dragPosition;

        [SerializeField] private bool _lockToBounds;
        [SerializeField] private Bounds _cameraBoundary;

        public bool DragEnabled = true;

        private void OnEnable()
        {
            TouchInput.OnTouchDown += OnTouchDown;
            TouchInput.OnTouchDragStay += OnTouchDragStay;
        }

        private void OnDisable()
        {
            TouchInput.OnTouchDown -= OnTouchDown;
            TouchInput.OnTouchDragStay -= OnTouchDragStay;
        }

        private void OnTouchDown(TouchInput.TouchData touchData)
        {
            if (CameraHelper.IsMouseOverUI() || DragEnabled == false || touchData.DownOverUI)
                return;

            Vector3 planeIntersectionPoint = RaycastPlane.QueryPlane();
            _dragStartPosition = planeIntersectionPoint;
        }

        private void OnTouchDragStay(TouchInput.TouchData touchData)
        {
            if (CameraHelper.IsMouseOverUI() || DragEnabled == false || touchData.DownOverUI)
                return;

            Vector3 planeIntersectionPoint = RaycastPlane.QueryPlane();
            Vector3 dragDirection = _dragStartPosition - planeIntersectionPoint;

            _dragPosition = transform.position + dragDirection;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _dragPosition, 0.5f);

            if (_lockToBounds)
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, _cameraBoundary.min.x, _cameraBoundary.max.x), 0f, Mathf.Clamp(transform.position.z, _cameraBoundary.min.z, _cameraBoundary.max.z));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_cameraBoundary.center, _cameraBoundary.size);
        }
    }
}