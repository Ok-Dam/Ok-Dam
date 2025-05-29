using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    // �̵� �� ���� ����
    public int moveDistance = 0;
    public bool canMove = false;
    public PlayerPiece piece; // �� �÷��̾��� ��

    // �� ��� ����
    public string currentYutResult = "";
    public int throwsLeft = 1;
    //public const int maxThrows = 3;

    // �̹� �Ͽ� ���� ��� �� ��� (��/��/��/��/��/���� �ε���)
    public List<int> yutResultIndices = new List<int>();

    // ���� ������ �� �ε��� (-1�̸� �̼���)
    public int selectedResultIndex = -1;

    // ���� ����
    public bool canStackPiece = false;
    public int nextMovePlus = 0;
    public bool nextBuffAutoSuccess = false;

    // ���� �ʱ�ȭ
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

    // ���� ���� �� ��� ó��
    public void RegisterYutResult(string result)
    {
        Debug.Log("public void RegisterYutResult " + result);
        currentYutResult = result;
        int idx = YutResultToIndex(result);
        if (idx >= 0)
            yutResultIndices.Add(idx);

        moveDistance = GetMoveDistance(result);

        // ��/��� ���ʽ�
        if (result == "��" || result == "��")
            AddThrowChance(1);

        throwsLeft--;
        Debug.Log("Throws: " + throwsLeft + " moveDist: " + moveDistance);
    }

    // ������ ���� �̵��Ÿ� ��ȯ (�� ���� �� ���)
    public int GetSelectedMoveDistance()
    {
        if (selectedResultIndex < 0 || selectedResultIndex >= yutResultIndices.Count)
            return 0;
        return GetMoveDistance(IndexToYutResult(yutResultIndices[selectedResultIndex]));
    }

    // ���ڿ� ����� �ε����� ��ȯ
    public int YutResultToIndex(string result)
    {
        switch (result)
        {
            case "��": return 0;
            case "��": return 1;
            case "��": return 2;
            case "��": return 3;
            case "��": return 4;
            case "����": return 5;
            default: return -1;
        }
    }

    // �ε����� ���ڿ� ����� ��ȯ
    public string IndexToYutResult(int idx)
    {
        switch (idx)
        {
            case 0: return "��";
            case 1: return "��";
            case 2: return "��";
            case 3: return "��";
            case 4: return "��";
            case 5: return "����";
            default: return "";
        }
    }

    // ���ڿ� ����� �̵��Ÿ� ��ȯ
    public int GetMoveDistance(string result)
    {
        switch (result)
        {
            case "��": return 1;
            case "��": return 2;
            case "��": return 3;
            case "��": return 4;
            case "��": return 5;
            case "����": return -1;
            default: return 0;
        }
    }

    // ���� �Ҹ� ó��
    public void ConsumeNextMovePlus()
    {
        nextMovePlus = 0;
    }
    public void ConsumeNextBuffAutoSuccess()
    {
        nextBuffAutoSuccess = false;
    }
}
