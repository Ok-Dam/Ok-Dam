using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    [Tooltip("�̴ϸ� ī�޶� ���� ����")]
    public float height = 50f;

    private Transform target;
    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
        Debug.Log("[MiniMap] Ÿ�� ��� �Ϸ�: " + playerTransform.name);
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
