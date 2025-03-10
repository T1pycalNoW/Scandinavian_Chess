using System;
using Mirror;
using UnityEngine;

public class PositionManagerScript : NetworkBehaviour
{
    public GameObject WhiteKing;
    public GameObject WhitePawn;
    public GameObject BlackPawn;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Создаем и расставляем фигуры на сервере
        SetPosition("b03b04b05b14b30b38b40b41b47b48b50b58b74b83b84b85w33w34w35w43k44w45w53w54w55");
    }

    [Server]
    public void SetPosition(string positionText)
    {
        GameObject currentFigure = null;
        int coo1 = 0;
        int coo2 = 0;
        Color currentColor = Color.None;
        TypeOfFigure currentFigureType = TypeOfFigure.None;

        for (int i = 0; i < positionText.Length; i += 3)
        {
            coo1 = Convert.ToInt32(positionText[i + 1].ToString());
            coo2 = Convert.ToInt32(positionText[i + 2].ToString());

            if (positionText[i] == 'k')
            {
                currentFigure = WhiteKing;
                currentColor = Color.White;
                currentFigureType = TypeOfFigure.King;
            }

            if (positionText[i] == 'w')
            {
                currentFigure = WhitePawn;
                currentColor = Color.White;
                currentFigureType = TypeOfFigure.Pawn;
            }

            if (positionText[i] == 'b')
            {
                currentFigure = BlackPawn;
                currentColor = Color.Black;
                currentFigureType = TypeOfFigure.Pawn;
            }

            SpawnFigure(currentFigure, currentColor, currentFigureType, coo1, coo2);
        }
    }

    [Server]
    public void SpawnFigure(GameObject GO, Color colorOfFigure, TypeOfFigure typeOfFigure, int coo1, int coo2)
    {
        GameObject NewFigure = Instantiate(GO);
        Square currentSquare = BasicBoardScript.BoardReady[coo1, coo2];

        // Спавним фигуру на сервере и синхронизируем с клиентами
        NetworkServer.Spawn(NewFigure);

        NewFigure.name = "Figure";
        NewFigure.transform.position = currentSquare.GO.transform.position;

        // Устанавливаем родителя для фигуры
        ChessPiece chessPiece = NewFigure.GetComponent<ChessPiece>();
        if (chessPiece != null)
        {
            chessPiece.SetParent(currentSquare.GetComponent<NetworkIdentity>());
        }

        // Обновляем состояние ячейки
        currentSquare.FigureColor = colorOfFigure;
        currentSquare.FigureType = typeOfFigure;
        currentSquare.HasFigure = true;
    }
}
