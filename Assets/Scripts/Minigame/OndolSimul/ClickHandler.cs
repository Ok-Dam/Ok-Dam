using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    [Tooltip("클릭을 감지할 3D 오브젝트의 태그")]
    public string targetObjectTag = "GudleJang";

    [Tooltip("활성화할 Canvas의 태그")]
    public string canvasTag = "GudleJang";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag(targetObjectTag))
                {
                    Debug.Log("Pressed T while pointing at: " + hit.collider.name);

                    GameObject[] canvases = GameObject.FindGameObjectsWithTag(canvasTag);
                    foreach (GameObject canvasObj in canvases)
                    {
                        canvasObj.SetActive(true); // Canvas 전체 GameObject 활성화
                    }
                }
            }
        }
    }
}
