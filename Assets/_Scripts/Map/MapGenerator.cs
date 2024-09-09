using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject planePrefab;  // Plane 프리팹
    public int mapWidth = 10;  // 가로로 생성할 개수
    public int mapHeight = 10;  // 세로로 생성할 개수
    public float planeSize = 5f;  // Plane의 크기

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                Vector3 position = new Vector3(x * planeSize, 0, z * planeSize);
                Instantiate(planePrefab, position, Quaternion.identity);
            }
        }
    }
}
