using System.Collections;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    private Renderer targetRenderer;
    private Material originalMaterial;
    private Coroutine blinkCoroutine;

    void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
        originalMaterial = new Material(targetRenderer.material); // 인스턴스화
        targetRenderer.material = originalMaterial;
    }

    public void StartBlink(float speed = 2f, Color? customColor = null)
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkRoutine(speed, customColor ?? highlightMaterial.color));
    }

    public void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        ResetMaterial();
    }

    private IEnumerator BlinkRoutine(float speed, Color targetColor)
    {
        float t = 0f;
        Color origColor = originalMaterial.color;
        Color highColor = targetColor;

        while (true)
        {
            t += Time.deltaTime * speed;
            float lerp = Mathf.PingPong(t, 1f);
            targetRenderer.material.color = Color.Lerp(origColor, highColor, lerp);

            if (targetRenderer.material.HasProperty("_EmissionColor"))
            {
                targetRenderer.material.SetColor("_EmissionColor",
                    Color.Lerp(Color.black, highColor, lerp) * 2f);
            }

            yield return null;
        }
    }

    private void ResetMaterial()
    {
        targetRenderer.material.CopyPropertiesFromMaterial(originalMaterial);
    }
}
