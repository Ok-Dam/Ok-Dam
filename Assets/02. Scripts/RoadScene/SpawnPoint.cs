using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string key = "Default";
    public string Key => key;

#if UNITY_EDITOR
    // 에디터에서 키 라벨 표시(선택)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.25f, key);
    }
#endif
}
