using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public BuffType type;
    public string description;

    public Buff(BuffType type, string description)
    {
        this.type = type;
        this.description = description;
    }
}

public class BuffManager : MonoBehaviour
{
    public List<Buff> buffs = new List<Buff>();

    private void Awake()
    {
        // Inspector���� ���� �߰��ص� ������, �ڵ�� �ʱ�ȭ�� ����
        if (buffs.Count == 0)
        {
            buffs.Add(new Buff(BuffType.ExtraThrow, "�� �� �� ������"));
            buffs.Add(new Buff(BuffType.NextMovePlus, "���� �̵� �� �̵� �Ÿ� +1"));
            buffs.Add(new Buff(BuffType.NextBuffAutoSuccess, "���� ���� ��忡�� �����ص� ���� ����"));
        }
    }

    public Buff GetRandomBuff()
    {
        int randomIndex = Random.Range(0, buffs.Count);
        return buffs[randomIndex];
    }
}
