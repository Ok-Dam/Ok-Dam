using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    public Transform flashlight;       // ������ (Spot Light)
    public float mouseSensitivity = 100f;
    public Transform playerCamera;     // Main Camera
    public GameObject startCanvas;     // ����ȭ�� ĵ����

    float xRotation = 0f;

    void Start()
    {
        // ó������ Ŀ�� ���̰� (Canvas�� ���� �����Ƿ�)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // ���� ȭ���� ������ ���� ���� ����
        if (startCanvas.activeSelf == false)
        {
            // Ŀ�� ��� & ����
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            flashlight.position = playerCamera.position;
            flashlight.rotation = playerCamera.rotation;
        }
        else
        {
            // ����ȭ�� ���� ������ Ŀ�� ���̰�
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
