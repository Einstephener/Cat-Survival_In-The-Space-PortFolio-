using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInputControllerTest : MonoBehaviour
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
    private float _walkSpeed = 5.0f; // 수정
    private float _runSpeed = 8.0f; // 수정
    private float _sitSpeed = 0.5f;

    private LayerMask _groundCheckLayer;
    private float _jumpForce = 5.0f;
    private Transform _groundCheck;
    private Vector3 _boxCastSize = new Vector3(0.5f, 0.1f, 0.5f); // 박스 캐스트 크기
    private float _groundDistance = 0.2f; // 박스 캐스트 높이.

    private bool _isGrounded;
    private bool _isRun;
    private bool _isSit;

    private IPlayerAnimation _playerAnimation;

    [Header("#UI")]
    public GameObject inventoryUIDiplay;

    private PlayerInteraction _playerInteraction;

    #endregion

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _currentSpeed = _walkSpeed;
        _groundCheckLayer = LayerMask.GetMask("Ground");
        _groundCheck = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked; // 커서 가운데 고정.

        if(TryGetComponent<PlayerInteraction>(out PlayerInteraction playerInteraction))
        {
            _playerInteraction = playerInteraction;
        }
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
        _isGrounded = Physics.BoxCast(boxCastPosition, _boxCastSize / 2, Vector3.down,
            Quaternion.identity, _groundDistance, _groundCheckLayer);
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector3>();
        _moveInput.y = 0;  // y축은 0으로 고정
        /*if (_moveInput.sqrMagnitude > 0)
        {
            if (_isSit)
            {
                _playerAnimation.CrouchWalkAnimation(value.isPressed);
            }
            else
            {
                _playerAnimation.WalkAnimation(value.isPressed);
            }
        }*/
    }

    private void OnRun(InputValue value)
    {
        //if (value.isPressed)
        //{
        //    _isRun = true;
        //}
        //else
        //{
        //    _isRun = false;
        //}

        if (!_isSit)
        {
            //_playerAnimation.RunAnimation(value.isPressed);
        }
        _isRun = value.isPressed;

        OnRunStateChanged?.Invoke(value.isPressed); // 달리기(스테미나 사용) 이벤트 호출.     
    }

    private void OnSit(InputValue value)
    {
        //_cameraController.SitSightChange(value.isPressed);
        _isSit = value.isPressed;

        //_playerAnimation.CrouchIdleAnimation(value.isPressed);
    }

    private void ChangeSpeed()
    {
        if (!_isGrounded) return;

        if (_isSit)
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
    }

    private void OnJump()
    {
        if (_isGrounded)
        {
            _rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            //_playerAnimation.JumpAnimation();
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

        if(_playerInteraction.enemyObject != null /* && 적이 집에 가는 상태가 아니라면 */)
        {
            if(_playerInteraction.enemyObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                //enemy.DamagedByPlayer(10f);
                enemy.TransitionToState(new EnemyHitState());
            }
            else if (_playerInteraction.enemyObject.TryGetComponent<Catcher>(out Catcher catcher))
            {
                //catcher.DamagedByPlayer(10f);
                enemy.TransitionToState(new EnemyHitState());
            }
            else
            {
                Debug.Log("Non-Enemy");
            }
        }
    }

    private void OnInteract(InputValue value)
    {
        Debug.Log("OnInteract" + value.ToString());
    }

    #region UI
    private void OnQuickSlot(InputValue value)
    {
        var inventory = Main.Inventory.inventoryUI;
        int index = 0;
        if (value.isPressed)
        {
            // 눌린 키에 따라 슬롯 인덱스를 설정
            switch (true)
            {
                case var _ when Keyboard.current.digit1Key.isPressed:
                    index = 0;
                    inventory.SelectSlot(index);

                    break;
                case var _ when Keyboard.current.digit2Key.isPressed:
                    index = 1;
                    inventory.SelectSlot(index);

                    break;
                case var _ when Keyboard.current.digit3Key.isPressed:
                    index = 2;
                    inventory.SelectSlot(index);

                    break;
                case var _ when Keyboard.current.digit4Key.isPressed:
                    index = 3;
                    inventory.SelectSlot(index);

                    break;
                case var _ when Keyboard.current.digit5Key.isPressed:
                    index = 4;
                    inventory.SelectSlot(index);

                    break;
            }
        }
    }

    private void OnUI_Inventory()
    {
        //Debug.Log("OnUI_Inventory");
        if (!inventoryUIDiplay.activeInHierarchy)
        {
            inventoryUIDiplay.SetActive(true);
            Main.Inventory.inventoryUI.boneFireInventoryUI.boneFireObject.SetActive(false);
            Main.Inventory.inventoryUI.AdjustParentHeight();
        }
        else
        {
            inventoryUIDiplay.SetActive(false);
            Main.Inventory.inventoryUI.boneFireInventoryUI.boneFireObject.SetActive(false);
            Main.Inventory.inventoryUI.AdjustParentHeight();
        }
    }

    #endregion
}
