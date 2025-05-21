using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SquirrelBehavior : MonoBehaviour
{
    public GameObject balloonUI;  // 말풍선 UI 오브젝트
    public Text balloonText;      // 말풍선 텍스트
    private NavMeshAgent agent;    // ? 다람쥐 이동용 에이전트
    private Transform player;     // 플레이어(카메라)

    private bool hasReacted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  // ? NavMeshAgent 컴포넌트 가져오기
        player = Camera.main.transform;        // 카메라 트랜스폼 가져오기
        balloonUI.SetActive(false);             // 말풍선 비활성화
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !hasReacted)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    // ? 다람쥐 멈추게 하기
                    if (agent != null)
                    {
                        agent.isStopped = true;
                    }

                    // ? 다람쥐를 플레이어 쪽으로 바라보게
                    Vector3 lookPos = player.position - transform.position;
                    lookPos.y = 0;
                    transform.rotation = Quaternion.LookRotation(lookPos);

                    // ? 말풍선 보여주기
                    balloonText.text = "안녕! 난 다람쥐야!";
                    balloonUI.SetActive(true);

                    // 말풍선 위치 다람쥐 머리 위
                    balloonUI.transform.position = transform.position + Vector3.up * 2f;

                    // ? 말풍선이 카메라 바라보게
                    balloonUI.transform.LookAt(player);

                    hasReacted = true;
                }
            }
        }
    }
}
