using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkingSoundArea : MonoBehaviour
{
    public AudioClip WalkSound;
    public AudioClip RunSound;
    public AudioClip SitSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<WalkSound>().WalkClip = WalkSound;
            other.gameObject.GetComponent<WalkSound>().RunClip = RunSound;
            other.gameObject.GetComponent<WalkSound>().SitClip = SitSound;
        }
    }
}
