using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    #region Field
    [SerializeField] private CameraController _cameraController;
    private float _eulerAngleX;
    private float _eulerAngleY;
    private float _limitMinX = -70;
    private float _limitMaxX = 70;
    private float _rotateSpeed = 0.5f;

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
    private bool isRun;
    #endregion

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _currentSpeed = _walkSpeed;
        _groundCheckLayer = LayerMask.GetMask("Ground");
        _groundCheck = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked; // 커서 가운데 고정.
    }

    private void FixedUpdate()
    {
        // 이동.
        ChangeSpeed();
        Vector3 cameraForward = _cameraController.transform.forward;
        Vector3 cameraRight = _cameraController.transform.right;

        Vector3 nextVec = (_moveInput.z * cameraForward + _moveInput.x * cameraRight).normalized * _currentSpeed * Time.fixedDeltaTime;
        nextVec.y = 0;
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
            isRun = true;
        else
            isRun = false;

        OnRunStateChanged?.Invoke(value.isPressed); // 달리기 이벤트 호출.
    }

    private void ChangeSpeed()
    {
        if (isGrounded)
        {
            if (isRun)
                _currentSpeed = _runSpeed;
            else
                _currentSpeed = _walkSpeed;
        }
    }

    private void OnJump()
    {
        if (isGrounded)
            _rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void OnLook(InputValue value)
    {
        Vector2 lookInput = value.Get<Vector2>();

        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        // 카메라 좌/우 회전.
        _eulerAngleY += mouseX * _rotateSpeed;
        // 카메라 위/아래 회전.
        _eulerAngleX -= mouseY * _rotateSpeed;

        // X축 회전 각도 제한
        _eulerAngleX = Mathf.Clamp(_eulerAngleX, _limitMinX, _limitMaxX);

        _cameraController.RotateTo(_eulerAngleX);
        transform.rotation = Quaternion.Euler(0, _eulerAngleY, 0);
    }

    private void OnFire(InputValue value)
    {
        Debug.Log("OnFire" + value.ToString());
    }

    private void OnInteract(InputValue value)
    {
        Debug.Log("OnInteract" + value.ToString());
    }
}
