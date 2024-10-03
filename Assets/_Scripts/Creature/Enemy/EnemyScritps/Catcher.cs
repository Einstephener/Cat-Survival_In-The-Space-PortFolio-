using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    //[SerializeField] public BossEnemyData bossSO;
    [SerializeField] public EnemyData enemySO;

    protected override void Awake()
    {
        base.Awake();
        //Init(bossSO);
        Init(enemySO);
    }

    public override void GetReward()
    {
        base.GetReward();
        //Main.Inventory.AddItem(_enemyData.rewardItem, 5);
        Debug.Log($"{_enemyData.rewardItem} 획득했습니다.");
    }

    public void CastSkill()
    {
        //Debug.Log($"{bossSO.skillDamage}의 데미지를 입히는 스킬을 시전합니다.");
    }
}
