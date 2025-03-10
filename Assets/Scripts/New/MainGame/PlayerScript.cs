using Mirror;
using UnityEngine;

public class PlayerScript : NetworkBehaviour
{
    [Command]
    public void CmdSetParent(int x, int y, NetworkIdentity squareIdentity)
    {
        // Получаем ячейку, на которую нужно переместить фигуру
        Square targetSquare = BasicBoardScript.BoardReady[x, y];

        if (targetSquare == null)
        {
            Debug.LogWarning("Целевая ячейка не найдена");
            return;
        }

        // Получаем текущую ячейку, с которой перемещается фигура
        Square currentSquare = squareIdentity.GetComponent<Square>();

        if (currentSquare == null)
        {
            Debug.LogWarning("Текущая ячейка не найдена");
            return;
        }

        // Ищем фигуру на текущей ячейке
        GameObject Figure = HelpfulMode.SearchChild(currentSquare.GO, "Figure");

        if (Figure == null)
        {
            Debug.LogWarning("Figure не найден");
            return;
        }

        // Устанавливаем родителя для фигуры
        ChessPiece chessPiece = Figure.GetComponent<ChessPiece>();

        if (chessPiece == null)
        {
            Debug.LogWarning("Компонент ChessPiece не найден");
            return;
        }

        // Перемещаем фигуру на новую ячейку
        chessPiece.SetParent(targetSquare.GetComponent<NetworkIdentity>());
        Figure.transform.position = targetSquare.GO.transform.position;

        // Обновляем состояние ячеек
        currentSquare.HasFigure = false;
        targetSquare.HasFigure = true;

        Debug.Log($"Фигура перемещена на ячейку ({x}, {y})");
    }
}
