using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        enemy.animator.SetBool("IsDead", true);
    }

    public void UpdateState(Enemy enemy)
    {
        
    }

    public void ExitState(Enemy enemy)
    {
        
    }
}
