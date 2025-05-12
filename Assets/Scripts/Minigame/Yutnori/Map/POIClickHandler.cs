using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class POIClickHandler : MonoBehaviour
{
    private PointOfInterest poi;
    private YutnoriGameManager gameManager;
    private NodeManager nodeManager;

    // Start is called before the first frame update
    void Start()
    {
        poi = GetComponent<PointOfInterest>();
        gameManager = FindObjectOfType<YutnoriGameManager>();
        nodeManager = FindObjectOfType<NodeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        if (gameManager.canMove && nodeManager.HighlightedNodes.Contains(poi))
        {
            gameManager.MoveSelectedPieceTo(poi);
        }
    }

}
