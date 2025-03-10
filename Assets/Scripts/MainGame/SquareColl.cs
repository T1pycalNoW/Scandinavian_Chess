using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class SquareColl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameManager gameManager;
    public PuzzleManager puzzleManager;
    public GameObject BoxPrefab;
    private GameObject GreenBoxCopy;
    public GreenBoxManager greenBoxManager;
    public int coo1;
    public int coo2;
    private bool clicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.ClearDeveloperConsole();
        
        if (gameManager.GreenBoxCopyExist && gameManager.canMove == true)
        {   
            if(gameManager.CheckRules(gameManager.GreenBoxCopy, this.gameObject))
            {
                gameManager.SwapActive(gameManager.GreenBoxCopy, this.gameObject);

                gameManager.MoveFigures(gameManager.GreenBoxCopy, this.gameObject);

                gameManager.TakeFigure(gameManager.BoardReady[coo1, coo2], coo1, coo2);

                Debug.Log(gameManager.ScanBoard());
            }

            Destroy(gameManager.SearchChild(gameManager.GreenBoxCopy, "GreenBoxN"));

            Destroy(GreenBoxCopy);
            gameManager.GreenBoxCopyExist = false;

            gameManager.GreenBoxCopy.GetComponent<SquareColl>().clicked = false;
        }

        else if (gameManager.FigureExist(this.gameObject))
        {
            clicked = true;

            gameManager.GreenBoxCopy = this.gameObject;
            gameManager.GreenBoxCopyExist = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GreenBoxCopy = Instantiate(BoxPrefab, this.gameObject.transform);
        greenBoxManager.AddBox(GreenBoxCopy);
        GreenBoxCopy.name = "GreenBoxN";
        Image childImage = transform.GetChild(transform.childCount-1).GetComponent<Image>();
        gameManager.SpawnGreenBox(childImage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clicked == false)
        {
            Destroy(GreenBoxCopy);
        }  
    }
}
