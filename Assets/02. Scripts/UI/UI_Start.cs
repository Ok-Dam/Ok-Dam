using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Start : MonoBehaviour
{
    public GameObject GOTUTORIAL;

    public void OnBtnStart()
    {
        SceneManager.LoadScene("MapScene",LoadSceneMode.Single);
    }
    public void OnBtnTutorial()
    {
        //Ʃ�丮�� â ����
        GOTUTORIAL.SetActive(true);
    }
}
