
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun; // TextMeshPro ����

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

    // ��ư �ؽ�Ʈ ���� �Լ�
    public void SetEndButtonText(string text)
    {
        if (endButtonText != null)
        {
            endButtonText.text = text;
        }
    }
}