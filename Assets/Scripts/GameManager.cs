using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    //need refernce to tileboard to clear and update stuff

    public TileBoard board;

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true; //to enabe user inputs
    }

    public void GameOver()
    {
        board.enabled = false;
    }

}
