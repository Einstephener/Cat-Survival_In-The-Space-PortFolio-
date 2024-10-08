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
    public string enemyName;

    public IAttackType attackType;
    public GameObject projectilePrefab;

    [Header("Reward")]
    public ItemData rewardItem;

    [Header("Stats")]
    public float maxHp;
    public float damage;
    public float addSpeed;

    public float sightRange;
    public float exitBuffer;
    public float attackRange;
}
