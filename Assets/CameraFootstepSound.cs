using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CameraFootstepSound : MonoBehaviour
{
    public float moveSpeed = 5f;                  // �̵� �ӵ�
    public AudioClip footstepClip;                // �߼Ҹ� ����� Ŭ��

    private CharacterController controller;       // ī�޶� �̵��� ����� ��Ʈ�ѷ�
    private AudioSource audioSource;              // ����� �ҽ�

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (footstepClip != null)
        {
            audioSource.clip = footstepClip;
            audioSource.loop = true; // �ȴ� �Ҹ� �ݺ�
        }
    }

    void Update()
    {
        // �̵� �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;

        // ī�޶� �̵�
        controller.Move(move * moveSpeed * Time.deltaTime);

        // �̵� ������ �Ǵ�
        bool isMoving = move.magnitude > 0.1f;

        // �̵� ���� ���� ȿ���� ���
        if (isMoving)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Pause();  // �Ǵ� audioSource.Stop();
        }
    }
}
