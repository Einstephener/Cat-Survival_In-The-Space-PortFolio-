using UnityEngine;
using System.Collections.Generic;

public class NaturePlacer : MonoBehaviour
{
    public Terrain terrain;
    public GameObject[] naturePrefabs;
    public int numberOfNatures = 0;
    public float minDistanceBetweenNatures = 5.0f;
    public LayerMask noNatureLayer;

    private List<Vector3> placedNaturePositions = new List<Vector3>();
    private GameObject natureParent; // Nature 빈 오브젝트

    void Start()
    {
        CreateNatureParent();
        PlaceNatures();
    }

    void CreateNatureParent()
    {
        // "Nature"라는 이름의 빈 오브젝트 생성
        natureParent = new GameObject("Nature");
        natureParent.transform.parent = this.transform;
    }

    void PlaceNatures()
    {
        TerrainData terrainData = terrain.terrainData;

        int naturePlaced = 0;
        while (naturePlaced < numberOfNatures)
        {
            float randomX = Random.Range(0f, terrainData.size.x);
            float randomZ = Random.Range(0f, terrainData.size.z);

            float worldX = terrain.transform.position.x + randomX;
            float worldZ = terrain.transform.position.z + randomZ;

            float worldY = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrain.transform.position.y;

            Vector3 newTreePosition = new Vector3(worldX, worldY, worldZ);

            // 충돌 감지: 특정 레이어에 위치한 오브젝트가 있는지 확인
            if (IsPositionValid(newTreePosition))
            {
                GameObject selectedTreePrefab = naturePrefabs[Random.Range(0, naturePrefabs.Length)];

                GameObject treeInstance = Instantiate(selectedTreePrefab, newTreePosition, Quaternion.identity);

                treeInstance.transform.parent = natureParent.transform;

                // 배치된 나무 위치 저장
                placedNaturePositions.Add(newTreePosition);

                naturePlaced++;
            }
        }
    }

    // 나무 간의 최소 거리 확인 및 충돌 검사
    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 placedPosition in placedNaturePositions)
        {
            float distance = Vector3.Distance(position, placedPosition);
            if (distance < minDistanceBetweenNatures)
            {
                return false; // 너무 가까운 위치에 나무가 있을 경우 false 반환
            }
        }

        // 특정 레이어에 위치한 오브젝트가 있는지 확인
        Collider[] colliders = Physics.OverlapSphere(position, 1f, noNatureLayer);
        if (colliders.Length > 0)
        {
            return false; // 해당 레이어의 오브젝트가 있을 경우 false 반환
        }

        return true; // 모든 조건을 만족하면 true 반환
    }
}
