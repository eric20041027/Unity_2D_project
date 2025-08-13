using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager boardManager;
    public PlayerController playerController;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        boardManager.Init();
        playerController.PlayerSpawn(boardManager,new Vector2Int(boardManager.width/2,boardManager.height/2));
    }
    
}
