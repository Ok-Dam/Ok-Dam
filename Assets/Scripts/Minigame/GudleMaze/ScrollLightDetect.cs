using TMPro;
using UnityEngine;

public class ScrollLightDetect : MonoBehaviour
{
    public TextMeshPro textMeshPro;

    void Start()
    {
        textMeshPro.gameObject.SetActive(false);  // 기본은 안 보이게
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flashlight"))  // 손전등에 Tag 설정 필수!
        {
            textMeshPro.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Flashlight"))
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }
}
