using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 
/// </summary>
public class Installation : Item
{
    public bool isInstallation = false;
    public Installation(ItemData data) : base(data)
    {

    }

    public override void Use()
    {

    }

    public virtual void PreView()
    {
        //프리뷰
    }
}
