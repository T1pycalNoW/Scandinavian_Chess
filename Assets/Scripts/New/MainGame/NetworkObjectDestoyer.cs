using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkObjectDestoyer : NetworkBehaviour
{
    public static NetworkObjectDestoyer instance;

    void Awake()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        instance = this;
    }

    [Client]
    public void TellServerToDestroyObject (GameObject GO)
    {
        CmdDestroyObject(GO);
    }

    [Command]
    private void CmdDestroyObject (GameObject GO)
    {
        if(!GO) return;

        NetworkServer.Destroy(GO);
    }
}
