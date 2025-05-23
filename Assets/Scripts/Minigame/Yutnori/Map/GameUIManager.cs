using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnCountText;

    public void UpdateTurn(int turnCount)
    {
        turnCountText.text = $"턴: {turnCount}";
    }

    // 추후 나가기 버튼 등도 여기에 추가
}
