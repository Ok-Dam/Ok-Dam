using System.Collections;
using UnityEngine;

public class SmokeMovement : MonoBehaviour
{
    public Transform[] points; // 5개의 지점
    public float moveSpeed = 2f; // 연기 이동 속도
    public Camera mainCamera; // 메인 카메라
    public Transform[] zoomPoints; // 각 지점 확대 위치
    public GameObject[] uiScreens; // 각 지점의 UI 화면

    private int currentPoint = 0;
    private bool isMoving = false;

    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;

    void Start()
    {
        // 초기 카메라 위치 저장
        initialCameraPosition = mainCamera.transform.position;
        initialCameraRotation = mainCamera.transform.rotation;
    }

    void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            if (currentPoint < points.Length)
            {
                StartCoroutine(MoveToNextPoint());
            }
            else
            {
                // 연기 오브젝트 비활성화
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator MoveToNextPoint()
    {
        isMoving = true;

        if (currentPoint >= points.Length) yield break;

        // 연기 이동
        Vector3 startPos = transform.position;
        Vector3 targetPos = points[currentPoint].position;
        float journey = 0f;

        while (journey < 1f)
        {
            journey += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, journey);
            yield return null;
        }

        // 카메라 확대 및 UI 표시
        mainCamera.transform.position = zoomPoints[currentPoint].position;
        mainCamera.transform.rotation = zoomPoints[currentPoint].rotation;

        uiScreens[currentPoint].SetActive(true);
        currentPoint++; // 여기서 증가

        // 마우스 클릭 대기 후 원위치 복귀
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = initialCameraRotation;

        uiScreens[currentPoint - 1].SetActive(false);

        isMoving = false;
    }
}
