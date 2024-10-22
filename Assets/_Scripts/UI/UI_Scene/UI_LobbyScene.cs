using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class UI_LobbyScene : UI_MainScene
{
    enum Buttons
    {
        btnStartGame,
        btnSetting,
        btnEndGame,
    }

    private Button btnStartGame;
    private Button btnSetting;
    private Button btnEndGame;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        SetBtn();

        return true;
    }


    private void SetBtn()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);

            switch (child.gameObject.name)
            {
                case "StartGameBTN":
                    btnStartGame = child.GetComponent<Button>();
                    break;
                case "SettingBTN":
                    btnSetting = child.GetComponent<Button>();
                    break;
                case "EndGameBTN":
                    btnEndGame = child.GetComponent<Button>();
                    break;
            }
        }
    }

    public void OnBtnStartGame()
    {
        Main.Scene.LoadScene("CutScene");
    }
    public void OnBtnEndGame()
    {

#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때 에디터 종료.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 환경에서 실행 중일 때 애플리케이션 종료.
        Application.Quit();
#endif
    }

    public void OnBtnSetting()
    {

    }

}
