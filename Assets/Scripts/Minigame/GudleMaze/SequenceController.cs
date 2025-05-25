using System.Collections;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    public Animator rotationAnimator;        // 360도 회전 애니메이터
    public Animator mainAnimator;            // 내가 만든 애니메이션
    public GameObject[] deactivateObjects;   // 비활성화할 오브젝트들
    public GameObject crossSection;          // 단면도 오브젝트
    public GameObject[] uiSequence;          // 순서대로 나올 UI 오브젝트들
    public GameObject cameraController;      // 시야 회전 스크립트가 붙은 오브젝트

    void Start()
    {
        // 시야 회전 비활성화
        DisableMouseRotation();

        // 1. 360도 회전 애니메이션 먼저 시작
        rotationAnimator.Play("Rotate360");

        // 2. 5초 후 단면도 등장 및 오브젝트 비활성화
        Invoke("ShowCrossSection", 5f);

        // 3. 메인 애니메이션 실행 (5초 후)
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(5f);
        mainAnimator.SetTrigger("StartMain"); // Animator에 Trigger 파라미터 필요
    }

    void ShowCrossSection()
    {
        // 단면도 보여주고, 불필요한 오브젝트 숨기기
        foreach (var obj in deactivateObjects)
            obj.SetActive(false);

        crossSection.SetActive(true);

        // UI 순차적으로 보여주기
        StartCoroutine(ActivateUISequence());

        // 시야 회전 다시 활성화 (필요 시)
        EnableMouseRotation();
    }

    IEnumerator ActivateUISequence()
    {
        for (int i = 0; i < uiSequence.Length; i++)
        {
            yield return new WaitForSeconds(1f);
            uiSequence[i].SetActive(true);
        }
    }

    public void DisableMouseRotation()
    {
        if (cameraController.TryGetComponent(out PlayerMovement moveScript))
            moveScript.enabled = false;
    }

    public void EnableMouseRotation()
    {
        if (cameraController.TryGetComponent(out PlayerMovement moveScript))
            moveScript.enabled = true;
    }
}
