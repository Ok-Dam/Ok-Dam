using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotationSpeed = 0.3f;

    void Update()
    {
        // Skybox를 회전시킴
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
