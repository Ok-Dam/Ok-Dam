using UnityEngine;

public class SquirrelResourceRotation : MonoBehaviour
{
    public Transform squirrelHead; // 다람쥐 머리 위치
    public float heightOffset = 2f; // 머리 위로 띄울 높이

    public float rotationSpeed = 20f; // 회전 속도

    void Start()
    {
        // 자원을 다람쥐 머리 위치 위로 배치
        if (squirrelHead != null)
        {
            transform.position = squirrelHead.position + new Vector3(0, heightOffset, 0);
        }
    }

    void Update()
    {
        // 자원이 Y축을 기준으로 계속 회전
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
