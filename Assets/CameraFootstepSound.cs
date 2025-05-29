using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CameraFootstepSound : MonoBehaviour
{
    public float moveSpeed = 5f;                  // 이동 속도
    public AudioClip footstepClip;                // 발소리 오디오 클립

    private CharacterController controller;       // 카메라 이동에 사용할 컨트롤러
    private AudioSource audioSource;              // 오디오 소스

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (footstepClip != null)
        {
            audioSource.clip = footstepClip;
            audioSource.loop = true; // 걷는 소리 반복
        }
    }

    void Update()
    {
        // 이동 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;

        // 카메라 이동
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 이동 중인지 판단
        bool isMoving = move.magnitude > 0.1f;

        // 이동 중일 때만 효과음 재생
        if (isMoving)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Pause();  // 또는 audioSource.Stop();
        }
    }
}
