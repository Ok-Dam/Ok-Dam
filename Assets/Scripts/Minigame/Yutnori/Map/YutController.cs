using System.Collections;
using UnityEngine;

public class YutController : MonoBehaviour
{
    public Rigidbody[] yutRigidbodies;
    private Vector3 dragStartPos;
    private bool isDragging = false;
    [SerializeField] private float throwForce = 500f;
    [SerializeField] private LayerMask yutLayer;
    [SerializeField] private YutnoriGameManager gameManager;
    private Camera mainCamera;
    [SerializeField] private float velocityThreshold = 0.1f;  // 정지 판정 속도
    [SerializeField] private float maxWaitTime = 5f;          // 최대 대기 시간 (인스펙터에서 조정)
    [SerializeField] private float faceUpThreshold = 0.7f;    // 앞면 판정 각도 (0.7 = 45도 이내)



    void Start()
    {
        mainCamera = Camera.main;
        yutRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void StartDrag()
    {
        if (gameManager.stage != GameStage.Interact) return;
        if (gameManager.isDraggingYut) return;

        gameManager.SetYutDragState(true);
        isDragging = true;
        dragStartPos = Input.mousePosition;
    }

    public void EndDrag()
    {
        gameManager.SetYutDragState(false);
        if (!isDragging) return;

        // 힘 계산 및 적용
        Vector3 dragEndPos = Input.mousePosition;
        Vector3 forceDir = (dragEndPos - dragStartPos);

        // Y축 힘 계산 (기본 300f + 드래그 길이 따라 변주)
        float yForce = 300f + (forceDir.magnitude * 0.5f);
        yForce = Mathf.Clamp(yForce, 250f, 450f); // 250~450 범위 제한


        forceDir.z = forceDir.y;
        forceDir.y = yForce;

        foreach (var rb in yutRigidbodies)
        {
            //  방향 변주 (메인 방향 유지 + 랜덤 편차)
            Vector3 variedForce = new Vector3(
                forceDir.x * Random.Range(0.9f, 1.1f), // X축 ±10% 변주
                forceDir.y * Random.Range(0.8f, 1.2f), // Y축 ±20% 변주
                forceDir.z * Random.Range(0.9f, 1.1f)  // Z축 ±10% 변주
            );

            //  힘 적용 (크기와 방향 모두 변주)
            rb.AddForce(variedForce.normalized * throwForce * Random.Range(0.95f, 1.05f));

            //  회전력 변주 강화
            rb.AddTorque(Random.insideUnitSphere * 80f); // 회전력 60% 증가
        }

        StartCoroutine(DelayedStateCheck());
        isDragging = false;
    }

    private IEnumerator DelayedStateCheck()
    {
        yield return new WaitForSeconds(0.2f); // 물리 적용 대기 시간
        StartCoroutine(CheckYutStateCoroutine());
    }

    // 코루틴: 윷 상태 체크
    private IEnumerator CheckYutStateCoroutine()
    {
        // 1차 검사: 최소 1회 이상 움직였는지 확인
        bool hasMoved = false;
        float timeout = 3f; // 움직임 감지 타임아웃

        while (timeout > 0 && !hasMoved)
        {
            foreach (var rb in yutRigidbodies)
            {
                if (rb.velocity.magnitude > 0.1f)
                {
                    hasMoved = true;
                    break;
                }
            }
            timeout -= Time.deltaTime;
            yield return null;
        }

        // 2차 검사: 실제 정지 감지
        float elapsed = 0f;
        while (!AllYutsStopped() && elapsed < maxWaitTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 결과 판정 및 출력
        int faceUpCount = CalculateFaceUpCount();
        string result = GetYutResult(faceUpCount);

        gameManager.setGameStage(GameStage.Move);
        Debug.Log($"결과: {result} ({faceUpCount}개 앞면)");
    }

    // 모든 윷이 정지했는지 확인
    private bool AllYutsStopped()
    {
        foreach (var rb in yutRigidbodies)
        {
            if (rb.velocity.magnitude > velocityThreshold ||
                rb.angularVelocity.magnitude > velocityThreshold)
                return false;
        }
        return true;
    }

    // 앞면 개수 계산
    private int CalculateFaceUpCount()
    {
        int count = 0;
        foreach (var rb in yutRigidbodies)
        {
            if (Vector3.Dot(rb.transform.up, Vector3.up) > faceUpThreshold)
                count++;
        }
        return count;
    }

    // 결과 문자열 변환
    // 빽도 윷이 배열의 0번째라고 가정
    private string GetYutResult(int faceUpCount)
    {
        // 빽도 판정: 3개 앞, 1개(빽도 윷)만 뒤
        if (faceUpCount == 3)
        {
            // 빽도 윷이 뒤집혔는지 확인 (예: 0번째)
            if (Vector3.Dot(yutRigidbodies[0].transform.up, Vector3.up) < -faceUpThreshold)
                return "빽도";
            else
                return "도";
        }
        else if (faceUpCount == 1)
        {
            return "걸";
        }
        else if (faceUpCount == 2)
        {
            return "개";
        }
        else if (faceUpCount == 0)
        {
            return "윷";
        }
        else if (faceUpCount == 4)
        {
            return "모";
        }
        return "ERROR";
    }


    void LateUpdate()
    {
        if (mainCamera == null) return;

        // 모든 자식 위치 제한
        foreach (var rb in yutRigidbodies)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(rb.position);
            viewportPos.x = Mathf.Clamp(viewportPos.x, 0.05f, 0.95f);
            viewportPos.y = Mathf.Clamp(viewportPos.y, 0.05f, 0.95f);
            Vector3 clampedPos = mainCamera.ViewportToWorldPoint(viewportPos);
            rb.position = new Vector3(clampedPos.x, rb.position.y, clampedPos.z);
        }
    }
}
