using UnityEngine;

public class FlashlightRevealMulti : MonoBehaviour
{
    [System.Serializable]
    public class RevealTarget
    {
        public GameObject targetObject; // 보일 Canvas
        public Transform targetTransform; // 위치 기준 (보통 targetObject.transform)
    }

    public Transform flashlight; // Spot Light (손전등)
    public float maxDistance = 10f; // 최대 거리
    public float angleThreshold = 20f; // 손전등 각도 허용 범위
    public RevealTarget[] targets; // 여러 개 대상

    void Update()
    {
        foreach (var target in targets)
        {
            Vector3 dirToTarget = target.targetTransform.position - flashlight.position;
            float angle = Vector3.Angle(flashlight.forward, dirToTarget);
            float distance = dirToTarget.magnitude;

            bool isVisible = angle < angleThreshold && distance < maxDistance;
            target.targetObject.SetActive(isVisible);
        }
    }
}
