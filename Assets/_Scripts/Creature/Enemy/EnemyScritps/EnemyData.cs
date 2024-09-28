using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IAttackType
{
    Melee,
    Ranged,
    Both,
    None
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy Types/BasicEnemy")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public string name;

    public IAttackType attackType;
    public GameObject projectilePrefab;

    public float maxHp;
    public float damage;
    public float addSpeed;
    public float attackTime;
    public float attackCooldown;

    public float sightRange;
    public float exitBuffer;
    public float attackRange;
}

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Enemy Types/BossEnemy")]
public class BossEnemyData : EnemyData
{
    public float skillRange;
    public float skillDamage;
    public float skillCooldown;
}
