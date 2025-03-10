using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkObjectSpawner : NetworkBehaviour
{
    public static NetworkObjectSpawner instance;

    void Awake()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        instance = this;
    }

    [Client]
    public void TellServerToSpawnObject (GameObject GO)
    {
        CmdSpawnObject(GO);
    }

    [Client]
    public void TellServerToSpawnObject (GameObject GO, Transform Pos)
    {
        CmdSpawnObject(GO, Pos);
    }

    [Command]
    private void CmdSpawnObject (GameObject GO)
    {
        if(!GO) return;

        NetworkServer.Spawn(GO);
    }

    [Command]
    private void CmdSpawnObject (GameObject GO, Transform Pos)
    {
        if(!GO) return;

        GO.transform.position = Pos.position;

        NetworkServer.Spawn(GO);
    }
}
