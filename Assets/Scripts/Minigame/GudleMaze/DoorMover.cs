using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DoorMover : MonoBehaviour
{
    public float moveDuration = 1.5f;
    private bool isMoving = false;

    [System.Serializable]
    public class DoorMovement
    {
        public string quizTag;        // ���� ������Ʈ �±�
        public string doorTag;        // ������ �� �±�
        public Vector3 targetPos;     // �̵��� ��ġ
        public string arrowTag;       // ȭ��ǥ ������Ʈ�� �±�
    }

    public List<DoorMovement> doorMovements = new List<DoorMovement>();

    void Start()
    {
        doorMovements = new List<DoorMovement>
        {
            new DoorMovement { quizTag = "MIRO_QUIZ_1_CORRECT", doorTag = "CORRECT_DOOR_1", targetPos = new Vector3(-24.6f, -2.46745f, 58.6f), arrowTag = "ARROW_1" },
            new DoorMovement { quizTag = "MIRO_QUIZ_2_CORRECT", doorTag = "CORRECT_DOOR_2", targetPos = new Vector3(-24.6f, -2.46745f, 53.3f), arrowTag = "ARROW_2" },
            new DoorMovement { quizTag = "MIRO_QUIZ_3_CORRECT", doorTag = "CORRECT_DOOR_3", targetPos = new Vector3(-70.7f, -2.46745f, -39.1f), arrowTag = "ARROW_3" },
            new DoorMovement { quizTag = "MIRO_QUIZ_4_CORRECT", doorTag = "CORRECT_DOOR_4", targetPos = new Vector3(-51.4f, -2.46745f, -54.5f), arrowTag = "ARROW_4" },
            new DoorMovement { quizTag = "MIRO_QUIZ_BONUS_CORRECT", doorTag = "CORRECT_DOOR_BONUS", targetPos = new Vector3(-51f, -2.46745f, -9.8f), arrowTag = "ARROW_11" },
            new DoorMovement { quizTag = "MIRO_QUIZ_5_CORRECT", doorTag = "CORRECT_DOOR_5", targetPos = new Vector3(-24.6f, -2.46745f, -64.5f), arrowTag = "ARROW_5" },
            new DoorMovement { quizTag = "MIRO_QUIZ_6_CORRECT", doorTag = "CORRECT_DOOR_6", targetPos = new Vector3(-24.6f, -2.46745f, -16.4f), arrowTag = "ARROW_6" },
            new DoorMovement { quizTag = "MIRO_QUIZ_7_CORRECT", doorTag = "CORRECT_DOOR_7", targetPos = new Vector3(23.1f, -2.46745f, doorZ("CORRECT_DOOR_7")), arrowTag = "ARROW_7" },
            new DoorMovement { quizTag = "MIRO_QUIZ_8_CORRECT", doorTag = "CORRECT_DOOR_8", targetPos = new Vector3(39.6f, -2.46745f, doorZ("CORRECT_DOOR_8")), arrowTag = "ARROW_8" },
            new DoorMovement { quizTag = "MIRO_QUIZ_9_CORRECT", doorTag = "CORRECT_DOOR_9", targetPos = new Vector3(doorX("CORRECT_DOOR_9"), -2.46745f, -14f), arrowTag = "ARROW_9" },
            new DoorMovement { quizTag = "MIRO_QUIZ_10_CORRECT", doorTag = "CORRECT_DOOR_10", targetPos = new Vector3(26.7f, -2.46745f, doorZ("CORRECT_DOOR_10")), arrowTag = "ARROW_10" },
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                foreach (DoorMovement dm in doorMovements)
                {
                    if (hit.collider.CompareTag(dm.quizTag))
                    {
                        GameObject door = GameObject.FindWithTag(dm.doorTag);
                        if (door != null)
                        {
                            GameObject arrow = GameObject.FindWithTag(dm.arrowTag);
                            StartCoroutine(MoveDoor(door, dm.targetPos, arrow));
                        }
                        else
                        {
                            Debug.LogError($"'{dm.doorTag}' �±׸� ���� ���� ã�� �� �����ϴ�.");
                        }
                        break;
                    }
                }
            }
        }
    }

    IEnumerator MoveDoor(GameObject door, Vector3 endPos, GameObject arrow)
    {
        isMoving = true;
        Vector3 startPos = door.transform.position;
        float elapsedTime = 0f;

        // �� ���� �� �Ҹ� ���
        AudioSource audio = door.GetComponent<AudioSource>();
        if (audio != null) audio.Play();

        // �ڽ� ������Ʈ Ȱ��ȭ
        foreach (Transform child in door.transform)
        {
            child.gameObject.SetActive(true);
        }

        // �� �̵�
        while (elapsedTime < moveDuration)
        {
            door.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.position = endPos;
        isMoving = false;

        // ȭ��ǥ Ȱ��ȭ
        if (arrow != null)
        {
            arrow.SetActive(true);
            Debug.Log("ȭ��ǥ Ȱ��ȭ��: " + arrow.name);
        }
        else
        {
            Debug.LogWarning("ȭ��ǥ �±׷� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    float doorX(string tag)
    {
        GameObject d = GameObject.FindWithTag(tag);
        return d != null ? d.transform.position.x : 0f;
    }

    float doorZ(string tag)
    {
        GameObject d = GameObject.FindWithTag(tag);
        return d != null ? d.transform.position.z : 0f;
    }
}
