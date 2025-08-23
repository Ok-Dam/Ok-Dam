using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    private Transform target; // ���� ���� Transform

    private bool shouldFollow = false;

    void Update()
    {
        if (!shouldFollow || target == null) return;

        Vector3 pos = transform.position;
        float targetY = target.position.y - 2f;
        pos.y = Mathf.Lerp(pos.y, targetY, followSpeed * Time.deltaTime);
        transform.position = pos;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void EnableFollow(bool enable)
    {
        shouldFollow = enable;
    }
}
