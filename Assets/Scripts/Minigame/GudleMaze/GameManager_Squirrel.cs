using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Squirrel : MonoBehaviour
{

    public int squirrelCount = 0;
    public Text squirrelText;
    public Slider heaterProgressSlider;
    public float progressSpeed = 0.1f;

    void Start()
    {
        // 초기 설정
        squirrelText.text = "다람쥐 수: " + squirrelCount;
    }

    void Update()
    {
        // 온돌 복구 진행
        if (heaterProgressSlider.value < 1)
        {
            heaterProgressSlider.value += progressSpeed * Time.deltaTime;
        }

        // 다른 게임 로직 추가
    }

    // 다람쥐의 자원을 훔쳐가는 로직을 추가
    public void AddSquirrel()
    {
        squirrelCount++;
        squirrelText.text = "다람쥐 수: " + squirrelCount;
    }
}
