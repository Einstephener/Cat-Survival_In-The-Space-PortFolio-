using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 플레이어를 추적하기 시작합니다.");
        enemy.SetSpeed(6f);
        enemy.aiPath.canMove = true;
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
        if (!enemy.IsTarget())
        {
            if (!enemy.IsHome())
            {
                Debug.Log("집갈래");
                enemy.TransitionToState(new EnemyWalkingState());
            }
        }
        else
        {
            if (enemy is Catcher catcher && catcher.IsCastingSkill() && catcher.IsSkillRange())
            {
                enemy.TransitionToState(new EnemyAttackState());
            }

                // 공격 범위 체크
            if (enemy.IsAttackRange())
            {
                enemy.TransitionToState(new EnemyAttackState());
            }

            PlayerCondition playerCondition = enemy.GetPlayerCondition();
            if (playerCondition != null && playerCondition.IsDead())
            {
                Debug.Log("플레이어가 죽어서 공격을 중단합니다.");
                enemy.TransitionToState(new EnemyWalkingState());
                return;
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 추적을 멈춥니다.");
    }

}
