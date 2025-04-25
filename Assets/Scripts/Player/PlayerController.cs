using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float rotationSpeed = 15f;

    private Animator m_animator;
    private Vector3 m_velocity;
    private bool m_wasGrounded;
    private bool m_isGrounded = true;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        // PhotonView가 내 것일 때만 카메라 설정
        if (photonView.IsMine)
        {
            // Main Camera의 CameraController 연결
            if (cameraController == null)
            {
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    cameraController = mainCam.GetComponent<CameraController>();
                }

                if (cameraController == null)
                {
                    Debug.LogError("[PlayerController] CameraController가 할당되지 않았습니다.");
                }
            }

            // 카메라의 타겟을 이 캐릭터로 설정
            if (cameraController != null)
            {
                cameraController.GetType().GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(cameraController, this.transform);
            }
        }
    }

    void Update()
    {
        // 내 캐릭터가 아니거나 채팅 중일 경우 조작 불가
        if (!photonView.IsMine) return;
        if (ChatManager.IsChatActive) return;

        m_animator.SetBool("Grounded", m_isGrounded);
        PlayerMove();
        m_wasGrounded = m_isGrounded;
    }

    private void PlayerMove()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float gravity = 20.0f;
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (controller.isGrounded)
        {
            // 카메라 기준으로 이동 방향 계산
            Vector3 camForward = cameraController.transform.forward;
            Vector3 camRight = cameraController.transform.right;
            camForward.y = camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            m_velocity = (camForward * input.z + camRight * input.x).normalized;

            // 속도 조절
            if (Input.GetKey(KeyCode.LeftShift)) m_velocity *= 2.0f;
            if (Input.GetKey(KeyCode.LeftControl)) m_velocity /= 2.0f;
            m_animator.SetFloat("MoveSpeed", m_velocity.magnitude * moveSpeed);

            // 점프 처리
            if (Input.GetKey(KeyCode.Space))
            {
                m_animator.SetTrigger("Jump");
                m_velocity.y = jumpForce;
            }

            // 방향 회전
            if (input.magnitude > 0.1f)
            {
                Vector3 targetDir = (camForward * input.z + camRight * input.x).normalized;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        // 중력 적용 및 이동
        m_velocity.y -= gravity * Time.deltaTime;
        controller.Move(m_velocity * moveSpeed * Time.deltaTime);
        m_isGrounded = controller.isGrounded;
    }
}
