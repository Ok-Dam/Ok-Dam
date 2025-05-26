using UnityEngine;

public class FlashlightFollowMouse : MonoBehaviour
{
    public Camera playerCamera;
    public Transform flashlight; // 손전등의 Transform
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // 마우스 자유롭게
        Cursor.visible = true;                   // 마우스 보이게
    }


    public Transform cameraTransform;

    void Update()
    {
        transform.position = cameraTransform.position; // 위치 따라가기
        transform.rotation = cameraTransform.rotation; // 회전 따라가기
    }

    public void EnableCursorForUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
