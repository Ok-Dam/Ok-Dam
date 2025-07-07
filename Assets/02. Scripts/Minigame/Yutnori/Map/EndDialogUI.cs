
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun; // TextMeshPro 사용시

public class EndPanelUI : MonoBehaviour
{
    public Button endButton;
    [SerializeField] private TextMeshProUGUI endButtonText;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnEndButton()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.LoadLevel("MapScene");
        }
    }

    // 버튼 텍스트 변경 함수
    public void SetEndButtonText(string text)
    {
        if (endButtonText != null)
        {
            endButtonText.text = text;
        }
    }
}