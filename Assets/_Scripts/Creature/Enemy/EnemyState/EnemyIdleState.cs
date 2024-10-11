using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public void EnterState(Enemy enemy);  // 상태로 전환될 때 호출되는 메서드
    public void UpdateState(Enemy enemy); // 매 프레임 상태 업데이트
    public void ExitState(Enemy enemy);   // 상태에서 나갈 때 호출되는 메서드
}

public class EnemyIdleState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 대기 상태입니다.");
        enemy.SetSpeed(0.1f);
        enemy.animator.SetFloat("Speed", 0.1f);
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.IsDead())
        {
            enemy.TransitionToState(new EnemyDeadState());
            return;
        }

        if (enemy.IsTarget())
        {
            enemy.TransitionToState(new EnemyChaseState());
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 대기 상태에서 벗어납니다.");
    }
}
