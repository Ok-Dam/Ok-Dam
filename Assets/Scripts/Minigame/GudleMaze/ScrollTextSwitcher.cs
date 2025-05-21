using TMPro;
using UnityEngine;

public class ScrollTextSwitcher : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public string ancientText = "Ondol is warm";  // 고대어 예시
    public string koreanText = "온돌은 따뜻해요";  // 해석된 텍스트

    private bool isTranslated = false;

    void Start()
    {
        textMeshPro.text = ancientText;
    }

    void OnMouseDown()
    {
        isTranslated = !isTranslated;
        textMeshPro.text = isTranslated ? koreanText : ancientText;
    }
}
