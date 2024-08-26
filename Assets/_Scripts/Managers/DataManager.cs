using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

//Test 임시 - 나중에 Data 추가 되면 삭제 ㄱㄱ
public class TestPlayerStateData
{
    public Vector3 position;
    public int health;
    public int hunger;
    public int woodCutting;
    public float stamina;
}


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager
{
    public Dictionary<string, DateData> _dateData = new();
    //public Dictionary<string, TestPlayerStateData> _playerData = new();
    //public Dictionary<string, TestPlayerStateData> _inventoryData = new();
    //public Dictionary<string, TestPlayerStateData> _ResurcesData = new();

    public Dictionary<int, Date> StatDict { get; private set; } = new Dictionary<int, Date>();


    private string savePath;

    //public void Init()
    //{
    //    // data 추가 시 코드 추가 2
    //    StatDict = LoadJson<DateData, int, Date>("StatData").MakeDict();
    //}

    //Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    //{
    //    //TextAsset textAsset = Main.Resource.Load<TextAsset>($"Data/{path}");
    //    //return JsonUtility.FromJson<Loader>(textAsset.text);
    //}




    private void Awake()
    {
        savePath = Application.persistentDataPath + "/";
    }

    public void Initialize()
    {
        
    }
    // 세이브
    // 1. key값을 이용하여 파일를 찾기
    // 2. 없으면 key.json으로 생성
    // 3. 있으면 덥어 씌워!!
    public void SaveData<T>(string key, T data) where T : class
    {
        string json = JsonUtility.ToJson(data); //메모리 저장 
        File.WriteAllText(savePath + key + ".json", json);
    }

    private Dictionary<string, dynamic> dataDictionary;

    void Start()
    {
        dataDictionary = new Dictionary<string, dynamic>();
    }
    /// <summary>




}
