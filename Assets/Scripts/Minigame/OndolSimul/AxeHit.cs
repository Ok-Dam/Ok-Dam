using UnityEngine;

public class AxeHit: MonoBehaviour
{
    public TreeCutter treeCutter;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Axe"))
        {
            treeCutter.CutTree();
            Destroy(this.gameObject);  // 더 이상 잘리지 않게 트리거 제거
        }
    }
}
