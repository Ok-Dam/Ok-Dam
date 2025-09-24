using UnityEngine;

public class HeatMapManager : MonoBehaviour
{
    private GridManager gridManager;

    private int gridWidth;
    private int gridHeight;

    [Tooltip("Assign the floor's Renderer here")]
    public Renderer floorRenderer; // Must be assigned in inspector
    public Color heatColor = new Color(1, 0, 0, 0.5f);
    public Color clearColor = new Color(0, 0, 0, 0);

    private Texture2D heatTexture;
    private bool[,] heatedCells;

    void Awake()
    {
        gridManager = GetComponent<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found on GameObject!");
            return;
        }

        gridWidth = gridManager.gridWidth;
        gridHeight = gridManager.gridHeight;

        // 빨간색 된 그리드 기록
        heatedCells = new bool[gridWidth, gridHeight];

        // 1. 투명 텍스쳐 생성 - 오버레이로 작동할 텍스쳐. 원본 텍스쳐랑 쭉 같이 떠있다. 투명이라 평소엔 원본만 있는 것처럼 보이는 거. 
        heatTexture = new Texture2D(gridWidth, gridHeight, TextureFormat.RGBA32, false);
        heatTexture.filterMode = FilterMode.Point;

        ClearTexture();

        if (floorRenderer == null)
        {
            Debug.LogError("Renderer not assigned! Please assign your floor's MeshRenderer in inspector.");
            return;
        }

        // 2. 내 커스텀 쉐이더(OverlayHeat.shader가 지정돼있어야 함)한테 방금 만든 투명 텍스쳐 전달. 추후 heatTexture만 수정해도 알아서 전달됨.
        // 쉐이더는 저 HeatTex랑 원본 텍스쳐를 투명도 따라 알아서 섞어서 잘 보여주는 역할
        floorRenderer.material.SetTexture("_HeatTex", heatTexture);

        // Optionally assign heat tint; you also have this in shader properties
        floorRenderer.material.SetColor("_HeatColor", heatColor);

        Debug.Log("Heat texture assigned to floor material.");
    }


    void ClearTexture()
    {
        Color[] clearPixels = new Color[gridWidth * gridHeight];
        for (int i = 0; i < clearPixels.Length; i++)
            clearPixels[i] = clearColor;

        heatTexture.SetPixels(clearPixels);
        heatTexture.Apply();
    }

    public bool HeatCell(int x, int y)
    {
        // 범위 밖이면
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return false;
        }

        // 이미 칠했으면
        if (heatedCells[x, y])
        {
            return false;
        }

        // 빈 칸이나 부품 칸 아니면
        int cellType = gridManager.gridMap[x, y];
        if (cellType != 0 && cellType != 4)
        {
            return false;
        }

        heatedCells[x, y] = true;

        // 3. 플레이어가 이동 완료 후 pacPlayercontroller에서 좌표 주면 그걸로 아까의 투명 텍스쳐도 같은 위치를 빨갛게 칠한다.
        // 유니티 texture 2d 좌표랑 내가 쓰는 좌표 달라서 보정.
        // gridWidth - 1 - x로 전달하면 x좌표가 21~1이고, -x-1로 하면 -2~-22다. 근데 둘 다 된다. 대체 왜지?????
        //Debug.Log($"Original x: {x}, Converted x: {-x-1}");
        heatTexture.SetPixel(-x-1, y, heatColor);
        heatTexture.Apply();


        return true;
    }
}
