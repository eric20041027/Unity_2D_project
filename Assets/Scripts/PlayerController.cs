using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private BoardManager boardManager;
    private Vector2Int playerPosition;
    public Animator animator;
    private CharacterController characterController;
    private NavMeshAgent navMeshAgent;
    

    public void PlayerSpawn(BoardManager boardManager, Vector2Int playerPosition)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.boardManager = boardManager;
        this.playerPosition = playerPosition;
        characterController = this.GetComponent<CharacterController>();
        MoveTo(playerPosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit,999f,boardManager.mouseColliderPlainLayer))
            {
                Vector3Int cellPos = boardManager._tilemap.WorldToCell(raycastHit.point);
                Vector2Int targetCell = new Vector2Int(cellPos.x, cellPos.y);
                Debug.Log(targetCell);
                BoardManager.CellData cellData = boardManager.GetCellData(targetCell);
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
}
