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
    /// Json을 Dictionary로 관리
    /// </summary>
    public Dictionary<string, DateData> Date = new();
    public Dictionary<string, RespawnData> Respawn = new();
    //public Dictionary<string, SlotData> Inventory = new();

    // 플레이어 리스폰 이벤트
    public delegate void PlayerRespawnEventHandler(bool isClickRespawn);
    public event PlayerRespawnEventHandler OnPlayerRespawn;

    public int Day = 1;

    #region Setting

    private float _mouse;
    private float _master;
    private float _bgm;
    private float _sfx;

    public float Mouse
    {
        get { return _mouse; }
        set
        {
            _mouse = value;
        }
    }

    public float Master
    {
        get { return _master; }
        set
        {
            _master = value;
        }
    }
    public float BGM
    {
        get { return _bgm; }
        set
        {
            _bgm = value;
        }
    }

    public float SFX
    {
        get { return _sfx; }
        set
        {
            _sfx = value;
        }
    }
    #endregion

    public void ResetSettingValue()
    {
        _mouse = 0.5f;
        _master = 0.5f;
        _bgm = 0.5f;
        _sfx = 0.5f;
    }


    public void Initialize()
    {
        LoadJson("dateData", ref Date);

        //// 초기화. 추후 1.0f대신 저장된 값으로 초기화.
        //_mouse = PlayerPrefs.GetFloat("Mouse", 1.0f);
        //_sound = PlayerPrefs.GetFloat("Sound", 1.0f);
        //_sfx = PlayerPrefs.GetFloat("SFX", 1.0f);
    }

    // 데이터를 로드 하기 - LoadJson<ClassName>() - Data를 상속 받은 Class만 가능
    private void LoadJson<T>(string fileName, ref Dictionary<string, T> dataDictionary) where T : Data
    {
        // JSON 파일 경로 설정
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // 파일이 존재하지 않는 경우
        if (!File.Exists(filePath))
        {
            Debug.Log($"File not found: {filePath}");
            return;
        }

        // 파일을 읽어 JSON 데이터를 리스트로 변환.
        var json = File.ReadAllText(filePath);
        var dataList = JsonConvert.DeserializeObject<List<T>>(json);

        // 리스트의 데이터를 Dictionary에 추가.
        foreach (var data in dataList)
        {
            dataDictionary[data.Key] = data; // Key를 기준으로 데이터를 저장
        }

    }

    // 데이터 세이브 - SaveJson(딕셔너리로 되어 있는 Data, "저장될 이름")
    public void SaveJson<T>(Dictionary<string, T> dataDictionary, string fileName) where T : Data
    {
        // Dictionary의 Value들을 JSON 포맷으로 변환
        string json = JsonConvert.SerializeObject(dataDictionary.Values, Formatting.Indented);

        // JSON 파일 경로 설정
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // JSON 데이터를 파일로 저장합니다.
        File.WriteAllText(filePath, json);
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
