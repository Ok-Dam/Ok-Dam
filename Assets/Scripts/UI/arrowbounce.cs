using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowbounce : MonoBehaviour
{
    public float bounceHeight = 0.3f;
    public float bounceSpeed = 8f;
    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = initialPos + new Vector3(0, newY, 0);
    }
}
