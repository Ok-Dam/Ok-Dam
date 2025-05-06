using UnityEngine;

public class ChildYutController : MonoBehaviour
{
    [SerializeField] private YutController yutController;

    void OnMouseDown() => yutController.StartDrag();
    void OnMouseUp() => yutController.EndDrag();
}