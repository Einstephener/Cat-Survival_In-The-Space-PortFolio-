using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1. 아이템이 있는지 확인 
/// 2. 
/// </summary>
public class BoneFire : Installation
{
    public InventorySlot slot;

    public BoneFire(ItemData data) : base(data)
    {

    }

    public void Get()
    {
        //UI창 생성
        slot.gameObject.SetActive(true);
    }
}
