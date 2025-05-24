using UnityEngine;

public class SpiralOrbitCamera : MonoBehaviour
{
    public Transform target;             // �߽��� �� ���๰
    public float orbitDuration = 6f;     // ��ü ȸ�� �ð�
    public float startDistance = 10f;    // ���� �Ÿ�
    public float endDistance = 5f;       // ���� �Ÿ�

    private float elapsed = 0f;

    void Update()
    {
        if (elapsed < orbitDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / orbitDuration;

            // ���� �Ÿ� (��������� ����)
            float currentDistance = Mathf.Lerp(startDistance, endDistance, t);

            // ȸ�� ���� (360�� ����)
            float angle = 360f * t;
            float radians = angle * Mathf.Deg2Rad;

            // ��ġ ��� (XZ ���)
            float x = Mathf.Sin(radians) * currentDistance;
            float z = Mathf.Cos(radians) * currentDistance;

            Vector3 orbitPosition = new Vector3(x, 20f, z) + target.position; // ���̴� ���� or ���� �����ص� ����

            transform.position = orbitPosition;
            transform.LookAt(target);
        }
    }
}
