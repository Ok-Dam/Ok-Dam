using UnityEngine;
using UnityEngine.UI;

public class FinishHudManager : MonoBehaviour
{
    public GameObject finishPieceImagePrefab; // 완주 말 이미지 프리팹
    public Transform imageContainer; // 이미지가 들어갈 컨테이너(=Grid Layout Group 붙은 오브젝트)

    public void AddFinishedPiece(Sprite pieceSprite)
    {
        GameObject imgObj = Instantiate(finishPieceImagePrefab, imageContainer);
        Image img = imgObj.GetComponent<Image>();
        img.sprite = pieceSprite;
    }
}

