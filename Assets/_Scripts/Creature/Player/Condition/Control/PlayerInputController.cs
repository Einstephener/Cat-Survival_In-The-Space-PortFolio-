using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    #region Field

    //[SerializeField] private CameraController _cameraController;
    [SerializeField] private Transform _cameraController; // Cat_Head 넣으면 됨 / 메인 카메라와 서브 카메라 같이 움직이도록 하기 위해 수정했습니다. - 문제가 발생하면 능권이에게 연락주시면 됩니다. :)
    private float _eulerAngleX;
    private float _eulerAngleY;
    private float _limitMinX = -70;
    private float _limitMaxX = 70;
    private float _rotateSpeed = 0.5f;

    private CapsuleCollider _collider;

    public delegate void RunStateChanged(bool isRunning);
    public event RunStateChanged OnRunStateChanged; // 달리기 이벤트.

    private Vector3 _moveInput;
    private Rigidbody _rigid;

    [HideInInspector] public ItemData currentItem;

    [Header("SpeedValue")]
    public float _currentSpeed;
    private float _walkSpeed = 3.0f;
    private float _runSpeed = 10.0f;
    private float _sitSpeed = 0.5f;
    private float _idleSpeed = 0.0f;

    private LayerMask _groundCheckLayer;
    private float _jumpForce = 5.0f;
    //private Transform _groundCheck;
    //private Vector3 _boxCastSize = new Vector3(0.8f, 0.1f, 0.8f); // 박스 캐스트 크기
    //private float _groundDistance = 0.2f; // 박스 캐스트 높이.

    [HideInInspector] public bool _isGrounded;
    private bool _isRun;
    private bool _isSit;
    private bool _isMoving;

    private Animator _playerAnimator;
    private float _animationWeightValue;

    [Header("AttackCoroutine")]
    private bool _canAttack = true; // 공격이 가능한지 여부
    [SerializeField] private float _attackCooldown = 1f; // 공격 쿨다운 시간 (1초)


    [Header("#UI")]
    [HideInInspector] public GameObject inventoryUIDiplay;
    [HideInInspector] public bool IsFist = true;

    private PlayerInteraction _playerInteraction;
    #endregion

    private void Awake()
    {
        _currentSpeed = _walkSpeed;
        _groundCheckLayer = LayerMask.GetMask("Ground");

        _rigid = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
        //_cameraController = transform.Find("Cat_Head");
        //Cursor.lockState = CursorLockMode.Locked; // 커서 가운데 고정.

        _collider = GetComponent<CapsuleCollider>();
        if (TryGetComponent<PlayerInteraction>(out PlayerInteraction playerInteraction))
        {
            _playerInteraction = playerInteraction;
        }
    }


    private void FixedUpdate()
    {
        // 이동.
        ChangeSpeed();
        Vector3 cameraForward = new Vector3(_cameraController.forward.x, 0, _cameraController.forward.z).normalized;
        Vector3 cameraRight = new Vector3(_cameraController.right.x, 0, _cameraController.right.z).normalized;


        Vector3 nextVec = (_moveInput.z * cameraForward + _moveInput.x * cameraRight).normalized * _currentSpeed * Time.fixedDeltaTime;
        nextVec.y = 0;
        _rigid.MovePosition(_rigid.position + nextVec);

        // 점프.
        CheckGround();

        // 마우스감도
        CheckMouseSpeed();

        // 애니메이션
        if (_playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.95f)
        {
            if (_animationWeightValue >= 0)
            {
                _animationWeightValue -= 2 * Time.deltaTime;
            }
            _playerAnimator.SetLayerWeight(1, _animationWeightValue);
        }

    }
    private void CheckMouseSpeed()
    {
        _rotateSpeed = Main.Data.Mouse;
    }

    private bool CheckGround()
    {
        //이전 boxCast로 바닥 체크 코드
        //Vector3 boxCastPosition = _groundCheck.position + Vector3.up * 0.1f;
        //_isGrounded = Physics.BoxCast(boxCastPosition, _boxCastSize / 2, Vector3.down,
        //    Quaternion.identity, _groundDistance, _groundCheckLayer);

        Ray[] raysList = new Ray[9]
        {
            new Ray(transform.position + (transform.up * 0.03f), -transform.up), //중앙
            new Ray(transform.position + (transform.forward * 0.25f) + (transform.up * 0.03f), -transform.up), // 앞
            new Ray(transform.position + (-transform.forward * 0.25f) + (transform.up * 0.03f), -transform.up), //뒤
            new Ray(transform.position + (-transform.right * 0.25f) + (transform.up * 0.03f), -transform.up), //좌
            new Ray(transform.position + (transform.right * 0.25f) + (transform.up * 0.03f), -transform.up), // 우

            new Ray(transform.position + (transform.forward * 0.15f) + (transform.right * 0.15f)+ (transform.up * 0.03f), -transform.up),
            new Ray(transform.position + (-transform.forward * 0.15f) + (transform.right * 0.15f)+ (transform.up * 0.03f), -transform.up),
            new Ray(transform.position + (transform.forward * 0.15f) + (-transform.right * 0.15f)+ (transform.up * 0.03f), -transform.up),
            new Ray(transform.position + (-transform.forward * 0.15f) + (-transform.right * 0.15f)+ (transform.up * 0.03f), -transform.up),
        };

        for (int i = 0; i < raysList.Length; i++)
        {
            if (Physics.Raycast(raysList[i], .1f, _groundCheckLayer))
            {
                _isGrounded = true;
                return _isGrounded;
            }
        }

        _isGrounded = false;
        return (_isGrounded);
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

        // 달리기(스테미나 사용) 이벤트 호출.
        OnRunStateChanged?.Invoke(value.isPressed);

        if (_isMoving)
        {
            _playerAnimator.SetBool("IsRun", _isRun);
        }
    }

    private void OnSit(InputValue value)
    {
        _isSit = value.isPressed;
        SitColliderChange(_isSit);
        _cameraController.GetComponent<CameraController>().SitSightChange(_isSit);
        _playerAnimator.SetBool("IsSit", _isSit);
    }

    private void OnJump()
    {
        if (_isGrounded)
        {
            SoundManager.Instance.PlaySFX("SFX_Jump", .5f);
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

        _cameraController.localRotation = Quaternion.Euler(_eulerAngleX, 0, 0);
        transform.rotation = Quaternion.Euler(0, _eulerAngleY, 0);
    }

    private void OnFire(InputValue value)
    {
        if (!_canAttack) return; // 공격이 불가능하면 함수 종료

        StartCoroutine(AttackCoroutine()); // 공격 실행 및 딜레이 시작
    }

    private IEnumerator AttackCoroutine()
    {
        _canAttack = false; // 공격 불가능 상태로 전환

        _animationWeightValue = 1f;

        // 공격 시 스테미나 감소
        GetComponent<PlayerCondition>().UpdateStamina(-10);


        // TODO: 현재 들고 있는 도구에 따라 다른 작용
        // 도구에 따른 공격 모션 변경
        IsFist = true;
        _playerAnimator.SetBool("IsPunch", IsFist);
        _playerAnimator.SetTrigger("IsAttack");

        ItemData curItem = Main.Inventory.InHand;

        // 손에 도구를 들고 있는 경우.
        if (curItem.Type == ItemType.Equipable)
        {
            WeaponItemData weaponItem = curItem as WeaponItemData;

            if (weaponItem != null)
            {
                // 자원 채취
                Debug.Log(weaponItem.WeaponDatas.type);
                if (_playerInteraction.natureObject != null)
                {
                    if (weaponItem.WeaponDatas.type == EquipableType.Ax)
                    {
                        Attack_Axe();
                    }
                    else if (weaponItem.WeaponDatas.type == EquipableType.Pick)
                    {
                        Attack_Pick();
                    }
                }

                // 적을 공격
                if (_playerInteraction.enemyObject != null)
                {
                    float attack = GetComponent<PlayerCondition>()._basicAttack;
                    if (weaponItem.WeaponDatas.type == EquipableType.Ax)
                    {
                        Attack_Enemy(attack);
                    }
                    else if (weaponItem.WeaponDatas.type == EquipableType.Pick)
                    {
                        Attack_Enemy(attack);
                    }
                    if (weaponItem.WeaponDatas.type == EquipableType.Weapon)
                    {
                        Attack_Enemy(2 * attack);
                    }
                }

                // 설치된 아이템 공격 - [11/29 - 능권이가 추가함]
                if(_playerInteraction.installtionItemObject != null)
                {
                    float attack = GetComponent<PlayerCondition>()._basicAttack;

                    if (weaponItem.WeaponDatas.type == EquipableType.Pick)
                    {
                        Attack_InstallationItem();
                    }

                }
            }
        }

        yield return new WaitForSecondsRealtime(_attackCooldown);

        _canAttack = true; // 공격 가능 상태로 전환
    }

    #region ClickAttackBTN

    private void Attack_Axe()
    {
        if (_playerInteraction.natureObject.TryGetComponent<CollectMatertial>(out CollectMatertial collectMatertial))
        {
            if (collectMatertial.NatureType == NatureResource.Wood)
            {
                collectMatertial.SpitMaterial();
            }
        }

    }
    private void Attack_Pick()
    {
        if (_playerInteraction.natureObject.TryGetComponent<CollectMatertial>(out CollectMatertial collectMatertial))
        {
            if (collectMatertial.NatureType == NatureResource.Stone)
            {
                collectMatertial.SpitMaterial();
            }
        }
    }
    private void Attack_Enemy(float attack)
    {
        if (_playerInteraction.enemyObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.OnHit(attack);
        }
    }

    private void Attack_InstallationItem()// [11/29 - 능권이가 추가함]
    {
        if (_playerInteraction.installtionItemObject.TryGetComponent<Installation>(out Installation installation))
        {
            installation.RemoveObject();
        }
    }

    #endregion

    private void OnInteract(InputValue value)
    {
        Debug.Log("OnInteract" + value.ToString());

        if (_playerInteraction.currentInteractObject != null) //Todo : (박기혁) 코드 개판인거... 수정해야함... 일단 땜빵 코드...
        {
            if (_playerInteraction.currentInteractObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {

            }
            else if (_playerInteraction.waterObject.TryGetComponent<Water>(out Water water))
            {
                GetComponent<PlayerCondition>().UpdateThirst(100);
            }
        }
        //Test 용도.
        //GetComponent<PlayerCondition>().UpdateHealth(-1000);
    }

    #endregion


    private void OnQuickSlot(InputValue value)
    {
        //TODO 현재 들고 있는 도구의 정보.
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
        //if (!inventoryUIDiplay.activeInHierarchy)
        //{
        //    inventoryUIDiplay.gameObject.SetActive(true);
        //    Main.Inventory.inventoryUI.boneFireObject.SetActive(false);
        //    Main.Inventory.inventoryUI.AdjustParentHeight();
        //}
        //else
        //{
        //    inventoryUIDiplay.gameObject.SetActive(false);
        //    Main.Inventory.inventoryUI.boneFireObject.SetActive(false);
        //    Main.Inventory.inventoryUI.AdjustParentHeight();
        //}

        //if(inventoryUIDiplay != null) 
        //{

        //    if (!inventoryUIDiplay.activeInHierarchy)
        //    {
        //        inventoryUIDiplay.SetActive(true);
        //        Main.Inventory.inventoryUI.boneFireObject.SetActive(false);
        //        Main.Inventory.inventoryUI.AdjustParentHeight();
        //    }
        //    else
        //    {
        //        inventoryUIDiplay.SetActive(false);
        //        Main.Inventory.inventoryUI.boneFireObject.SetActive(false);
        //        Main.Inventory.inventoryUI.AdjustParentHeight();
        //    }
        //}
        //else
        //{
        //    inventoryUIDiplay = Main.Inventory.inventoryUI.gameObject;
        //}
        Main.Inventory.inventoryUI.InventoryUISet();
        Main.UI.ShowPopupUI<InventoryUI>("Inventory");
    }

    private void OnUI_Map()
    {
        Main.UI.ShowPopupUI<UI_Map>("UI_Map");
    }

    private void OnUI_Tablet()
    {
        Main.UI.ShowPopupUI<UI_Tablet>("UI_CraftingTabletUI");
    }
    private void OnUI_Setting(InputValue value)
    {
        Main.UI.ShowPopupUI<UI_Setting>("UI_Setting");

    }

    private void OnShortcutKey(InputValue value)
    {
        bool isShortcutKey = false;
        isShortcutKey = value.isPressed;

        Main.Inventory.inventoryUI.ShortcutKey(isShortcutKey);
    }


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
            if (GetComponent<PlayerCondition>().updater._isStaminaLock)
            {
                _currentSpeed = _walkSpeed;
            }
            else
            {
                _currentSpeed = _runSpeed;
            }
        }
        else
        {
            _currentSpeed = _walkSpeed;
        }

        _playerAnimator.SetFloat("Speed", _currentSpeed);
    }

    private void SitColliderChange(bool isSit)
    {
        if (isSit)
        {
            _collider.height = 1.2f;
            _collider.center = new Vector3(0, 0.6f, 0);
        }
        else
        {
            _collider.height = 2f;
            _collider.center = new Vector3(0, 1f, 0);
        }
    }
}