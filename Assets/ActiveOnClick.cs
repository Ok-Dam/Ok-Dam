using UnityEngine;

public class ActiveOnClick : MonoBehaviour
{
    // Ŭ�� �� Ȱ��ȭ�� ������Ʈ�� �ν����Ϳ��� �Ҵ��ϼ���
    public GameObject objectToActivate;

    void OnMouseDown()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}
