using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodHub.Core.Runtime
{
    public class WindowDragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private Window _targetWindow;

        private Vector2 _beginDragPointerPosition;
        private Vector2 _beginDragWindowPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _beginDragPointerPosition = eventData.position;
            _beginDragWindowPosition = ((RectTransform)_targetWindow.transform).anchoredPosition;

            Debug.LogError($"{_beginDragPointerPosition}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 dragDelta = eventData.position - _beginDragPointerPosition;
            ((RectTransform)_targetWindow.transform).anchoredPosition = _beginDragWindowPosition + dragDelta;

            Debug.LogError($"{dragDelta}");
        }
    }
}