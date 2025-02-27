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
        //mapscene으로 이동
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_MAPSCENE);
    }
    public void OnBtnTutorial()
    {
        //튜토리얼 창 생성
        GOTUTORIAL.SetActive(true);
    }
}
