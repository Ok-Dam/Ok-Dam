using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // 따라가야 할 대상
    public Transform targetTr;
    private Transform camTr;

    // 따라가야 할 대상으로부터 얼마나 떨어져 있을지
    [Range(2.0f, 20.0f)] // 변수 입력 범위 제한, 인스펙터 뷰에 슬라이드바
    public float distance = 3.5f;

    // Y축으로 이동할 높이, 카메라 높이
    [Range(0.0f, 10.0f)]
    public float height = 1.5f;

    //카메라 LookAt의 offset 값
    public float targetOffset = 1.7f;

    // 카메라 반응 속도
    public float damping = 0.1f;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // Main Camera 자신의 Transform 컴포넌트 추출
        camTr = GetComponent<Transform>();
    }

    // Update에서 이동 로직 한 후 실행하기 위해
    void LateUpdate()
    {
        // 추적해야 할 대상의 뒤쪽으로 distance만큼, 위로 height만큼 이동
        // 타깃의 위치 + (타겟의 뒤쪽 방향 * 떨어질 거리) + (y축 방향 * 높이)
        Vector3 pos = targetTr.position
            + (Vector3.back * distance)
            + (Vector3.up * height);

        // 구면 선형 보간 사용, 위치 부드럽게 바꾸기
        //camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime*damping);

        // SmoothDamp로 위치 보간
        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);

        //// 피벗 좌표 향해 회전
        //camTr.LookAt(targetTr.position + (targetTr.up * targetOffset));
    }
}
