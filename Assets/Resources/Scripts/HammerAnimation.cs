using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HammerAnimation : MonoBehaviour
{
    private Vector3 originalPosition; // 초기 위치
    private Quaternion originalRotation; // 초기 회전값
    public float hammerDownAngle = 30f; // 내려칠 각도 (Z축)
    public float animationSpeed = 0.1f; // 애니메이션 속도

    void Start()
    {
        // 현재 위치와 회전값 저장
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // 마우스 오른쪽 버튼 클릭하면 애니메이션 실행
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(SwingHammer());
        }
    }

    IEnumerator SwingHammer()
    {
        // 내리치는 회전값 설정 (현재 회전값 기준)
        Quaternion downRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 70f);

        // 내리찍기 (빠르게)
        float elapsedTime = 0;
        while (elapsedTime < animationSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, downRotation, elapsedTime / animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = downRotation; // 최종 위치 고정

        // 잠시 대기 (0.2초)
        yield return new WaitForSeconds(0.2f);

        // 원래 위치로 복귀 (부드럽게)
        elapsedTime = 0;
        while (elapsedTime < animationSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, elapsedTime / animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = originalRotation; // 최종 위치 고정
    }
}
