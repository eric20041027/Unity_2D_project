using System;
<<<<<<< HEAD
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
=======
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
>>>>>>> fde3e1a (8/18)
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private BoardManager boardManager;
    private Vector2Int playerPosition;
    public Animator animator;
<<<<<<< HEAD
    private CharacterController characterController;
    private NavMeshAgent navMeshAgent;
    
=======
    private NavMeshAgent navMeshAgent;
    public int walkRange;
>>>>>>> fde3e1a (8/18)

    public void PlayerSpawn(BoardManager boardManager, Vector2Int playerPosition)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
<<<<<<< HEAD
        this.boardManager = boardManager;
        this.playerPosition = playerPosition;
        characterController = this.GetComponent<CharacterController>();
        MoveTo(playerPosition);
    }

    private void Update()
    {
=======
        navMeshAgent.enabled = false;
        this.boardManager = boardManager;
        this.playerPosition = playerPosition;
        MoveTo(playerPosition);
        boardManager.CreateActionTile(playerPosition,walkRange);
        navMeshAgent.enabled = true;
    }

    private void Update()
    {   
>>>>>>> fde3e1a (8/18)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit,999f,boardManager.mouseColliderPlainLayer))
            {
                Vector3Int cellPos = boardManager._tilemap.WorldToCell(raycastHit.point);
                Vector2Int targetCell = new Vector2Int(cellPos.x, cellPos.y);
                Debug.Log(targetCell);
                BoardManager.CellData cellData = boardManager.GetCellData(targetCell);
<<<<<<< HEAD
                if (cellData != null && cellData.Passible)
                {
                    MoveTo(targetCell);
                }
            }
        }
    }
   void MoveTo(Vector2Int cellPosition){
       /*
       playerPosition = cellPosition;
       transform.position = boardManager.GetCellPosition(cellPosition);
       */
       playerPosition = cellPosition;
       Vector3 targerPosition = boardManager.GetCellPosition(cellPosition);
       navMeshAgent.destination = targerPosition;
    }
=======
                if (cellData != null && cellData.Passible && cellData.reachable)
                {
                    WalkTo(targetCell);
                }
            }
        }
        
    }
   void MoveTo(Vector2Int cellPosition){
       playerPosition = cellPosition;
       transform.position = boardManager.GetCellPosition(cellPosition);
       boardManager.CreateActionTile(cellPosition,walkRange);
    }

    void WalkTo(Vector2Int cellPosition)
    {
        playerPosition = cellPosition;
        Vector3 targerPosition = boardManager.GetCellPosition(cellPosition);
        navMeshAgent.destination = targerPosition;
        boardManager.CreateActionTile(cellPosition,walkRange);
    }

    
>>>>>>> fde3e1a (8/18)
}
