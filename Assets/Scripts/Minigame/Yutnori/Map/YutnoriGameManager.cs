using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum GameStage { Start, Throw, Move, Interact, End}
public class YutnoriGameManager : MonoBehaviour
{
    public GameStage stage;

    public bool isDraggingYut { get; private set; } 
    public void SetYutDragState(bool state) => isDraggingYut = state; 

    // Start is called before the first frame update
    void Start()
    {
        setGameStage(GameStage.Interact);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGameStage(GameStage stage) 
    {
        this.stage = stage; 
    }

}
