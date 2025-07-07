using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject popupUI;
    public GameObject arrowUI;

    private bool isPopupOpen = false; // 현재 팝업 열려있는지 여부 체크
    public AudioSource audioSource;
    public AudioClip UIopenSound;
    public AudioClip dooropenSound;
    public AudioClip narration;


    public bool isLeftDoor = true; // 왼쪽 문인지 여부 (왼쪽은 +90, 오른쪽은 -90)
    private bool isOpen = false; // 문 열림,닫힘 상태



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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        // 효과음 재생
        if (audioSource != null && UIopenSound != null)
            {
                audioSource.PlayOneShot(UIopenSound);
                audioSource.PlayOneShot(narration);

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
            audioSource.Stop();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        Debug.Log(gameObject.name + " popup UI closed!");
        }
    }

    public void OnCloseButtonClick()
    {
        ClosePopupUI();
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

        if (!isOpen)
        {
            // 왼쪽 문이면 +90도, 오른쪽 문이면 -90도 회전
            targetY = currentY + (isLeftDoor ? 90f : -90f);
        }
        else
        {
            // 닫을 때는 원래 각도로 돌려놓기 (현재에서 반대 방향 회전)
            targetY = currentY + (isLeftDoor ? -90f : 90f);
        }

        Quaternion openRotation = Quaternion.Euler(0, targetY, 0);
        door.transform.rotation = openRotation;

        // 문 상태 토글: 열려있으면 닫힘으로, 닫혀있으면 열림으로 변경
        isOpen = !isOpen;

        if (audioSource != null && dooropenSound != null)
        {
            audioSource.PlayOneShot(dooropenSound);
        }

    }
}

