using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

//Test �ӽ� - ���߿� Data �߰� �Ǹ� ���� ����
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
    //    // data �߰� �� �ڵ� �߰� 2
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
    // ���̺�
    // 1. key���� �̿��Ͽ� ���ϸ� ã��
    // 2. ������ key.json���� ����
    // 3. ������ ���� ����!!
    public void SaveData<T>(string key, T data) where T : class
    {
        string json = JsonUtility.ToJson(data); //�޸� ���� 
        File.WriteAllText(savePath + key + ".json", json);
    }

    private Dictionary<string, dynamic> dataDictionary;

    void Start()
    {
        dataDictionary = new Dictionary<string, dynamic>();
    }
    /// <summary>




}
