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

    private void Start()
    {
        ToolCraftingPanel.SetActive(false);
        FurnitureCraftingPanel.SetActive(false);
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
