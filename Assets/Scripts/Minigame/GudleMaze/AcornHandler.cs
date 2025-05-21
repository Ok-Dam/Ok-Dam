using UnityEngine;

public class AcornHandler : MonoBehaviour
{
    public GameObject hand;               // HAND 오브젝트
    public GameObject dotoriPrefab;       // DOTORI 프리팹
    public AudioClip soundEffect;         // 효과음
    private Animator animator;            // A 객체의 애니메이터
    private int collisionCount = 0;       // DOTORI와의 충돌 횟수

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // "DOTORI" 태그를 가진 객체와 충돌 시
        if (collision.gameObject.CompareTag("DOTORI"))
        {
            collisionCount++;  // 충돌 횟수 증가

            if (collisionCount == 5)
            {
                // 횟수가 5번일 때 애니메이션2 부여 + 효과음
                animator.SetTrigger("Animation2");
                AudioSource.PlayClipAtPoint(soundEffect, transform.position);
            }
            else
            {
                // 애니메이션1 부여 + DOTORI를 HAND에 붙이기
                animator.SetTrigger("Animation1");
                AttachDotoriToHand(collision.gameObject);

                // 3초 후에 본 상태로 돌아가며 랜덤으로 돌아다니기
                Invoke("ReturnToOriginalState", 3f);
            }
        }
    }

    // DOTORI를 HAND에 부착시키는 함수
    void AttachDotoriToHand(GameObject dotori)
    {
        dotori.transform.SetParent(hand.transform); // DOTORI를 HAND에 붙임
        dotori.transform.localPosition = Vector3.zero; // 위치를 HAND의 중앙으로 맞춤
        dotori.transform.localRotation = Quaternion.identity; // 회전 초기화
    }

    // 3초 후 본 상태로 돌아가며, A 객체는 랜덤으로 돌아다니게 하는 함수
    void ReturnToOriginalState()
    {
        // DOTORI가 HAND에서 떨어짐
        GameObject dotori = hand.transform.GetChild(0).gameObject;
        if (dotori != null)
        {
            dotori.transform.SetParent(null); // DOTORI를 부모에서 분리
            Destroy(dotori);  // DOTORI 객체 제거 (혹은 비활성화)
        }

        // A 객체의 랜덤 이동 코드 호출 (이미 구현된 랜덤 이동 코드)
        // RandomMovement(); // 랜덤 이동 구현 부분 호출
    }
}

