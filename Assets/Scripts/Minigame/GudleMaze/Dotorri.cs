using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dotorri : MonoBehaviour
{
    // 도토리가 다람쥐와 충돌할 때 호출
    private void OnTriggerEnter(Collider other)
    {
        // 다람쥐와 충돌했을 때만 도토리 회수
        if (other.CompareTag("Squirrel")) // 다람쥐 오브젝트의 태그가 "Squirrel"일 경우
        {
            Collect();
        }
    }

    // 도토리 회수하는 메서드
    void Collect()
    {
        // 도토리를 회수하고 없애기
        Destroy(gameObject); // 도토리 오브젝트를 삭제
    }
}
