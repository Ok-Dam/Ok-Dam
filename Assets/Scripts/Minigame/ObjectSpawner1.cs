using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner1 : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public Vector3 spawnPosition = new Vector3(25, 0, 17); // 생성 위치
    public Vector3 spawnRotation = new Vector3(216.556f, 175.347f, 51.504f); // 생성 회전값
    private int spawnCount = 0; // 버튼 클릭 횟수(생성된 오브젝트 개수)
    private GameObject spawnedObject; // 생성된 오브젝트 저장

    public void SpawnObject()
    {
        spawnCount++; // 버튼 누를 때마다 증가

        if (spawnedObject == null) // 처음 생성할 경우
        {
            spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.Euler(spawnRotation));
            spawnedObject.name = "SpawnedObject_" + spawnCount;

            // 드래그 스크립트 추가
            if (!spawnedObject.GetComponent<ObjectDraggable>())
            {
                spawnedObject.AddComponent<ObjectDraggable>();
            }
        }

        // 버튼을 짝수 번 누르면 비활성화, 홀수 번 누르면 활성화
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(spawnCount % 2 != 0);
        }
    }
}
