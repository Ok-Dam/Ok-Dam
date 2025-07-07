using UnityEngine;
using UnityEngine.UI;

public class FinishHudManager : MonoBehaviour
{
    public GameObject finishPieceImagePrefab; // ���� �� �̹��� ������
    public Transform imageContainer; // �̹����� �� �����̳�(=Grid Layout Group ���� ������Ʈ)

    public void AddFinishedPiece(Sprite pieceSprite)
    {
        GameObject imgObj = Instantiate(finishPieceImagePrefab, imageContainer);
        Image img = imgObj.GetComponent<Image>();
        img.sprite = pieceSprite;
    }
}

