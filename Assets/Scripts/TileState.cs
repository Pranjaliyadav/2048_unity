using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Tile State")]
//so with this, in editor when we are using create menu for creating something, we'll see TileState there,just like UI, create empty, 2d etc. 
//we can simply create that and then it'll have 2 props backgroundColor and textColor, that we can set from editor itself

public class TileState : ScriptableObject
{
    //to determine the background color of our tile and the color of our text, making a data structure for it

    public Color backgroundColor;
    public Color textColor;




}
