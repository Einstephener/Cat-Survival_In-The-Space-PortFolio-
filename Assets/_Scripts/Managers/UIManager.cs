using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

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

    //private int _popUpOrder = 10; // 팝업에 사용할 오더 값
    //private int _settingOrder = 10; // 팝업에 사용할 오더 값
    //private GameObject _alreayOpenPopUpUI; // 이미 열려있는 PopUp UI

    #endregion
    public void SetCanvas(GameObject obj)
    {
        if (!obj.TryGetComponent<Canvas>(out Canvas canvas))
        {
            // 오브젝트에 Canvas 컴포넌트 유무 확인, 확인 후 없으면 추가해줌.
           canvas = obj.AddComponent<Canvas>();
        }
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;



        if (!obj.TryGetComponent<CanvasScaler>(out CanvasScaler scaler))
        {
            // 오브젝트에 CanvasScaler 컴포넌트 유무 확인, 확인 후 없으면 추가해줌.
            scaler = obj.AddComponent<CanvasScaler>();
        }
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new(1920, 1080);
    }

    #region Scene UI

    // UI_Scene 자식들만 가능
    //public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    //{
    //    if (string.IsNullOrEmpty(name))
    //        name = typeof(T).Name;

    //    GameObject go = Main.ResourceManager.Instantiate($"UI/Scene/{name}");
    //    T sceneUI = Util.GetOrAddComponent<T>(go);
    //    _sceneUI = sceneUI;

    //    go.transform.SetParent(Root.transform);

    //    return sceneUI;
    //}


    #endregion


    #region PopUp UI

    // TODO 이미 열려있는 팝업이 있는 경우, 팝업 요청 무시.
    // TODO PopUp 리스트로 관리하는 거 수정.

    // UI_Popup 자식들만 가능
    //public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    //{
    //    if (string.IsNullOrEmpty(name)) // 이름을 안받았다면 T로 ㄱㄱ
    //        name = typeof(T).Name;

    //    GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
    //    T popup = Util.GetOrAddComponent<T>(go);
    //    _popupStack.Push(popup);

    //    go.transform.SetParent(Root.transform);

    //    Time.timeScale = 0.0f;

    //    return popup;
    //}

    //public void ClosePopupUI()
    //{

    //    Time.timeScale = 1.0f;
    //    if (_popupStack.Count == 0)
    //        return;

    //    UI_Popup popup = _popupStack.Pop();
    //    Managers.Resource.Destroy(popup.gameObject);
    //    popup = null;
    //    _order--; // order 줄이기
    //}


    #region Setting PopUp UI

    // TODO 구상중...

    #endregion

    #endregion

}
