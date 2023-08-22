using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] private bool _isStart;
    [SerializeField] private bool _isFirstTouch;
    [SerializeField] private Vector2 _lastTouchPosition;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private Vector2 _bounds = new Vector2(-6, 6);
    [SerializeField] private List<Transform> _snakeBody;
    [SerializeField] private float _distanceBetweenParts = 1f;
    [SerializeField] private float _horizontalVelocity = 5f;
    [SerializeField] private float _verticalVelocity = 1f;

    private Transform _camera;
    private float HorizontalTouchDelta => Input.GetMouseButton(0) ? _lastTouchPosition.x - Input.mousePosition.x : 0;

    private float HorizontalDeltaScreenNormalized => HorizontalTouchDelta / Screen.width;

    private void Start()
    {
        _camera = Camera.main.transform;
        _velocity = Vector2.zero;
        _isStart = false;
        UpdateCamera();
    }

    private void FixedUpdate()
    {
        var isTouch = Input.GetMouseButton(0);
        
        if (!_isStart && isTouch) _isStart = true;
        
        if (!_isStart) return;
        
        switch (isTouch)
        {
            case false when !_isFirstTouch:
                _isFirstTouch = true;
                break;
            case true when _isFirstTouch:
                _isFirstTouch = false;
                UpdateLastTouchPosition();
                break;
        }

        _velocity = new Vector2(HorizontalDeltaScreenNormalized * _horizontalVelocity, _verticalVelocity);
        
        if (isTouch && !_isFirstTouch) UpdateLastTouchPosition();

        UpdateHeadPosition();
        UpdateCamera();
        UpdateTail();
    }

    private void UpdateLastTouchPosition()
    {
        _lastTouchPosition = Input.mousePosition;
    }
    
    private void UpdateHeadPosition()
    {
        var newPos = transform.position + (Vector3) _velocity;
        newPos[0] = Mathf.Clamp(newPos[0], _bounds.x, _bounds.y);
        transform.position = newPos;
    }

    private void UpdateCamera()
    {
        var cameraPos = _camera.position;
        cameraPos[1] = transform.position.y;
        _camera.position = cameraPos;
    }

    private void UpdateTail()
    {
        for (int i = 0; i < _snakeBody.Count; i++)
        {
            var part = _snakeBody[i];
            var target = i == 0 ? transform : _snakeBody[i - 1];
        }
    }
}
