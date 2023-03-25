using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    
    //each row has to keep track of cells within that row

    public TileCell[] cells { get; private set; }

    private void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();


    }


}
