using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Enemy
{
    [SerializeField] public EnemyData enemySO;

    protected override void Awake()
    {
        base.Awake();
        Init(enemySO);
    }

    public override void GetReward()
    {
        base.GetReward();
        //Main.Inventory.AddItem(_enemyData.rewardItem, 5);
        Debug.Log($"{_enemyData.rewardItem} 획득했습니다.");
    }
}
