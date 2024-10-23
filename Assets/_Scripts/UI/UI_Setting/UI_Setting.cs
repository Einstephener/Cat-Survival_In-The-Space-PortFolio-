using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Setting : UI_Base
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.SetCanvas(gameObject, OrderValue._settingOrder);
        return true;
    }

    #region Field
        
    //public GameObject CancelBtn;
    //public GameObject SaveBtn;
    //public GameObject EndGameBtn;

    #endregion  

    public void CancelSetting()
    {
        //저장 값 취소
        gameObject.SetActive(false);
    }
    public void SaveSetting()
    {
        //저장 값 취소
        gameObject.SetActive(false);
    }
    public void EndGame()
    {
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때 에디터 종료.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 환경에서 실행 중일 때 애플리케이션 종료.
        Application.Quit();
#endif
    }




}
