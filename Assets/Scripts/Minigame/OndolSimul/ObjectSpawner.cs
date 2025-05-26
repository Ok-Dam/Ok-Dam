using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // ������ ������Ʈ ������
    public Vector3 spawnPosition = new Vector3(20, 4.196167f, -15); // ���� ��ġ
    private int spawnCount = 0; // ��ư Ŭ�� Ƚ��(������ ������Ʈ ����)

    private List<GameObject> spawnedObjects = new List<GameObject>(); // ������ ������Ʈ ���

    void Update()
    {
        // �����̽� �� ������ ��� ������ ������Ʈ�� y��ǥ�� 0���� ����
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
        // ������Ʈ ����
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        newObject.name = "SpawnedObject_" + spawnCount; // ������ �̸� ����
        spawnCount++; // ������ ���� ����

        // ����Ʈ�� �߰�
        spawnedObjects.Add(newObject);

        // ������ ������Ʈ�� �巡�� ��ũ��Ʈ �߰� (�ʿ��ϸ� ���)
        if (!newObject.GetComponent<ObjectDraggable>())
        {
            newObject.AddComponent<ObjectDraggable>();
        }
    }
}
