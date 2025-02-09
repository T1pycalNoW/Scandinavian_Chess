using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChessGameOnline : NetworkBehaviour
{
    public GameManager gameManager;   
    [SyncVar] public string BoardState;

    void Start()
    {
        BoardState = gameManager.ScanBoard();
    }  

    [Command]
    public void CmdUpdateBoardState(string newState)
    {
        BoardState = newState;
    }
}
