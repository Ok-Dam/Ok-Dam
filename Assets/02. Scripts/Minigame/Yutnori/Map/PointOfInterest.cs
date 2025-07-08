using System.Collections.Generic;
using UnityEngine;

public enum POIType { Start, Room, Yard, Event }

public class PointOfInterest : MonoBehaviour
{
    public POIType Type;
    public List<PointOfInterest> NextPointsOfInterest = new();
    public List<PointOfInterest> PreviousPointsOfInterest = new();
    public string description;

    [Header("분기점 여부 (Inspector에서 직접 체크)")]
    public bool isJunction = false;
    public PointOfInterest shortcutTarget;

    [Header("노드 번호 (1~29)")]
    public int nodeNumber;
}
