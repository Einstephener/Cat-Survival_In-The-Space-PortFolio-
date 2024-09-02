using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    #region Field
    public delegate void RunStateChanged(bool isRunning);
    public event RunStateChanged OnRunStateChanged; // 달리기 이벤트.

    private Vector3 _moveInput;
    private Rigidbody _rigid;
    private float _currentSpeed;
    private float _walkSpeed = 3.0f;
    private float _runSpeed = 10.0f;

    private LayerMask _groundCheckLayer;
    private float _jumpForce = 5.0f;
    private Transform _groundCheck;
    private Vector3 _boxCastSize = new Vector3(0.5f, 0.1f, 0.5f); // 박스 캐스트 크기
    private float _groundDistance = 0.2f; // 박스 캐스트 높이.

    private bool isGrounded;
    #endregion

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _currentSpeed = _walkSpeed;
        _groundCheckLayer = LayerMask.GetMask("Ground");
        _groundCheck = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        // 이동.
        Vector3 nextVec = _moveInput * _currentSpeed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);

        // 점프.
        Vector3 boxCastPosition = _groundCheck.position + Vector3.up * 0.1f;
        isGrounded = Physics.BoxCast(boxCastPosition, _boxCastSize / 2, Vector3.down,
                                     Quaternion.identity, _groundDistance, _groundCheckLayer);
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector3>();
    }

    private void OnRun(InputValue value)
    {
        if (value.isPressed)
            _currentSpeed = _runSpeed;
        else
            _currentSpeed = _walkSpeed;

        OnRunStateChanged?.Invoke(value.isPressed); // 달리기 이벤트 호출.
    }

    private void OnJump()
    {
        if (isGrounded)
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
