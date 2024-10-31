using System.Collections;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    [HideInInspector] public AudioSource PlayerAudioSource;
    [HideInInspector] public AudioClip WalkClip;
    [HideInInspector] public AudioClip RunClip;
    [HideInInspector] public AudioClip SitClip;

    private float _speed;
    private float _tempSpeed;

    private void Start()
    {
        PlayerAudioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        PlayerWalk();
    }

    private void ChangeSound(AudioClip audioClip)
    {
        PlayerAudioSource.clip = audioClip;
    }

    private void PlayerWalk()
    {
        if (!GetComponent<PlayerInputController>()._isGrounded)
        {
            PlayerAudioSource.volume = 0;
            return;
        }
        else
        {
            PlayerAudioSource.volume = 0.4f;
            _speed = GetComponent<PlayerInputController>()._currentSpeed;
            if (_tempSpeed != _speed)
            {
                if (_speed != 0)
                {
                    if (_speed == 3.0f) // 걷기
                    {
                        ChangeSound(WalkClip);
                        PlayerAudioSource.Play();
                    }
                    else if (_speed == 10.0f) // 달리기
                    {
                        ChangeSound(RunClip);
                        PlayerAudioSource.Play();
                    }
                    else // 앉기
                    {
                        ChangeSound(SitClip);
                        PlayerAudioSource.Play();
                    }
                }
                else
                {
                    PlayerAudioSource.Stop();
                }
                _tempSpeed = _speed;
            }
        }
    }
}
