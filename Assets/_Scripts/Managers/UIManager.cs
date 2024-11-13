using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum OrderValue { _sceneOrder = 5, _popUpOrder = 10, _playerDeadOrder = 15, _settingOrder = 20 }
/*
_sceneOrder = 0; // 팝업에 사용할 오더 값
_popUpOrder = 10; // 팝업에 사용할 오더 값
_settingOrder = 20; // 설정창에 사용할 오더 값
*/

public class UIManager
{
    // 게임 씬 상에서 생길 여러가지 "UI 캔버스 프리팹" 들의 생성과 삭제를 관리하는 스크립트로 사용할 것

    /// <summary>
    /// "SortOrder" 사용관련 정리.
    /// 
    /// 팝업 UI 캔버스들은 렌더링 순서 또한 관리되어야 한다. "sort order" 사용
    /// Why? 항상 고정되어있는 UI와 팝업 UI의 렌더링 순서를 관리해야 하기 때문.
    /// 고정 UI -> UI_Scene
    /// sort order 값 0으로 고정(가장 밑에 표시되도록)
    /// 팝업 UI -> UI_PopUp
    /// sort order 값 10 (고정 UI위에 표시)
    /// 한번에 열 수 있는 팝업 UI는 항상 1개.-> 이미 열려있는 팝업이 있는경우, 팝업 요청 무시.
    /// ESC키를 누르면 나오는 환경 설정 창의 경우 20으로 (인벤을 열고서도 환경설정 창을 사용 가능하도록)
    /// </summary>



    // UI들 정리용 부모 오브젝트
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    #region Fields
    private GameObject _alreayOpenPopUpUI = null; // 이미 열려있는 PopUp UI
    private GameObject _alreayOpenSetting = null; // 이미 열려있는 Setting
    [HideInInspector] public GameObject GameSceneUI; // 열려있는  Scene UI
    [HideInInspector] public TextMeshProUGUI PromtText; // 상호작용 txt
    [HideInInspector] public QuickSlot[] QuickSlots;
    [HideInInspector] public Dictionary<string, GameObject> _uiPopUpDictionary = new Dictionary<string, GameObject>(); // 팝업 UI 관리


    [HideInInspector] public InputActionAsset inputActionAsset;
    private InputActionMap playerActionMap;    // Player용 ActionMap
    private InputActionMap uiActionMap;        // UI용 ActionMap

    #endregion

    public void SwitchToPlayer()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerActionMap == null || uiActionMap == null)
        {
            playerActionMap = inputActionAsset.FindActionMap("Player");
            uiActionMap = inputActionAsset.FindActionMap("UI");

            playerActionMap.Enable();
        }
        uiActionMap.Disable();
        playerActionMap.Enable();
    }

    public void SwitchToUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (playerActionMap == null || uiActionMap == null)
        {
            playerActionMap = inputActionAsset.FindActionMap("Player");
            uiActionMap = inputActionAsset.FindActionMap("UI");

            playerActionMap.Enable();
        }
        playerActionMap.Disable();
        uiActionMap.Enable();
    }


    public void SetCanvas(GameObject obj, OrderValue sort)
    {
        if (!obj.TryGetComponent(out Canvas canvas))
        {
            // 오브젝트에 Canvas 컴포넌트 유무 확인, 확인 후 없으면 추가해줌.
            canvas = obj.AddComponent<Canvas>();
        }
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        canvas.sortingOrder = (int)sort;

        if (!obj.TryGetComponent(out CanvasScaler scaler))
        {
            // 오브젝트에 CanvasScaler 컴포넌트 유무 확인, 확인 후 없으면 추가해줌.
            scaler = obj.AddComponent<CanvasScaler>();
        }
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new(1920, 1080);
    }

    #region Scene UI

    //UI_Scene 자식들만 가능
    public void ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Main.Resource.Instantiate(name); // 임시. 폴더 로드 필요.
        go.SetActive(true);
        if (name == "UI_MainScene")
        {
            QuickSlots = go.GetComponent<UI_MainScene>().UI_QuickSlots;
            GameSceneUI = go;

            PromtText = go.GetComponent<UI_MainScene>().interactionTXT;
        }

        go.transform.SetParent(Root.transform);

    }


    #endregion


    #region PopUp UI


    // UI_Popup 자식들만 가능
    public void ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        // 이미 열려 있는 팝업이 있는 경우 비활성화
        if (_alreayOpenPopUpUI != null)
        {
            ClosePopupUI(_alreayOpenPopUpUI);
        }
        else
        {
            // 팝업이 이미 생성되어 있는지 확인
            if (!_uiPopUpDictionary.TryGetValue(name, out GameObject popup))
            {
                // 팝업이 생성되지 않았으면 새로 생성
                popup = Main.Resource.Instantiate(name); // 임시. 리소스 매니저 필요
                popup.transform.SetParent(Root.transform);
                _uiPopUpDictionary[name] = popup; // 딕셔너리에 저장
            }

            // 팝업 활성화
            popup.SetActive(true);
            SwitchToUI();
            _alreayOpenPopUpUI = popup;

            //Time.timeScale = 0.0f; // 팝업이 열리면 시간 멈춤
        }
    }

    public void ClosePopupUI(GameObject obj)
    {
        if (_alreayOpenPopUpUI == obj)
        {
            SwitchToPlayer();
            obj.SetActive(false);
        }
        _alreayOpenPopUpUI = null;
    }




    #endregion

    #region Setting PopUp UI

    public void ShowSettingPopupUI<T>(string name = null) where T : UI_Setting
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        // 이미 열려 있는 팝업이 있는 경우 비활성화
        if (_alreayOpenSetting != null)
        {
            CloseSetting(_alreayOpenSetting);
        }
        else
        {
            // 팝업이 이미 생성되어 있는지 확인
            if (!_uiPopUpDictionary.TryGetValue(name, out GameObject popup))
            {
                // 팝업이 생성되지 않았으면 새로 생성
                popup = Main.Resource.Instantiate(name);
                popup.transform.SetParent(Root.transform);
                _uiPopUpDictionary[name] = popup;
            }

            popup.SetActive(true);
            SwitchToUI();
            _alreayOpenSetting = popup;

            // 팝업이 열리면 시간 멈춤
            Time.timeScale = 0.0f;
        }
    }

    public void CloseSetting(GameObject obj)
    {
        if (_alreayOpenSetting == null)
        {
            ClosePopupUI(_alreayOpenPopUpUI);
        }
        else if (_alreayOpenSetting == obj)
        {
            SwitchToPlayer();
            obj.SetActive(false);

            // 팝업이 열리면 시간 멈춤
            Time.timeScale = 1.0f;
            _alreayOpenSetting = null;
        }

    }

    #endregion

    #region Lobby에서 한번 초기화 시켜주는 코드

    public void ClearDictionary()
    {
        List<string> keysToRemove = new List<string>();

        // 딕셔너리 내 파괴된 객체들을 찾음
        foreach (var entry in _uiPopUpDictionary)
        {
            keysToRemove.Add(entry.Key);
        }

        foreach (var key in keysToRemove)
        {
            _uiPopUpDictionary.Remove(key);
        }
    }


    #endregion


}
