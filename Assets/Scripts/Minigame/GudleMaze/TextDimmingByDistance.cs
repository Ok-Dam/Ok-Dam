using UnityEngine;
using UnityEngine.UI; // ← Text 컴포넌트는 여기서 옴

public class TextDimmingByDistance : MonoBehaviour
{
    public Transform cameraTransform; // 주인공 카메라
    public float maxDistance = 10f;   // 이 거리 이상이면 완전히 어두움
    private Text uiText;              // Unity UI의 기본 텍스트

    void Start()
    {
        uiText = GetComponent<Text>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, cameraTransform.position);
        float brightness = Mathf.Clamp01(1f - dist / maxDistance); // 가까울수록 밝음

        // 현재 색상 유지하면서 밝기 조절
        Color baseColor = Color.white;
        uiText.color = baseColor * brightness;
    }
}
