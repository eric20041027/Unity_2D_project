using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using Random = System.Random;

public class BoardManager : MonoBehaviour
{
   public class CellData
   {
      public bool Passible;
      public GameObject CotainedObject;
      public bool HasCreatedTile;
   }

   public CellData GetCellData(Vector2Int cellIndex)
   {
      if (cellIndex.x < 0 || cellIndex.x > m_BoardData.GetLength(0) || cellIndex.y < 0 || cellIndex.y > m_BoardData.GetLength(1))
      {
         return null;
      } 
      return m_BoardData[cellIndex.x, cellIndex.y];
   }

   public Vector3 GetCellPosition(Vector2Int cellIndex)
   {
      return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
   }

   public int cellSize;
   private List<Vector2Int> _emptyCellList;
   public int height;
   public int width;
   public Tilemap _tilemap;
   private Grid _grid;
   private CellData[,] m_BoardData;
   public Tile grassTile;
   //public Tile dirtTile;
   //public Tile clayTile;
   private int _tileCount = 0;
   public float fillPercentage = 0.5f;
   public float waitTime = 0.05f;
   public List<WalkerObject> Walkers;
   public int maxWalkerNumber =10;

   public void Init()
   {
      PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
      _grid = GetComponent<Grid>();
      _emptyCellList = new List<Vector2Int>();
      m_BoardData = new CellData[width, height];
      InitialGrid();
   }

   void InitialGrid()
   {
      for (int x = 0; x < width; x++)
      {
         for (int y = 0; y < height; y++)
         {
            m_BoardData[x, y] = new CellData();
            m_BoardData[x, y].CotainedObject = null;
            m_BoardData[x, y].HasCreatedTile = false;
            m_BoardData[x, y].Passible = false;
            if (y == height - 1 ||  x == width - 1 || x == 0 || y == 0)
            {
               m_BoardData[x, y].HasCreatedTile = true;
            }
         }
      }

      Walkers = new List<WalkerObject>();
      WalkerObject walker = new WalkerObject (new Vector2Int(m_BoardData.GetLength(0)/2,m_BoardData.GetLength(1)/2),GetDirection(),0.5f);
      _tilemap.SetTile(new Vector3Int(walker.Position.x,walker.Position.y,0), grassTile);
      m_BoardData[walker.Position.x, walker.Position.y].HasCreatedTile = true;
      m_BoardData[walker.Position.x, walker.Position.y].Passible = true;
      Walkers.Add(walker);
      _tileCount++;
      StartCoroutine(CreateBoard());
   }

   Vector2 GetDirection()
   {
      int direction = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);
      switch (direction)
      {
         case 0:  
            return Vector2.right;
         case 1:  
            return Vector2.left;
         case 2:  
            return Vector2.up;
         case 3:  
            return Vector2.down;
      }
      return Vector2.zero;
   }

   IEnumerator CreateBoard()
   {
      while (_tileCount /(float)m_BoardData.Length < fillPercentage)
      {
         bool hasCreated = false;
         foreach (WalkerObject walker in Walkers)
         {
            Vector3Int currentPosition = new Vector3Int(walker.Position.x,walker.Position.y,0);
            if (m_BoardData[walker.Position.x, walker.Position.y].HasCreatedTile == false)
            {
               _tilemap.SetTile(currentPosition, grassTile);
               _tileCount++;
               _emptyCellList.Add(walker.Position);
               m_BoardData[walker.Position.x, walker.Position.y].HasCreatedTile = true;
               m_BoardData[walker.Position.x, walker.Position.y].Passible = true;
               hasCreated = true;
            }
         }
         WalkerMoveFoward();
         WalkerSpawn();
         WalkerTurn();
         if (hasCreated)
         {
            yield return new WaitForSeconds(waitTime);
         }
      }
   }

   
   void WalkerMoveFoward()
   {
      for (int i = 0; i < Walkers.Count; i++)
      {
         WalkerObject walker = Walkers[i];
         if (UnityEngine.Random.value > walker.ChanceToChange)
         {
            if (walker.Direction == Vector2.right && walker.Position.x < width - 1)
            {
               walker.Position.x += 1;
            }
            else if (walker.Direction == Vector2.left && walker.Position.x > 0)
            {
               walker.Position.x -= 1;
            }
            else if (walker.Direction == Vector2.up && walker.Position.y < height - 1)
            {
               walker.Position.y += 1;
            }

            else if (walker.Direction == Vector2.down && walker.Position.y > 0)
            {
               walker.Position.y -= 1;
            }
            else
            {
               WalkerTurn();
            }
         }
      }
   }

   void WalkerTurn()
   {
      for (int i = 0; i < Walkers.Count; i++)
      {
         WalkerObject walker = Walkers[i];
         if (UnityEngine.Random.value > walker.ChanceToChange)
         {
            walker.Direction = GetDirection(); 
         }
      }
      
   }

   void WalkerSpawn()
   {
      for (int i = 0; i < Walkers.Count; i++)
      {
         WalkerObject walker = Walkers[i];
         if (UnityEngine.Random.value > walker.ChanceToChange && Walkers.Count < maxWalkerNumber)
         {
            Vector2 newDirection = GetDirection();
            Vector2Int newPosition = walker.Position;
            WalkerObject newWalker = new WalkerObject(newPosition, newDirection,0.5f);
            Walkers.Add(newWalker);
         }
      }
   }

   
}
