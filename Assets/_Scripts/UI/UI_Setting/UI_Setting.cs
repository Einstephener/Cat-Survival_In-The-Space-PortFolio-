using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.SetCanvas(gameObject, OrderValue._settingOrder);
        return true;
    }

    #region Field
    public AudioMixer audioMixer;


    public Slider MouseSlider;
    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    #endregion

    private void Awake()
    {
        Main.Data.Mouse = 0.5f;
        Main.Data.Master = 0.5f;
        Main.Data.BGM = 0.5f;
        Main.Data.SFX = 0.5f;

        MouseSlider.onValueChanged.AddListener(SetMouseSpeed);
        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private new void OnEnable()
    {
        MouseSlider.value = Main.Data.Mouse;

        MasterSlider.value = Main.Data.Master;
        BGMSlider.value = Main.Data.BGM;
        SFXSlider.value = Main.Data.SFX;
    }
    public void SetMouseSpeed(float value)
    {
        Main.Data.Mouse = value;        
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        Main.Data.Master = volume;
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        Main.Data.BGM = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        Main.Data.SFX = volume;
    }

    #region ButtonMethod

    public void CloseSetting()
    {
        // 저장 값 적용.
        Main.UI.ClosePopupUI(gameObject);
    }
    public void ResetSetting()
    {
        // 저장 값 초기화.
        Main.Data.ResetSettingValue();

        // 초기화된 값 동기화.
        MouseSlider.value = Main.Data.Mouse;
        MasterSlider.value = Main.Data.Master;
        BGMSlider.value = Main.Data.BGM;
        SFXSlider.value = Main.Data.SFX;
    }
    public void EndGame()
    {
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때 에디터 종료.

        // 자동 저장하기 필요.
        SceneManager.LoadScene("LobbyScene");
#else
        // 빌드된 환경에서 실행 중일 때 애플리케이션 종료.
        
        Application.Quit();
#endif
    }

    #endregion



}
