using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float lookSpeedX = 3.0f;  // 좌우 회전 속도
    public float lookSpeedY = 3.0f;  // 상하 회전 속도
    public float lookSpeedZ = 3.0f;
    public Transform playerBody;     // 플레이어의 몸체(카메라와 연결된 객체)

    private float currentXRotation = 0.0f;  // 상하 회전 각도
    private float rotationY = 0.0f;         // 좌우 회전 각도

    void Update()
    {
        // 마우스 이동에 따른 시야 회전
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;  // 좌우 마우스 이동
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;  // 상하 마우스 이동
        float mouseZ = Input.GetAxis("Mouse Z") * lookSpeedZ;

        // 상하 회전 제한
        currentXRotation -= mouseY;
        currentXRotation = Mathf.Clamp(currentXRotation, -90f, 90f);  // -90도 ~ 90도 범위 내에서 회전

        // 좌우 회전 (마우스 왼쪽 버튼을 눌렀을 때)
        rotationY += mouseX;

        // 카메라의 상하 회전
        transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);

        // 플레이어의 몸체(주로 캐릭터) 좌우 회전
        playerBody.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}
