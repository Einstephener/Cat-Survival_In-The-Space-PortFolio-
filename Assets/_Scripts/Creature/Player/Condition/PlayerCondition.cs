using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#region Condition 옵저버
// 상태 변화를 관찰하는 옵저버가 구현해야 하는 메서드를 선언.
public interface IObserver
{
    void OnPlayerStateChanged(PlayerStatus state);
}

#endregion

#region Condition 주체(Subject)
// 옵저버를 관리하고 상태 변화를 알리는 역할.
public interface ISubject
{
    public void Attach(IObserver observer);
    public void Detach(IObserver observer);
    public void Notify();
}
#endregion


// "ISubject" 인터페이스를 구현하고, 상태 변화를 알리는 역할.
public class PlayerCondition : MonoBehaviour, ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private PlayerStatus _state;
    [HideInInspector] public PlayerStatusUpdater updater;
    private float _maxValue = 100f;
    [HideInInspector] public float _basicAttack = 10f;
    private UI_PlayerCondition uI_PlayerCondition;
    private UI_Damaged uI_Damaged;
    private bool isAttached = false;

    private void Start()
    {
        // TODO: 저장 시점에 어떤 값을 가지고 있는지, 게임 시작시 초기화.
        _state = new PlayerStatus(_maxValue, _maxValue, _maxValue, _maxValue);

        // PlayerStateUpdater를 추가하여 시간에 따른 상태 감소를 처리.
        updater = gameObject.AddComponent<PlayerStatusUpdater>();
        updater.Initialize(this, _state);

        // 캐릭터 사망시 사용할 플레이어 ragdoll
        PlayerRagdoll playerRagdoll = gameObject.AddComponent<PlayerRagdoll>();
        playerRagdoll.Initialize();
    }

    #region 옵저버 관리
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }
    #endregion

    public void Notify()
    {
        if(!isAttached) // 수정 필요
        {
            if (uI_PlayerCondition == null)
            {
                uI_PlayerCondition = FindObjectOfType<UI_PlayerCondition>();
                uI_Damaged = FindObjectOfType<UI_Damaged>();
            }
            else
            {
                Attach(uI_PlayerCondition);
                Attach(uI_Damaged);
                isAttached = true;
            }
        }
        else
        {
            // 모든 옵저버들에게 공지.
            foreach (var observer in _observers)
            {
                observer.OnPlayerStateChanged(_state);
            }
        }
    }

    #region 플레이어 State 수치 Update

    // 배고픔 변경.
    public void UpdateHunger(float amount)
    {
        _state.Hunger += amount;

        if (_state.Hunger > _maxValue)
        {
            _state.Hunger = _maxValue;
        }

        Notify();
    }

    // 체력 변경.
    public void UpdateHealth(float amount)
    {
        _state.Health += amount;

        if (_state.Health > _maxValue)
        {
            _state.Health = _maxValue;
        }

        Notify();
    }

    public bool IsDead()
    {
        return updater.IsPlayerDead();
    }

    // 목마름 변경.
    public void UpdateThirst(float amount)
    {
        _state.Thirst += amount;

        if (_state.Thirst > _maxValue)
        {
            _state.Thirst = _maxValue;
        }

        Notify();
    }

    // 스테미나 변경.
    public void UpdateStamina(float amount)
    {
        _state.Stamina += amount;

        if (_state.Stamina > _maxValue)
        {
            _state.Stamina = _maxValue;
        }

        Notify();
    }

    //// 배고픔 변경.
    //public void UpdateAttack(float amount)
    //{
    //    _state.Attack += amount;
                

    //    Notify();
    //}

    #endregion
}
