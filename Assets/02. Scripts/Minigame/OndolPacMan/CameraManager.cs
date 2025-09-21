using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GridManager gridManager;
    public Camera mainCamera;

    public GameObject markerPrefab;

    void Start()
    {
        PlaceMarkers();
        PositionCamera();
    }

    void PlaceMarkers()
    {
        Vector3 topLeft = gridManager.CoordToWorldPos(0, gridManager.gridHeight - 1);
        Vector3 bottomRight = gridManager.CoordToWorldPos(gridManager.gridWidth - 1, 0);

        Instantiate(markerPrefab, topLeft + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(markerPrefab, bottomRight + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    void PositionCamera()
    {
        float midX = (gridManager.gridWidth / 2f) * gridManager.cellSize;
        float midZ = (gridManager.gridHeight / 2f) * gridManager.cellSize;

        mainCamera.transform.position = new Vector3(midX, 10, midZ - 5);
        mainCamera.transform.rotation = Quaternion.Euler(45, 0, 0);
    }
}
