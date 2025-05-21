using UnityEngine;

public class ResetObjectPosition : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // 초기 위치 저장
        initialPosition = transform.position;
    }

    // 초기화 함수
    public void ResetPosition()
    {
        transform.position = initialPosition;
    }
}
