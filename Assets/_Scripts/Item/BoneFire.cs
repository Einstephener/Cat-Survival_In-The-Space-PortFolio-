using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1. 플레이어 탐색
/// 2. Item상호작용
/// </summary>
public class BoneFire : Installation
{
    public Slot slot;

    public BoneFire(ItemData data) : base(data)
    {

    }

    public void Get()
    {
        //UI창 생성
        slot.gameObject.SetActive(true);
    }
}
