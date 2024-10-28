using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    //TODO 시간 총괄하는 Manager 필요할 듯.

    [Range(0.0f, 1.0f)]
    public float CurrentTime;
    [SerializeField] private float _fullDayLength;
    [SerializeField] private float _startTime = 0.4f;
    [SerializeField] private Vector3 _noon;
    private float _timeRate;

    [Header("Sun")]
    public Light Sun;
    public Gradient SunColor;
    public AnimationCurve SunIntensity;

    [Header("Moon")]
    public Light Moon;
    public Gradient MoonColor;
    public AnimationCurve MoonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve LightingIntensityMultiplier;
    public AnimationCurve ReflectionIntensityMultiplier;

    [SerializeField] private Material skyboxMaterial;

    private void Start()
    {        
        _timeRate = 1.0f / _fullDayLength;
        CurrentTime = _startTime;
    }

    private void Update()
    {
        float lastTime = CurrentTime;
        CurrentTime = (CurrentTime + _timeRate * Time.deltaTime) % 1.0f;

        if (lastTime > CurrentTime)
        {
            Main.Data.Day += 1;
        }

        UpdateLighting(Sun, SunColor, SunIntensity);
        UpdateLighting(Moon, MoonColor, MoonIntensity);

        RenderSettings.ambientIntensity = LightingIntensityMultiplier.Evaluate(CurrentTime);
        RenderSettings.reflectionIntensity = ReflectionIntensityMultiplier.Evaluate(CurrentTime);


        UpdateSkyBlend();

    }

    private void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(CurrentTime);

        lightSource.transform.eulerAngles = (CurrentTime - (lightSource == Sun ? 0.25f : 0.75f)) * _noon * 4.0f;
        lightSource.color = colorGradiant.Evaluate(CurrentTime);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
    private void UpdateSkyBlend()
    {
        float dotProduct = Vector3.Dot(Sun.transform.forward, Vector3.up);
        float blend;

        // Sun이 비활성화되었을 때 Moon의 강도를 고려하여 블렌딩
        if (Sun.intensity > 0)
        {
            blend = Mathf.Lerp(0, 1, (dotProduct + 1) * 0.5f);
        }
        else
        {
            // Sun이 비활성화되었을 때 Moon의 강도에 따라 블렌딩
            float moonDotProduct = Vector3.Dot(Moon.transform.forward, Vector3.up);
            blend = Mathf.Lerp(0, 1, (moonDotProduct + 1) * 0.5f);
        }

        skyboxMaterial.SetFloat("_Blend", blend);
    }

}
