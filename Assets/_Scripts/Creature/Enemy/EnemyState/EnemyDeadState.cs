using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        if (!enemy.IsDead()) return;

        enemy.animator.SetBool("IsDead", true);
        enemy.GetReward();
    }

    public void UpdateState(Enemy enemy)
    {
        
    }

    public void ExitState(Enemy enemy)
    {
        
    }
}
