using UnityEngine;

public class BubbleMotion : MonoBehaviour
{
    public float scaleAmount = 0.05f;
    public float speed = 2f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = originalScale + new Vector3(scaleFactor, scaleFactor, 0);
    }
}
