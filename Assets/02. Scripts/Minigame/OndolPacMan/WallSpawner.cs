using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    private GridManager gridManager;
    public GameObject wallPrefab;

    void Start()
    {
        gridManager = GetComponent<GridManager>();
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
                    Vector3 pos = gridManager.CoordToWorldPos(x, y);
                    Instantiate(wallPrefab, pos, Quaternion.identity);
                }
            }
        }
    }
}
