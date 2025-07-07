using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject popupUI;
    public GameObject arrowUI;

    private bool isPopupOpen = false; // ���� �˾� �����ִ��� ���� üũ
    public AudioSource audioSource;
    public AudioClip UIopenSound;
    public AudioClip dooropenSound;
    public AudioClip narration;


    public bool isLeftDoor = true; // ���� ������ ���� (������ +90, �������� -90)
    private bool isOpen = false; // �� ����,���� ����



    //FŰ�� ������ �� ȣ��ȴ�
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

        // ȿ���� ���
        if (audioSource != null && UIopenSound != null)
            {
                audioSource.PlayOneShot(UIopenSound);
                audioSource.PlayOneShot(narration);

            }
            Debug.Log(gameObject.name + " popup UI opened!");
        }
        else
        {
            Debug.LogWarning("Popup UI�� ����Ǿ� ���� �ʽ��ϴ�!");
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
            // ���� ���̸� +90��, ������ ���̸� -90�� ȸ��
            targetY = currentY + (isLeftDoor ? 90f : -90f);
        }
        else
        {
            // ���� ���� ���� ������ �������� (���翡�� �ݴ� ���� ȸ��)
            targetY = currentY + (isLeftDoor ? -90f : 90f);
        }

        Quaternion openRotation = Quaternion.Euler(0, targetY, 0);
        door.transform.rotation = openRotation;

        // �� ���� ���: ���������� ��������, ���������� �������� ����
        isOpen = !isOpen;

        if (audioSource != null && dooropenSound != null)
        {
            audioSource.PlayOneShot(dooropenSound);
        }

    }
}

