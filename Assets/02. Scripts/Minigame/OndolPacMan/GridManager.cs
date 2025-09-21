using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 23;
    public int gridHeight = 13;
    public float cellSize = 20f;

    public int[,] gridMap;

    // 맵 데이터 (0=빈칸, 1=벽, 2=입구, 3=출구, 4=부품)
    int[,] designedMap = new int[,]
    {
        {1,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,1},
        {1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,1},
        {1,0,1,1,0,1,1,1,0,1,1,1,1,1,0,1,1,1,0,1,1,0,1},
        {1,0,1,0,0,1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,1,0,1},
        {1,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,1,0,0,1,0,1,0,0,1,0,0,0,0,0,0,1},
        {1,1,1,1,1,0,1,1,1,0,0,4,0,0,1,1,1,0,1,1,1,1,1},
        {1,0,0,0,0,0,0,1,0,0,1,0,1,0,0,1,0,0,0,0,0,0,1},
        {1,0,0,0,0,1,0,0,0,0,1,0,1,0,0,0,0,1,0,0,0,0,1},
        {1,0,1,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,1,0,1},
        {1,0,1,1,0,1,0,1,1,1,1,0,1,1,1,1,0,1,0,1,1,0,1},
        {1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,1},
        {1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1},
    };

    void Awake()
    {
        gridMap = new int[gridWidth, gridHeight];
        InitializeGrid();
    }

    void InitializeGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if ((x == 11 && y == 0) || (x == 11 && y == 12))
                    gridMap[x, y] = 0; // 입구, 출구는 빈 칸으로 처리
                else
                    gridMap[x, y] = designedMap[y, x];
            }
        }
    }

    public Vector3 CoordToWorldPos(int x, int y)
    {
        float worldX = x * cellSize ;
        float worldZ = (gridHeight - 1 - y) * cellSize + cellSize * 0.5f;  // y축 반전 + 중앙 위치 조정
        return new Vector3(worldX, 0, worldZ);
    }
}
