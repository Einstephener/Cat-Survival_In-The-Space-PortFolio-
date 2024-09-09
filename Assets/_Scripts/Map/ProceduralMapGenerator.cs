using System.Collections.Generic;
using UnityEngine;

public class ProceduralMapGenerator : MonoBehaviour
{
    public GameObject planePrefab;
    public int chunkSize;  // 한 Chunk에 들어갈 Plane의 개수
    public float planeSize;  // Plane의 크기
    public int renderDistance;  // 플레이어 주변에서 렌더링할 Chunk의 거리 (Chunk 단위)

    private Transform player;
    private Dictionary<Vector2, GameObject> chunks = new Dictionary<Vector2, GameObject>();  // 생성된 Chunk들을 관리할 딕셔너리

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateInitialChunks();  // 초기 Chunk들을 생성
    }

    void Update()
    {
        UpdateChunks();  // 플레이어가 이동할 때마다 Chunk 업데이트
    }

    // 처음에 플레이어 주변의 Chunk를 생성
    void GenerateInitialChunks()
    {
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                CreateChunkAt(x, z);
            }
        }
    }

    // 플레이어의 위치를 기준으로 시야 범위 내에 있는 Chunk를 생성하거나 제거
    void UpdateChunks()
    {
        List<Vector2> chunksToRemove = new List<Vector2>();  // 시야 범위 밖에 있는 Chunk를 제거하기 위한 리스트

        int playerX = Mathf.FloorToInt(player.position.x / (chunkSize * planeSize));  // 플레이어의 X좌표를 기준으로 Chunk 좌표 계산
        int playerZ = Mathf.FloorToInt(player.position.z / (chunkSize * planeSize));  // 플레이어의 Z좌표를 기준으로 Chunk 좌표 계산

        // 시야 범위 내의 Chunk가 존재하지 않으면 생성
        for (int x = playerX - renderDistance; x <= playerX + renderDistance; x++)
        {
            for (int z = playerZ - renderDistance; z <= playerZ + renderDistance; z++)
            {
                Vector2 chunkPos = new Vector2(x, z);
                if (!chunks.ContainsKey(chunkPos))
                {
                    CreateChunkAt(x, z);  // 새로운 Chunk 생성
                }
            }
        }

        // 시야 범위 밖의 Chunk들을 비활성화 및 제거
        foreach (var chunk in chunks)
        {
            Vector2 chunkPos = chunk.Key;
            if (Mathf.Abs(playerX - chunkPos.x) > renderDistance || Mathf.Abs(playerZ - chunkPos.y) > renderDistance)
            {
                chunksToRemove.Add(chunkPos);  // 제거할 Chunk를 리스트에 추가
            }
        }

        // 리스트에 추가된 Chunk들을 실제로 제거
        foreach (var chunkPos in chunksToRemove)
        {
            Destroy(chunks[chunkPos]);  // Chunk 오브젝트 삭제
            chunks.Remove(chunkPos);  // 딕셔너리에서 해당 Chunk 제거
        }
    }

    // 특정 좌표에 새로운 Chunk를 생성
    void CreateChunkAt(int x, int z)
    {
        Vector3 position = new Vector3(x * chunkSize * planeSize, 0, z * chunkSize * planeSize);  // Chunk의 위치 계산
        GameObject chunk = Instantiate(planePrefab, position, Quaternion.identity);  // Chunk 생성
        chunks[new Vector2(x, z)] = chunk;  // 딕셔너리에 추가
    }
}
