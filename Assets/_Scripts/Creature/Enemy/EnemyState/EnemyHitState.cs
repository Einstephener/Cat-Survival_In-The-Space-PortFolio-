using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("아야");
        enemy.animator.SetTrigger("OnHit");
        //enemy.OnHit(10f); // TODO : 플레이어 데미지.
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.IsDead())
        {
            enemy.TransitionToState(new EnemyDeadState());
            return;
        }

        enemy.TransitionToState(new EnemyAttackState());
    }

    public void ExitState(Enemy enemy)
    {
        
    }
}
