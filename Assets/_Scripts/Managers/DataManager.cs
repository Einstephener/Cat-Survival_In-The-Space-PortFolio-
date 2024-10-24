using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Linq;

public class Data
{
    public string Key { get; set; }
}

public class DataManager
{
    /// <summary>
    /// 데이터 추가 되면 추가 하도록 하겠습니다.
    /// </summary>
    public Dictionary<string, DateData> Date = new();
    public Dictionary<string, RespawnData> Respawn = new();
    //public Dictionary<string, SlotData> Inventory = new();

    // 플레이어 리스폰 이벤트
    public delegate void PlayerRespawnEventHandler(bool isClickRespawn);
    public event PlayerRespawnEventHandler OnPlayerRespawn;


    public void Initialize()
    {
        Date = LoadJson<DateData>();
    }

    // 데이터를 로드 하기 - LoadJson<ClassName>() - Data를 상속 받은 Class만 가능
    private Dictionary<string, T> LoadJson<T>() where T : Data
    {
        var dataList = JsonConvert.DeserializeObject<List<T>>(Main.Resource.Get<TextAsset>($"{typeof(T).Name}").text);
        var dictionary = new Dictionary<string, T>();

        foreach (var data in dataList)
        {
            dictionary[data.Key] = data; // 키가 중복될 경우 덮어씁니다.
        }

        return dictionary;
    }

    // 데어터 세이브 - SaveJson(딕셔너리로 되어 있는 Data, "저장될 이름")
    public void SaveJson<T>(Dictionary<string, T> data, string fileName) where T : Data
    {
        string json = JsonConvert.SerializeObject(data.Values.ToList(), Formatting.Indented);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, $"{fileName}.json"), json);
    }

    public void PlayerRespawn()
    {
        OnPlayerRespawn?.Invoke(true);
    }

    //public void SaveData<T>(string key, T data) where T : class
    //{
    //    string json = JsonUtility.ToJson(data); //메모리 저장 
    //    File.WriteAllText(savePath + key + ".json", json);
    //}
}
