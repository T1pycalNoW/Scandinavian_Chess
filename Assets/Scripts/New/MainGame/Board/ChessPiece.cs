using Mirror;

public class ChessPiece : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnParentChanged))]
    private NetworkIdentity parentIdentity; // Синхронизируем родителя через NetworkIdentity

    private void OnParentChanged(NetworkIdentity oldParent, NetworkIdentity newParent)
    {
        if (newParent != null)
        {
            transform.SetParent(newParent.transform);
        }
        else
        {
            transform.SetParent(null);
        }
    }

    [Server]
    public void SetParent(NetworkIdentity newParent)
    {
        parentIdentity = newParent;
    }
}
