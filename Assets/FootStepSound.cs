using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudio;     // AudioSource ������Ʈ
    public AudioClip footstepClip;        // �߼Ҹ� Ŭ�� (ª�� ȿ����)
    public float stepInterval = 0.5f;     // �߼Ҹ� ��� ���� (��)

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
            stepTimer = stepInterval; // Ű ���� �ٷ� �ٽ� �Ҹ� �����ϰ� �ʱ�ȭ
        }
    }
}
