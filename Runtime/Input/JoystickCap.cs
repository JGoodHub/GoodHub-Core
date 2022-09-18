using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickCap : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public RectTransform Rect => (RectTransform)transform;
	
	private bool _dragging = false;
	public bool Dragging => _dragging;

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		_dragging = true;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		_dragging = false;
	}
	
}
