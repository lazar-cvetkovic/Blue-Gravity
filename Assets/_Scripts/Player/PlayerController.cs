using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _collisionOffset = 0.02f;
    [SerializeField] ContactFilter2D _movementFilter;

    List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

    Vector2 _movementInput;
    Rigidbody2D _rigidbody;
    Animator _animator;

    bool _canMove = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;

        if (_movementInput != Vector2.zero)
        {
            HandleMovement();
            AdjustMovementSprite();
        }
        else
            _animator.SetBool("isMoving", false);
    }

    private void HandleMovement()
    {
        bool success = TryMove(_movementInput);

        if (!success)
        {
            success = TryMove(new Vector2(_movementInput.x, 0));
        }

        if (!success)
        {
            success = TryMove(new Vector2(0, _movementInput.y));
        }

        _animator.SetBool("isMoving", success);
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return false;
        }

        float distance = _moveSpeed * Time.fixedDeltaTime + _collisionOffset;
        int collisionsCount = _rigidbody.Cast(direction, _movementFilter, _castCollisions, distance);

        if (collisionsCount == 0)
        {
            _rigidbody.MovePosition(_rigidbody.position + direction * _moveSpeed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }

    private void AdjustMovementSprite()
    {
        _animator.SetFloat("Horizontal", _movementInput.x);
        _animator.SetFloat("Vertical", _movementInput.y);
    }

    private void OnMove(InputValue movementValue) => _movementInput = movementValue.Get<Vector2>();

    public void LockMovement() => _canMove = false;

    public void UnlockMovement() => _canMove = true;
}
