using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackHandler : MonoBehaviour
{
    public GameObject crackedPrefab; // "cracked" 프리팹
    public GameObject dugPrefab; // "dug" 프리팹

    private int hitCount = 0; // 충돌 횟수

    private void OnCollisionEnter(Collision collision)
    {
        // 망치가 충돌했을 때
        if (collision.gameObject.CompareTag("hammer"))
        {
            hitCount++; // 충돌 횟수 증가

            // "cracked" 오브젝트 생성
            Instantiate(crackedPrefab, transform.position, transform.rotation);

            // 4회 충돌 시 현재 "crack" 오브젝트를 비활성화하고 "dug" 오브젝트 생성
            if (hitCount >= 4)
            {
                GameObject dugObject = Instantiate(dugPrefab, transform.position, transform.rotation);
                dugObject.tag = "dug"; // 태그 설정
                gameObject.SetActive(false); // crack 오브젝트 비활성화
            }
        }
    }
}
