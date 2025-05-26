using System.Collections;
using UnityEngine;

public class SequentialAnimator : MonoBehaviour
{
    public GameObject animator1Object;   // 첫 번째 애니메이션 GameObject
    public GameObject animator2Object;   // 두 번째 애니메이션 GameObject
    public float animator1Duration = 5f; // 첫 번째 애니메이션 재생 시간
    public float animator2Duration = 4f; // 두 번째 애니메이션 재생 시간

    private Animator animator1;
    private Animator animator2;

    void Start()
    {
        animator1 = animator1Object.GetComponent<Animator>();
        animator2 = animator2Object.GetComponent<Animator>();

        // 시작할 때 두 번째는 꺼두기
        animator2Object.SetActive(false);

        // 첫 번째 애니메이션 시작
        animator1Object.SetActive(true);
        animator1.Play("Simulation_Camera"); // 첫 번째 애니메이션 클립 이름
        StartCoroutine(PlaySecondAnimationAfter(animator1Duration));
    }

    IEnumerator PlaySecondAnimationAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 첫 번째 애니메이션 끝 → 비활성화
        animator1Object.SetActive(false);

        // 두 번째 애니메이션 시작
        animator2Object.SetActive(true);
        animator2.Play("Smoke_Simulation"); // 두 번째 애니메이션 클립 이름

        // 끝나면 비활성화
        StartCoroutine(DisableAfter(animator2Object, animator2Duration));
    }

    IEnumerator DisableAfter(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
