using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 플레이어를 추적하기 시작합니다.");
        enemy.aiPath.canMove = true;
        enemy.SetSpeed(6f);
        enemy.animator.SetFloat("Speed", enemy.aiPath.maxSpeed);
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.IsDead())
        {
            enemy.TransitionToState(new EnemyDeadState());
            return;
        }

        // A* Pathfinding에서의 타겟을 플레이어로 설정.
        if (enemy.IsTarget())
        {
            // 공격 범위 체크
            if(enemy.IsAttackRange())
            {
                enemy.TransitionToState(new EnemyAttackState());
            }
        }
        else
        {
            if (!enemy.IsHome())
            {
                Debug.Log("집갈래");
                enemy.TransitionToState(new EnemyWalkingState());
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 추적을 멈춥니다.");
    }

}
