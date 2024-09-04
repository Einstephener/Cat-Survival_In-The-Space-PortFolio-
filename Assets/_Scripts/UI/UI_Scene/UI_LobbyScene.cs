using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class UI_LobbyScene : UI_Scene
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
        Main.Scene.LoadScene("MainScene");
    }
    public void OnBtnEndGame()
    {

    }
    public void OnBtnSetting()
    {

    }

}
