using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Main.UI.SetCanvas(gameObject, OrderValue._popUpOrder);
    }


    //public override bool Initialize()
    //{
    //    if (!base.Initialize()) return false;

    //    this.SetCanvas();
    //    _canvas = this.GetComponent<Canvas>();
    //    SetOrder();

    //    _panel = this.transform.Find("Panel");

    //    return true;
    //}

    public virtual void ClosePopupUI()  // 팝업이니까 고정 캔버스(Scene)과 다르게 닫는게 필요
    {
        Main.UI.ClosePopupUI(gameObject);
    }
}
