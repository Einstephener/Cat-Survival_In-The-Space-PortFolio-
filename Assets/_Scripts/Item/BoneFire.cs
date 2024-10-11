using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1. 아이템이 있는지 확인 
/// 2. 상호작용하면 bonefire의 SlotData를 Inventory의 있는 bonefireSlot_UI에 연결하는 작업이 필요함 그렇게 하기 위해 UI에서 연결하는 코드를 만들어 
/// </summary>
public class BoneFire : Installation
{
    public GameObject boneFireSlots;

    //test - BoneFire의 ItemData
    public SlotData boneFireSlotData;
    public SlotData nextBoneFireSlotData;

    //인벤토리와 연결할 Slot
    public InventorySlot boneFireSlot;
    public InventorySlot nextBoneFireSlot;

    public float cookingTime = 5f; // 요리 시간
    public float timer = 0;
    private Coroutine cookingCoroutine;

    public ItemData TestItem1;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize() // 에러 (수정해야 할 함수임 - InventorySlot - > SlotData 로 변경해서 해야 함) - 변경을 했지만 새로운 모닥불이 생기면 이전에 있는 데이터가 날라감
    {
        if (boneFireSlotData == null && nextBoneFireSlotData == null) // SlotData 생성
        {
            boneFireSlotData = new SlotData();
            nextBoneFireSlotData = new SlotData();
        }

        //잠시 주석
        boneFireSlot = Main.Inventory.inventoryUI.boneFireSlots[0];
        nextBoneFireSlot = Main.Inventory.inventoryUI.boneFireSlots[1];


        //Main.Inventory.inventoryUI.boneFireSlots[0] = boneFireSlot;
        //Main.Inventory.inventoryUI.boneFireSlots[1] = nextBoneFireSlot;

        Main.Inventory.inventoryUI.BoneFireSlotsGet(boneFireSlotData, nextBoneFireSlotData);
        //BoneFireUpdateUI();

        Main.Inventory.inventoryUI.BoneFireUpdateUI();

    }

    #region Test
    private void Test_Initialize()
    {
        AddBoneFireSlot(TestItem1, 3);
    }

    public void AddBoneFireSlot(ItemData _itemdata, int _amount = 1)
    {
        if (boneFireSlotData == null || boneFireSlotData.IsEmpty())
        {
            boneFireSlotData = new SlotData();
            boneFireSlotData.itemData = _itemdata;
            boneFireSlotData.amount = _amount;
            //Debug.Log($"{boneFireSlot.curSlot.itemData} += {_itemdata} // ItemName : {boneFireSlot.curSlot.itemData.DisplayName}, Amount : {boneFireSlot.curSlot.amount}");
            //BoneFireUpdateUI();
            Main.Inventory.inventoryUI.BoneFireSlotsGet(boneFireSlotData, nextBoneFireSlotData);
            Main.Inventory.inventoryUI.BoneFireUpdateUI();
            return;
        }
    }
    #endregion

    public BoneFire(ItemData data) : base(data)
    {

    }

    public override void Use()
    {
        base.Use();
        if (isCooking())
        {
            // boneFireSlot에 아이템이 있는지 확인
            if (boneFireSlotData != null && !boneFireSlotData.IsEmpty())
            {
                if (cookingCoroutine == null)
                {
                    cookingCoroutine = StartCoroutine(Cooking());
                }
            }
            else
            {
                //Debug.Log("요리를 할 수 없습니다: boneFireSlot에 아이템이 없습니다.");
                // 필요시 UI 업데이트 코드 추가
            }
        }
        else
        {
            // 요리를 할 수 없을 때 처리 (필요시 UI 업데이트)
            // Debug.Log("요리를 할 수 없습니다.");
        }
    }

    public override void UIInterac()
    {
        base .UIInterac();

        UISet();
        Main.Inventory.inventoryUI.AdjustParentHeight();
    }

    private void Update()
    {
        Use();
    }


    #region UI
    //private void UpdateSlotUI(InventorySlot slot)
    //{
    //    if (!slot.curSlot.IsEmpty()) //에러
    //    {
    //        Debug.Log("UpdateSlotUI");
    //        //따로 set 만들자
    //        slot.SetSlot(slot.curSlot);
    //    }
    //    else
    //    {
    //        Debug.Log("UpdateSlotUI");
    //        slot.ClearSlot();
    //    }
    //}

    //public void BoneFireUpdateUI()
    //{
    //    UpdateSlotUI(boneFireSlot);
    //    UpdateSlotUI(nextBoneFireSlot);
    //}

    public void UISet()
    {
        //UI창 생성
        //boneFireSlots.SetActive(true);
        Main.Inventory.inventoryUI.gameObject.SetActive(true);

        Main.Inventory.inventoryUI.boneFireObject.SetActive(true);

        
    }

    public void UIRemove()
    {
        //if (Input.GetKeyDown(KeyCode.I)) 
        //{
        //    if (Main.Inventory.inventoryUI.gameObject.activeSelf)
        //    {
        //        Main.Inventory.inventoryUI.boneFireObject.SetActive(false);
        //        Main.Inventory.inventoryUI.gameObject.SetActive(false);
        //    }
        //    //else
        //    //{
        //    //    Main.Inventory.inventoryUI.boneFireObject.SetActive(true);
        //    //}
        //}
    }

