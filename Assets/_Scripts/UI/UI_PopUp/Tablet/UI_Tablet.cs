using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tablet : UI_Popup
{
    public Button Option_ToolBTN;
    public Button Option_FurnitureBTN;
    public GameObject ToolCraftingPanel;
    public GameObject FurnitureCraftingPanel;

    public GameObject CraftList_Prefab;

    public GameObject FurnitureContentsParent;
    public GameObject ToolsContentsParent;

    public List<GameObject> ToolsWeapons;
    public List<GameObject> Furnitures;

    private List<UI_CraftList> childs = new List<UI_CraftList>(); // 초기화 추가;

    private void Start()
    {
        ToolCraftingPanel.SetActive(true);
        FurnitureCraftingPanel.SetActive(false);

        // 지정해놓은 제작 품목들 제작창에 동기화.
        foreach (GameObject tool in ToolsWeapons)
        {
            GameObject _craftList = Instantiate(CraftList_Prefab, ToolsContentsParent.transform);
            _craftList.TryGetComponent(out UI_CraftList ui_CraftList);
            ui_CraftList.InitSetting(tool, this);
            if (_craftList.TryGetComponent<UI_CraftList>(out UI_CraftList craftList))
            {
                childs.Add(craftList);
            }
        }
        foreach (GameObject furniture in Furnitures)
        {
            GameObject _craftList = Instantiate(CraftList_Prefab, FurnitureContentsParent.transform);
            _craftList.TryGetComponent(out UI_CraftList ui_CraftList);
            ui_CraftList.InitSetting(furniture, this);
            if (_craftList.TryGetComponent<UI_CraftList>(out UI_CraftList craftList))
            {
                childs.Add(craftList);
            }
        }
    }

    public override bool Initialize()
    {
        ReUpdateResource();
        if (!base.Initialize()) return false;
        return true;
    }

    public void ChooseToolCraftBTN()
    {
        ToolCraftingPanel.SetActive(true);
        FurnitureCraftingPanel.SetActive(false);
    }

    public void ChooseFurnitureCraftBTN()
    {
        ToolCraftingPanel.SetActive(false);
        FurnitureCraftingPanel.SetActive(true);
    }

    public void ReUpdateResource()
    {
        foreach (UI_CraftList uI_CraftList in childs)
        {
            uI_CraftList.UpdateResourceTexts();
        }
    }
}
