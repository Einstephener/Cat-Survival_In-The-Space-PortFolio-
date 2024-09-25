using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tablet : MonoBehaviour
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



    private void Start()
    {
        ToolCraftingPanel.SetActive(false);
        FurnitureCraftingPanel.SetActive(false);
        foreach(var tool in ToolsWeapons)
        {
            GameObject _craftList = Instantiate(CraftList_Prefab, ToolsContentsParent.transform);
            _craftList.GetComponent<UI_CraftList>().InitSetting(tool);
        }
        foreach(var furniture in Furnitures)
        {
            GameObject _craftList = Instantiate(CraftList_Prefab, FurnitureContentsParent.transform);
            _craftList.GetComponent<UI_CraftList>().InitSetting(furniture);
        }
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
}
