using System.Collections;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    
    private float _speed;
    private float _soundDelay;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(AudioSoundPlay());
    }

    public void ChangeSound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
    }


    private void PlayerWalk()
    {
        _speed = GetComponent<PlayerInputController>()._currentSpeed;

        if (_speed != 0)
        {
            if (_speed == 3.0f)
            {
                _soundDelay = 0.2f;
            }
            else if (_speed == 10.0f)
            {
                _soundDelay = 0.05f;
            }
            else
            {
                _soundDelay = .5f;
            }
        }
        else
        {
            _soundDelay = 0f;
        }
    }

    private IEnumerator AudioSoundPlay()
    {
        while (true)
        {
            PlayerWalk();
            if (_speed != 0 && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            yield return new WaitForSecondsRealtime(_soundDelay);
        }
    }
}
