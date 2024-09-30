using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public enum OrderValue { _sceneOrder = 5, _popUpOrder = 10, _settingOrder = 20 }
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
    private GameObject _alreayOpenPopUpUI; // 이미 열려있는 PopUp UI

    #endregion
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
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = new GameObject();

        //GameObject go = Main.Resource.Instantiate($"Prefabs/{name}"); // 임시. 폴더 로드 필요.

        if (!go.TryGetComponent(out T sceneUI))
        {
            // 오브젝트에 Canvas 컴포넌트 유무 확인, 확인 후 없으면 추가해줌.
            sceneUI = go.AddComponent<T>();
        }

        //_sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }


    #endregion


    #region PopUp UI


    // UI_Popup 자식들만 가능
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) // 이름을 안받았다면 T로 ㄱㄱ
            name = typeof(T).Name;

        if (_alreayOpenPopUpUI = null)
        {
            GameObject go = new GameObject();

            if (!go.TryGetComponent(out T popup))
            {
                // 오브젝트에 Canvas 컴포넌트 유무 확인, 확인 후 없으면 추가해줌.
                popup = go.AddComponent<T>();
            }

            _alreayOpenPopUpUI = go;

            go.transform.SetParent(Root.transform);

            Time.timeScale = 0.0f;

            return popup;
        }
        else
        {
            // 이미 열려있는 팝업이 있는 경우, 팝업 요청 무시.
            return null;
        }
        //GameObject go = Main.Resource.Instantiate($"UI/Popup/{name}");
    }

    public void ClosePopupUI(GameObject obj)
    {
        Time.timeScale = 1.0f;

        if (_alreayOpenPopUpUI == obj)
        {
            obj.SetActive(false);
        }
        _alreayOpenPopUpUI = null;        
    }


    #region Setting PopUp UI

    // TODO 구상중...

    #endregion

    #endregion

}
