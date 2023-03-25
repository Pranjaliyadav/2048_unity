using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates; //assign them in editor, put them in order, lowest to highest


    private TileGrid grid;

    private List<Tile> tiles;

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


}


