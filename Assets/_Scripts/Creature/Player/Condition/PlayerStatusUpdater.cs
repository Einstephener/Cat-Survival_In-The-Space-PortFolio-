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
    [HideInInspector] public bool _isStaminaLock = false;
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

    #region 스테미나 로직
    private void StaminaUpdate(bool isRunning = false)
    {
        if (isRunning && CanDecreaseStamina()) // 달리기 중이고 스태미나가 충분할 때
        {
            DecreaseStamina();
        }
        else
        {
            RecoverStamina(); // 달리지 않거나 스태미나가 부족한 경우 회복
        }

        CheckStaminaLock(); // 스태미나 상태를 점검하여 락을 업데이트
    }

    private void CheckStaminaLock()
    {
        if (_status.Stamina <= 0)
        {
            _isStaminaLock = true; // 스태미나 0 이하일 때 락 설정
        }
        else if (_status.Stamina >= 20)
        {
            _isStaminaLock = false; // 스태미나가 20 이상으로 회복되면 락 해제
        }
    }

    private bool CanDecreaseStamina()
    {
        return !_isStaminaLock && _status.Stamina > 0; // 스태미나가 0보다 크고, 락이 걸려있지 않을 때만 가능
    }

    private void DecreaseStamina()
    {
        _playerCondition.UpdateStamina(-staminaDecreaseRate * Time.deltaTime); // 스태미나 감소
    }

    private void RecoverStamina()
    {
        if (_status.Stamina < 100) // 스태미나가 최대치 미만일 때만 회복
        {
            float recoveryRate = _status.Hunger >= 50 ? staminaDecreaseRate : staminaDecreaseRate * 0.5f;
            _playerCondition.UpdateStamina(recoveryRate * Time.deltaTime);
        }
    }
    #endregion

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
