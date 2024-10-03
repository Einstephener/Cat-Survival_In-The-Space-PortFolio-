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

    public override bool IsDead()
    {
        if (_currentHp <= 0)
        {
            //Main.Inventory.AddItem(_enemyData.rewardItem, 5);
            Debug.Log($"{_enemyData.rewardItem}를 획득헀습니다.");
            return true;
        }
        else return false;
    }

    public void CastSkill()
    {
        //Debug.Log($"{bossSO.skillDamage}의 데미지를 입히는 스킬을 시전합니다.");
    }
}
