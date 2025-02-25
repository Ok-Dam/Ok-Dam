using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MeshDeformation : MonoBehaviour
{
    public MeshFilter meshFilter;   // 변형할 MeshFilter
    public Vector3 holeCenter;      // 구멍이 생길 위치
    public float holeRadius = 1f;   // 구멍의 크기
    public float holeDepth = 0.5f;  // 구멍의 깊이

    void Start()
    {
        DeformMesh();
    }

    void DeformMesh()
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            float distance = Vector3.Distance(vertex, holeCenter);

            if (distance < holeRadius)
            {
                float deformation = Mathf.Lerp(0, holeDepth, 1 - (distance / holeRadius)); // 깊이를 부여
                vertices[i] -= new Vector3(0, deformation, 0); // Y축으로 깊이만큼 내림
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // 정점 계산
        mesh.RecalculateBounds();  // 바운드 계산
    }
}

