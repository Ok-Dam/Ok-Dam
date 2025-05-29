using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioClip clickSound;             // 효과음 클립
    private AudioSource audioSource;

    void Start()
    {
        // 오디오소스 자동 추가 or 기존 것 사용
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.clip = clickSound;
    }

    void OnMouseDown()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
