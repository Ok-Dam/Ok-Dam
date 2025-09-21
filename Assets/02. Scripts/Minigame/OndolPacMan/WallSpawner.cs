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

    void SpawnWalls()
    {
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                if (gridManager.gridMap[x, y] == 1)
                {
                    // Ä­ ÁÂÇÏ´Ü ±âÁØ À§Ä¡ °è»ê (Áß¾Ó offset Á¦°Å)
                    float worldX = x * gridManager.cellSize;
                    float worldZ = (gridManager.gridHeight - 1 - y) * gridManager.cellSize;

                    Vector3 pos = new Vector3(worldX, 0, worldZ);
                    Instantiate(wallPrefab, pos, Quaternion.identity);
                }
            }
        }
    }

}
