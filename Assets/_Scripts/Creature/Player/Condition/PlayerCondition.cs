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
    private float _maxValue = 100f;

    private void Start()
    {
        // TODO: 저장 시점에 어떤 값을 가지고 있는지, 게임 시작시 초기화.
        _state = new PlayerStatus(_maxValue, _maxValue, _maxValue, _maxValue);

        UI_PlayerCondition uI_PlayerCondition = FindObjectOfType<UI_PlayerCondition>();
        if (uI_PlayerCondition != null)
        {
            Attach(uI_PlayerCondition);
        }

        // PlayerStateUpdater를 추가하여 시간에 따른 상태 감소를 처리.
        PlayerStatusUpdater updater = gameObject.AddComponent<PlayerStatusUpdater>();
        updater.Initialize(this, _state);
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
        // 모든 옵저버들에게 공지.
        foreach (var observer in _observers)
        {
            observer.OnPlayerStateChanged(_state);
        }
    }

    #region 플레이어 State 수치 Update

    public void UpdateHunger(float amount)
    {
        _state.Hunger += amount;
        Notify();
    }

    public void UpdateHealth(float amount)
    {
        _state.Health += amount;
        Notify();
    }

    public void UpdateThirst(float amount)
    {
        _state.Thirst += amount;
        Notify();
    }

    public void UpdateStamina(float amount)
    {
        _state.Stamina += amount;
        Notify();
    }

    #endregion
}
