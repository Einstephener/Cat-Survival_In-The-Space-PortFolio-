using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
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
    [SerializeField] private float _currentSpeed;
    private float _walkSpeed = 3.0f;
    private float _runSpeed = 10.0f;
    private float _sitSpeed = 0.5f;
    private float _idleSpeed = 0.0f;

    private LayerMask _groundCheckLayer;
    private float _jumpForce = 5.0f;
    private Transform _groundCheck;
    private Vector3 _boxCastSize = new Vector3(0.8f, 0.1f, 0.8f); // 박스 캐스트 크기
    private float _groundDistance = 0.2f; // 박스 캐스트 높이.

    private bool _isGrounded;
    private bool _isRun;
    private bool _isSit;
    private bool _isMoving;

    private Animator _playerAnimator;
    private float _animationWeightValue;
    #endregion

    private void Awake()
    {
        _currentSpeed = _walkSpeed;
        _groundCheckLayer = LayerMask.GetMask("Ground");

        _rigid = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<Transform>();
        _playerAnimator = GetComponent<Animator>();
        //Cursor.lockState = CursorLockMode.Locked; // 커서 가운데 고정.
    }


    private void FixedUpdate()
    {
        // 이동.
        ChangeSpeed();
        Vector3 cameraForward = new Vector3(_cameraController.transform.forward.x, 0, _cameraController.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(_cameraController.transform.right.x, 0, _cameraController.transform.right.z).normalized;


        Vector3 nextVec = (_moveInput.z * cameraForward + _moveInput.x * cameraRight).normalized * _currentSpeed * Time.fixedDeltaTime;
        nextVec.y = 0;
        _rigid.MovePosition(_rigid.position + nextVec);

        // 점프.
        Vector3 boxCastPosition = _groundCheck.position + Vector3.up * 0.1f;
        _isGrounded = Physics.BoxCast(boxCastPosition, _boxCastSize / 2, Vector3.down,
            Quaternion.identity, _groundDistance, _groundCheckLayer);

        // 애니메이션
        if (_playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f)
        {
            if (_animationWeightValue >= 0)
            {
                _animationWeightValue -= Time.deltaTime;
            }
            _playerAnimator.SetLayerWeight(1, _animationWeightValue);
        }
        
    }


    #region PlayerControl
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector3>();
        _moveInput.y = 0;  // y축은 0으로 고정

        _isMoving = _moveInput != Vector3.zero;
    }

    private void OnRun(InputValue value)
    {
        _isRun = value.isPressed;

        OnRunStateChanged?.Invoke(value.isPressed); // 달리기(스테미나 사용) 이벤트 호출.        

        if (_isMoving)
        {
            _playerAnimator.SetBool("IsRun", _isRun);
        }
    }

    private void OnSit(InputValue value)
    {
        _isSit = value.isPressed;

        _playerAnimator.SetBool("IsSit", _isSit);

    }

    private void OnJump()
    {
        if (_isGrounded)
        {
            _rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _playerAnimator.SetTrigger("OnJump");
            //TODO: 점프 착지시 동작 연결[Animator탭에서 수정할듯.]
        }
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
        _animationWeightValue = 1f;
        _playerAnimator.SetTrigger("IsAttack");
    }

    private void OnInteract(InputValue value)
    {
        Debug.Log("OnInteract" + value.ToString());
    }

    private void OnQuickSlot(InputValue value)
    {
        // TODO : 퀵슬롯 부분 추가
    }
    #endregion


    private void ChangeSpeed()
    {
        if (!_isGrounded) return;

        if (!_isMoving)
        {
            _currentSpeed = _idleSpeed;
        }
        else if (_isSit)
        {
            _currentSpeed = _sitSpeed;
        }
        else if (_isRun)
        {
            _currentSpeed = _runSpeed;
        }
        else
        {
            _currentSpeed = _walkSpeed;
        }

        _playerAnimator.SetFloat("Speed", _currentSpeed);
    }
}