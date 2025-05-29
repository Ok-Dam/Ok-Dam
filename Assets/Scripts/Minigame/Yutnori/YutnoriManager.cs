using UnityEngine;

public class YutnoriManager : MonoBehaviour
{
    public GameObject yutnoriRootPrefab; // ������ ��Ʈ ������
    private GameObject yutnoriRootInstance; // ���� �ν��Ͻ�

    public void StartYutnori()
    {
        if (yutnoriRootInstance != null)
            Destroy(yutnoriRootInstance);

        yutnoriRootInstance = Instantiate(yutnoriRootPrefab);
    }

    public void EndYutnori()
    {
        if (yutnoriRootInstance != null)
        {
            Destroy(yutnoriRootInstance);
            yutnoriRootInstance = null;
        }
    }
}
