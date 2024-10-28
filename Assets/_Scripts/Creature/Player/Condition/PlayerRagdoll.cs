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

    #region Ragdoll On/Off

    // 메서드 호출 순서 중요.

    // Ragdoll 활성화 (애니메이터를 끄고 물리 적용)
    public void EnableRagdoll()
    {
        GetComponent<Animator>().enabled = false;
        SetChildRigidbodyState(false);
        SetChildColliderState(true);
        SetParentRigidbodyCollider(true);
    }

    // Ragdoll 비활성화 (애니메이터를 켜고 물리 끔)
    public void DisableRagdoll()
    {
        GetComponent<Animator>().enabled = true;
        SetChildRigidbodyState(true);
        SetChildColliderState(false);
        SetParentRigidbodyCollider(false);
    }
    #endregion
}
