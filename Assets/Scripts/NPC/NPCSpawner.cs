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

    public List<NPCPrefabInfo> npcPrefabs; // Inspector에서 여러 프리팹 등록
    public float spawnInterval = 15f;      // 스폰 시도 간격

    void Start()
    {
        InvokeRepeating(nameof(TrySpawnNPCs), 0f, spawnInterval);
    }

    // Random 안 넣으면 순서대로 골라져서 npc가 나가자마자 다시 들어옴
    // LINQ : 컬렉션에서 원하는 조건에 맞는 요소만 쿼리문(Where, Select 등)으로 추출하고, ToList() 등으로 새로운 리스트로 만들어주는 C#의 기능
    // 핵심 메서드로 Where() 등 있다
    void TrySpawnNPCs()
    {
        // 1. 아직 스폰되지 않은 NPC만 리스트로 만듦
        var available = npcPrefabs.Where(npc => npc.instance == null).ToList();

        // 2. 스폰할 NPC가 있다면 랜덤하게 하나 선택
        if (available.Count > 0)
        {
            var npcInfo = available[Random.Range(0, available.Count)];
            npcInfo.instance = Instantiate(npcInfo.prefab, transform.position, Quaternion.identity);
            npcInfo.instance.GetComponent<NPCWalker>().Init(this, transform.position);
        }
    }

    // NPC가 사라질 때 호출
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
