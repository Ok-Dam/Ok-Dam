using System.Collections.Generic;
using UnityEngine;

public enum POIType { Component, Upgrade, Shortcut, Buff }

public class PointOfInterest : MonoBehaviour
{
    public POIType Type;
    public List<PointOfInterest> NextPointsOfInterest { get; set; } = new();
}