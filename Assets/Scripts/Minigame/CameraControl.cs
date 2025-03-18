using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float rotationSpeed = 3.0f;
    public float moveSpeed = 5.0f;
    private Vector2 currentRotation;
    private bool isRotating = false;

    void Start()
    {
        // 시작 시 카메라 회전 초기값을 (0, 180, 0)으로 설정
        currentRotation = new Vector2(0, 180);
    }

    void Update()
    {
        // 마우스 왼쪽 버튼을 누르고 있을 때 카메라 회전
        if (Input.GetMouseButton(0))  // 0은 왼쪽 마우스 버튼
        {
            isRotating = true;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // 카메라 회전 계산
            currentRotation.x -= mouseY * rotationSpeed;
            currentRotation.y += mouseX * rotationSpeed;

            // 회전 제한 (세로 회전 제한)
            currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

            // 카메라에 회전 적용
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        }
        else
        {
            isRotating = false;
        }

        // WASD 키로 이동
        if (!isRotating)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            // 카메라 이동
            transform.Translate(moveX, 0, moveZ);
        }
    }
}
