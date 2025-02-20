using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    public Animator ANI;


  public void OnBtnClose()
    {
        //튜토리얼 닫는 애니메이션 실행
        ANI.SetTrigger("close");
    }
    
  public void OnAnimationEnd()
    {
        transform.gameObject.SetActive(false);
    }
}
