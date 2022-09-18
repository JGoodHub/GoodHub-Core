using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
	private JoystickCap _cap;

	private Vector2 _direction;

	[SerializeField] private float _deadzone = 0.05f;
	[SerializeField] private int _reach = 100;
	[SerializeField] private float _resetSpeed = 20f;

	[SerializeField] private bool _logOutput;

	private void Awake()
	{
		_cap = GetComponentInChildren<JoystickCap>();
	}

	private void Update()
	{
		if (_cap.Rect.anchoredPosition.magnitude > _reach)
			_cap.Rect.anchoredPosition = _cap.Rect.anchoredPosition.normalized * _reach;

		_direction = _cap.Rect.anchoredPosition / _reach;

		if (_direction.magnitude < _deadzone)
			_direction = Vector2.zero;

		if (_cap.Dragging == false)
			_cap.Rect.anchoredPosition = Vector2.Lerp(_cap.Rect.anchoredPosition, Vector2.zero, _resetSpeed * Time.deltaTime);

		if (_logOutput)
			Debug.Log($"Joystick {name} - X: {_direction.x}, Y: {_direction.y}");
	}

}
