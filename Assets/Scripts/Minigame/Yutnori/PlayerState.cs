using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    // 이동 및 선택 관련
    public int moveDistance = 0;
    public bool canMove = false;
    public PlayerPiece piece; // 이 플레이어의 말

    // 윷 결과 관련
    public string currentYutResult = "";
    public int throwsLeft = 1;
    //public const int maxThrows = 3;

    // 이번 턴에 던진 모든 윷 결과 (도/개/걸/윷/모/빽도 인덱스)
    public List<int> yutResultIndices = new List<int>();

    // 현재 선택한 패 인덱스 (-1이면 미선택)
    public int selectedResultIndex = -1;

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
        throwsLeft = 1;
        yutResultIndices.Clear();
        selectedResultIndex = -1;
        piece = null;
    }

    public void AddThrowChance(int amount)
    {
        throwsLeft += amount;
        //throwsLeft = Mathf.Min(throwsLeft + amount, maxThrows);
    }

    // 윷을 던진 후 결과 처리
    public void RegisterYutResult(string result)
    {
        Debug.Log("public void RegisterYutResult " + result);
        currentYutResult = result;
        int idx = YutResultToIndex(result);
        if (idx >= 0)
            yutResultIndices.Add(idx);

        moveDistance = GetMoveDistance(result);

        // 윷/모면 보너스
        if (result == "윷" || result == "모")
            AddThrowChance(1);

        throwsLeft--;
        Debug.Log("Throws: " + throwsLeft + " moveDist: " + moveDistance);
    }

    // 선택한 패의 이동거리 반환 (패 선택 후 사용)
    public int GetSelectedMoveDistance()
    {
        if (selectedResultIndex < 0 || selectedResultIndex >= yutResultIndices.Count)
            return 0;
        return GetMoveDistance(IndexToYutResult(yutResultIndices[selectedResultIndex]));
    }

    // 문자열 결과를 인덱스로 변환
    public int YutResultToIndex(string result)
    {
        switch (result)
        {
            case "도": return 0;
            case "개": return 1;
            case "걸": return 2;
            case "윷": return 3;
            case "모": return 4;
            case "빽도": return 5;
            default: return -1;
        }
    }

    // 인덱스를 문자열 결과로 변환
    public string IndexToYutResult(int idx)
    {
        switch (idx)
        {
            case 0: return "도";
            case 1: return "개";
            case 2: return "걸";
            case 3: return "윷";
            case 4: return "모";
            case 5: return "빽도";
            default: return "";
        }
    }

    // 문자열 결과로 이동거리 반환
    public int GetMoveDistance(string result)
    {
        switch (result)
        {
            case "도": return 1;
            case "개": return 2;
            case "걸": return 3;
            case "윷": return 4;
            case "모": return 5;
            case "빽도": return -1;
            default: return 0;
        }
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
