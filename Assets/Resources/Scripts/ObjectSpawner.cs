using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public Vector3 spawnPosition = new Vector3(25, 0, 17); // 생성 위치
    private int spawnCount = 0; // 버튼 클릭 횟수(생성된 오브젝트 개수)

    public void SpawnObject()
    {
        // 오브젝트 생성
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        newObject.name = "SpawnedObject_" + spawnCount; // 고유한 이름 설정
        spawnCount++; // 생성된 개수 증가

        // 생성된 오브젝트에 드래그 스크립트 추가 (필요하면 사용)
        if (!newObject.GetComponent<ObjectDraggable>())
        {
            newObject.AddComponent<ObjectDraggable>();
        }
    }
}
