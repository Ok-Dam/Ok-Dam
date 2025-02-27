using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class SliderTimer : MonoBehaviour
{
    public Slider timerSlider;  // UI 슬라이더
    public float gameTime = 30f; // 30초 타이머

    private float elapsedTime = 0f; // 경과 시간

    void Update()
    {
        if (elapsedTime < gameTime)
        {
            elapsedTime += Time.deltaTime;  // 시간 증가
            timerSlider.value = elapsedTime; // 슬라이더 값 반영
        }
        else
        {
            EndGame();  // 30초가 지나면 씬 변경
        }
    }

    void EndGame()
    {
        Debug.Log("게임 종료! Gudle_End 씬으로 이동");
        SceneManager.LoadScene("Gudle_End"); // 씬 변경
    }
}

