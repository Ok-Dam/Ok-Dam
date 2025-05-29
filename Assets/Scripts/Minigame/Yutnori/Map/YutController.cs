using System.Collections;
using UnityEngine;

public class YutController : MonoBehaviour
{
    public Rigidbody[] yutRigidbodies;
    private Vector3 dragStartPos;
    private bool isDragging = false;
    [SerializeField] private float throwForce = 500f;
    [SerializeField] private LayerMask yutLayer;
    [SerializeField] private YutnoriGameManager gameManager;
    private Camera mainCamera;
    [SerializeField] private float velocityThreshold = 0.1f;
    [SerializeField] private float maxWaitTime = 5f;
    [SerializeField] private float faceUpThreshold = 0.7f;

    void Start()
    {
        mainCamera = Camera.main;
        yutRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void StartDrag()
    {
        if (gameManager.stage != GameStage.Throw) return;
        if (gameManager.isDraggingYut) return;

        gameManager.SetYutDragState(true);
        isDragging = true;
        dragStartPos = Input.mousePosition;
    }
    public void EndDrag()
    {
        gameManager.SetYutDragState(false);
        if (!isDragging) return;

        Vector3 dragEndPos = Input.mousePosition;
        Vector3 dragVector = dragEndPos - dragStartPos;

        // �� ���: �巡�� ������ ũ��� ������ �״�� ���
        float upForce = 100f + (dragVector.magnitude * 1.2f);
        upForce = Mathf.Clamp(upForce, 80f, 600f);

        Vector3 forceDir = new Vector3(
            -dragVector.y,
            upForce,
            dragVector.x
        );

        foreach (var rb in yutRigidbodies)
        {
            Vector3 variedForce = new Vector3(
                forceDir.x * Random.Range(0.9f, 1.1f),
                forceDir.y * Random.Range(0.8f, 1.2f),
                forceDir.z * Random.Range(0.9f, 1.1f)
            );
            // throwForce�� ������ ���� (�Ǵ� 1.0f�� ����)
            rb.AddForce(variedForce * Random.Range(0.75f, 0.9f));
            rb.AddTorque(Random.insideUnitSphere * 80f);
        }

        StartCoroutine(DelayedStateCheck());
        isDragging = false;
    }

    private IEnumerator DelayedStateCheck()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CheckYutStateCoroutine());
    }

    private IEnumerator CheckYutStateCoroutine()
    {
        bool hasMoved = false;
        float timeout = 3f;

        while (timeout > 0 && !hasMoved)
        {
            foreach (var rb in yutRigidbodies)
            {
                if (rb.velocity.magnitude > 0.1f)
                {
                    hasMoved = true;
                    break;
                }
            }
            timeout -= Time.deltaTime;
            yield return null;
        }

        float elapsed = 0f;
        while (!AllYutsStopped() && elapsed < maxWaitTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        int faceUpCount = CalculateFaceUpCount();
        string result = GetYutResult(faceUpCount);

        gameManager.ProcessYutResult(result);
    }

    private bool AllYutsStopped()
    {
        foreach (var rb in yutRigidbodies)
        {
            if (rb.velocity.magnitude > velocityThreshold ||
                rb.angularVelocity.magnitude > velocityThreshold)
                return false;
        }
        return true;
    }

    private int CalculateFaceUpCount()
    {
        int count = 0;
        foreach (var rb in yutRigidbodies)
        {
            // �� �������� y���� ������ ���ϵ��� ���õǾ� �־�� ���� ����
            if (Vector3.Dot(rb.transform.up, Vector3.up) > faceUpThreshold)
                count++;
        }
        return count;
    }

    private string GetYutResult(int faceUpCount)
    {
        return "����";
        // ���� ���� �迭�� 0��°��� ����
        if (faceUpCount == 3)
        {
            if (Vector3.Dot(yutRigidbodies[0].transform.up, Vector3.up) < -faceUpThreshold)
                return "����";
            else
                return "��";
        }
        else if (faceUpCount == 1)
        {
            return "��";
        }
        else if (faceUpCount == 2)
        {
            return "��";
        }
        else if (faceUpCount == 0)
        {
            return "��";
        }
        else if (faceUpCount == 4)
        {
            return "��";
        }
        return "ERROR";
    }
}
