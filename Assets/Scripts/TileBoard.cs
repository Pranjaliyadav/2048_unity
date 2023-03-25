using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileBoard : MonoBehaviour
{
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

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void CreateTile()
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
        for(int x = startX;x>=0 && x < grid.width; x+= incrementX)
        {
            for(int y = startY;y>=0 && y < grid.height; y+=incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    MoveTile(cell.tile, direction);
                }
            }
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

    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;
        
        //todo - create a new tile, check for game over

    }


}


