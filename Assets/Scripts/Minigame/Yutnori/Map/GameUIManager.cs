using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnCountText;

    [Header("�� ��� ���� �̹��� UI")]
    [SerializeField] private GameObject yutResultImagePrefab; // �ݵ�� Image+Button ������Ʈ ����
    [SerializeField] private Transform yutResultImageContainer; // Layout Group ���� ������Ʈ

    [Header("��/��/��/��/��/���� Sprite (������� 0~5)")]
    [SerializeField] private Sprite[] yutResultSprites; // 0:��, 1:��, 2:��, 3:��, 4:��, 5:����

    private List<GameObject> spawnedYutResultImages = new List<GameObject>();
    private int selectedIndex = -1;

    // GameManager ���� (Inspector���� �Ҵ� �Ǵ� Start���� FindObjectOfType�� �Ҵ�)
    [SerializeField] private YutnoriGameManager gameManager;

    public void UpdateTurn(int turnCount)
    {
        turnCountText.text = $"��: {turnCount}";
    }

    /// <summary>
    /// �� ��� �ε��� ����Ʈ(0:��, 1:��, 2:��, 3:��, 4:��, 5:����)�� �޾� �������� UI�� ǥ��
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

            // Button Ŭ�� �̺�Ʈ ����
            Button btn = imgObj.GetComponent<Button>();
            int capturedIndex = i; // Ŭ���� ���� ����
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
        selectedIndex = -1; // ���� ��� �� ���� ����
    }

    /// <summary>
    /// �� ��� UI �̹��� ��� ����
    /// </summary>
    public void ClearYutResults()
    {
        foreach (var obj in spawnedYutResultImages)
            Destroy(obj);
        spawnedYutResultImages.Clear();
        selectedIndex = -1;
    }

    /// <summary>
    /// �� �̹��� Ŭ�� �� ȣ�� (idx: Ŭ���� ���� �ε���)
    /// </summary>
    public void OnYutResultImageClicked(int idx)
    {
        selectedIndex = idx;
        // ���̶���Ʈ ȿ��
        for (int i = 0; i < spawnedYutResultImages.Count; i++)
        {
            Image img = spawnedYutResultImages[i].GetComponent<Image>();
            img.color = (i == idx) ? new Color(0.9f, 0.9f, 0.9f, 0.4f) : Color.white;
        }

        // GameManager�� ���õ� �� �ε��� ���� (�ʿ��)
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


    // ���� ������ ��ư � ���⿡ �߰�
}
