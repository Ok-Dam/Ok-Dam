public class PlayerState
{
    // 이동 및 선택 관련
    public int moveDistance = 0;
    public bool canMove = false;
    public PlayerPiece piece; // 이 플레이어의 말

    // 윷 결과 관련
    public string currentYutResult = "";
    public int bonusThrowCount = 0; // 버프 혹은 윷/모

    // 버프 관련
    public bool canStackPiece = false;
    public int nextMovePlus = 0;
    public bool nextBuffAutoSuccess = false;

    // 상태 초기화
    public void ResetTurn()
    {
        moveDistance = 0;
        canMove = false;
        currentYutResult = "";
        bonusThrowCount = 0;
        // selectedPiece는 piece로 일원화
    }

    // 버프 소멸 처리
    public void ConsumeNextMovePlus()
    {
        nextMovePlus = 0;
    }
    public void ConsumeNextBuffAutoSuccess()
    {
        nextBuffAutoSuccess = false;
    }
}
