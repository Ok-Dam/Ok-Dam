using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    private GridManager gridManager;
    public GameObject wallPrefab;

    void Start()
    {
        gridManager = GetComponent<GridManager>();
        if (gridManager == null)
            Debug.LogError("GridManager is not set in WallSpawner!");
        SpawnWalls();
    }

    // 그냥 instantiate
    //void SpawnWalls()
    //{
    //    Debug.Log("SpawnWalls 시작: " + Time.frameCount);
    //    for (int x = 0; x < gridManager.gridWidth; x++)
    //    {
    //        for (int y = 0; y < gridManager.gridHeight; y++)
    //        {
    //            if (gridManager.gridMap[x, y] == 1)
    //            {
    //                // 칸 좌하단 기준 위치 계산 (중앙 offset 제거)
    //                float worldX = x * gridManager.cellSize;
    //                float worldZ = (gridManager.gridHeight - 1 - y) * gridManager.cellSize;

    //                Vector3 pos = new Vector3(worldX, 0, worldZ);
    //                Instantiate(wallPrefab, pos, Quaternion.identity);
    //                // GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
    //                //wall.isStatic = true;
    //            }
    //        }
    //    }
    //    Debug.Log("SpawnWalls 종료: " + Time.frameCount);
    //}
    
    // static batching 쓴 버전 
    void SpawnWalls()
    {
        GameObject wallParent = new GameObject("WallParent");

        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                if (gridManager.gridMap[x, y] == 1)
                {
                    float worldX = x * gridManager.cellSize;
                    float worldZ = (gridManager.gridHeight - 1 - y) * gridManager.cellSize;

                    Vector3 pos = new Vector3(worldX, 0, worldZ);
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
                    wall.transform.parent = wallParent.transform;
                }
            }
        }

        StaticBatchingUtility.Combine(wallParent);
    }

}
