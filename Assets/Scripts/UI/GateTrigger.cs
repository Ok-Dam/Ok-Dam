using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GateTrigger : MonoBehaviour
{
    public VideoController videoController;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            videoController.PlayVideo();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }
}
