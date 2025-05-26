using UnityEngine;

public class FlashlightFollowMouse : MonoBehaviour
{
    public Camera playerCamera;
    public Transform flashlight; // �������� Transform
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // ���콺 �����Ӱ�
        Cursor.visible = true;                   // ���콺 ���̰�
    }


    public Transform cameraTransform;

    void Update()
    {
        transform.position = cameraTransform.position; // ��ġ ���󰡱�
        transform.rotation = cameraTransform.rotation; // ȸ�� ���󰡱�
    }

    public void EnableCursorForUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
