using UnityEngine;

public class DoorMovementDetector : MonoBehaviour
{
    public Transform targetColliderTransform; // 기준이 되는 위치 (Collider가 있는 오브젝트의 Transform)
    public Transform playerCamera;            // 1인칭 카메라
    public GameObject panelToActivate;        // 활성화할 UI Panel
    public float activationDistance = 3f;     // 거리 기준

    private bool hasActivated = false;

    void Start()
    {
        if (panelToActivate != null)
            panelToActivate.SetActive(false);
    }

    void Update()
    {
        if (hasActivated || targetColliderTransform == null || playerCamera == null)
            return;

        float distance = Vector3.Distance(playerCamera.position, targetColliderTransform.position);

        if (distance <= activationDistance)
        {
            panelToActivate.SetActive(true);
            hasActivated = true;
            Debug.Log("거리가 가까워져서 패널이 활성화됨");
        }
    }
}
