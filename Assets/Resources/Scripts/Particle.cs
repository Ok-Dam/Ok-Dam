using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float lifetime = 30f; // Particle이 30초까지 유지됨

    void Start()
    {
        Destroy(gameObject, lifetime); // 30초 후에 삭제
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Smoke"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Smoke"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}

