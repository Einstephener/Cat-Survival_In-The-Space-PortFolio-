using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MeleeHitbox : MonoBehaviour
{
    private float _damage;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    // 공격 시 이 메서드를 호출하여 히트박스를 활성화.
    public void Activate(float damage)
    {
        _damage = damage;
        if (_collider != null)
        {
            _collider.enabled = true;
        }
        else Debug.Log("콜라이더업덩");
    }

    // 공격이 끝난 후 히트박스를 비활성화.
    public void Deactivate()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO : 플레이어 Hp 깎기.
            Debug.Log($"근거리 공격으로 {_damage} 데미지를 입혔습니다.");

            if (other.gameObject.TryGetComponent<PlayerCondition>(out PlayerCondition playerCondition))
            {
                playerCondition.UpdateHealth(-_damage); // 체력 감소이므로 - 부호를 곱함.
            }
        }
    }
}
