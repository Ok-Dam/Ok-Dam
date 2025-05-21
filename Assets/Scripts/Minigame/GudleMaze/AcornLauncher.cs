using System.Collections.Generic;
using UnityEngine;

public class AcornLauncher : MonoBehaviour
{
    public Rigidbody acornPrefab;
    public Transform launchPoint;
    public float launchPower = 10f;
    public float maxDragDistance = 5f;

    public LineRenderer lineRenderer; // 예측선 표시용
    public int predictionSteps = 30;  // 예측 궤적 점 개수
    public float timeStep = 0.05f;    // 시간 간격

    private Camera mainCamera;
    private Vector3 dragStartPos;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.positionCount = predictionSteps;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                dragStartPos = hit.point;
                isDragging = true;
                lineRenderer.enabled = true;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 dragEndPos = hit.point;
                Vector3 launchDirection = dragStartPos - dragEndPos;
                float dragDistance = Mathf.Clamp(launchDirection.magnitude, 0, maxDragDistance);
                Vector3 launchVelocity = launchDirection.normalized * launchPower * dragDistance;
                DrawTrajectory(launchVelocity);
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 dragEndPos = hit.point;
                LaunchAcorn(dragEndPos);
            }
            isDragging = false;
            lineRenderer.enabled = false;
        }
    }

    void LaunchAcorn(Vector3 dragEndPos)
    {
        Vector3 launchDirection = dragStartPos - dragEndPos;
        float dragDistance = Mathf.Clamp(launchDirection.magnitude, 0, maxDragDistance);

        Rigidbody acorn = Instantiate(acornPrefab, launchPoint.position, Quaternion.identity);
        acorn.AddForce(launchDirection.normalized * launchPower * dragDistance, ForceMode.Impulse);
    }

    void DrawTrajectory(Vector3 launchVelocity)
    {
        Vector3 currentPosition = launchPoint.position;
        Vector3 currentVelocity = launchVelocity;

        for (int i = 0; i < predictionSteps; i++)
        {
            lineRenderer.SetPosition(i, currentPosition);
            currentVelocity += Physics.gravity * timeStep;
            currentPosition += currentVelocity * timeStep;
        }
    }
}
