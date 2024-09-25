using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy Types/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public string name;

    public float maxHp;
    public float damage;

    public float sightRange;
    public float exitBuffer;
    public float attackRange;
}
