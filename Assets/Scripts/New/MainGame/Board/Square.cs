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
    private DefaultGameManagerScript MainGameManager;
    private AllGreenBoxesController GreenBoxController;
    private bool clicked;   

    void Start()
    {
        if(GreenBox == null)
        {
            GreenBox = GameObject.FindGameObjectWithTag("GreenBox");
        }
        if (GreenBox == null)
        {
            GreenBox = AllGreenBoxesController.Instance.GreenBox;
        }
        
        MainGameManager = DefaultGameManagerScript.Instance;
        GreenBoxController = AllGreenBoxesController.Instance;

        if (GreenBox != null)
        {
            GreenBox.SetActive(false);
        }
    }

    void OnConnectedToServer()
    {
        if(GreenBox == null)
        {
            GreenBox = GameObject.FindGameObjectWithTag("GreenBox");
        }
        if (GreenBox == null)
        {
            GreenBox = AllGreenBoxesController.Instance.GreenBox;
        }
        
        if (GreenBox != null)
        {
            GreenBox.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        if (GreenBox != null)
        {
            GreenBox.SetActive(true);
            GreenBox.transform.position = this.transform.position;
        }
    }

    void OnMouseExit()
    {
        if (GreenBox != null)
        {       
            if (!clicked)
            {
            // Скрываем BoxPrefab, если он не был кликнут
                GreenBox.SetActive(false);
                GreenBox.transform.position = new Vector3(0f, 200f, 0f);
            }
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

        if (HelpfulMode.SearchChild(this.gameObject, "Figure") || GreenBoxController.GreenBoxExist)
        {
            if (GreenBoxController.GreenBoxExist)
            {
                if (MainGameManager.CheckRules(GreenBoxController.ConnectedField, this.gameObject))
                {
                    // Выполняем перемещение фигуры
                    MainGameManager.MoveFigures(GreenBoxController.ConnectedField, this.gameObject);
                    MainGameManager.TakeFigure(GreenBoxController.ConnectedField, this.gameObject);
                    GreenBoxController.ResetBasicFields();
                }
                else
                {
                    GreenBoxController.ResetBasicFields();
                    Debug.Log("Reset");
                }
            }
            else
            {
                // Создаем новый BoxPrefab только для локального клиента
                GameObject newBox = Instantiate(GreenBox, this.transform.position, Quaternion.identity);
                newBox.SetActive(true);
                Debug.Log("Copy created");
 
                // Устанавливаем новый BoxPrefab в контроллер
                GreenBoxController.SetNewCopy(this.gameObject, newBox);
            }
        }
    }
}
