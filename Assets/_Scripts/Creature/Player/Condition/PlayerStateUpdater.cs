using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateUpdater : MonoBehaviour
{

    private PlayerCondition _playerCondition;
    private PlayerState _state;

    public float hungerDecreaseRate = 0.1f;  // 배고픔 감소 속도
    public float thirstDecreaseRate = 0.1f; // 목마름 감소 속도
    public float healthDecreaseRate = 1f;    // 체력 감소 속도 

    public void Initialize(PlayerCondition playerCondition, PlayerState state)
    {
        this._playerCondition = playerCondition;
        this._state = state;
    }

    private void Update()
    {
        TimeToDecrease();
        HealthDecrease();
    }

    private void TimeToDecrease()
    {
        // 시간에 따라 배고픔과 목마름이 감소
        _playerCondition.UpdateHunger(-hungerDecreaseRate * Time.deltaTime);
        _playerCondition.UpdateThirst(-thirstDecreaseRate * Time.deltaTime);
    }

    private void HealthDecrease()
    {
        // 배고픔이나 목마름이 0이면 체력 감소
        if (_state.Hunger <= 0 || _state.Thirst <= 0)
        {
            _playerCondition.UpdateHealth(-healthDecreaseRate * Time.deltaTime);
        }
    }

    //TODO
    /*
     플레이어 상태
        배고픔
            배고픔 0일때 체력 감소(기본값 * 1)
            50% 이상인 경우: 정상(기본값 * 1)
            50% 이하인 경우: 스테미너가 느리게 찬다(기본값 * .5)
        목마름
            목마름 0일때 체력 감소(기본값 * 2)
        스테미너
            스테미너 0일 때는 달리기 불가능
                한번 0으로 떨어진 후
                스테미너가 20퍼센트 까지 차오르기 전에는 달리기 불가능
        체력
            체력 0일 시 사망.
     */

}
