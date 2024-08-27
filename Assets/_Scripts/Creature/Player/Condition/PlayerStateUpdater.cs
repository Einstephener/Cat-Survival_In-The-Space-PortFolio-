using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateUpdater : MonoBehaviour
{
    #region Field
    private PlayerCondition _playerCondition;
    private PlayerState _state;

    [HideInInspector] public float hungerDecreaseRate = 0.1f;  // 배고픔 감소 속도
    [HideInInspector] public float thirstDecreaseRate = 0.1f; // 목마름 감소 속도
    [HideInInspector] public float healthDecreaseRate = 1f;    // 체력 감소 속도 
    [HideInInspector] public float staminaDecreaseRate = 1f;    // 체력 감소 속도 

    [HideInInspector] public bool isRun = false; //TODO 달리기 상태일때 is Run값 바꿔주기.
    #endregion

    public void Initialize(PlayerCondition playerCondition, PlayerState state)
    {
        this._playerCondition = playerCondition;
        this._state = state;
    }

    private void Update()
    {
        TimeToDecrease();
        HealthDecrease();
        StaminaUpdate(isRun);
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
        if (_state.Hunger <= 0)
        {
            _playerCondition.UpdateHealth(-healthDecreaseRate * Time.deltaTime);
        }
        // 목마름이 0이면 체력 감소.
        if (_state.Thirst <= 0)
        {
            _playerCondition.UpdateHealth(-healthDecreaseRate * 2f * Time.deltaTime);
        }
    }

    private void StaminaUpdate(bool isRun = false)
    {
        if (isRun)
        {
            // 스테미나 감소.
            _playerCondition.UpdateStamina(-staminaDecreaseRate * Time.deltaTime);
        }
        else
        {
            // 스테미나 회복.
            if (_state.Stamina != 0)
            {
                // 배고픔에 따라 다른 스테미나 회복량.
                if (_state.Hunger >= 50)
                    _playerCondition.UpdateStamina(staminaDecreaseRate * Time.deltaTime);
                else
                    _playerCondition.UpdateStamina(staminaDecreaseRate * 0.5f * Time.deltaTime);
            }
        }
    }

    //TODO
    /*
     플레이어 상태

        스테미너
            스테미너 0일 때는 달리기 불가능
                한번 0으로 떨어진 후
                스테미너가 20퍼센트 까지 차오르기 전에는 달리기 불가능
        체력
            체력 0일 시 사망.
     */

}
