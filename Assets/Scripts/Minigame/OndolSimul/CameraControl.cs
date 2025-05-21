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
        // 마우스 오른쪽 버튼 누르고 있는 동안 회전
        if (Input.GetMouseButton(1))  // 1 = 마우스 오른쪽 버튼
        {
            isRotating = true;

            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            currentRotation.x -= mouseY;
            currentRotation.y += mouseX;

            // 상하 회전 제한 (카메라가 뒤집히지 않도록)
            currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

            // 회전 적용 (오일러 각)
            transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        }
        else
        {
            isRotating = false;
        }

        // 마우스 오른쪽 버튼 안 눌렀을 때만 이동 허용
        if (!isRotating)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            transform.Translate(moveX, 0, moveZ);
        }
    }

}
