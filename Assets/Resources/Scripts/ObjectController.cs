using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{


    public GameObject objectA;  // 객체 A
    public GameObject objectB;  // 객체 B (Particle System)

    void Start()
    {
        // 게임 시작 시 objectB를 비활성화
        objectB.SetActive(false);
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            // 객체 A를 클릭했을 때
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == objectA)
                {
                    // 객체 A가 클릭되면 객체 A를 사라지게 하고
                    objectA.SetActive(false);

                    // 객체 B를 활성화
                    objectB.SetActive(true);
                }
            }
        }
    }
}
