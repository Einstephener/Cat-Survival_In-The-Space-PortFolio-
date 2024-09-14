using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 플레이어를 추적하기 시작합니다.");
        enemy.SetSpeed(4f);
    }

    public void UpdateState(Enemy enemy)
    {
        Debug.Log("Chase");
        //if (enemy.IsTargetAttackRange())
        //{
        //    enemy.TransitionToState(new EnemyAttackState());
        //}
        //else if (!enemy.IsTarget())
        //{
        //    enemy.TransitionToState(new EnemyIdleState());
        //}
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 추적을 멈춥니다.");
    }

}
