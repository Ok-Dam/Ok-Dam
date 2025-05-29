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
        // Inspector에서 직접 추가해도 되지만, 코드로 초기화도 가능
        if (buffs.Count == 0)
        {
            buffs.Add(new Buff(BuffType.ExtraThrow, "한 번 더 던지기"));
            buffs.Add(new Buff(BuffType.NextMovePlus, "다음 이동 시 이동 거리 +1"));
            buffs.Add(new Buff(BuffType.NextBuffAutoSuccess, "다음 버프 노드에서 실패해도 버프 받음"));
        }
    }

    public Buff GetRandomBuff()
    {
        int randomIndex = Random.Range(0, buffs.Count);
        return buffs[randomIndex];
    }
}
