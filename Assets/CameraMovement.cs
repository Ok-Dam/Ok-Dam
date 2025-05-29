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

        // ���� ��� (ī�޶� ���� �����¿�)
        Vector3 move = transform.right * h + transform.forward * v;

        // �̵� ����
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}