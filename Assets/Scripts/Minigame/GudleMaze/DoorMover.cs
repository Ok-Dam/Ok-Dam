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
        public string quizTag;
        public string doorTag;
        public Vector3 targetPos;
        public GameObject arrowToActivate; // null 가능
    }

    public List<DoorMovement> doorMovements = new List<DoorMovement>();

    // Inspector에서 수동으로 연결하지 않도록 자동 초기화
    void Start()
    {
        // 화살표 오브젝트 찾기 (없을 경우 null이 들어감)
        GameObject arrow1 = GameObject.Find("ARROW_1");
        GameObject arrow2 = GameObject.Find("ARROW_2");
        GameObject arrow3 = GameObject.Find("ARROW_3");

        // 미리 정의된 데이터 삽입
        doorMovements = new List<DoorMovement>
    {
        new DoorMovement { quizTag = "MIRO_QUIZ_1_CORRECT", doorTag = "CORRECT_DOOR_1", targetPos = new Vector3(-24.6f, -2.46745f, 58.6f), arrowToActivate = arrow1 },
        new DoorMovement { quizTag = "MIRO_QUIZ_2_CORRECT", doorTag = "CORRECT_DOOR_2", targetPos = new Vector3(-24.6f, -2.46745f, 53.3f), arrowToActivate = arrow2 },
        new DoorMovement { quizTag = "MIRO_QUIZ_3_CORRECT", doorTag = "CORRECT_DOOR_3", targetPos = new Vector3(-70.7f, -2.46745f, -39.1f), arrowToActivate = arrow3 },
        new DoorMovement { quizTag = "MIRO_QUIZ_4_CORRECT", doorTag = "CORRECT_DOOR_4", targetPos = new Vector3(-51.4f, -2.46745f, -54.5f), arrowToActivate = null },
        new DoorMovement { quizTag = "MIRO_QUIZ_BONUS_CORRECT", doorTag = "CORRECT_DOOR_BONUS", targetPos = new Vector3(-51f, -2.46745f, -9.8f), arrowToActivate = null },
        new DoorMovement { quizTag = "MIRO_QUIZ_5_CORRECT", doorTag = "CORRECT_DOOR_5", targetPos = new Vector3(-24.6f, -2.46745f, -64.5f), arrowToActivate = null },
        new DoorMovement { quizTag = "MIRO_QUIZ_6_CORRECT", doorTag = "CORRECT_DOOR_6", targetPos = new Vector3(-24.6f, -2.46745f, -16.4f), arrowToActivate = null },
        new DoorMovement { quizTag = "MIRO_QUIZ_7_CORRECT", doorTag = "CORRECT_DOOR_7", targetPos = new Vector3(23.1f, -2.46745f, doorZ("CORRECT_DOOR_7")), arrowToActivate = null },
        new DoorMovement { quizTag = "MIRO_QUIZ_8_CORRECT", doorTag = "CORRECT_DOOR_8", targetPos = new Vector3(39.6f, -2.46745f, doorZ("CORRECT_DOOR_8")), arrowToActivate = null },
        new DoorMovement { quizTag = "MIRO_QUIZ_9_CORRECT", doorTag = "CORRECT_DOOR_9", targetPos = new Vector3(doorX("CORRECT_DOOR_9"), -2.46745f, -14f), arrowToActivate = null },
        new DoorMovement { quizTag = "MIRO_QUIZ_10_CORRECT", doorTag = "CORRECT_DOOR_10", targetPos = new Vector3(26.7f, -2.46745f, doorZ("CORRECT_DOOR_10")), arrowToActivate = null },
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
                            StartCoroutine(MoveDoor(door, dm.targetPos, dm.arrowToActivate));
                        }
                        else
                        {
                            Debug.LogError($"'{dm.doorTag}' 태그를 가진 문을 찾을 수 없습니다.");
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

        while (elapsedTime < moveDuration)
        {
            door.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.position = endPos;
        isMoving = false;

        if (arrow != null)
            arrow.SetActive(true);
    }

    // Helper for maintaining original X or Z value when only one axis is modified
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