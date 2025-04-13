using UnityEngine;
using Mirror;

public class ChessPiece : NetworkBehaviour
{
    [SyncVar] public int x, y; // Синхронизированные координаты
    public bool isWhite; // Принадлежность к цвету

    [Server]
    public void MoveTo(int newX, int newY)
    {
        x = newX;
        y = newY;
        RpcUpdatePosition(newX, newY);
        Debug.Log($"Фигура перемещена на ({newX}, {newY})");
    }

    [ClientRpc]
    void RpcUpdatePosition(int newX, int newY)
    {
        transform.position = new Vector3(newX, newY, 0);
        Debug.Log($"Клиент получил новую позицию: ({newX}, {newY})");
    }
}
