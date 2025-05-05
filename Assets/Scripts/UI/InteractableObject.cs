using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject popupUI; // 띄울 UI 오브젝트 연결
    public GameObject arrowUI;
    private bool isPopupOpen = false; // 현재 팝업 열려있는지 여부 체크

    // 이건 F키를 눌렀을 때 호출된다
    public void Interact()
    {
        ShowPopupUI();
    }

    public void ShowPopupUI()
    {
        if (popupUI != null)
        {
            popupUI.SetActive(true);
            isPopupOpen = true;
            Debug.Log(gameObject.name + " popup UI opened!");
        }
        else
        {
            Debug.LogWarning("Popup UI가 연결되어 있지 않습니다!");
        }
    }

    public void ClosePopupUI()
    {
        if (popupUI != null)
        {
            popupUI.SetActive(false);
            isPopupOpen = false;
            Debug.Log(gameObject.name + " popup UI closed!");
        }
    }

    public bool IsPopupOpen()
    {
        return isPopupOpen;
    }

    public void ShowArrowUI()
    {
        if (arrowUI != null)
        {
            arrowUI.SetActive(true);
        }
    }

    public void HideArrowUI()
    {
        if (arrowUI != null)
        {
            arrowUI.SetActive(false);
        }
    }
}
