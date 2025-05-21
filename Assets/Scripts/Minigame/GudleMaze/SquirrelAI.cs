using UnityEngine;
using UnityEngine.AI;

public class SquirrelAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform squirrelHandTransform;
    public GameObject acornPrefab;
    public Animator animator;
    public Transform exitPoint;  // 나갈 위치 (걸어서 나갈 때 필요)

    private int hitCount = 0;
    private bool isDefeated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Acorn") && !isDefeated)  // 도토리에 맞았는지 체크
        {
            hitCount++;

            if (hitCount >= 3)
            {
                DefeatSquirrel();
            }
        }
    }

    void DefeatSquirrel()
    {
        isDefeated = true;
        agent.isStopped = true;  // AI 멈추기

        // 도토리를 손에 쥠
        GameObject acorn = Instantiate(acornPrefab, squirrelHandTransform.position, Quaternion.identity);
        acorn.transform.SetParent(squirrelHandTransform);
        acorn.transform.localPosition = Vector3.zero;
        acorn.transform.localRotation = Quaternion.identity;

        // 표정 변경 (애니메이션 트리거)
        animator.SetTrigger("Surprised");

        // 나가기 선택 (걸어서 나가기 or 사라지기)
        bool walkAway = Random.value > 0.5f; // 50% 확률로 선택
        if (walkAway)
        {
            agent.SetDestination(exitPoint.position);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}
