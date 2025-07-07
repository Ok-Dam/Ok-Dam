using UnityEngine;
using UnityEngine.UI;

public class YutResultSlot : MonoBehaviour
{
    private Image image; // �� Image�� �� Ŭ�� ���
    private Button button; // �� Button�� ���� Ŭ�� ����
    public int index;

    private GameUIManager uiManager;

    public void Init(Sprite sprite, int idx, GameUIManager manager)
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        image.sprite = sprite;
        index = idx;
        uiManager = manager;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        uiManager.OnYutResultImageClicked(index);
    }

    public void SetHighlight(bool highlight)
    {
        image.color = highlight ? Color.yellow : Color.white;
    }
}
