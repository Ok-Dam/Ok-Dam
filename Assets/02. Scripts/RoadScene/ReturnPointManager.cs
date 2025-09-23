using UnityEngine;

public class ReturnPointManager : MonoBehaviour
{
    public static ReturnPointManager Instance;

    // 플레이어가 돌아갈 위치
    private Vector3 returnPoint;
    private Quaternion returnRotation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 현재 위치를 리턴포인트로 저장
    /// </summary>
    public void SetReturnPoint(Transform target)
    {
        returnPoint = target.position;
        returnRotation = target.rotation;
    }

    /// <summary>
    /// 저장된 리턴포인트 위치/회전값을 불러오기
    /// </summary>
    public void MoveToReturnPoint(Transform target)
    {
        target.position = returnPoint;
        target.rotation = returnRotation;
    }

    /// <summary>
    /// 리턴포인트가 설정되어 있는지 확인
    /// </summary>
    public bool HasReturnPoint()
    {
        return returnPoint != Vector3.zero;
    }
}
