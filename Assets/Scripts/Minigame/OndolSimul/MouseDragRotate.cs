using UnityEngine;

public class MouseDragRotate : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private bool isDragging = false;

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotY = -Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.World);
        }
    }
}
