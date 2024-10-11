using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 프리뷰
/// </summary>
public class Installation : Item
{
    public bool isInstallation = false;
    public Installation(ItemData data) : base(data)
    {

    }

    public override void Use()
    {
        //Debug.Log("설치 아이템 상호작용");
    }

    public virtual void UIInterac()
    {
        //Debug.Log("설치 아이템 UI 상호작용");
    }

    public virtual void PreView() //프리뷰 생성
    {
        //프리뷰
        InstallationItemData installationData = itemData as InstallationItemData;
        if (installationData != null)
        {
            GameObject preview = installationData.preViewObject;
        }
        else
        {
            Debug.LogWarning("itemData가 InstallationData가 아닙니다.");
        }
    }
}
