using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator m_animator;
    private Vector3 m_velocity;

    private bool m_wasGrounded;
    private bool m_isGrounded = true;

    public float m_moveSpeed = 4.0f;
    public float m_jumpForce = 5.0f;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Animator 파라미터 업데이트
        m_animator.SetBool("Grounded", m_isGrounded);

        PlayerMove();

        // 직전 상태 업데이트
        m_wasGrounded = m_isGrounded;
    }

    private void PlayerMove()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float gravity = 20.0f;

        if (controller.isGrounded)
        {
            // 이동 방향 계산. 키에서 손 떼도 지속되는 거 방지 위해 GetAxis 대신 GetAxisRaw
            m_velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            m_velocity = m_velocity.normalized;

            // 전력질주
            if (Input.GetKey(KeyCode.LeftShift)) 
            {
                // 전역인 movement에 곱해버리면 이속이 순식간에 엄청 커져버림
                m_velocity *= 2.0f;
            }
            // 걷기
            if (Input.GetKey(KeyCode.LeftControl)) 
            {
                m_velocity /= 2.0f;
            }
            // m_velocity.magnitude는 0.5/1/2 중 하나. 
            m_animator.SetFloat("MoveSpeed", m_velocity.magnitude*m_moveSpeed);
        

            if (Input.GetKey(KeyCode.Space))
            {
                // 점프 처리
                m_animator.SetTrigger("Jump");
                m_velocity.y = m_jumpForce;
            }
            else if (m_velocity.magnitude > 0.1f)
            {
                transform.LookAt(transform.position + m_velocity); // 움직이는 방향으로 회전
            }
        }

        // 중력 적용 및 이동 처리
        m_velocity.y -= gravity * Time.deltaTime;
        controller.Move(m_velocity * m_moveSpeed * Time.deltaTime);

        // 땅에 닿았는지 확인
        m_isGrounded = controller.isGrounded;
    }
}
