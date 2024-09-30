using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1. 아이템이 있는지 확인 
/// 2. 
/// </summary>
public class BoneFire : Installation
{
    public GameObject boneFireSlots;

    //public SlotData boneFireSlotData;
    //public SlotData nextBoneFireSlotData;

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

    private void Initialize()
    {

        boneFireSlot.curSlot = new SlotData();
        nextBoneFireSlot.curSlot = new SlotData();

        BoneFireUpdateUI();
        Test_Initialize();
    }

    private void Test_Initialize()
    {
        AddBoneFireSlot(TestItem1, 5);
    }

    public void AddBoneFireSlot(ItemData _itemdata, int _amount = 1)
    {
        if (boneFireSlot.curSlot == null || boneFireSlot.curSlot.IsEmpty())
        {
            boneFireSlot.curSlot = new SlotData();
            boneFireSlot.curSlot.itemData = _itemdata;
            boneFireSlot.curSlot.amount = _amount;
            Debug.Log($"{boneFireSlot.curSlot.itemData} += {_itemdata} // ItemName : {boneFireSlot.curSlot.itemData.DisplayName}, Amount : {boneFireSlot.curSlot.amount}");
            BoneFireUpdateUI();
            return;
        }
    }

    public BoneFire(ItemData data) : base(data)
    {

    }

    public override void Use()
    {
        base.Use();
        if (cookingCoroutine == null)
        {
            cookingCoroutine = StartCoroutine(Cooking());
        }
    }

    private void Update()
    {
        Use();
    }


    #region UI
    private void UpdateSlotUI(InventorySlot slot)
    {
        if (slot.curSlot.itemData != null)
        {
            Debug.Log("UpdateSlotUI");
            slot.SetSlot(slot.curSlot);
        }
        else
        {
            Debug.Log("UpdateSlotUI");
            slot.ClearSlot();
        }
    }

    public void BoneFireUpdateUI()
    {
        UpdateSlotUI(boneFireSlot);
        UpdateSlotUI(nextBoneFireSlot);
    }
    #endregion

    IEnumerator Cooking()
    {
        Debug.Log("요리 시작");
        timer = 0f;

        while (isCooking())
        {
            timer += Time.deltaTime;

            // 현재 타이머 값을 디버그 로그로 출력
            Debug.Log($"현재 요리 시간: {timer:F2}초"); // 소수점 둘째 자리까지 표시

            if (timer >= cookingTime)
            {
                // 요리가 완료되었을 때 처리
                CompleteCooking();
                yield break; // 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }
        cookingCoroutine = null; // 요리가 중단되면 코루틴 null로 설정
    }

    public void CompleteCooking()
    {
        // nextBoneFireSlot의 ItemData가 있는지 확인
        if (nextBoneFireSlot.curSlot != null && nextBoneFireSlot.curSlot.itemData != null)
        {
            // nextBoneFireSlot의 갯수 증가
            nextBoneFireSlot.curSlot.amount++; // IncrementCount는 슬롯의 갯수를 증가시키는 메서드라고 가정
            boneFireSlot.curSlot.amount--;
        }
        else
        {
            PotionItemData boneFirePotionData = boneFireSlot.curSlot.itemData as PotionItemData; // 들어가는 Item
            ItemData cookingData = boneFirePotionData.CookingItemData;
            //AddBoneFireSlot(cookingData);
            nextBoneFireSlot.curSlot.itemData = cookingData;
            nextBoneFireSlot.curSlot.amount = 1;
            Debug.Log("nextBoneFireSlot에 ItemData가 없습니다.");
        }
        BoneFireUpdateUI();
    }


    public bool isCooking()
    {
        // 들어가는 ItemData의 PotionItemData의 CookingItemData가 필요
        // 나가는 ItemData 필요
        // 두 데이터가 같아야 함

        if (boneFireSlot.curSlot != null)
        {
            // 슬롯의 itemData를 PotionItemData로 캐스팅
            PotionItemData boneFirePotionData = boneFireSlot.curSlot.itemData as PotionItemData; // 들어가는 Item
            ItemData nextPotionData = nextBoneFireSlot?.curSlot?.itemData; // 나가는 Item (null 가능성 처리)

            // 포션 데이터가 유효한지 확인
            if (boneFirePotionData != null)
            {
                ItemData cookingData = boneFirePotionData.CookingItemData;

                // nextBoneFireSlot가 null인 경우에도 true 반환
                if (nextPotionData == null || nextPotionData == cookingData)
                {
                    Debug.Log("두 아이템은 동일한 CookingItemData를 가지고 있습니다.");
                    // 동일할 경우 추가 로직 실행
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
    public void Get()
    {
        //UI창 생성
        boneFireSlots.SetActive(true);
    }

    public override void PreView()
    {
        base.PreView();
    }
}
