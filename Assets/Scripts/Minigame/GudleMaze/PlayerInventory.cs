using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int acornCount = 0;

    // 도토리 충돌 시 예시
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Acorn"))
        {
            FindObjectOfType<PlayerInventory>().acornCount++;
            Destroy(other.gameObject);
        }
    }

}
