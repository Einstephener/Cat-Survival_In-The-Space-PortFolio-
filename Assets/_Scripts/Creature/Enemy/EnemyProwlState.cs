using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProwlState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("적이 배회를 시작합니다.");
        enemy.SetSpeed(2f); // 배회 중일 때 이동 속도 감소.
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy._isChasing)
        {
            enemy.TransitionToState(new EnemyChaseState());
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("적이 배회를 멈춥니다.");
    }

}
