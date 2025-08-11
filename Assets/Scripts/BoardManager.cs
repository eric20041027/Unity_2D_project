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
      for (int x = 0; x < gridHandler.GetLength(0); x++)
      {
         for (int y = 0; y < gridHandler.GetLength(1); y++)
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
         //Walker Methods
         ChanceToRemove();
         ChanceToRedirect();
         ChanceToCreate();
         UpdatePosition();

         if (hasCreatedFloor)
         {
            yield return new WaitForSeconds(waitTime);
         }
      }
      StartCoroutine(CreateWall());
   }

   IEnumerator CreateWall()
   {
      for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
      {
         for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
         {
            if (gridHandler[x, y] == Grid.Floor)
            {
               bool hasCreatedWall = false;
               if (gridHandler[x+1, y] == Grid.Empty)
               {
                  tileMap.SetTile(new Vector3Int(x+1,y,0), Wall);
                  gridHandler[x+1, y] = Grid.Wall;
                  hasCreatedWall = true;
               }
               if (gridHandler[x-1, y] == Grid.Empty)
               {
                  tileMap.SetTile(new Vector3Int(x-1,y,0), Wall);
                  gridHandler[x-1, y] = Grid.Wall;
                  hasCreatedWall = true;
               }
               if (gridHandler[x, y+1] == Grid.Empty)
               {
                  tileMap.SetTile(new Vector3Int(x,y+1,0), Wall);
                  gridHandler[x, y+1] = Grid.Wall;
                  hasCreatedWall = true;
               }
               if (gridHandler[x, y-1] == Grid.Empty)
               {
                  tileMap.SetTile(new Vector3Int(x,y-1,0), Wall);
                  gridHandler[x, y-1] = Grid.Wall;
                  hasCreatedWall = true;
               }

               if (hasCreatedWall)
               {
                  yield return new WaitForSeconds(waitTime);
               }
            }
         }
      }
   }

   void ChanceToRemove()
   {
      int updatedCount = Walkers.Count;
      for (int i = 0; i < updatedCount; i++)
      {
         if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
         {
            Walkers.RemoveAt(i);
            break;
         }
      }
   }

   void ChanceToRedirect()
   {
      for (int i = 0; i < Walkers.Count; i++)
      {
         if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
         {
            WalkerObject curWalker = Walkers[i];
            curWalker.Direction = GetDirection();
            Walkers[i] = curWalker;
         }
      }
   }

   void ChanceToCreate()
   {
      int updatedCount = Walkers.Count;
      for (int i = 0; i < updatedCount; i++)
      {
         if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < maxWalker)
         {
            Vector2 newDirection = GetDirection();
            Vector2 newPosition = Walkers[i].Position;

            WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
            Walkers.Add(newWalker);
         }
      }
   }

   void UpdatePosition()
   {
      for (int i = 0; i < Walkers.Count; i++)
      {
         WalkerObject foundWalker = Walkers[i];
         foundWalker.Position += foundWalker.Direction;
         foundWalker.Position.x = Mathf.Clamp(foundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
         foundWalker.Position.y = Mathf.Clamp(foundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
         Walkers[i] = foundWalker;
      }
   }
   

}
