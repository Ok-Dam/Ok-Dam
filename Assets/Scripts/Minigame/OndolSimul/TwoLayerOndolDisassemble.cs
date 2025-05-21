using UnityEngine;
using System.Collections;

public class OnDolDisassemble : MonoBehaviour
{
    public GameObject[] ondolParts;  // 온돌 부품들을 담을 배열

    private Vector3[] initialPositions; // 각 부품의 초기 위치
    private Quaternion[] initialRotations; // 각 부품의 초기 회전

    public float animationDuration = 2f;  // 애니메이션 지속 시간

    private Camera mainCamera;

    void Start()
    {
        // 메인 카메라 찾기
        mainCamera = Camera.main;

        // 각 부품의 초기 상태 저장
        initialPositions = new Vector3[ondolParts.Length];
        initialRotations = new Quaternion[ondolParts.Length];

        for (int i = 0; i < ondolParts.Length; i++)
        {
            initialPositions[i] = ondolParts[i].transform.position;
            initialRotations[i] = ondolParts[i].transform.rotation;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);  // 카메라에서 클릭한 지점으로 레이 발사
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // 클릭한 지점에 오브젝트가 있으면
            {
                if (hit.collider.gameObject == this.gameObject)  // 이 스크립트가 붙은 오브젝트(Cube)를 클릭했으면
                {
                    StartCoroutine(DisassembleCoroutine());
                }
            }
        }
    }

    // 부드럽게 해체하는 Coroutine
    IEnumerator DisassembleCoroutine()
    {
        float elapsedTime = 0f;

        // 각 부품을 목표 위치로 서서히 이동
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            for (int i = 0; i < ondolParts.Length; i++)
            {
                // 각 부품을 부드럽게 이동
                Vector3 targetPosition = initialPositions[i] + new Vector3(0, -3f, 0);  // 예시로 Y축으로 내려간다고 가정
                ondolParts[i].transform.position = Vector3.Lerp(initialPositions[i], targetPosition, t);

                // 회전도 부드럽게 변경 가능
                Quaternion targetRotation = initialRotations[i] * Quaternion.Euler(0, 90f, 0);  // 회전 예시
                ondolParts[i].transform.rotation = Quaternion.Lerp(initialRotations[i], targetRotation, t);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 끝난 후 완전한 위치와 회전
        for (int i = 0; i < ondolParts.Length; i++)
        {
            ondolParts[i].transform.position = initialPositions[i] + new Vector3(0, -3f, 0);  // 최종 위치
            ondolParts[i].transform.rotation = initialRotations[i] * Quaternion.Euler(0, 90f, 0);  // 최종 회전
        }

        Debug.Log("온돌 해체 완료!");
    }
}
