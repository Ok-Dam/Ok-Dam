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

    private NPCUI npcUI;
    private Animator m_animator;
    private Vector3 m_velocity;
    private bool m_wasGrounded;
    private bool m_isGrounded = true;
    private bool isTalking = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        // PhotonView�� �� ���� ���� ī�޶� ����
        if (photonView.IsMine)
        {
            // Main Camera�� CameraController ����
            if (cameraController == null)
            {
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    cameraController = mainCam.GetComponent<CameraController>();
                }

                if (cameraController == null)
                {
                    Debug.LogError("[PlayerController] CameraController�� �Ҵ���� �ʾҽ��ϴ�.");
                }
            }

            // ī�޶��� Ÿ���� �� ĳ���ͷ� ����
            if (cameraController != null)
            {
                cameraController.GetType().GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(cameraController, this.transform);
            }
        }

        if (photonView.IsMine)
        {
            GameObject miniMapCamObj = GameObject.Find("MiniMapCamera");
            if (miniMapCamObj != null)
            {
                MiniMapCameraFollow follow = miniMapCamObj.GetComponent<MiniMapCameraFollow>();
                if (follow != null)
                {
                    follow.SetTarget(this.transform);
                }
            }
        }
    }

    void Update()
    {
        // �� ĳ���Ͱ� �ƴϰų�, ä�� ���̰ų�, npc ��ȭ ui�� �������� ���� �Ұ�
        if (!photonView.IsMine) return;
        if (ChatManager.IsChatActive) return;
        if (isTalking) return;

        m_animator.SetBool("Grounded", m_isGrounded);
        PlayerMove();
        m_wasGrounded = m_isGrounded;
    }

    public void setIsTalking(bool isTalking)
    {
        this.isTalking = isTalking;
    }

    private void PlayerMove()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float gravity = 20.0f;
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (controller.isGrounded)
        {
            // ī�޶� �������� �̵� ���� ���
            Vector3 camForward = cameraController.transform.forward;
            Vector3 camRight = cameraController.transform.right;
            camForward.y = camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            m_velocity = (camForward * input.z + camRight * input.x).normalized;

            // �ӵ� ����
            if (Input.GetKey(KeyCode.LeftShift)) m_velocity *= 2.0f;
            if (Input.GetKey(KeyCode.LeftControl)) m_velocity /= 2.0f;
            m_animator.SetFloat("MoveSpeed", m_velocity.magnitude * moveSpeed);

            // ���� ó��
            if (Input.GetKey(KeyCode.Space))
            {
                m_animator.SetTrigger("Jump");
                m_velocity.y = jumpForce;
            }

            // ���� ȸ��
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

        // �߷� ���� �� �̵�
        m_velocity.y -= gravity * Time.deltaTime;
        controller.Move(m_velocity * moveSpeed * Time.deltaTime);
        m_isGrounded = controller.isGrounded;
    }
}
