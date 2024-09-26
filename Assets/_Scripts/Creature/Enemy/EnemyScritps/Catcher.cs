using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    [SerializeField] public BossEnemyData bossSO;

    protected override void Awake()
    {
        base.Awake();
        Init(bossSO);
    }

    public void CastSkill()
    {
        Debug.Log($"{bossSO.skillDamage}의 데미지를 입히는 스킬을 시전합니다.");
    }
}
