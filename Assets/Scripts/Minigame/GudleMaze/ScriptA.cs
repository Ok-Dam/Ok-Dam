using UnityEngine;

public class ScriptA : MonoBehaviour
{
    public PlayerMovement scriptB; // B 스크립트를 인스펙터에 할당하세요

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
