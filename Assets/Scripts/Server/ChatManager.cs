using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    [Header("UI Reference")]
    public GameObject chatPanel;                         // Layout 오브젝트
    public TMP_InputField chatInputField;                // Input/InputField
    public TMP_Text chatText;                            // Output/Scroll View/Viewport/Content 안 TMP_Text
    public ScrollRect scrollRect;                        // Output/Scroll View

    private ChatClient chatClient;
    private string currentChannel = "Global";
    private bool isChatActive = false;

    void Start()
    {
        // 닉네임 자동 지정
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
            PhotonNetwork.NickName = "User" + Random.Range(1000, 9999);

        chatPanel.SetActive(false);

        chatClient = new ChatClient(this);
        chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            "1.0",
            new AuthenticationValues(PhotonNetwork.NickName)
        );
    }

    void Update()
    {
        chatClient?.Service();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isChatActive)
            {
                OpenChat(); // 채팅창 열기
            }
            else
            {
                string message = chatInputField.text.Trim();

                if (!string.IsNullOrEmpty(message))
                {
                    SendChatMessage(message);
                    chatInputField.text = ""; // 메시지 전송 후 입력창만 초기화
                    chatInputField.ActivateInputField(); // 포커스 유지
                }
                else
                {
                    CloseChat(); // 아무 입력 없을 경우에만 닫기
                }
            }
        }

        if (isChatActive && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChat();
        }
    }


    void OpenChat()
    {
        chatPanel.SetActive(true);
        chatInputField.text = "";
        chatInputField.ActivateInputField();
        isChatActive = true;
    }

    void CloseChat()
    {
        chatInputField.DeactivateInputField();
        chatPanel.SetActive(false);
        isChatActive = false;
    }

    void SendChatMessage(string message)
    {
        chatClient.PublishMessage(currentChannel, message);
        chatInputField.text = "";
    }

    public void OnConnected()
    {
        Debug.Log("Connected to Photon Chat");
        chatClient.Subscribe(new string[] { currentChannel });
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            chatText.text += $"\n<color=yellow>{senders[i]}</color>: {messages[i]}";
        }

        ScrollToBottom();
    }

    void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    // 필수 인터페이스 메서드들
    public void OnChatStateChange(ChatState state) { }
    public void OnDisconnected() => Debug.Log("Disconnected from Photon Chat");
    public void OnSubscribed(string[] channels, bool[] results) { }
    public void OnUnsubscribed(string[] channels) { }
    public void OnPrivateMessage(string sender, object message, string channelName) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }
    public void DebugReturn(DebugLevel level, string message) => Debug.Log(message);
}
