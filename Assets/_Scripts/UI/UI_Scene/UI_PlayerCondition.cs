using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerCondition : UI_MainScene, IObserver
{

    [HideInInspector] public Image health;
    [HideInInspector] public Image hunger;
    [HideInInspector] public Image thirsty;
    [HideInInspector] public Image stamina;


    private void Start()
    {
        //PlayerController.instance.heartAnim = condition.transform.Find("Health/Image").GetComponent<Animator>();
        SetImage();
    }

    private void SetImage()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("ConditionUI");
        for (int i = 0; i < go.Length; i++)
        {
            switch (go[i].name)
            {
                case "HungerImage":
                    hunger = go[i].GetComponent<Image>();
                    break;
                case "ThirstyImage":
                    thirsty = go[i].GetComponent<Image>();
                    break;
                case "StaminaImage":
                    stamina = go[i].GetComponent<Image>();
                    break;
                case "HealthImage":
                    health = go[i].GetComponent<Image>();
                    break;
            }
        }
    }

    public void OnPlayerStateChanged(PlayerStatus state)
    {
        hunger.fillAmount = state.Hunger / 100f;
        thirsty.fillAmount = state.Thirst / 100f;
        stamina.fillAmount = state.Stamina / 100f;
        health.fillAmount = state.Health / 100f;
    }
}
