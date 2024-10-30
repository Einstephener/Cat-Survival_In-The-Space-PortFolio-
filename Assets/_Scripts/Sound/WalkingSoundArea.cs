using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkingSoundArea : MonoBehaviour
{
    public AudioClip WalkSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<WalkSound>().ChangeSound(WalkSound);
        }
    }
}
