using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, PlayerInput.IPlayerActions
{
    [SerializeField] private float _speed = 15.0f;
    [SerializeField] private float _jumpHeight = 15.0f;
    [SerializeField] private float _gravity = 30f;
    
    private bool _isGrounded;
    private Vector3 _move;
    private CharacterController _controller;
    private PlayerInput.IPlayerActions _playerActionsImplementation;
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null) Debug.LogError("Character Controller::Player is NULL");
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        _isGrounded = _controller.isGrounded;
        
        if (_isGrounded)
            _move.x = 0f;
        if (_isGrounded && _move.y < 0)
            _move.y = 0f;

        _move.y -= _gravity * Time.deltaTime;
        _controller.Move(_move * Time.deltaTime);
        
        if (_move != Vector3.zero)
            gameObject.transform.forward = _move;
        
        _controller.transform.rotation = new Quaternion(0f, 0f, 0f, 0);

        Debug.Log(_move);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        _move = new Vector3(0, 0, direction.x) * _speed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (_isGrounded)
        {
            _move.y += _jumpHeight;
        }
    }
}
