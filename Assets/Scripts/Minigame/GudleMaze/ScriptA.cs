using UnityEngine;

public class ScriptA : MonoBehaviour
{
    public PlayerMovement scriptB; // B ��ũ��Ʈ�� �ν����Ϳ� �Ҵ��ϼ���

    void OnEnable()
    {
        if (scriptB != null)
        {
            scriptB.enabled = false;
        }
    }

    void OnDisable()
    {
        if (scriptB != null)
        {
            scriptB.enabled = true;
        }
    }
}
