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
    private bool _isDraggingYut;
    [SerializeField] private float minY = 5f;
    [SerializeField] private float maxY = 15f;
    [SerializeField] private LayerMask yutLayer; // 인스펙터에서 "Yut" 레이어 


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
        if (gameManager.stage != GameStage.Interact) return;

        // 마우스 클릭 시작 시 윷인지 판별
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, Mathf.Infinity, yutLayer))
            {
                _isDraggingYut = true;
                return;
            }
            _dragStartPos = _mainCam.ScreenToViewportPoint(Input.mousePosition);
        }

        // 윷을 드래그 중이 아니고, 마우스가 눌려있을 때 카메라 이동
        if (!_isDraggingYut && Input.GetMouseButton(0))
        {
            Vector3 currentPos = _mainCam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 delta = _dragStartPos - currentPos;
            float newY = transform.position.y + delta.y * dragSpeed * 100; // 감도 조정
            newY = Mathf.Clamp(newY, minY, maxY);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            _dragStartPos = currentPos;
        }

        // 마우스 버튼에서 손을 뗄 때 초기화
        if (Input.GetMouseButtonUp(0))
        {
            _isDraggingYut = false;
        }
    }
}
