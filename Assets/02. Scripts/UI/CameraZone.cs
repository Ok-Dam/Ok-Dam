using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public GameObject cameraUI;        
    public GameObject arrowUI;           
    public GameObject groundParticle;    
    public Camera mainCamera;
 

    public Vector3 zoomOffset = new Vector3(0f, 0.1f, -2.5f); //ī�޶� ������(�÷��̾� �Ⱥ��̰� ���� ���� ��� ������ ��)
    public float zoomFOV = 30f;

    private bool isPlayerInZone = false;
    private Vector3 originalCamPosition;
    private float originalFOV;
    private bool zoomedIn = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (arrowUI != null) arrowUI.SetActive(true);
            if (groundParticle != null) groundParticle.SetActive(true);

            if (mainCamera != null && !zoomedIn)
            {
                originalCamPosition = mainCamera.transform.localPosition;
                originalFOV = mainCamera.fieldOfView;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            //if (arrowUI != null) arrowUI.SetActive(false);
            //if (groundParticle != null) groundParticle.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            EnterCameraMode();
        }
    }

    void EnterCameraMode()
    {

        if (mainCamera != null && !zoomedIn)
        {
            mainCamera.transform.localPosition += zoomOffset;
            mainCamera.fieldOfView = zoomFOV;
            zoomedIn = true;
        }

        if (cameraUI != null) cameraUI.SetActive(true);
        if (arrowUI != null) arrowUI.SetActive(false);
        if (groundParticle != null) groundParticle.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("ī�޶� ��� ����");
    }

    // ������ �� �� �Լ� ȣ��
    public void ExitCameraMode()
    {
        if (mainCamera != null && zoomedIn)
        {
            mainCamera.transform.localPosition = originalCamPosition;
            mainCamera.fieldOfView = originalFOV;
            zoomedIn = false;
        }

        cameraUI?.SetActive(false);
        arrowUI?.SetActive(true);
        groundParticle?.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("���� ��� ����");
    }
}

