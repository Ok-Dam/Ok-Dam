using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public Camera mainCamera; // 메인 카메라
    public Button makeOndolButton; // "Make Ondol" 버튼

    public Vector3 targetPosition = new Vector3(54.68f, 5.33f, 1.43f);
    public Vector3 targetRotation = new Vector3(0f, 180f, 0f);

    void Start()
    {
        if (makeOndolButton != null)
        {
            makeOndolButton.onClick.AddListener(MoveCameraToOndol);
        }
        else
        {
            Debug.LogError("Make Ondol Button is not assigned!");
        }
    }

    public void MoveCameraToOndol()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = targetPosition;
            mainCamera.transform.rotation = Quaternion.Euler(targetRotation);
        }
        else
        {
            Debug.LogError("Main Camera is not assigned!");
        }
    }
}
