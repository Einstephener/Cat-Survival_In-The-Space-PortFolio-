using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("Player Move")]
    [SerializeField] private Vector3 _moveInput;

    private Rigidbody _rigid;
    [SerializeField] private float _speed;
    private float _walkSpeed = 3.0f;
    private float _runSpeed = 10.0f;
    private float _jumpForce = 3.0f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 nextVec = _moveInput * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector3>();
    }

    private void OnRun(InputValue value)
    {
        if (value.isPressed)
            _speed = _runSpeed;
        else
            _speed = _walkSpeed;
    }

    private void OnJump()
    {
        _rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void OnLook(InputValue value)
    {
        //Debug.Log("OnLook" + value.ToString());
    }

    private void OnFire(InputValue value)
    {
        //Debug.Log("OnFire" + value.ToString());
    }
}
