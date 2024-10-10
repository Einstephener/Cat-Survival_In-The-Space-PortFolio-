using UnityEngine;
using System.Collections.Generic;

public class TreePlacer : MonoBehaviour
{
    public Terrain terrain; // 사용할 Terrain
    public GameObject[] treePrefabs; // 나무 프리팹 배열
    public int numberOfTrees = 500; // 배치할 나무 수
    public float minDistanceBetweenTrees = 5.0f; // 나무들 간의 최소 거리

    private List<Vector3> placedTreePositions = new List<Vector3>(); // 배치된 나무들의 위치 목록

    void Start()
    {
        PlaceTrees();
    }

    void PlaceTrees()
    {
        TerrainData terrainData = terrain.terrainData;

        int treesPlaced = 0;
        while (treesPlaced < numberOfTrees)
        {
            // 나무가 배치될 랜덤 위치 생성
            float randomX = Random.Range(0f, terrainData.size.x); // Terrain의 X 좌표
            float randomZ = Random.Range(0f, terrainData.size.z); // Terrain의 Z 좌표

            // Terrain의 왼쪽 아래 꼭지점을 기준으로 절대 좌표 계산
            float worldX = terrain.transform.position.x + randomX;
            float worldZ = terrain.transform.position.z + randomZ;

            // 해당 위치의 지형 높이 가져오기
            float worldY = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrain.transform.position.y;

            // 나무 배치할 위치
            Vector3 newTreePosition = new Vector3(worldX, worldY, worldZ);

            // 기존 나무들과의 거리 체크
            if (IsPositionValid(newTreePosition))
            {
                // 나무 프리팹 중 하나를 랜덤 선택
                GameObject selectedTreePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

                // 나무 배치
                GameObject treeInstance = Instantiate(selectedTreePrefab, newTreePosition, Quaternion.identity);
                treeInstance.transform.parent = terrain.transform; // Terrain의 자식으로 설정

                // 배치된 나무 위치 저장
                placedTreePositions.Add(newTreePosition);

                treesPlaced++;
            }
        }
    }

    // 나무 간의 최소 거리 확인
    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 placedPosition in placedTreePositions)
        {
            float distance = Vector3.Distance(position, placedPosition);
            if (distance < minDistanceBetweenTrees)
            {
                return false; // 너무 가까운 위치에 나무가 있을 경우 false 반환
            }
        }
        return true; // 모든 나무가 일정 거리 이상 떨어져 있으면 true 반환
    }
}
