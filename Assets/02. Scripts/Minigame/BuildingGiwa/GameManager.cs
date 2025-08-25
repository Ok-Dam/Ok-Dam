using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI perfectText;
    public CameraFollow cameraFollow;


    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject resultPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI resultText;

    [Header("Game Settings")]
    public GameObject blockPrefab;
    public float blockHeight = 0.5f;
    public float defaultBlockWidth = 10f;

    [Header("House Background")]
    public GameObject houseBackground;
    public float houseDropPerFloor = 0.1f;

    [HideInInspector]
    public float currentBlockWidth;

    private int currentFloor = 0;
    private Vector3 houseStartPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowStartUI();
        houseStartPos = houseBackground.transform.position;

    }

    public void ShowStartUI()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void StartGame()
    {
        currentFloor = 5;
        currentBlockWidth = defaultBlockWidth;

        ClearAllBlocks();

        houseStartPos = houseBackground.transform.position;

        startPanel.SetActive(false);
        resultPanel.SetActive(false);
        gamePanel.SetActive(true);

        houseBackground.SetActive(true);

        SpawnNextBlock();
        UpdateFloorText();
        cameraFollow.EnableFollow(true);
    }

    public void SpawnNextBlock()
    {
        Vector3 spawnPos = new Vector3(-3f, currentFloor * blockHeight, 0f);
        GameObject block = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        block.tag = "Block";

        Vector3 scale = block.transform.localScale;
        scale.x = currentBlockWidth;
        block.transform.localScale = scale;

        cameraFollow.SetTarget(block.transform);



        if (currentFloor == 5)
        {
            Vector3 roofSpawnPos = spawnPos + new Vector3(3f, -1f, 0f);
            GameObject roof = Instantiate(houseBackground, roofSpawnPos, Quaternion.identity);
            roof.tag = "Block";
        }
    }

    public void NextFloor()
    {
        currentFloor++;
        UpdateFloorText();
        SpawnNextBlock();

        

    }

    private void UpdateFloorText()
    {
        floorText.text = $"{currentFloor - 5}층";
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        resultPanel.SetActive(true);
        resultText.text = $"당신의 기록은 {currentFloor - 5}층입니다!";
        cameraFollow.EnableFollow(false);

        houseBackground.SetActive(false);
    }

    public void Restart()
    {
        StartGame();
    }

    private void ClearAllBlocks()
    {
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("Block"))
        {
            Destroy(b);
        }
    }

    public void ShowPerfect()
    {
        StopAllCoroutines();
        StartCoroutine(ShowPerfectText());
    }

    private IEnumerator ShowPerfectText()
    {
        perfectText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f); // 텍스트 표시 시간
        perfectText.gameObject.SetActive(false);
    }

}
