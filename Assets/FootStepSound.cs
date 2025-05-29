using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudio;     // AudioSource 컴포넌트
    public AudioClip footstepClip;        // 발소리 클립 (짧은 효과음)
    public float stepInterval = 0.5f;     // 발소리 재생 간격 (초)

    private float stepTimer = 0f;

    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                footstepAudio.PlayOneShot(footstepClip);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = stepInterval; // 키 떼면 바로 다시 소리 가능하게 초기화
        }
    }
}
