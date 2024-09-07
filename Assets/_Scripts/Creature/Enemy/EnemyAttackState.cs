using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 공격을 시작합니다.");
    }

    public void UpdateState(Enemy enemy)
    {
        // 적이 플레이어를 공격하는 로직
        enemy.Attack();

        if (!enemy._isAttack)
        {
            enemy.TransitionToState(new EnemyChaseState());
        }

    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 공격을 멈춥니다.");
    }

}
