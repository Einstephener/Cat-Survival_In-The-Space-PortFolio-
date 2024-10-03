using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusUpdater : MonoBehaviour
{
    #region Field
    private PlayerCondition _playerCondition;
    private PlayerStatus _status;

    private GameObject _ragdollObject;

    [HideInInspector] public float hungerDecreaseRate = 1f;  // 배고픔 감소 속도
    [HideInInspector] public float thirstDecreaseRate = 1f; // 목마름 감소 속도
    [HideInInspector] public float healthDecreaseRate = 5f;    // 체력 감소 속도 
    [HideInInspector] public float staminaDecreaseRate = 10f;    // 스테미나 감소 속도 

    [HideInInspector] public bool isRun = false; //TODO 달리기 상태일때 is Run값 바꿔주기.
    [HideInInspector] public bool canRun = false; //TODO 달리기 상태일때 is Run값 바꿔주기.
    private bool _isStaminaLock = false;
    #endregion
    private void Start()
    {
        GetInputController();
    }

    private void GetInputController()
    {
        // PlayerInputController의 OnRunStateChanged 이벤트 구독.
        if (TryGetComponent<PlayerInputController>(out PlayerInputController inputController))
        {
            inputController.OnRunStateChanged += HandleRunStateChanged;
            _ragdollObject = inputController.RagdollObject;
        }
        else // 지후님 Test 전용 코드
        {
            PlayerInputControllerTest inputControllerTest = GetComponent<PlayerInputControllerTest>();
            inputControllerTest.OnRunStateChanged += HandleRunStateChanged;
        }
    }

    private void HandleRunStateChanged(bool isRunning)
    {
        isRun = isRunning;  // 이벤트를 통해 isRun 업데이트
    }

    public void Initialize(PlayerCondition playerCondition, PlayerStatus state)
    {
        this._playerCondition = playerCondition;
        this._status = state;
    }

    private void Update()
    {
        TimeToDecrease();
        HealthDecrease();
        StaminaUpdate(isRun);
        IsPlayerDead();
    }

    private void TimeToDecrease()
    {
        // 시간에 따라 배고픔과 목마름이 감소.
        _playerCondition.UpdateHunger(-hungerDecreaseRate * Time.deltaTime);
        _playerCondition.UpdateThirst(-thirstDecreaseRate * Time.deltaTime);
    }

    private void HealthDecrease()
    {
        // 배고픔이 0이면 체력 감소.
        if (_status.Hunger <= 0)
        {
            _playerCondition.UpdateHealth(-healthDecreaseRate * Time.deltaTime);
        }
        // 목마름이 0이면 체력 감소.
        if (_status.Thirst <= 0)
        {
            _playerCondition.UpdateHealth(-healthDecreaseRate * 2f * Time.deltaTime);
        }
    }

    private void StaminaUpdate(bool isRun = false)
    {
        // 스태미나 회복 시도 (달리기 키를 누르고 있지 않을 때, 달리키를 누르더라도 달릴 수 없는 상황일 때.)
        if (!isRun || _isStaminaLock)
        {
            // 스태미나 회복.
            if (_status.Stamina <= 100)
            {
                // 배고픔에 따라 다른 스태미나 회복량.
                if (_status.Hunger >= 50)
                    _playerCondition.UpdateStamina(staminaDecreaseRate * Time.deltaTime);
                else
                    _playerCondition.UpdateStamina(staminaDecreaseRate * 0.5f * Time.deltaTime);
            }
        }
        else
        {
            // 달리기 중이고 스태미나가 0 이상일 때만 감소
            if (CheckStamina())
            {
                // 스태미나 감소.
                _playerCondition.UpdateStamina(-staminaDecreaseRate * Time.deltaTime);
            }
        }
    }

    private bool CheckStamina()
    {
        if (_status.Stamina <= 0) // 스태미나가 0 이하일 때
        {
            _isStaminaLock = true;
            return false;
        }

        if (_status.Stamina >= 20) // 스태미나가 20 이상일 때
        {
            _isStaminaLock = false;
            return true;
        }

        // 스태미나가 0과 20 사이일 때
        return !_isStaminaLock;
    }

    public bool IsPlayerDead()
    {
        // TODO 죽을 때 화면 등등..
        if (_status.Health <= 0)
        {
            if (TryGetComponent<PlayerRagdoll>(out PlayerRagdoll playerRagdoll))
            {
                GetComponent<Animator>().enabled = false;
                playerRagdoll.SetParentRigidbodyCollider(true);
                playerRagdoll.SetChildRigidbodyState(false);
                playerRagdoll.SetChildColliderState(true);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

}
