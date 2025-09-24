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

        heatedCells = new bool[gridWidth, gridHeight];

        heatTexture = new Texture2D(gridWidth, gridHeight, TextureFormat.RGBA32, false);
        heatTexture.filterMode = FilterMode.Point;

        ClearTexture();

        if (floorRenderer == null)
        {
            Debug.LogError("Renderer not assigned! Please assign your floor's MeshRenderer in inspector.");
            return;
        }

        // IMPORTANT: assign heatTexture to '_HeatTex' property of your custom shader!
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

        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return false;
        }

        if (heatedCells[x, y])
        {
            return false;
        }

        // ºó Ä­ÀÌ³ª ºÎÇ° Ä­ ¾Æ´Ï¸é
        int cellType = gridManager.gridMap[x, y];
        if (cellType != 0 && cellType != 4)
        {
            return false;
        }

        heatedCells[x, y] = true;

        // À¯´ÏÆ¼ texture 2d ÁÂÇ¥¶û ³»°¡ ¾²´Â ÁÂÇ¥ ´Þ¶ó¼­ º¸Á¤.
        heatTexture.SetPixel(-x-1, y, heatColor);
        heatTexture.Apply();


        return true;
    }
}
