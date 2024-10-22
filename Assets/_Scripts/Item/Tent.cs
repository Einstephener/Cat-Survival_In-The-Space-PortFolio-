using System.Collections;
using System.Collections.Generic;
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
    public Transform RespawnTransform;
    public Tent(ItemData data) : base(data)
    {
    }

    public override void UIInterac() // UI
    {
        base.UIInterac();
        SaveRespawnLocation();
    }

    private void SaveRespawnLocation()
    {
        Debug.Log("Tent - SaveRespawnLocation()");
        // 리스폰 위치를 데이터로 변환
        var respawnData = new RespawnData
        {
            Key = "TentRespawn", // 고유 키 설정
            Position = RespawnTransform.position
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
        dataManager.SaveJson(dataManager.Respawn, "RespawnData");
    }

}
