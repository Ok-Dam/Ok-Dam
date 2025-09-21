using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10; // 가로로 몇 칸. 실제 길이 x
    public int gridHeight = 10; // 세로로 몇 칸
    public float cellSize = 1f; // 한 칸의 크기

    public int[,] gridMap;

    void Start()
    {
        // 0으로 초기화 (빈 칸)
        gridMap = new int[gridWidth, gridHeight];
        InitializeGrid();
        ShowGridBounds();
    }

    void InitializeGrid()
    {
        // 예시: 테두리를 벽(1)으로 설정
        for (int x = 0; x < gridWidth; x++)
        {
            gridMap[x, 0] = 1;
            gridMap[x, gridHeight - 1] = 1;
        }
        for (int y = 0; y < gridHeight; y++)
        {
            gridMap[0, y] = 1;
            gridMap[gridWidth - 1, y] = 1;
        }
        // 필요 시 추가 벽 지정 가능
    }

    // 모든 셀 타입 출력 확인용 로그함수 (디버그)
    void ShowGridBounds()
    {
        Debug.Log($"Grid Size: {gridWidth}x{gridHeight}");
    }

    // 그리드 좌표 → 월드 위치 변환 함수
    public Vector3 CoordToWorldPos(int x, int y)
    {
        float worldX = x * cellSize;
        float worldZ = y * cellSize;
        return new Vector3(worldX, 0, worldZ); // y=0면 평면
    }
}
