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
        Date = LoadJson<DateData>();

        //// 초기화. 추후 1.0f대신 저장된 값으로 초기화.
        //_mouse = PlayerPrefs.GetFloat("Mouse", 1.0f);
        //_sound = PlayerPrefs.GetFloat("Sound", 1.0f);
        //_sfx = PlayerPrefs.GetFloat("SFX", 1.0f);
    }

    // 데이터를 로드 하기 - LoadJson<ClassName>() - Data를 상속 받은 Class만 가능
    private Dictionary<string, T> LoadJson<T>() where T : Data
    {
        // Resource 매니저에서 특정 Json 파일을 로드.
        // -> JsonConvert.DeserializeObject<List<T>>()를 사용해 JSON 데이터를 클래스의 리스트로 역직렬화.
        var dataList = JsonConvert.DeserializeObject<List<T>>(Main.Resource.Get<TextAsset>($"{typeof(T).Name}").text);

        // 리스트를 Dictionary<string, T> 로 변환하여 관리.
        var dictionary = new Dictionary<string, T>();

        foreach (var data in dataList)
        {
            dictionary[data.Key] = data; // 키가 중복될 경우 덮어씁니다.
        }

        return dictionary;
    }

    // 데이터 세이브 - SaveJson(딕셔너리로 되어 있는 Data, "저장될 이름")
    public void SaveJson<T>(Dictionary<string, T> data, string fileName) where T : Data
    {
        // 입력받은 Dictionary에서 값 추출.(data.Values.ToList())
        // JsonConvert.SerializeObject로 Json 문자열로 직렬화
        // Application.persistentDataPath이란 지정된 경로에 Json 파일 저장.
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
