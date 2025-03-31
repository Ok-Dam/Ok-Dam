using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainDeformation : MonoBehaviour
{
    public Terrain terrain;  // Terrain 객체
    public Vector3 holePosition; // 파헤쳐진 위치
    public float holeRadius = 3f;  // 구멍의 반지름
    public float holeDepth = 2f;   // 파헤쳐진 깊이

    void Start()
    {
        DeformTerrain();
    }

    void DeformTerrain()
    {
        // Terrain의 높이맵 가져오기
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution; ;

        // 높이맵 배열 가져오기
        float[,] heights = terrainData.GetHeights(0, 0, width, height);

        // 파헤친 부분의 위치와 반경에 대해 높이값을 수정
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // 현재 위치와 구멍 중심 거리 계산
                Vector3 worldPos = terrain.transform.TransformPoint(x, 0, z);
                float distance = Vector3.Distance(worldPos, holePosition);

                if (distance < holeRadius)
                {
                    // 파헤친 깊이만큼 높이를 낮추기
                    float depthFactor = Mathf.Clamp01(1 - (distance / holeRadius));  // 거리 비율에 따라 깊이 계산
                    heights[x, z] -= depthFactor * holeDepth;
                }
            }
        }

        // 수정된 높이맵 다시 적용
        terrainData.SetHeights(0, 0, heights);
    }
}

