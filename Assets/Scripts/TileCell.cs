using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    //cells wont move, but we need to give them coordinates so we know their position on board


    public Vector2Int coordinates { get; set; }

    public Tile tile { get; set; }

    public bool empty => tile == null;

    public bool occupied => tile != null;



}