    #endregion


    #region cook
    IEnumerator Cooking()
    {
        Debug.Log("요리 시작");
        timer = 0f;

        while (true) // 무한 루프, 종료 조건은 내부에서 처리
        {
            // boneFireSlot 아이템 확인
            if (boneFireSlotData == null || boneFireSlotData.IsEmpty() || boneFireSlotData.amount <= 0)
            {
                //Debug.Log("요리를 종료합니다: boneFireSlot에 아이템이 없습니다.");
                cookingCoroutine = null; // 코루틴 참조 초기화
                yield break; // 코루틴 종료
            }

            timer += Time.deltaTime;

            // 현재 타이머 값을 디버그 로그로 출력
            //Debug.Log($"현재 요리 시간: {timer:F2}초"); // 소수점 둘째 자리까지 표시

            if (timer >= cookingTime)
            {
                CompleteCooking();

                // 요리 완료 후 아이템 상태 확인
                if (boneFireSlotData == null || boneFireSlotData.IsEmpty() || boneFireSlotData.amount <= 0)
                {
                    //Debug.Log("요리를 종료합니다: 요리 후 boneFireSlot에 아이템이 없습니다.");
                    cookingCoroutine = null; // 코루틴 참조 초기화
                    yield break; // 코루틴 종료
                }

                timer = 0f; // 타이머 초기화
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void CompleteCooking()
    {
        // nextBoneFireSlot의 ItemData가 있는지 확인
        if (nextBoneFireSlotData != null && nextBoneFireSlotData.itemData != null)
        {
            // 다음 슬롯의 갯수 증가
            nextBoneFireSlotData.amount++;
        }
        else
        {
            // 현재 슬롯의 아이템이 PotionItemData로 캐스팅
            PotionItemData boneFirePotionData = boneFireSlotData.itemData as PotionItemData;

            // 요리 결과 아이템 데이터 가져오기
            ItemData cookingData = boneFirePotionData.CookingItemData;

            // nextBoneFireSlot에 아이템 데이터 추가
            nextBoneFireSlotData.itemData = cookingData;
            nextBoneFireSlotData.amount = 1;
            Debug.Log("nextBoneFireSlot에 새로운 아이템이 추가되었습니다.");
        }

        // boneFireSlot의 아이템 수량 감소
        boneFireSlotData.amount--;

        //boneFireSlot의 수량이 0이 되면 curSlot을 null로 설정
        if (boneFireSlotData.IsEmpty())
        {
            //BoneFireUpdateUI();
            Main.Inventory.inventoryUI.BoneFireSlotsGet(boneFireSlotData, nextBoneFireSlotData);
            Main.Inventory.inventoryUI.BoneFireUpdateUI();
            Debug.Log("boneFireSlot의 아이템 수량이 0이 되어 슬롯이 비어졌습니다.");
        }

        //BoneFireUpdateUI(); // UI 업데이트
        Main.Inventory.inventoryUI.BoneFireSlotsGet(boneFireSlotData, nextBoneFireSlotData);
        Main.Inventory.inventoryUI.BoneFireUpdateUI();
    }


    public bool isCooking()
    {
        // 들어가는 ItemData의 PotionItemData의 CookingItemData가 필요
        // 나가는 ItemData 필요
        // 두 데이터가 같아야 함

        // 들어가는 ItemData의 PotionItemData의 CookingItemData가 필요
        if (boneFireSlotData != null) //에러
        {
            PotionItemData boneFirePotionData = boneFireSlotData.itemData as PotionItemData; // 들어가는 Item
            ItemData nextPotionData = nextBoneFireSlotData?.itemData; // 나가는 Item (null 가능성 처리)

            // 포션 데이터가 유효한지 확인
            if (boneFirePotionData != null)
            {
                ItemData cookingData = boneFirePotionData.CookingItemData;

                // nextBoneFireSlot가 null인 경우에도 true 반환
                if (nextPotionData == null || nextPotionData == cookingData)
                {
                    Debug.Log("두 아이템은 동일한 CookingItemData를 가지고 있습니다.");
                    return true; // 동일한 경우 true 반환
                }
                else
                {
                    Debug.Log("두 아이템은 다른 CookingItemData를 가지고 있습니다.");

                    // 코루틴 작동 방지
                    if (cookingCoroutine != null)
                    {
                        StopCoroutine(cookingCoroutine);
                        cookingCoroutine = null; // 코루틴 참조 초기화
                    }
                    return false; // 다른 경우 false 반환
                }
            }
            else
            {
                Debug.Log("하나 이상의 슬롯이 유효한 PotionItemData가 아닙니다.");
                return false; // 유효하지 않은 경우 false 반환
            }
        }
        else
        {
            Debug.Log("슬롯 중 하나가 비어 있습니다.");
            return false; // 슬롯이 비어 있는 경우 false 반환
        }
    }

    public void Fire()
    {

    }

    #endregion

    //public override void PreView()
    //{
    //    base.PreView();
    //}
}
