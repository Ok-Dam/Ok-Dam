using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StampManager : MonoBehaviour
{
    public GameObject stampPrefab;      // 도장 이미지 프리팹 (Image + Text)
    public Transform canvasTransform;   // 도장이 올라갈 캔버스 (Screen Space Canvas)
    public Vector2 stampPosition = new Vector2(0, 100); // 도장 위치 (Screen Space 기준)
    public Vector2 stampSize = new Vector2(300, 300);   // 도장 크기 (Width, Height)
    public int textFontSize = 40;       // 텍스트 폰트 크기 (int)
    public Color textColor = Color.black; // 텍스트 색상

    void Start()
    {
        StartCoroutine(PlayStampEffect());
    }

    IEnumerator PlayStampEffect()
    {
        GameObject stamp = Instantiate(stampPrefab, canvasTransform);
        RectTransform rt = stamp.GetComponent<RectTransform>();

        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = stampPosition + new Vector2(0, 500f); // 위에서 시작
        rt.sizeDelta = stampSize;
        rt.localScale = Vector3.one * 3f;  // 크게 시작

        Transform textTransform = stamp.transform.Find("Text");
        if (textTransform == null)
        {
            Debug.LogError("자식 오브젝트에 'Text'가 없습니다. 이름 확인해주세요.");
            yield break;
        }

        Text uiText = textTransform.GetComponent<Text>();
        if (uiText == null)
        {
            Debug.LogError("Text 오브젝트에 Text 컴포넌트가 없습니다.");
            yield break;
        }

        // 텍스트 초기엔 비활성화
        uiText.enabled = false;

        uiText.fontSize = textFontSize;
        uiText.color = textColor;

        float duration = 0.5f;
        float time = 0f;

        Vector2 startPos = rt.anchoredPosition;
        Vector2 endPos = stampPosition;

        Vector3 startScale = rt.localScale;
        Vector3 endScale = Vector3.one;

        while (time < duration)
        {
            float t = time / duration;
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, EaseOutCubic(t));
            rt.localScale = Vector3.Lerp(startScale, endScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = endPos;
        rt.localScale = endScale;

        // 도장이 내려온 후에 텍스트 활성화
        uiText.enabled = true;
    }

    float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3);
    }
}
