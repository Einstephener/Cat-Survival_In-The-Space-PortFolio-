using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
    #region Field
    [SerializeField]
    private GameObject UI_Tablet;
    private bool IsTabletON = false;
    [SerializeField]
    private GameObject UI_Setting;
    private bool IsSettingON = false;
    [SerializeField]
    private GameObject UI_Inventory;
    private bool IsInventoryON = false;
    [SerializeField]
    private GameObject UI_Map;
    private bool IsMapON = false;

    public InputActionAsset inputActionAsset;
    private InputActionMap playerActionMap;    // Player용 ActionMap
    private InputActionMap uiActionMap;        // UI용 ActionMap
    #endregion


    private void Start()
    {
        playerActionMap = inputActionAsset.FindActionMap("Player");
        uiActionMap = inputActionAsset.FindActionMap("UI");

        playerActionMap.Enable();
    }

    //TODO : 플레이어 inputaction map 변경 메서드.
    #region inputActionMap 변경
    public void SwitchToPlayer()
    {
        uiActionMap.Disable();
        playerActionMap.Enable();
    }

    public void SwitchToUI()
    {
        playerActionMap.Disable();
        uiActionMap.Enable();
    }

    #endregion

    #region UIControl
    private void OnUI_Tablet(InputValue value)
    {
        if (IsTabletON)
        {
            UI_Tablet.SetActive(false);
            IsTabletON = false;
            SwitchToPlayer();
        }
        else
        {
            UI_Tablet.SetActive(true);
            IsTabletON = true;
            SwitchToUI();
        }
    }

    private void OnUI_Setting(InputValue value)
    {
        if (IsSettingON)
        {
            UI_Setting.SetActive(false);
            IsSettingON = false;
            SwitchToPlayer();
        }
        else
        {
            UI_Setting.SetActive(true);
            IsSettingON = true;
            SwitchToUI();
        }
    }
    private void OnUI_Map(InputValue value)
    {
        if (IsMapON)
        {
            UI_Map.SetActive(false);
            IsMapON = false;
            SwitchToPlayer();
        }
        else
        {
            UI_Map.SetActive(true);
            IsMapON = true;
            SwitchToUI();
        }
    }
    private void OnUI_Inventory(InputValue value)
    {
        if (IsInventoryON)
        {
            UI_Inventory.SetActive(false);
            IsInventoryON = false;
            SwitchToPlayer();
        }
        else
        {
            UI_Inventory.SetActive(true);
            IsInventoryON = true;
            SwitchToUI();
        }
    }
    #endregion

}
