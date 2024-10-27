using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{

    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 공격을 시작합니다.");
        //공격 애니메이션 재생, 공격 사운드, 공격 딜레이 등등 설정. 
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.IsDead())
        {
            enemy.TransitionToState(new EnemyDeadState());
            return;
        }

        if (enemy is Catcher catcher && catcher.IsCastingSkill())
        {
            //if (!catcher.IsSkillRange())
            //{
            //    enemy.animator.SetBool("IsSkillAttack", false);
            //    enemy.TransitionToState(new EnemyChaseState());
            //}
            //else
            //{
            //    enemy.animator.SetBool("IsSkillAttack", true);
            //}
        }
        else
        {
            // 플레이어와의 거리 체크 (만약 플레이어가 멀어졌다면 다시 추적)
            if (!enemy.IsAttackRange())
            {
                enemy.animator.SetBool("IsAttack", false);
                enemy.TransitionToState(new EnemyChaseState());
            }
            else
            {
                if (enemy.AttackCooldownCheck())
                {
                    enemy.animator.SetTrigger("OnAttack");
                }
                else
                {
                    enemy.animator.SetBool("IsAttack", true);
                }
            }
        }

    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 공격을 멈춥니다.");
        //공격 애니메이션 중지, 공격 사운드 정지, 기타 등등 초기화. 
    }

}
