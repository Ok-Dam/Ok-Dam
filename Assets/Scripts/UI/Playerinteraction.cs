using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinteraction : MonoBehaviour
{
    public float interactionDistance = 10f;
    public LayerMask interactableLayer;
    public Camera playerCamera;

    private InteractableObject currentTarget = null;
    private InteractableObject openedTarget = null; //팝업 열린 오브젝트 기억

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                if (currentTarget != interactable)
                {
                    
                    if (currentTarget != null)
                    {
                        currentTarget.HideArrowUI();
                    }

                    currentTarget = interactable;
                    currentTarget.ShowArrowUI();

                    Debug.Log("New target: " + currentTarget.gameObject.name);
                }
            }
        }
        else
        {
            if (currentTarget != null)
            {
                currentTarget.HideArrowUI();
                Debug.Log("Lost target: " + currentTarget.gameObject.name);
                currentTarget = null;
            }
        }

        // F키를 누르면 설명 UI 열기
        if (currentTarget != null && Input.GetKeyDown(KeyCode.F))
        {
            currentTarget.Interact();
            openedTarget = currentTarget; //팝업 열린 오브젝트 기억
        }

        //ESC 키를 누르면 팝업 닫기
        if (openedTarget != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (openedTarget.IsPopupOpen())
            {
                openedTarget.ClosePopupUI();
                openedTarget = null; // 닫았으면 초기화
            }
        }
    }
}
