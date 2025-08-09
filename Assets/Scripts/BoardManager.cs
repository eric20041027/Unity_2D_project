using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class BoardManager : MonoBehaviour
{
   public enum Grid
   {
      Wall,
      Floor,
      Empty,
   }

   public Grid[,] gridHandler;
   public List<WalkerObject> Walkers;
   public Tilemap tileMap;
   public Tile Floor;
   public Tile Empty;
   public Tile Wall;
   public int mapHeight = 30;
   public int mapWidth = 30;

   public int maxWalker = 10;
   public int tileCount = 0;
   public float fillPercentage = 0.4f;
   public float waitTime = 0.05f;

   void Start()
   {
      InitialGrid();
   }

   void InitialGrid()
   {
      gridHandler = new Grid[mapHeight, mapWidth];
      for (int y = 0; y < gridHandler.Length; y++)
      {
         for (int x = 0; x < gridHandler.Length; x++)
         {
            gridHandler[x,y] = Grid.Empty; 
         }
      }
      
      Walkers = new List<WalkerObject>();
      Vector3Int tileCenter = new Vector3Int(gridHandler.GetLength(0)/2,gridHandler.GetLength(1)/2,0);
      WalkerObject currentWalker = new WalkerObject(new Vector2(tileCenter.x, tileCenter.y),GetDirection(),0.5f);
      gridHandler[tileCenter.x, tileCenter.y] = Grid.Floor;
      tileMap.SetTile(tileCenter, Floor);
      Walkers.Add(currentWalker);
      tileCount++;
      StartCoroutine(CreateFloor());
   }

   Vector2 GetDirection()
   {
      int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

      switch (choice)
      {
         case 0:
            return Vector2.down;
         case 1:
            return Vector2.left;
         case 2:
            return Vector2.up;
         case 3:
            return Vector2.right;
         default:
            return Vector2.zero;
      }
   }

   IEnumerator CreateFloor()
   {
      while ((float)tileCount / (float)gridHandler.Length < fillPercentage)
      {
         bool hasCreatedFloor = false;
         foreach (WalkerObject currentWalker in Walkers)
         {
            Vector3Int currentPosition = new Vector3Int((int)currentWalker.Position.x, (int)currentWalker.Position.y, 0);
            if (gridHandler[currentPosition.x, currentPosition.y] != Grid.Floor)
            {
               tileMap.SetTile(currentPosition, Floor);
               tileCount++;
               gridHandler[currentPosition.x, currentPosition.y] = Grid.Floor;
               hasCreatedFloor = true;
            }
         }

         if (hasCreatedFloor)
         {
            yield return new WaitForSeconds(waitTime);
         }
      }
   }
  

}
