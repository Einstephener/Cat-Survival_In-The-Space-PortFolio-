using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    enum Buttons
    {
        btnStartGame,
        btnNewGame,
        btnLoadGame,
        btnSetting,
        btnEndGame,
    }

    public GameObject BtnNewGame;
    public GameObject BtnLoadGame;

    private Button btnStartGame;
    private Button btnNewGame;
    private Button btnLoadGame;
    private Button btnSetting;
    private Button btnEndGame;

    private bool _isclicked = false;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        _isclicked = false;
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
                case "NewGameBTN":
                    btnNewGame = child.GetComponent<Button>();
                    break;                
                case "LoadGameBTN":
                    btnLoadGame = child.GetComponent<Button>();
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

    public void OpenStartBtn()
    {
        _isclicked = !_isclicked;
        BtnNewGame.SetActive(_isclicked);
        // 저장 기능 비활성화
        //BtnLoadGame.SetActive(_isclicked);
    }
    public void OnBtnStartNewGame()
    {
        Main.Data.ResetData();
        Main.Data.LoadPlayerDataFromJson();
        Main.Scene.LoadScene("CutScene");
    }
    public void OnBtnStartLoadGame()
    {
        Main.Data.LoadPlayerDataFromJson();
        Main.Scene.LoadScene("MainScene");
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
