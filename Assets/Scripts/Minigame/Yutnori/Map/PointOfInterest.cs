using System.Collections.Generic;
using UnityEngine;

public enum POIType { Component, Upgrade, Shortcut, Buff }

public class PointOfInterest : MonoBehaviour
{
    public POIType Type;
    // 직후 노드. 여러 분기 가능해서 List 
    public List<PointOfInterest> NextPointsOfInterest { get; set; } = new();
    //public List<PointOfInterest> PreviousPointsOfInterest = new();
}