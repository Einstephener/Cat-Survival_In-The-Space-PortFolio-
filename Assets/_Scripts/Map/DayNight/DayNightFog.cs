using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightFog : MonoBehaviour
{
    private bool _isNight = false;
    private float _time;
    private float _nightFogDensity = 0.1f; // 밤 상태의 Fog 밀도
    private float _dayFogDensity = 0.003f; // 낮 상태의 Fog 밀도
    private float _fogDensityCalc = 0.01f; // 증감량 비율
    private float _currentFogDensity;

    private void Start()
    {
        _time = GetComponent<DayNightCycle>().CurrentTime;
        _currentFogDensity = _dayFogDensity;
        RenderSettings.fogDensity = _currentFogDensity;
    }

    private void Update()
    {
        // Time 값을 업데이트
        _time = GetComponent<DayNightCycle>().CurrentTime;

        if (_time <= 0.85f && _time >= 0.15f)
        {
            _isNight = false;
        }
        else 
        {
            _isNight = true;
        }

        if (_isNight)
        {
            if (_currentFogDensity < _nightFogDensity)
            {
                _currentFogDensity += _fogDensityCalc * Time.deltaTime;
                _currentFogDensity = Mathf.Clamp(_currentFogDensity, 0, _nightFogDensity); 
                RenderSettings.fogDensity = _currentFogDensity;
            }
        }
        else
        {
            if (_currentFogDensity > _dayFogDensity)
            {
                _currentFogDensity -= _fogDensityCalc * Time.deltaTime;
                _currentFogDensity = Mathf.Clamp(_currentFogDensity, _dayFogDensity, _nightFogDensity);
                RenderSettings.fogDensity = _currentFogDensity;
            }
        }
    }
}