using Photon.Pun.Demo.PunBasics;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class YutnoriCameraController : MonoBehaviour
{
    [SerializeField]
    private YutnoriGameManager gameManager;
    private Camera _mainCam;
    private Vector3 _dragStartPos;
    [SerializeField] private LayerMask yutLayer; // �ν����Ϳ��� "Yut" ���̾� 

    [SerializeField] private MapGenerator mapGenerator;
    private float minZ, maxZ;


    private float dragSpeed = 1.0f;
    void Awake()
    {
        _mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ���� �Ŵ��� ���·� ��� ���� ó��
        // if (gameManager.stage != GameStage.Interact || gameManager.isDraggingYut)
        if (gameManager.isDraggingYut)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _dragStartPos = _mainCam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = _mainCam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 delta = _dragStartPos - currentPos;
            float newZ = Mathf.Clamp(
                transform.position.z + delta.y * dragSpeed * 100,
                minZ,
                maxZ
            );
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
            _dragStartPos = currentPos;
        }
    }

    // �� ���� �� MapGenerator.cs���� �� �Լ� �ҷ��� ��ũ�� ���� ���� ������Ʈ
    public void UpdateClampBounds()
    {
        Vector2 zBounds = mapGenerator.GetZBounds();
        float margin = 3.0f;
        minZ = zBounds.x - margin;
        maxZ = zBounds.y + margin;
    }
}
