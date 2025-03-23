using UnityEngine;
using Mirror;
public class Square : NetworkBehaviour
{
    public GameObject GO; // Оставляем поле GameObject GO

    [SyncVar]
    public Color FigureColor = Color.None;

    [SyncVar]
    public TypeOfFigure FigureType = TypeOfFigure.None;

    [SyncVar]
    public bool HasFigure = false;

    public int ID1;
    public int ID2;

    private GameObject GreenBox;

    void Start()
    {
        GreenBox = GameObject.FindGameObjectWithTag("GreenBox");
    }

    void OnMouseEnter()
    {
        if (GreenBox != null)
        {
            GreenBox.transform.position = this.transform.position;
        }
    }

    void OnMouseExit()
    {
        if (GreenBox != null)
        {
            GreenBox.transform.position = new Vector3(0f, 200f, 0f);
        }
    }

    void OnMouseDown()
    {
        // Получаем объект игрока
        PlayerScript player = NetworkClient.localPlayer?.GetComponent<PlayerScript>();

        /*if (player != null)
        {
            // Отправляем запрос на сервер через объект игрока
            player.CmdSetParent(2, 2, GetComponent<NetworkIdentity>());
        }*/
    }
}
