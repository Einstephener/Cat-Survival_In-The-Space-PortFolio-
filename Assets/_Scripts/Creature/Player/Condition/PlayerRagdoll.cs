using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private GameObject Player;

    public void Initialize()
    {
        Player = transform.gameObject;
        SetChildRigidbodyState(true);
        SetChildColliderState(false);
        SetParentRigidbodyCollider(false);
    }

    public void SetChildRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
    }
    public void SetChildColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
    }

    public void SetParentRigidbodyCollider(bool state)
    {
        Player.GetComponent<Rigidbody>().isKinematic = state;
        Player.GetComponent<Collider>().enabled = !state;
    }

}
