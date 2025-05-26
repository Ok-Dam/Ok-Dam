using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public Vector3 spawnPosition = new Vector3(20, 4.196167f, -15); // 생성 위치
    private int spawnCount = 0; // 버튼 클릭 횟수(생성된 오브젝트 개수)

    private List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 오브젝트 목록

    void Update()
    {
        // 스페이스 바 누르면 모든 생성된 오브젝트의 y좌표를 0으로 설정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null)
                {
                    Vector3 pos = obj.transform.position;
                    pos.y = 0f;
                    obj.transform.position = pos;
                }
            }
        }
    }

    public void SpawnObject()
    {
        // 오브젝트 생성
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        newObject.name = "SpawnedObject_" + spawnCount; // 고유한 이름 설정
        spawnCount++; // 생성된 개수 증가

        // 리스트에 추가
        spawnedObjects.Add(newObject);

        // 생성된 오브젝트에 드래그 스크립트 추가 (필요하면 사용)
        if (!newObject.GetComponent<ObjectDraggable>())
        {
            newObject.AddComponent<ObjectDraggable>();
        }
    }
}
