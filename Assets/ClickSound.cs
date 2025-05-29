using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioClip clickSound;             // ȿ���� Ŭ��
    private AudioSource audioSource;

    void Start()
    {
        // ������ҽ� �ڵ� �߰� or ���� �� ���
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
