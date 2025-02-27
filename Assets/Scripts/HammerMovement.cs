using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HammerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도

    private void Update()
    {
        // WASD 키 입력 감지 (W = 위, A = 왼쪽, S = 아래, D = 오른쪽)
        float horizontal = 0f;
        float vertical = 0f;

        // W (위로 이동) => Z값 증가
        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1f;
        }
        // S (아래로 이동) => Z값 감소
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1f;
        }

        // A (왼쪽으로 이동) => X값 감소
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }
        // D (오른쪽으로 이동) => X값 증가
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }

        // 이동 방향 벡터 계산
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

        // 망치 이동
        transform.Translate(moveDirection);
    }
}

