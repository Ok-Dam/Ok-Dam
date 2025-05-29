using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player; // ���� ��� (�÷��̾� Transform �Ҵ� �ʼ�)

    [Header("Settings")]
    [SerializeField] private float distance = 5f; // �÷��̾���� �Ÿ�
    [SerializeField] private float mouseSensitivity = 1600f; // ���콺 ȸ�� ����
    [SerializeField] private float minVerticalAngle = -40f; // ī�޶� ���� ȸ�� �ּҰ� (�Ϲ� ����)
    [SerializeField] private float maxVerticalAngle = 80f; // ī�޶� ���� ȸ�� �ִ밢 (��� ����)
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0); // �÷��̾� ��ġ ���� ������ (Y�� ���� ����)

    private float xRotation; // ���� X�� ȸ���� (����)
    private float yRotation; // ���� Y�� ȸ���� (�¿�)
    private Vector3 smoothVelocity; // SmoothDamp�� ���Ǵ� �ӵ� ���� ����

    void Start()
    {
        // ���콺 Ŀ���� ȭ�� �߾ӿ� ���� �� ����
        Cursor.lockState = CursorLockMode.Locked;

        // �ʱ� ī�޶� ȸ���� ����
        Vector3 angles = transform.eulerAngles;
        xRotation = angles.x;
        yRotation = angles.y;
    }

    void LateUpdate()
    {
        if (player == null) return;

        //npcui ī�޶� ����
        if (NPCUI.IsTalkingToNPC) return;
        //���콺�� ������� ���� ȸ�� ���(ui ���ö��� ȸ��X)
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // ���콺 �Է� ó�� (��ŸŸ�� ���� �����ӷ� ������)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ȸ�� ���� ���
        yRotation += mouseX; // �¿� ȸ�� ����
        xRotation -= mouseY; // ���� ȸ�� ����
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle); // ���� ���� ����

        // ���� ȸ���� ��� (���ʹϾ����� ��ȯ)
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0);

        // �÷��̾� ��ġ ���� ī�޶� ��ġ ���
        // 1. �÷��̾� ��ġ + ������ ����
        // 2. ȸ�� ������ �ݴ� ����(-forward)���� distance��ŭ �̵�
        Vector3 targetPosition = player.position + offset - targetRotation * Vector3.forward * distance;

        // �ε巯�� ȸ�� ���� (Slerp: ���� ���� ����)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // �ε巯�� ��ġ �̵� (SmoothDamp: ���� ȿ��)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 0.1f);
    }
}
