using System.Collections;
using UnityEngine;

public class SequentialAnimator : MonoBehaviour
{
    public GameObject animator1Object;   // ù ��° �ִϸ��̼� GameObject
    public GameObject animator2Object;   // �� ��° �ִϸ��̼� GameObject
    public float animator1Duration = 5f; // ù ��° �ִϸ��̼� ��� �ð�
    public float animator2Duration = 4f; // �� ��° �ִϸ��̼� ��� �ð�

    private Animator animator1;
    private Animator animator2;

    void Start()
    {
        animator1 = animator1Object.GetComponent<Animator>();
        animator2 = animator2Object.GetComponent<Animator>();

        // ������ �� �� ��°�� ���α�
        animator2Object.SetActive(false);

        // ù ��° �ִϸ��̼� ����
        animator1Object.SetActive(true);
        animator1.Play("Simulation_Camera"); // ù ��° �ִϸ��̼� Ŭ�� �̸�
        StartCoroutine(PlaySecondAnimationAfter(animator1Duration));
    }

    IEnumerator PlaySecondAnimationAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ù ��° �ִϸ��̼� �� �� ��Ȱ��ȭ
        animator1Object.SetActive(false);

        // �� ��° �ִϸ��̼� ����
        animator2Object.SetActive(true);
        animator2.Play("Smoke_Simulation"); // �� ��° �ִϸ��̼� Ŭ�� �̸�

        // ������ ��Ȱ��ȭ
        StartCoroutine(DisableAfter(animator2Object, animator2Duration));
    }

    IEnumerator DisableAfter(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
