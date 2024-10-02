using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("아야");
        enemy.animator.SetTrigger("OnHit");
        enemy.OnHit(10f); // TODO : 플레이어 데미지.
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.IsDead())
        {
            enemy.TransitionToState(new EnemyDeadState());
            return;
        }
        // TODO : 집에 가는 상태가 아니라면 맞으면 적도 때리기.
        enemy.TransitionToState(new EnemyAttackState());
    }

    public void ExitState(Enemy enemy)
    {
        
    }
}
