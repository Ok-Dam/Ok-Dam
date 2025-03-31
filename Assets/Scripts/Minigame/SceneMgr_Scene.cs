using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SceneMgr : MonoBehaviour
{
    public eSCENE Scene;

    public void ChangeScene(eSCENE _e)
    {
        if (Scene == _e)
            return;

        Scene = _e;

        SceneManager.LoadScene((int)Scene);
    }
}
