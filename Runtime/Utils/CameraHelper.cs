using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodHub.Core.Runtime
{
    public static class CameraHelper
    {
        private static Camera _camera;

        public static Camera Camera => _camera ??= Camera.main;

        public static Ray GetMouseRay()
        {
            return Camera.ScreenPointToRay(Input.mousePosition);
        }

        public static bool GetMouseRaycast(out RaycastHit rayHit, float distance)
        {
            return Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out rayHit, distance);
        }

        public static bool IsMouseOverUI()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach (RaycastResult raycastResult in raycastResults)
            {
                if (raycastResult.gameObject.layer == 5)
                    return true;
            }

            return false;
        }
    }
}