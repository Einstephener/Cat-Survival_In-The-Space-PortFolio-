using UnityEngine;

public class GoatSpawner : MonoBehaviour
{
    public GameObject[] goatPrefabs;
    public int numberOfGoats = 10;
    public float wanderRadius = 50f;
    public Terrain terrain;

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

            Vector3 randomPosition = GetRandomPointOnTerrain(wanderRadius);

            Instantiate(goatPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointOnTerrain(float radius)
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * radius;
        randomPos.y = terrain.SampleHeight(randomPos) + terrain.transform.position.y;
        return randomPos;
    }
}
