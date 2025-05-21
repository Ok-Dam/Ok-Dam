using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    // 마우스 버튼을 누를 때
    void OnMouseDown()
    {
        // 객체의 월드 좌표와 화면 좌표 간의 차이를 계산
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    // 마우스를 드래그할 때
    void OnMouseDrag()
    {
        // 마우스 위치를 기준으로 객체를 움직임
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;
        transform.position = currentWorldPoint;
    }
}
