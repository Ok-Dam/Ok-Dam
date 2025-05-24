using System.Collections;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    public Animator rotationAnimator;        // 360도 회전 애니메이터
    public Animator mainAnimator;            // 내가 만든 애니메이션
    public GameObject[] deactivateObjects;   // 비활성화할 오브젝트들
    public GameObject crossSection;          // 단면도 오브젝트
    public GameObject[] uiSequence;          // 순서대로 나올 UI 오브젝트들

    void Start()
    {
        // 1. 360도 회전 애니메이션 먼저 시작
        rotationAnimator.Play("Rotate360");

        // 2. 5초 후 단면도 등장 및 오브젝트 비활성화
        Invoke("ShowCrossSection", 5f);
    }

    void ShowCrossSection()
    {
        foreach (var obj in deactivateObjects)
            obj.SetActive(false);

        crossSection.SetActive(true);

        // 3. 메인 애니메이션 실행
        mainAnimator.SetTrigger("StartMain"); // Animator에 trigger 매개변수 필요

        // 4. UI 차례로 나타나기
        StartCoroutine(ActivateUISequence());
    }

    IEnumerator ActivateUISequence()
    {
        for (int i = 0; i < uiSequence.Length; i++)
        {
            yield return new WaitForSeconds(1f); // 1초 간격 (원하면 조절 가능)
            uiSequence[i].SetActive(true);
        }
    }
}
