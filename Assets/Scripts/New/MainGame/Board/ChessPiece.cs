using Mirror;

public class ChessPiece : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnActiveChanged))]
    private bool isActive = true;

    [SyncVar(hook = nameof(OnParentChanged))]
    private NetworkIdentity parentIdentity; // Синхронизируем родителя через NetworkIdentity
    [SyncVar]
    private string figureName = "Figure";

    private void OnActiveChanged(bool oldValue, bool newValue)
    {
        gameObject.SetActive(newValue); // Синхронизируем активность объекта
    }

    [Server]
    public void SetActive(bool active)
    {
        isActive = active; // Изменяем состояние на сервере
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateNameOnClient();
    }

    [Client]
    private void UpdateNameOnClient ()
    {
        gameObject.name = figureName;
    }
    [Server]
    public void SetFigureName (string newName)
    {
        figureName = newName;
        RpcUpdateName(newName);
    }
    [ClientRpc]
    private void RpcUpdateName (string newName)
    {
        gameObject.name = newName;
    }

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
