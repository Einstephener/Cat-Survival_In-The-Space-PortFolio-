using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region 메모장!
    ///<summary>
    ///1. 슬롯 아이템 생성
    ///2. 슬롯 아이템 제거
    ///3. A슬롯 B슬롯 위치 교체
    ///3-1.아이템이 서로 같은 constansItem인 경우
    ///3-2 같은 아이템인지 확인 후 합치기
    ///
    ///4. 아이템 정렬
    ///5. 아이템 팝업창
    ///6. 슬롯 상태 및 UI 갱신
    ///
    /// 
    /// Q.아이템 슬롯은 어떻게 관리 할 것인가?
    /// Q.
    /// </summary>
    /// 
#endregion
    public Dictionary<ItemData, int> itemContainer = new();
    public int[] qickslots = new int[5];

    #region
    //아이템 있는지 여부
    public bool HadItem()
    {
        return false;
    }

    //셀 수 있는 아이템인지 여부
    public bool IsCountTableItem()
    {
        return false ;
    }
    #endregion

    public void AddItem(ItemData itemdata, int amount = 1)
    {

    }

    public void RemoveItem(ItemData itemdata, int amount = 1)
    {

    }

    public void SwapItem()
    {

    }

}
