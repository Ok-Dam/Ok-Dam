using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    public Transform flashlight;       // 손전등 (Spot Light)
    public float mouseSensitivity = 100f;
    public Transform playerCamera;     // Main Camera
    public GameObject startCanvas;     // 시작화면 캔버스

    float xRotation = 0f;

    void Start()
    {
        // 처음에는 커서 보이게 (Canvas가 켜져 있으므로)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // 시작 화면이 꺼졌을 때만 조작 가능
        if (startCanvas.activeSelf == false)
        {
            // 커서 잠금 & 숨김
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
            // 시작화면 켜져 있으면 커서 보이게
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
