using UnityEngine;
using System.Collections;

public class pacGameManager : MonoBehaviour
{
    private EnvironmentSpawner environmentSpawner;
    private EnemyManager enemyManager;
    private GridManager gridManager;
    public pacPlayerController playerController;

    [SerializeField] float firstEnemyGenTime = 3.0f;
    [SerializeField] float enemyGenTime = 15.0f;

    void Start()
    {
        environmentSpawner = GetComponent<EnvironmentSpawner>();
        enemyManager = GetComponent<EnemyManager>();  
        gridManager = GetComponent<GridManager>();

        // Initialize environment features
        if (environmentSpawner != null && gridManager != null)
        {
            environmentSpawner.Initialize(gridManager);
            environmentSpawner.SpawnObjects();
        }
        else
        {
            Debug.LogError("env spawner or gridmgr gone");
        }

        // Spawn enemies after a delay (or directly)
        if (enemyManager != null)
        {
            StartCoroutine(SpawnEnemiesRepeatedly());
        }
        else
        {
            Debug.LogError("EnemyManager not assigned in GameManager.");
        }

        // Initialize player if needed, e.g. reset position (optional)
        if (playerController != null)
        {
            // playerController.Setup() if needed (depending on design)
        }
    }

    IEnumerator SpawnEnemiesRepeatedly()
    {
        yield return new WaitForSeconds(firstEnemyGenTime); // Initial delay of 3 seconds

        while (true)
        {
            enemyManager.SpawnEnemyAtExit();
            yield return new WaitForSeconds(enemyGenTime); // Spawn every 15 seconds after the first spawn
        }
    }

}
