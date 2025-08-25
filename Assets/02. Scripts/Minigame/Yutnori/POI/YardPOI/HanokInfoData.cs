using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InfoPerPart
{
    public HanokPart part;
    [TextArea] public string infoText;
    public Sprite image;
}

[CreateAssetMenu(fileName = "HanokInfoData", menuName = "Quiz/HanokInfoData", order = 3)]
public class HanokInfoData : ScriptableObject
{
    public int nodeNumber;
    public List<InfoPerPart> infos;
}
