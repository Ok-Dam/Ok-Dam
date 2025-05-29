using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 방향 계산 (카메라 기준 전후좌우)
        Vector3 move = transform.right * h + transform.forward * v;

        // 이동 실행
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}