using UnityEngine;

public class GoatSpawner : MonoBehaviour
{
    public GameObject[] goatPrefabs;
    public int numberOfGoats = 10;
    public float wanderRadius = 50f;
    public Terrain terrain;
    public LayerMask noSpawnZoneLayer;

    void Start()
    {
        SpawnGoats();
    }

    void SpawnGoats()
    {
        for (int i = 0; i < numberOfGoats; i++)
        {
            int randomIndex = Random.Range(0, goatPrefabs.Length);
            GameObject goatPrefab = goatPrefabs[randomIndex];

            Vector3 randomPosition;
            int attempts = 0;

            // "noSpawnZoneLayer" 레이어와 충돌하지 않는 위치를 찾을 때까지 반복
            do
            {
                randomPosition = GetRandomPointOnTerrain(wanderRadius);
                attempts++;
                // 최대 시도 횟수를 정하여 무한 루프를 방지
                if (attempts > 100)
                {
                    Debug.LogWarning("Failed to find a suitable spawn position after 100 attempts.");
                    return;
                }
            } while (Physics.CheckSphere(randomPosition, 1f, noSpawnZoneLayer)); // 충돌 체크

            GameObject goatInstance = Instantiate(goatPrefab, randomPosition, Quaternion.identity);
            goatInstance.transform.parent = transform;
        }
    }

    Vector3 GetRandomPointOnTerrain(float radius)
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * radius;
        randomPos.y = terrain.SampleHeight(randomPos) + terrain.transform.position.y;
        return randomPos;
    }
}
