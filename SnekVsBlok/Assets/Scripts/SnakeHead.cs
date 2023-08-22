using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] private SnakePath _snakePath = new SnakePath();
    
    [SerializeField] private bool _isStart;
    [SerializeField] private bool _isFirstTouch;
    [SerializeField] private Vector2 _lastTouchPosition;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private Vector2 _bounds = new Vector2(-6, 6);
    [SerializeField] private float _cameraForwardOffset = 3f;
    [SerializeField] private List<Transform> _snakeBody;
    //[SerializeField] private float _distanceBetweenParts = 1f;
    [SerializeField] private float _horizontalVelocity = 5f;
    [SerializeField] private float _verticalVelocity = 1f;

    private Transform _camera;
    private float HorizontalTouchDelta => Input.GetMouseButton(0) ? _lastTouchPosition.x - Input.mousePosition.x : 0;

    private float HorizontalDeltaScreenNormalized => HorizontalTouchDelta / Screen.width;

    private void Start()
    {
        Application.targetFrameRate = 60;
        _camera = Camera.main.transform;
        _velocity = Vector2.zero;
        _isStart = false;
        UpdateCamera();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        var isTouch = Input.GetMouseButton(0);

        if (!_isStart && isTouch)
        {
            UpdateLastTouchPosition();
            _isStart = true;
        }
        
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
        //transform.position = newPos;
        _snakePath.SnakeLength = _snakeBody.Count;
        _snakePath.AddHeadPosition(newPos);
    }

    private void UpdateCamera()
    {
        var cameraPos = _camera.position;
        cameraPos[1] = transform.position.y + _cameraForwardOffset;
        _camera.position = cameraPos;
    }

    private void UpdateTail()
    {
        for (var i = 0; i < _snakeBody.Count; i++)
        {
            var part = _snakeBody[i];
            part.position = _snakePath[i];
            // var target = (i == 0) ? transform : _snakeBody[i - 1];
            // var targetPos = target.position;
            // var dir = targetPos - part.position;
            // var newDir = Vector3.ClampMagnitude(dir,_distanceBetweenParts);
            // part.position = targetPos - newDir;
        }
    }
}
