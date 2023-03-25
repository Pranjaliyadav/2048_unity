using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;

    public Tile tilePrefab;
    public TileState[] tileStates; //assign them in editor, put them in order, lowest to highest


    private TileGrid grid;

    private List<Tile> tiles;

    private bool waiting; //waiting for the animation to end, so we can move tile again 

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16); 
    }

   //clear board of any existig tiles if there are any
   public void ClearBoard()
    {
        foreach(var cell in grid.cells)
        {
            //clear all cells to make sure there's nothing occupying them
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject); //if u only destroy tile, it'll only destroy the script, not the entire game object
        }
        tiles.Clear();

    }

    public void CreateTile()
    {
        Tile tile =  Instantiate(tilePrefab,grid.transform);

        //set initial state and give position on board

        tile.SetState(tileStates[0], 2);

        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private void Update()
    {
        if(!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for(int x = startX;x>=0 && x < grid.width; x+= incrementX)
        {
            for(int y = startY;y>=0 && y < grid.height; y+=incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                   changed |=  MoveTile(cell.tile, direction); // |= or equals, if changed set to true first time, if changed is then false asecond time, it'll take true only from frst one. so change || false kindoff situation
                }
            }
        }
        if(changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        //main logic of game

        TileCell newCell = null; //cell we are trying to move to

        TileCell adjacentCell = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacentCell != null)
        {
            if (adjacentCell.occupied)
            {
                //merging

                if(CanMerge(tile, adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);
                    return true; //as their is state change so we return true
                }
                break;
            }

            newCell = adjacentCell; //search for next adjacent cell
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);

        }

        if(newCell != null)
        {
            tile.MoveTo(newCell);
            return true; //if we moved a tile
            //waiting = true; //as we are movig piece right now, so we set waiting to true.
            //StartCoroutine(WaitForChanges());
        }

        return false; //if we didnt move a tile
    }

    //to check if we can actually merge two cells,need to be same number
    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1); //index stays in bounds of our tile state
        //if we exceed 2048 state, it'll just keep using 2048 state as its clamped

        int number = b.number * 2;

        b.SetState(tileStates[index], number); //update state of tile

        gameManager.IncreaseScore(number);
    }

    private int IndexOf(TileState state)
    {
        for(int i = 0; i < tileStates.Length; i++)
        {
            if(state == tileStates[i]) return i;

        }

        return -1;

    }
    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;

        foreach( var tile in tiles)
        {
            tile.locked = false; //when all changed done, we just unock tile, so it can be used for merging
        }

        //creating a tile
        if(tiles.Count != grid.size)
        { //only creating it if we have a left space
        CreateTile();

        }

        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }

        //todo - create a new tile, check for game over

    }

    private bool CheckForGameOver()
    {
        if(tiles.Count != grid.size)
        {
            return false;
        }
        foreach(var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if(up != null && CanMerge(tile, up.tile))
            { //if we can still move up 
                return false;
            }
            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }
            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }
            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }

        }
        return true;

    }


}


