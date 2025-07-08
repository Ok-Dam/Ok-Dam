using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NPCSpawner : MonoBehaviour
{
    [System.Serializable]
    public class NPCPrefabInfo
    {
        public GameObject prefab;
        [HideInInspector] public GameObject instance;
    }

    public List<NPCPrefabInfo> npcPrefabs; // Inspector���� ���� ������ ���
    public float spawnInterval = 15f;      // ���� �õ� ����

    void Start()
    {
        InvokeRepeating(nameof(TrySpawnNPCs), 0f, spawnInterval);
    }

    // Random �� ������ ������� ������� npc�� �����ڸ��� �ٽ� ����
    // LINQ : �÷��ǿ��� ���ϴ� ���ǿ� �´� ��Ҹ� ������(Where, Select ��)���� �����ϰ�, ToList() ������ ���ο� ����Ʈ�� ������ִ� C#�� ���
    // �ٽ� �޼���� Where() �� �ִ�
    void TrySpawnNPCs()
    {
        // 1. ���� �������� ���� NPC�� ����Ʈ�� ����
        var available = npcPrefabs.Where(npc => npc.instance == null).ToList();

        // 2. ������ NPC�� �ִٸ� �����ϰ� �ϳ� ����
        if (available.Count > 0)
        {
            var npcInfo = available[Random.Range(0, available.Count)];
            npcInfo.instance = Instantiate(npcInfo.prefab, transform.position, Quaternion.identity);
            npcInfo.instance.GetComponent<NPCWalker>().Init(this, transform.position);
        }
    }

    // NPC�� ����� �� ȣ��
    public void OnNPCDespawn(GameObject npc)
    {
        foreach (var npcInfo in npcPrefabs)
        {
            if (npcInfo.instance == npc)
            {
                npcInfo.instance = null;
                break;
            }
        }
    }
}
