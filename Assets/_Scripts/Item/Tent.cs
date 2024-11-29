using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 시간확인 후 잘수 있는지 확인
/// 1. 위치 저장
/// DataManagerd에게 전달
/// </summary>
/// 
public class RespawnData : Data
{
    public Vector3 Position { get; set; }
}

public class Tent : Installation
{
    public Vector3 RespawnTransform;
    public Tent(ItemData data) : base(data)
    {
    }
    private void Start()    //임시/Test
    {
        //RespawnTransform = this.gameObject.transform;
        RespawnTransform = transform.position + Vector3.right * 3;
        //Debug.Log($"{RespawnTransform}");

        var dataManager = Main.Data; // Main의 싱글톤을 통해 DataManager에 접근
        if (dataManager.Respawn.TryGetValue("TentRespawn", out RespawnData respawnData))
        {
            Debug.Log($"Respawn Position: {respawnData.Position}");
        }
        else
        {
            Debug.Log("Respawn data not found.");
        }
    }

    public override void UIInteract()
    {
        base.UIInteract();
        Debug.Log($"Tent - SaveRespawnLocation(): running");

        Main.Data.savedData.lastPosition = RespawnTransform;
        Main.Data.SavePlayerDataToJson();
        //SaveRespawnLocation();
    }

    private void SaveRespawnLocation()  //Test
    {
        Debug.Log("Tent - SaveRespawnLocation()");
        // 리스폰 위치를 데이터로 변환
        var respawnData = new RespawnData
        {
            Key = "TentRespawn", // 고유 키 설정
            Position = RespawnTransform
        };

        // DataManager에 저장
        var dataManager = Main.Data; // Main의 싱글톤을 통해 DataManager에 접근

        if (dataManager.Respawn.ContainsKey(respawnData.Key))
        {
            dataManager.Respawn[respawnData.Key] = respawnData; // 기존 데이터 업데이트
        }
        else
        {
            dataManager.Respawn.Add(respawnData.Key, respawnData); // 새로운 데이터 추가
        }

        // 데이터 저장
        //dataManager.SaveJson(dataManager.Respawn, "RespawnData");
    }

}
