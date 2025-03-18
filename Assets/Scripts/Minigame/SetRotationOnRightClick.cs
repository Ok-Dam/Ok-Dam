using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotationOnRightClick : MonoBehaviour
{
    public float targetZRotation = 70f; // 목표 Z 회전 값
    public float rotationSpeed = 5f; // 회전 속도
    private Quaternion targetRotation; // 목표 회전값
    private bool isRotating = false; // 회전 중 여부

    void Start()
    {
        targetRotation = transform.rotation; // 초기 회전값 저장
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭
        {
            if (!isRotating) // 이미 회전 중이 아니면 실행
            {
                isRotating = true;
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetZRotation);
            }
        }

        if (isRotating) // 회전 중이면 부드럽게 회전
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 목표 회전값에 거의 도달하면 정지
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
}
