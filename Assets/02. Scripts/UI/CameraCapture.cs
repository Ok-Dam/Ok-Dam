using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Rendering;

public class CameraCapture : MonoBehaviour
{
    public RawImage photoPreviewUI;  // ���� ���� ������ UI
    public GameObject photoPreviewPanel; // �г� ��ü (RawImage ����)
    public Texture2D predefinedPhoto; // ��� �̹���

    public AudioSource shutterSound; // ���� ȿ����
    public GameObject flashImage;    // �÷��� ȿ�� �̹���
    public GameObject[] uiOverlayToHide; //���� UI��

    public CameraZone cameraZone;

    public void OnCaptureButtonClick()
    {
        StartCoroutine(CaptureAndShowPhoto());
    }

    private IEnumerator CaptureAndShowPhoto()
    {
        //UI �����
        foreach (GameObject ui in uiOverlayToHide)
        {
            if (ui != null) ui.SetActive(false);
        }

        yield return new WaitForEndOfFrame();

        // ȭ�� ĸó
        //Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //tex.Apply();

        // �Կ� ȿ��(�÷���)
        if (flashImage != null)
        {
            flashImage.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            flashImage.SetActive(false);
        }
        //���ͼҸ�
        if (shutterSound != null)
        {
            shutterSound.Play();
        }

        // ���� �����ֱ�
        //photoPreviewUI.texture = tex;
        //photoPreviewPanel.gameObject.SetActive(true);

        // ��� ���� �����ֱ�
        photoPreviewUI.texture = predefinedPhoto;
        photoPreviewPanel.SetActive(true);


    }

    public void OnClickConfirm()
    {
        photoPreviewPanel.SetActive(false);
        // ���� ��� ���� ó��
        cameraZone?.ExitCameraMode();

        foreach (GameObject ui in uiOverlayToHide)
        {
            if (ui != null) ui.SetActive(true); // ���� ui(�̴ϸ�) �ٽ� �ѱ�
        }

       
    }

}
