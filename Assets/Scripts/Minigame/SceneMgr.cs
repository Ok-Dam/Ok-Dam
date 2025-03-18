using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneMgr : MonoBehaviour
{
    private void Awake()
    {
        Shared.SceneMgr = this;

        DontDestroyOnLoad(this);
    }
}
