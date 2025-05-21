using UnityEngine;

public class FlashlightHideObject : MonoBehaviour
{
    public Transform flashlight; // 손전등의 Transform
    public float maxDistance = 10f;
    public float angleThreshold = 20f;

    void Update()
    {
        Vector3 dirToObject = transform.position - flashlight.position;
        float angle = Vector3.Angle(flashlight.forward, dirToObject);
        float distance = dirToObject.magnitude;

        if (angle < angleThreshold && distance < maxDistance)
        {
            gameObject.SetActive(false); // 자신을 비활성화
        }
    }
}
