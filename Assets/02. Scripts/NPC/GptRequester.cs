using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using System.Collections.Generic;

public class GptRequester : MonoBehaviour
{
    public string gptServerUrl = "http://localhost:3000/gpt";

    [System.Serializable]
    public class Message
    {
        public string playerId;
        public string message;
    }

    [System.Serializable]
    public class GptMessage
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class GptChoice
    {
        public GptMessage message;
    }

    [System.Serializable]
    public class GptResponse
    {
        public List<GptChoice> choices;
    }

    public System.Action<string> OnGptResponse;

    public void RequestGpt(string userInput)
    {
        var body = new Message
        {
            playerId = PhotonNetwork.NickName,
            message = userInput
        };

        StartCoroutine(SendGptRequest(body));
    }

    IEnumerator SendGptRequest(Message body)
    {
        string json = JsonUtility.ToJson(body);
        UnityWebRequest req = new UnityWebRequest(gptServerUrl, "POST");
        byte[] jsonBytes = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(jsonBytes);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GPT 응답 원문: " + req.downloadHandler.text);
            GptResponse gptRes = JsonUtility.FromJson<GptResponse>(req.downloadHandler.text);
            string reply = gptRes.choices[0].message.content;
            OnGptResponse?.Invoke(reply);
        }
        else
        {
            Debug.LogError("GPT 요청 실패: " + req.error);
        }
    }
}
