using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    [SerializeField] public EnemyData enemySO;

    protected override void Awake()
    {
        base.Awake();
        Init(enemySO);
    }
}
