using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotationSpeed = 0.3f;

    void Update()
    {
        // Skybox�� ȸ����Ŵ
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
