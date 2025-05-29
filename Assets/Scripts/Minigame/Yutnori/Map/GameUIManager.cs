using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnCountText;

    [Header("윷 결과 동적 이미지 UI")]
    [SerializeField] private GameObject yutResultImagePrefab; // 반드시 Image+Button 컴포넌트 포함
    [SerializeField] private Transform yutResultImageContainer; // Layout Group 붙은 오브젝트

    [Header("도/개/걸/윷/모/빽도 Sprite (순서대로 0~5)")]
    [SerializeField] private Sprite[] yutResultSprites; // 0:도, 1:개, 2:걸, 3:윷, 4:모, 5:빽도

    private List<GameObject> spawnedYutResultImages = new List<GameObject>();
    private int selectedIndex = -1;

    // GameManager 참조 (Inspector에서 할당 또는 Start에서 FindObjectOfType로 할당)
    [SerializeField] private YutnoriGameManager gameManager;

    public void UpdateTurn(int turnCount)
    {
        turnCountText.text = $"턴: {turnCount}";
    }

    /// <summary>
    /// 윷 결과 인덱스 리스트(0:도, 1:개, 2:걸, 3:윷, 4:모, 5:빽도)를 받아 동적으로 UI에 표시
    /// </summary>
    public void ShowYutResults(List<int> resultIndices)
    {
        ClearYutResults();
        for (int i = 0; i < resultIndices.Count; i++)
        {
            int idx = resultIndices[i];
            if (idx < 0 || idx >= yutResultSprites.Length)
                continue;

            GameObject imgObj = Instantiate(yutResultImagePrefab, yutResultImageContainer);
            Image img = imgObj.GetComponent<Image>();
            img.sprite = yutResultSprites[idx];
            spawnedYutResultImages.Add(imgObj);

            // Button 클릭 이벤트 연결
            Button btn = imgObj.GetComponent<Button>();
            int capturedIndex = i; // 클로저 문제 방지
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnYutResultImageClicked(capturedIndex));
            }
            else
            {
                Debug.LogWarning($"No Button component on prefab: {imgObj.name}");
            }
        }
        selectedIndex = -1; // 새로 띄울 때 선택 해제
    }

    /// <summary>
    /// 윷 결과 UI 이미지 모두 제거
    /// </summary>
    public void ClearYutResults()
    {
        foreach (var obj in spawnedYutResultImages)
            Destroy(obj);
        spawnedYutResultImages.Clear();
        selectedIndex = -1;
    }

    /// <summary>
    /// 패 이미지 클릭 시 호출 (idx: 클릭한 패의 인덱스)
    /// </summary>
    public void OnYutResultImageClicked(int idx)
    {
        selectedIndex = idx;
        // 하이라이트 효과
        for (int i = 0; i < spawnedYutResultImages.Count; i++)
        {
            Image img = spawnedYutResultImages[i].GetComponent<Image>();
            img.color = (i == idx) ? new Color(0.9f, 0.9f, 0.9f, 0.4f) : Color.white;
        }

        // GameManager에 선택된 패 인덱스 전달 (필요시)
        if (gameManager != null)
            gameManager.OnYutResultSelected(idx);
    }

    public void DeleteYutResultImage(int idx)
    {
        if (idx >= 0 && idx < spawnedYutResultImages.Count)
        {
            Destroy(spawnedYutResultImages[idx]);
            spawnedYutResultImages.RemoveAt(idx);
        }
    }


    // 추후 나가기 버튼 등도 여기에 추가
}
