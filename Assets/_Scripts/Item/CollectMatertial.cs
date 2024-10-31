using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NatureResource
{
    Wood,
    Stone
}

public class CollectMatertial : MonoBehaviour
{
    private int curCount;
    public int maxCount;
    public GameObject MaterialPrefab;
    public NatureResource NatureType;

    private void OnEnable()
    {
        curCount = maxCount;
    }

    public void SpitMaterial()
    {
        if (curCount > 0)
        {
            // 특정 구역 안에서 임의의 방향 사용
            Vector3 randomDirection = Random.insideUnitSphere;
            // 위쪽으로만 이동하게끔 y값을 양수로 설정
            randomDirection.y = Mathf.Abs(randomDirection.y); 

            Vector3 spawnPosition = transform.position + randomDirection;

            GameObject material = Instantiate(MaterialPrefab, spawnPosition, Quaternion.identity);

            // Rigidbody가 있을 경우 해당 방향으로 힘을 가함
            if (material.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddForce(randomDirection.normalized, ForceMode.Impulse);
            }

            curCount--;

            if (curCount == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
