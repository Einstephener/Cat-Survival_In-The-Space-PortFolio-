using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class UI_Damaged : UI_Scene, IObserver
{
    public Image RedBackGround;
    private float _fadeDuration = 1f; // 알파값이 줄어드는 데 걸리는 시간
    private float _tempHealth;

    private Color _initialColor;

    private void Awake()
    {
        _tempHealth = 100f;
        _initialColor = RedBackGround.color;
    }

    public void PlayerHit()
    {
        StartCoroutine(ColorChanged());
    }

    private IEnumerator ColorChanged()
    {
        _initialColor.a = 0.2f; // 알파값 0.2로 설정
        RedBackGround.color = _initialColor;

        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0.2f, 0f, elapsedTime / _fadeDuration); // 알파값 점진적으로 줄이기

            if (newAlpha < 0)
            {
                _initialColor.a = 0;
                break;
            }

            _initialColor.a = newAlpha;
            RedBackGround.color = _initialColor;
            yield return null; // 한 프레임 대기
        }
    }

    public void OnPlayerStateChanged(PlayerStatus state)
    {
        if(_tempHealth != state.Health && state.Health > 0)
        {
            PlayerHit();
            _tempHealth = state.Health;
        }
    }
}
