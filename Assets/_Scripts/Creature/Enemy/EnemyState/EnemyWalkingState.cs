using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkingState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        enemy.SetSpeed(2f);
        enemy.RegainHp();
        enemy.animator.SetFloat("Speed", enemy.aiPath.maxSpeed);
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.IsDead())
        {
            enemy.TransitionToState(new EnemyDeadState());
            return;
        }

        if (enemy.IsHome()) enemy.TransitionToState(new EnemyIdleState());

        //_animator.SetFloat("Speed", _aiPath.maxSpeed);
    }

    public void ExitState(Enemy enemy)
    {

    }
}
