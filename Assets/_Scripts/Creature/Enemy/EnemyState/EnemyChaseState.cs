using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 플레이어를 추적하기 시작합니다.");
        enemy.Chasing();
        enemy.SetSpeed(4f); // 추적 중일 때 이동 속도를 증가.
    }

    public void UpdateState(Enemy enemy)
    {
        if(enemy._isAttack)
        {
            enemy.TransitionToState(new EnemyAttackState());
        }

        if (!enemy._isChasing && !enemy._isAttack)
        {
            enemy.TransitionToState(new EnemyIdleState());
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 추적을 멈춥니다.");
        enemy.Chasing();
    }

}
