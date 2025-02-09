using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    public int ID = 0;
    
    void Start ()
    {
        Debug.Log(isLocalPlayer);
    }
}
