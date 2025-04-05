using UnityEngine;
using UnityEngine.EventSystems;
 
public class GreenBoxManagerScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject BoxPrefab; // Префаб для BoxPrefab
    public DefaultGameManagerScript MainGameManager;
    public AllGreenBoxesController GreenBoxController;
    private bool clicked;
 
    private void Start()
    {
        // Убедимся, что BoxPrefab изначально неактивен
        BoxPrefab.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Активируем BoxPrefab и устанавливаем его позицию
        BoxPrefab.SetActive(true);
        BoxPrefab.transform.position = this.transform.position;
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!clicked)
        {
            // Скрываем BoxPrefab, если он не был кликнут
            BoxPrefab.SetActive(false);
        }
    }
 
    public void OnPointerClick(PointerEventData eventData)
    {
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
                GameObject newBox = Instantiate(BoxPrefab, this.transform.position, Quaternion.identity);
                newBox.SetActive(true);
                Debug.Log("Copy created");
 
                // Устанавливаем новый BoxPrefab в контроллер
                GreenBoxController.SetNewCopy(this.gameObject, newBox);
            }
        }
    }
}