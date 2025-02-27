using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option : MonoBehaviour
{
    public GameObject GOOPTION;

    public void OnBtnOption()
    {
        GOOPTION.SetActive(true);
    }

    public void OnBtnOptionClose()
    {
        GOOPTION.SetActive(false);
    }
}
