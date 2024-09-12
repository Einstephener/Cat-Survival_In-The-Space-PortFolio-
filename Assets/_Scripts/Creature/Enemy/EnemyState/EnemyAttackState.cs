using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 공격을 시작합니다.");
        enemy.Attack();
        //공격 애니메이션 재생, 공격 사운드, 공격 딜레이 등등 설정. 
    }

    public void UpdateState(Enemy enemy)
    {
        if (!enemy._isAttack)
        {
            enemy.TransitionToState(new EnemyChaseState());
        }

    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 공격을 멈춥니다.");
        //enemy.Attack();
        //공격 애니메이션 중지, 공격 사운드 정지, 기타 등등 초기화. 
    }

}
