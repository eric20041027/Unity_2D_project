using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private BoardManager boardManager;
    private Vector2Int playerPosition;
    public Animator animator;

    

    public void PlayerSpawn(BoardManager boardManager, Vector2Int playerPosition)
    {
        this.boardManager = boardManager;
        this.playerPosition = playerPosition;
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
    /*    void Update()
    { 
        bool hasMoved = false;
        Vector2Int newTargeCell = playerPosition;
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            Debug.Log("Up arrow key pressed");
            newTargeCell.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            Debug.Log("Down arrow key pressed");
            newTargeCell.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            Debug.Log("Left arrow key pressed");
            newTargeCell.x -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            Debug.Log("Right arrow key pressed");
            newTargeCell.x += 1;
            hasMoved = true;
        }
        if (hasMoved)
        {
            BoardManager.CellData cellData = boardManager.GetCellData(newTargeCell);
            if (cellData != null && cellData.Passible)
            {
                MoveTo(newTargeCell);
            }
        } 
    }*/
    
   void MoveTo(Vector2Int cellPosition){
       playerPosition = cellPosition;
       transform.position = boardManager.GetCellPosition(cellPosition);
    }
}
