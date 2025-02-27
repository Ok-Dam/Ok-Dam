using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SaveObject : MonoBehaviour
{
    private bool isLocked = false; // 이동 불가능 상태인지 확인

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dug"))
        {
            // "save" 오브젝트를 "dug" 오브젝트 위치로 이동
            transform.position = other.transform.position;

            // "dug" 오브젝트 제거
            Destroy(other.gameObject);

            // 이동 불가능 상태로 설정
            isLocked = true;

            DisableMovement();

            // Rigidbody가 있다면 중력 및 이동 제한
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // 물리적 이동 제한
            }
        }
    }

    private void DisableMovement()
    {
        // Rigidbody가 있다면 물리적 이동 제한
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // 물리적인 힘을 받지 않도록 설정
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 드래그 이동을 제어하는 스크립트가 있다면 비활성화
        if (TryGetComponent(out ObjectDraggable draggable))
        {
            draggable.enabled = false;
        }

        // Transform 고정 (필요 시)
        transform.SetParent(null); // 부모가 있다면 해제
    }
}

