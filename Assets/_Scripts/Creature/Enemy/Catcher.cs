using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        //_sightRange = 10f;
        //_exitBuffer = 3f;
        //_attackRange = 5f;
    }

    //protected override void Start()
    //{
    //    base.Start();
    //}

    //public override void UpdateState()
    //{
    //    base.UpdateState();
    //}


    #region Gizmos
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
    #endregion
}
