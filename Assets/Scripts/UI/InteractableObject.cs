using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject popupUI; // 띄울 UI 오브젝트 연결
    public GameObject arrowUI;
    private bool isPopupOpen = false; // 현재 팝업 열려있는지 여부 체크
    public AudioSource audioSource;
    public AudioClip UIopenSound;
    public AudioClip dooropenSound;



    //F키를 눌렀을 때 호출된다
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

            // 효과음 재생
            if (audioSource != null && UIopenSound != null)
            {
                audioSource.PlayOneShot(UIopenSound);
            }
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

    public void OpenDoor(GameObject door)
    {
        float currentY = door.transform.eulerAngles.y;
        float targetY;

        if (door.CompareTag("LeftDoor"))
        {
            targetY = currentY + 90f;
        }
        else if (door.CompareTag("RightDoor"))
        {
            targetY = currentY - 90f;
        }
        else
        {
            targetY = currentY; // 기본값
        }

        Quaternion openRotation = Quaternion.Euler(0, targetY, 0);
        door.transform.rotation = openRotation;

        //문 여는 소리 재생
        if (audioSource != null && dooropenSound != null)
        {
            audioSource.PlayOneShot(dooropenSound);
        }

        Debug.Log("문 열림: " + door.name);
    }
}
