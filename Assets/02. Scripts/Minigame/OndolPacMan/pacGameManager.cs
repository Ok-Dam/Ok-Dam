using UnityEngine;
using System.Collections;

public class pacGameManager : MonoBehaviour
{
    public static pacGameManager Instance { get; private set; }

    private EnvironmentSpawner environmentSpawner;
    private EnemyManager enemyManager;
    private GridManager gridManager;
    public pacPlayerController playerController;

    [SerializeField] float firstEnemyGenTime = 3.0f;
    [SerializeField] float enemyGenTime = 15.0f;

    private int totalCollectibles = 5;
    private int collectedCount = 0;

    // 데운 그리드 수 
    private int heatedCount = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

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
            Debug.LogError("EnvironmentSpawner or GridManager is missing!");
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

        // Initialize player if needed
        if (playerController != null)
        {
            // playerController.Setup() if needed
        }
    }

    IEnumerator SpawnEnemiesRepeatedly()
    {
        yield return new WaitForSeconds(firstEnemyGenTime); // Initial delay

        while (true)
        {
            enemyManager.SpawnEnemyAtExit();
            yield return new WaitForSeconds(enemyGenTime);
        }
    }

    public void IncrementHeatedCount()
    {
        heatedCount++;
    }

    public int GetHeatedCount()
    {
        return heatedCount;
    }

    // Called by collectible on pickup
    public void CollectibleCollected()
    {
        collectedCount++;
        Debug.Log($"Collectibles collected: {collectedCount} / {totalCollectibles}");
    }

    public bool HasCollectedAll()
    {
        return collectedCount >= totalCollectibles;
    }

    // Called when player reaches exit
    public void CheckWinCondition()
    {
        if (HasCollectedAll())
            WinGame();
        else
            LoseGame();
    }

    private void WinGame()
    {
        Debug.Log("You win!");
        // TODO: add win UI, scene transition, etc.
    }

    private void LoseGame()
    {
        Debug.Log("You lose!");
        // TODO: add lose UI, retry logic, etc.
    }
}
