using UnityEngine;

public class DragXZ : MonoBehaviour
{
    private Plane dragPlane;
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        // 객체가 있는 위치 기준으로 평면 설정 (Y 고정 → XZ 평면)
        dragPlane = new Plane(Vector3.up, transform.position);

        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        float enter;

        if (dragPlane.Raycast(camRay, out enter))
        {
            offset = transform.position - camRay.GetPoint(enter);
        }
    }

    void OnMouseDrag()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        float enter;

        if (dragPlane.Raycast(camRay, out enter))
        {
            Vector3 point = camRay.GetPoint(enter);
            transform.position = point + offset;
        }
    }
}
