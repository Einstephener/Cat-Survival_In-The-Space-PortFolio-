using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DayWeather : MonoBehaviour
{
    public TMP_Text Date;
    public TMP_Text Weather;

    private void Update()
    {
        Date.text = Main.Data.Day.ToString();
    }

}
