using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    
    //track all rows within grid and sometimes cells too

    public TileRow[] rows {  get; private set; }
    public TileCell[] cells { get; private set; }

    //writing them here, not hardcoding the size of grid, 
    public int size => cells.Length; 

    public int height => rows.Length;

    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        for(int y = 0; y < rows.Length; y++) //y because, rows are placed vertically, we going top to bottom
        {
            for(int x=0; x < rows[y].cells.Length; x++) // x because cells are placed horizontally in a row, we going left to right
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y); //position of each cell
            }
        }
    }

    public TileCell GetRandomEmptyCell() 
    {//to find random empty cell

        int index = Random.Range(0, cells.Length);
        int startingIndex = index; //to avoid infinite loop of checking already occupied cells, we keep this startingIndex, if index == startingIndex that maeans all cells are occupied


        while (cells[index].occupied)
        {
            index++;
            if (index >= cells.Length)
            {
                index = 0; // wrapping index back to 0, 
            }

            if(index == startingIndex)
            {   //all cells are occupied
                return null;
            }
        }

        return cells[index];
    }

}
