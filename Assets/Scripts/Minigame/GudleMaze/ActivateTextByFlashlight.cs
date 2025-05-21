using UnityEngine;

public class ActivateTextByFlashlight : MonoBehaviour
{
    public Light flashlight; // Spot Light
    public GameObject textObject; // 활성화할 텍스트 오브젝트
    public float maxDistance = 10f;

    void Update()
    {
        if (flashlight.enabled)
        {
            Ray ray = new Ray(flashlight.transform.position, flashlight.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                if (hit.collider.CompareTag("TargetText")) // 텍스트에 이 태그 붙이기
                {
                    textObject.SetActive(true);
                    return;
                }
            }
        }

        // 빛이 닿지 않으면 비활성화
        textObject.SetActive(false);
    }
}
