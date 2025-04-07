using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Info : MonoBehaviour
{
    public GameObject GOINFO;

    public void OnbtnOk()
    {
        GOINFO.SetActive(false);
    }
}
