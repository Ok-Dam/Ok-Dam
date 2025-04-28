using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Info : MonoBehaviour
{
    public GameObject GOINFO;

    void Update()
    {
        if (GOINFO.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            GOINFO.SetActive(false);
        }
    }
}
