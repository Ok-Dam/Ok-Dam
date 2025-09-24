using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GridManager gridManager;
    public GameObject player;

    private Vector2Int exitPos; // 출구 좌표 저장
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        gridManager = GetComponent<GridManager>();
        exitPos = FindExitPosition();
    }
    //IEnumerator DelayedSpawn()
    //{
    //    yield return new WaitForSeconds(2.0f); // 2초 대기 (원하는 시간으로 수정)
    //    SpawnEnemyAtExit();
    //}

    // 출구 위치를 gridMap에서 특정 값(예: 3)으로 찾아 저장
    Vector2Int FindExitPosition()
    {
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                if (gridManager.gridMap[x, y] == 3) // 출구 표시 값
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        // 출구 발견 못 하면 기본값(오른쪽 아래 모서리)
        return new Vector2Int(gridManager.gridWidth - 1, gridManager.gridHeight - 1);
    }

    // 출구 위치에 적 생성
    public void SpawnEnemyAtExit()
    {
        Vector2Int clampedExitPos = gridManager.ClampToGrid(exitPos);

        Vector3 worldPos = gridManager.CoordToWorldPos(clampedExitPos.x, clampedExitPos.y);

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(worldPos.x, 0, worldPos.z), Quaternion.identity);

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        ai.Init(gridManager, player);

        enemies.Add(enemy);
    }
}
