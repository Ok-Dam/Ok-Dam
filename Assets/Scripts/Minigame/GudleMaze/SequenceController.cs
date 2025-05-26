using System.Collections;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    public Animator rotationAnimator;        // 360�� ȸ�� �ִϸ�����
    public Animator mainAnimator;            // ���� ���� �ִϸ��̼�
    public GameObject[] deactivateObjects;   // ��Ȱ��ȭ�� ������Ʈ��
    public GameObject crossSection;          // �ܸ鵵 ������Ʈ
    public GameObject[] uiSequence;          // ������� ���� UI ������Ʈ��
    public GameObject cameraController;      // �þ� ȸ�� ��ũ��Ʈ�� ���� ������Ʈ

    void Start()
    {
        // �þ� ȸ�� ��Ȱ��ȭ
        DisableMouseRotation();

        // 1. 360�� ȸ�� �ִϸ��̼� ���� ����
        rotationAnimator.Play("Rotate360");

        // 2. 5�� �� �ܸ鵵 ���� �� ������Ʈ ��Ȱ��ȭ
        Invoke("ShowCrossSection", 5f);

        // 3. ���� �ִϸ��̼� ���� (5�� ��)
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(5f);
        mainAnimator.SetTrigger("StartMain"); // Animator�� Trigger �Ķ���� �ʿ�
    }

    void ShowCrossSection()
    {
        // �ܸ鵵 �����ְ�, ���ʿ��� ������Ʈ �����
        foreach (var obj in deactivateObjects)
            obj.SetActive(false);

        crossSection.SetActive(true);

        // UI ���������� �����ֱ�
        StartCoroutine(ActivateUISequence());

        // �þ� ȸ�� �ٽ� Ȱ��ȭ (�ʿ� ��)
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
