using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Respawn : UI_Scene, IObserver
{
    public Button LobbyBTN;
    public Button RespawnBTN;

    public GameObject BG_Panel;
    private Image _bgImage;
    private Color _colorSet;
    private float _fadeDuration; // 알파값이 줄어드는 데 걸리는 시간

    private bool _openPanel = false;




    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.SetCanvas(gameObject, OrderValue._playerDeadOrder);

        return true;
    }

    private void Awake()
    {
        _fadeDuration = 2f;
        BG_Panel.SetActive(false);
        _openPanel = false;
        _bgImage = BG_Panel.GetComponent<Image>();
    }
    public void OpenPanel()
    {
        BG_Panel.SetActive(true);
        _openPanel = true;
        Main.UI.SwitchToUI();
        _colorSet = _bgImage.color;

        StartCoroutine(DarkBackGround());
    }

    private IEnumerator DarkBackGround()
    {
        _colorSet.a = 0;
        _bgImage.color = _colorSet;

        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 0.8f, elapsedTime / _fadeDuration);

            if (newAlpha > 0.8f)
            {
                _colorSet.a = 0.8f;
                break;
            }

            _colorSet.a = newAlpha;
            _bgImage.color = _colorSet;
            yield return null;
        }
    }

    public void ClickRespawnBtn()
    {
        BG_Panel.SetActive(false);
        _openPanel = false;
        Main.UI.SwitchToPlayer();

        Main.Data.PlayerRespawn();
    }

    public void ClickLobbyBtn()
    {
        BG_Panel.SetActive(false);
        _openPanel = false;
        Main.UI.SwitchToPlayer();
        SceneManager.LoadScene("LobbyScene");
    }
    public void OnPlayerStateChanged(PlayerStatus state)
    {
        if(state.Health <= 0 && !_openPanel)
        {
            OpenPanel();
        }        
    }
}
