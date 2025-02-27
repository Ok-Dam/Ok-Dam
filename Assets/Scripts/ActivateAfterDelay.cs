using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterDelay : MonoBehaviour
{
    private List<GameObject> prefabs = new List<GameObject>(); // 하위 프리팹 리스트
    private List<GameObject> remainingPrefabs; // 활성화할 프리팹 리스트 (랜덤 선택용)
    public float startDelay = 3f; // 첫 번째 프리팹 활성화 딜레이 (3초)
    public float interval = 4f; // 프리팹 활성화 간격 (4초)

    void Start()
    {
        // 하위 오브젝트 찾기
        foreach (Transform child in transform)
        {
            prefabs.Add(child.gameObject);
            child.gameObject.SetActive(false); // 처음엔 모두 비활성화
        }

        // 활성화할 리스트 초기화
        remainingPrefabs = new List<GameObject>(prefabs);

        // 3초 후 첫 번째 프리팹 활성화 시작
        InvokeRepeating(nameof(ActivateRandomPrefab), startDelay, interval);
    }

    void ActivateRandomPrefab()
    {
        if (remainingPrefabs.Count == 0)
        {
            CancelInvoke(nameof(ActivateRandomPrefab)); // 모든 프리팹이 활성화되면 종료
            return;
        }

        int randomIndex = Random.Range(0, remainingPrefabs.Count); // 랜덤 인덱스 선택
        GameObject selectedPrefab = remainingPrefabs[randomIndex]; // 해당 프리팹 가져오기
        remainingPrefabs.RemoveAt(randomIndex); // 리스트에서 제거
        selectedPrefab.SetActive(true); // 프리팹 활성화
    }
}
