using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Enemy Types/BossEnemy")]
public class EnemyBossData : EnemyData
{
    public float skillRange;
    public float skillDamage;
    public float skillCooldown;
}
