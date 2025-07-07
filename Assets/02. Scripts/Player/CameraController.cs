using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player; // 추적 대상 (플레이어 Transform 할당 필수)

    [Header("Settings")]
    [SerializeField] private float distance = 5f; // 플레이어와의 거리
    [SerializeField] private float mouseSensitivity = 1600f; // 마우스 회전 감도
    [SerializeField] private float minVerticalAngle = -40f; // 카메라 수직 회전 최소각 (하방 제한)
    [SerializeField] private float maxVerticalAngle = 80f; // 카메라 수직 회전 최대각 (상방 제한)
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0); // 플레이어 위치 기준 오프셋 (Y축 높이 조정)

    private float xRotation; // 현재 X축 회전값 (상하)
    private float yRotation; // 현재 Y축 회전값 (좌우)
    private Vector3 smoothVelocity; // SmoothDamp에 사용되는 속도 참조 변수

    void Start()
    {
        // 마우스 커서를 화면 중앙에 고정 및 숨김
        Cursor.lockState = CursorLockMode.Locked;

        // 초기 카메라 회전값 설정
        Vector3 angles = transform.eulerAngles;
        xRotation = angles.x;
        yRotation = angles.y;
    }

    void LateUpdate()
    {
        if (player == null) return;

        //npcui 카메라 정지
        if (NPCUI.IsTalkingToNPC) return;
        //마우스가 잠겨있을 때만 회전 허용(ui 나올때는 회전X)
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // 마우스 입력 처리 (델타타임 곱해 프레임률 독립적)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 회전 각도 계산
        yRotation += mouseX; // 좌우 회전 누적
        xRotation -= mouseY; // 상하 회전 누적
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle); // 상하 각도 제한

        // 최종 회전값 계산 (쿼터니언으로 변환)
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0);

        // 플레이어 위치 기준 카메라 위치 계산
        // 1. 플레이어 위치 + 오프셋 적용
        // 2. 회전 방향의 반대 방향(-forward)으로 distance만큼 이동
        Vector3 targetPosition = player.position + offset - targetRotation * Vector3.forward * distance;

        // 부드러운 회전 적용 (Slerp: 구면 선형 보간)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // 부드러운 위치 이동 (SmoothDamp: 감쇠 효과)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 0.1f);
    }
}
