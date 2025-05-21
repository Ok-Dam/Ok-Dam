using UnityEngine;

public class SquirrelTrader : MonoBehaviour
{
    public GameObject rewardItemPrefab; // 벽돌 프리팹
    public Transform rewardSpawnPoint;  // 벽돌이 생성될 위치
    public int requiredAcorns = 1;      // 필요한 도토리 수

    private bool playerInRange = false;

    void Update()
    {
        // 플레이어가 근처에 있고 F키 누르면 거래
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TryTrade();
        }
    }

    void TryTrade()
    {
        // 도토리를 가진 플레이어가 있는지 확인
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null && inventory.acornCount >= requiredAcorns)
        {
            // 도토리 감소
            inventory.acornCount -= requiredAcorns;

            // 보상 아이템 생성
            Instantiate(rewardItemPrefab, rewardSpawnPoint.position, Quaternion.identity);
            Debug.Log("거래 성공! 벽돌 지급됨.");
        }
        else
        {
            Debug.Log("도토리가 부족해요!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
