using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    [Tooltip("미니맵 카메라가 따라갈 높이")]
    public float height = 50f;

    private Transform target;
    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
        Debug.Log("[MiniMap] 타겟 등록 완료: " + playerTransform.name);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = target.position;
        pos.y = height;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
