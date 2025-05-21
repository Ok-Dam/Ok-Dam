using UnityEngine;

public class MultiDoorController : MonoBehaviour
{
    private void OnMouseDown()
    
        {
            switch (gameObject.tag)
            {
                case "MIRO_QUIZ_1_CORRECT":
                    MoveDoor("CORRECT_DOOR_1", newZ: 58.6f);
                    ActivateArrow("ARROW_1");
                    break;
                case "MIRO_QUIZ_2_CORRECT":
                    MoveDoor("CORRECT_DOOR_2", newZ: 53.3f);
                    ActivateArrow("ARROW_2");
                    break;
                case "MIRO_QUIZ_3_CORRECT":
                    MoveDoor("CORRECT_DOOR_3", newX: -70.7f, newZ: -39.1f);
                    ActivateArrow("ARROW_3");
                    break;
                case "MIRO_QUIZ_4_CORRECT":
                    MoveDoor("CORRECT_DOOR_4", newX: -51.4f, newZ: -54.5f);
                    break;
                case "MIRO_QUIZ_BONUS_CORRECT":
                    MoveDoor("CORRECT_DOOR_BONUS", newX: -51f, newZ: -9.8f);
                    break;
                case "MIRO_QUIZ_5_CORRECT":
                    MoveDoor("CORRECT_DOOR_5", newZ: -64.5f);
                    break;
                case "MIRO_QUIZ_6_CORRECT":
                    MoveDoor("CORRECT_DOOR_6", newZ: -16.4f);
                    break;
                case "MIRO_QUIZ_7_CORRECT":
                    MoveDoor("CORRECT_DOOR_7", newX: 23.1f);
                    break;
                case "MIRO_QUIZ_8_CORRECT":
                    MoveDoor("CORRECT_DOOR_8", newX: 39.6f);
                    break;
                case "MIRO_QUIZ_9_CORRECT":
                    MoveDoor("CORRECT_DOOR_9", newZ: -14f);
                    break;
                case "MIRO_QUIZ_10_CORRECT":
                    MoveDoor("CORRECT_DOOR_10", newX: 26.7f);
                    break;
            }
        }

        void MoveDoor(string tag, float? newX = null, float? newY = null, float? newZ = null)
        {
            GameObject door = GameObject.FindWithTag(tag);
            if (door != null)
            {
                Vector3 pos = door.transform.position;
                pos.x = newX ?? pos.x;
                pos.y = newY ?? pos.y;
                pos.z = newZ ?? pos.z;
                door.transform.position = pos;
            }
            else
            {
                Debug.LogWarning("문 태그를 찾을 수 없습니다: " + tag);
            }
        }

        void ActivateArrow(string arrowName)
        {
            GameObject arrow = GameObject.Find(arrowName);
            if (arrow != null)
            {
                arrow.SetActive(true);
            }
            else
            {
                Debug.LogWarning("화살표 오브젝트를 찾을 수 없습니다: " + arrowName);
            }
        }
    }