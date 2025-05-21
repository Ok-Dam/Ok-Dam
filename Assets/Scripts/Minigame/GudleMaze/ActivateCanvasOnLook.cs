using UnityEngine;

public class ActivateCanvasOnLook : MonoBehaviour
{
    public GameObject canvasToActivate;       // 보여줄 캔버스
    public float detectionDistance = 5f;       // 큐브 감지 거리
    public string targetObjectTag = "TargetCube"; // 감지할 큐브 태그

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 카메라 정면으로 Ray 발사
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionDistance))
            {
                if (hit.collider.CompareTag(targetObjectTag))
                {
                    canvasToActivate.SetActive(true);
                }
            }
        }
    }
}
