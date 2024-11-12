using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Craft : MonoBehaviour
{
    public GameObject itemPrefab; // 실제
    public GameObject Preview_ItemObject; // 프리펩
    public ItemData itemData;


    private bool isPreViewActivated = false;

    [SerializeField]
    private Transform playerTransform;

    [Header("#RayCast")]
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    public void Update()
    {
        if (isPreViewActivated)
        {
            if (itemData == Main.Inventory.inventoryUI.GetSelectItemData())
            {
                PreviewPositionUpdate();
            }
            else
            {
                Cancel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Cancel();
        }
    }

    public void OnUse() // OnInteract() -> OnUse();
    {
        ItemData selectedItemData = Main.Inventory.inventoryUI.GetSelectItemData();

        if (selectedItemData == null)
        {
            return;
        }

        Set();

        // itemData가 null이 아니고 InstallationItemData로 캐스팅 가능한 경우만 실행
        if (selectedItemData is InstallationItemData installationData)
        {
            GetPreview();
        }
    }

    public void OnFire(InputValue value)
    {
        Build();
    }

    private void Build()
    {
        if (isPreViewActivated && Preview_ItemObject.GetComponent<PreviewObject>().IsBuildeble())
        {
            Instantiate(itemPrefab, hitInfo.point, Quaternion.identity);
            Destroy(Preview_ItemObject);
            RemoveSet();

            //Main.Inventory.RemoveItem(Main.Inventory.inventoryUI.selectSlot.curSlot.itemData); // [10/23] 설치하면 인벤토리에서 아이템 제거 - 임시/Test
            //Main.Inventory.inventoryUI.UpdateUI();
            Main.Inventory.Select_RemoveItem(Main.Inventory.inventoryUI.selectSlot.index);
        }
    }

    private void Set()
    {
        Cancel();
        //if (itemData == null && itemData == itemData as InstallationItemData)
        //{
        //    itemData = Main.Inventory.inventoryUI.GetSelectItemData();
        //    InstallationItemData installationData = itemData as InstallationItemData;
        //    Preview_ItemObject = installationData.preViewObject;
        //    itemPrefab = installationData.InstallationItemPrefab;
        //}

        // 현재 선택된 아이템 데이터 가져오기
        if (Main.Inventory.inventoryUI.GetSelectItemData() != null)
        {
            ItemData selectedItemData = Main.Inventory.inventoryUI.GetSelectItemData();
            // itemData가 null이 아니고 InstallationItemData로 캐스팅 가능한 경우만 실행
            if (selectedItemData != null && selectedItemData is InstallationItemData installationData)
            {
                // 필드 변수를 업데이트
                itemData = installationData;

                // 프리뷰 오브젝트와 프리팹 설정
                Preview_ItemObject = installationData.preViewObject;
                itemPrefab = installationData.InstallationItemPrefab;
            }
            else
            {
                Debug.LogWarning("Selected item data is either null or not an InstallationItemData.");
            }
        }
    }

    private void RemoveSet()
    {
        isPreViewActivated = false;
        Preview_ItemObject = null;
        itemPrefab = null;
        itemData = null;
    }

    public void GetPreview()
    {
        //Set();

        Preview_ItemObject = Instantiate(Preview_ItemObject, playerTransform.position + playerTransform.forward, Quaternion.identity);
        isPreViewActivated = true;

    }

    public void Cancel()
    {
        if (isPreViewActivated)
        {
            Destroy(Preview_ItemObject);
        }

        RemoveSet();
        isPreViewActivated = false;
    }

    private void PreviewPositionUpdate()
    {
        Debug.DrawRay(playerTransform.position, playerTransform.forward * range, Color.blue);

        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                Preview_ItemObject.transform.position = _location;

            }
        }
    }
}