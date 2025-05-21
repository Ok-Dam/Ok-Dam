using UnityEngine;
using System.Collections.Generic;

public class OndolResetter : MonoBehaviour
{
    public GameObject parentObject;

    private class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    private Dictionary<Transform, TransformData> initialStates = new Dictionary<Transform, TransformData>();

    void Start()
    {
        if (parentObject == null)
        {
            Debug.LogError("parentObject가 지정되지 않았습니다.");
            return;
        }

        foreach (Transform child in parentObject.transform)
        {
            TransformData data = new TransformData
            {
                position = child.position,
                rotation = child.rotation,
                scale = child.localScale
            };
            initialStates[child] = data;
        }
    }

    void OnMouseDown() // 이 스크립트가 붙은 오브젝트(Cube)를 클릭하면 실행됨
    {
        foreach (var pair in initialStates)
        {
            pair.Key.position = pair.Value.position;
            pair.Key.rotation = pair.Value.rotation;
            pair.Key.localScale = pair.Value.scale;
        }

        Debug.Log("자식 오브젝트들의 위치/회전/스케일을 초기 상태로 복원했습니다.");
    }
}
