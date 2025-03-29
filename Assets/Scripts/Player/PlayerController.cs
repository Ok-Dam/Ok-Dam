using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float rotationSpeed = 15f; // 회전 속도 추가

    private Animator m_animator;
    private Vector3 m_velocity;
    private bool m_wasGrounded;
    private bool m_isGrounded = true;

    void Start() => m_animator = GetComponent<Animator>();

    void Update()
    {
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
            // 1. 이동 방향 계산 (카메라 기준)
            Vector3 camForward = cameraController.transform.forward;
            Vector3 camRight = cameraController.transform.right;
            camForward.y = camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            m_velocity = (camForward * input.z + camRight * input.x).normalized;

            // 2. 속도 조절 (전력질주/걷기)
            if (Input.GetKey(KeyCode.LeftShift)) m_velocity *= 2.0f;
            if (Input.GetKey(KeyCode.LeftControl)) m_velocity /= 2.0f;
            m_animator.SetFloat("MoveSpeed", m_velocity.magnitude * moveSpeed);

            // 3. 점프 처리
            if (Input.GetKey(KeyCode.Space))
            {
                m_animator.SetTrigger("Jump");
                m_velocity.y = jumpForce;
            }

            // 4. 방향키 입력에 따른 캐릭터 회전 (예시 조건 충족)
            if (input.magnitude > 0.1f)
            {
                // 입력 방향 기준으로 회전 (카메라 기준)
                Vector3 targetDir = (camForward * input.z + camRight * input.x).normalized;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        // 5. 중력 적용 및 이동
        m_velocity.y -= gravity * Time.deltaTime;
        controller.Move(m_velocity * moveSpeed * Time.deltaTime);
        m_isGrounded = controller.isGrounded;
    }
}
