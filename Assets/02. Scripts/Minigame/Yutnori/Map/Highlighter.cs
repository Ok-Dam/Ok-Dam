using System.Collections;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial; // 커스텀 머티리얼 할당
    private Renderer targetRenderer;
    private Material originalMaterial;
    private Coroutine blinkCoroutine;

    void Awake()
    {
        targetRenderer = GetComponentInChildren<Renderer>();

        // 원본 머티리얼 백업 (커스텀 셰이더 머티리얼로 초기화)
        originalMaterial = new Material(targetRenderer.material);
        targetRenderer.material = originalMaterial;
    }

    public void StartBlink(float speed = 2f, Color? customColor = null)
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkRoutine(speed, customColor ?? highlightMaterial.color));
    }

    public void StopBlink()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        ResetMaterial();
    }

    private IEnumerator BlinkRoutine(float speed, Color targetColor)
    {
        float t = 0f;
        Color origColor = originalMaterial.color;

        while (true)
        {
            t += Time.deltaTime * speed;
            float lerp = Mathf.PingPong(t, 1f);

            // 머티리얼 color만 변경 (SpriteRenderer/MeshRenderer 공통 처리)
            targetRenderer.material.color = Color.Lerp(origColor, targetColor, lerp);

            yield return null;
        }
    }

    private void ResetMaterial()
    {
        targetRenderer.material.CopyPropertiesFromMaterial(originalMaterial);
    }
}
