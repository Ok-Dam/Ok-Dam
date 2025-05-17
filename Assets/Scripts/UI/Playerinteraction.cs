using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class Playerinteraction : MonoBehaviour
{
    public float interactionDistance = 10f;
    public LayerMask interactableLayer;

    private Transform playerTransform;
    private InteractableObject currentTarget = null;
    private InteractableObject openedTarget = null; // 팝업 열린 오브젝트 기억

    private NPCUI npcUI;


    void Start()
    {
        // 플레이어가 생성된 후 자동으로 찾아서 연결
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        npcUI = FindObjectOfType<NPCUI>();
    }
    void Update()
    {
        // 혹시 플레이어가 아직 생성 안되었을 때를 대비해서 다시 시도
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                return; // 플레이어 없으면 실행 안 함
            }
        }

        // 대화 중이거나 대화창이 떠 있으면 상호작용 금지
    if (npcUI != null && (npcUI.getIsChoiceActive() || npcUI.dialoguePanel.activeSelf))
        return;


        Ray ray = new Ray(playerTransform.position + Vector3.up * 1.5f, playerTransform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            GameObject hitObj = hit.collider.gameObject;
            InteractableObject interactable = hitObj.GetComponent<InteractableObject>();
            NPCController npc = hitObj.GetComponent<NPCController>();

            if (interactable != null)
            {
                if (currentTarget != interactable)
                {
                    if (currentTarget != null)
                        currentTarget.HideArrowUI();

                    currentTarget = interactable;
                    currentTarget.ShowArrowUI();
                    Debug.Log("New target: " + currentTarget.gameObject.name);
                }

                // F키를 눌렀을 때 Tag에 따라 상호작용
                if (Input.GetKeyDown(KeyCode.F))
                {
                    //소개 ui
                    if (hitObj.CompareTag("Introduce"))
                    {
                        interactable.Interact();
                        openedTarget = interactable;
                    }
                    //문 열림
                    else if (hitObj.CompareTag("LeftDoor") || hitObj.CompareTag("RightDoor"))
                    {
                        interactable.OpenDoor(hitObj);

                    }
                    else if (hitObj.CompareTag("NPC"))
                    {
                        var npcController = hitObj.GetComponentInParent<NPCController>();
                        if (npcController != null)
                        {
                            // NPCUI를 찾고, ConnectToNPC 호출
                            if (npcUI != null)
                            {
                                npcUI.ConnectToNPC(npcController);
                            }
                            else Debug.Log("npcUI null");
                            npcController.InteractWithPlayer();
                        }
                        else Debug.Log("npcController null");
                    }
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

        // ESC 키로 팝업 닫기
        if (openedTarget != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (openedTarget.IsPopupOpen())
            {
                openedTarget.ClosePopupUI();
                openedTarget = null;
            }
        }
    }

    //private void OpenDoor(GameObject door)
    //{
    //    float currentY = door.transform.eulerAngles.y;
    //    float targetY;

    //    if (door.CompareTag("LeftDoor"))
    //    {
    //        targetY = currentY + 90f;  
    //    }
    //    else if (door.CompareTag("RightDoor"))
    //    {
    //        targetY = currentY - 90f;  
    //    }
    //    else
    //    {
    //        targetY = currentY; // 기본값
    //    }

    //    Quaternion openRotation = Quaternion.Euler(0, targetY, 0);
    //    door.transform.rotation = openRotation;

    //    Debug.Log("문 열림: " + door.name);
    //}

}