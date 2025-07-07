using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Rendering;

public class CameraCapture : MonoBehaviour
{
    public RawImage photoPreviewUI;  // 찍은 사진 보여줄 UI
    public GameObject photoPreviewPanel; // 패널 전체 (RawImage 포함)
    public Texture2D predefinedPhoto; // 기와 이미지

    public AudioSource shutterSound; // 셔터 효과음
    public GameObject flashImage;    // 플래시 효과 이미지
    public GameObject[] uiOverlayToHide; //숨길 UI들

    public CameraZone cameraZone;

    public void OnCaptureButtonClick()
    {
        StartCoroutine(CaptureAndShowPhoto());
    }

    private IEnumerator CaptureAndShowPhoto()
    {
        //UI 숨기기
        foreach (GameObject ui in uiOverlayToHide)
        {
            if (ui != null) ui.SetActive(false);
        }

        yield return new WaitForEndOfFrame();

        // 화면 캡처
        //Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //tex.Apply();

        // 촬영 효과(플래시)
        if (flashImage != null)
        {
            flashImage.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            flashImage.SetActive(false);
        }
        //셔터소리
        if (shutterSound != null)
        {
            shutterSound.Play();
        }

        // 사진 보여주기
        //photoPreviewUI.texture = tex;
        //photoPreviewPanel.gameObject.SetActive(true);

        // 기와 사진 보여주기
        photoPreviewUI.texture = predefinedPhoto;
        photoPreviewPanel.SetActive(true);


    }

    public void OnClickConfirm()
    {
        photoPreviewPanel.SetActive(false);
        // 게임 모드 복귀 처리
        cameraZone?.ExitCameraMode();

        foreach (GameObject ui in uiOverlayToHide)
        {
            if (ui != null) ui.SetActive(true); // 숨긴 ui(미니맵) 다시 켜기
        }

       
    }

}
