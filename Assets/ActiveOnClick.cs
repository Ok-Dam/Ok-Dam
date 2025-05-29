using UnityEngine;

public class ActiveOnClick : MonoBehaviour
{
    // 클릭 시 활성화할 오브젝트를 인스펙터에서 할당하세요
    public GameObject objectToActivate;

    void OnMouseDown()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}
