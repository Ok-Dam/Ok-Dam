using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas; // RawImage Æ÷ÇÔ Äµ¹ö½º
    public Button skipButton;

    private void Start()
    {
        videoPlayer.Prepare();
        videoPlayer.loopPointReached += EndReached;
        skipButton.onClick.AddListener(SkipVideo);
        videoCanvas.SetActive(false);
    }

    public void PlayVideo()
    {
        videoCanvas.SetActive(true);
        videoPlayer.Play();
    }

    private void EndReached(VideoPlayer vp)
    {
        CloseVideo();
    }

    private void SkipVideo()
    {
        videoPlayer.Stop();
        CloseVideo();
    }

    private void CloseVideo()
    {
        videoCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }
}
