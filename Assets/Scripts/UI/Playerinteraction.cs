using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class Playerinteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    private Transform playerTransform;
    private InteractableObject currentTarget = null;
    private InteractableObject openedTarget = null; // �˾� ���� ������Ʈ ���

    private NPCUI npcUI;


    void Start()
    {
        // �÷��̾ ������ �� �ڵ����� ã�Ƽ� ����
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        npcUI = FindObjectOfType<NPCUI>();
    }
    void Update()
    {
        // Ȥ�� �÷��̾ ���� ���� �ȵǾ��� ���� ����ؼ� �ٽ� �õ�
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                return; // �÷��̾� ������ ���� �� ��
            }
        }

        // ��ȭ ���̰ų� ��ȭâ�� �� ������ ��ȣ�ۿ� ����
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

                // FŰ�� ������ �� Tag�� ���� ��ȣ�ۿ�
                if (Input.GetKeyDown(KeyCode.F))
                {
                    //�Ұ� ui
                    if (hitObj.CompareTag("Introduce"))
                    {
                        interactable.Interact();
                        openedTarget = interactable;
                    }
                    //�� ����
                    else if (hitObj.CompareTag("LeftDoor") || hitObj.CompareTag("RightDoor"))
                    {
                        interactable.OpenDoor(hitObj);

                    }
                    else if (hitObj.CompareTag("NPC"))
                    {
                        var npcController = hitObj.GetComponentInParent<NPCController>();
                        if (npcController != null)
                        {
                            // NPCUI�� ã��, ConnectToNPC ȣ��
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

        // ESC Ű�� �˾� �ݱ�
        //if (openedTarget != null && Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (openedTarget.IsPopupOpen())
        //    {
        //        openedTarget.ClosePopupUI();
        //        openedTarget = null;
        //    }
        //}
    }

    
}