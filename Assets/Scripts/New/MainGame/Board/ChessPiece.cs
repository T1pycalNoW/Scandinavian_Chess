using Mirror;

public class ChessPiece : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnActiveChanged))]
    private bool isActive = true;

    [SyncVar(hook = nameof(OnParentChanged))]
    private NetworkIdentity parentIdentity; // Синхронизируем родителя через NetworkIdentity

    private void OnActiveChanged(bool oldValue, bool newValue)
    {
        gameObject.SetActive(newValue); // Синхронизируем активность объекта
    }

    [Server]
    public void SetActive(bool active)
    {
        isActive = active; // Изменяем состояние на сервере
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
