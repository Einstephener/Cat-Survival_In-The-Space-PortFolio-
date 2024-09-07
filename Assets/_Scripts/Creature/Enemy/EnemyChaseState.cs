using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEnemyState
{
    public void EnterState(Enemy enemy);  // 상태로 전환될 때 호출되는 메서드
    public void UpdateState(Enemy enemy); // 매 프레임 상태 업데이트
    public void ExitState(Enemy enemy);   // 상태에서 나갈 때 호출되는 메서드
}

public class EnemyChaseState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 플레이어를 추적하기 시작합니다.");
        enemy.SetSpeed(4f); // 추적 중일 때 이동 속도를 증가시킴
    }

    public void UpdateState(Enemy enemy)
    {
        if(enemy._isAttack)
        {
            enemy.TransitionToState(new EnemyAttackState());
        }

        if (!enemy._isChasing)
        {
            enemy.TransitionToState(new EnemyProwlState());
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 추적을 멈춥니다.");
    }

}
