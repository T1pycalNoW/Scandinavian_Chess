using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasicBoardScript : NetworkBehaviour
{
    public static Square[,] BoardReady = new Square[9, 9];

    private Vector2 Offset = new Vector2(-4, 4);
    public GameObject BlackSquarePrefab; // Префаб черной ячейки
    public GameObject WhiteSquarePrefab; // Префаб белой ячейки
    public GameObject BoxPrefab; // Префаб для BoxPrefab
    public DefaultGameManagerScript MainGameManager;
    public AllGreenBoxesController GreenBoxController;

    public override void OnStartServer()
    {
        base.OnStartServer();
        TransformToMatrix();
    }

    [Server]
    public void TransformToMatrix()
    {
        for (int i = 0; i < BoardReady.GetLength(0); i++) // Строки (по Y)
        {
            for (int j = 0; j < BoardReady.GetLength(1); j++) // Столбцы (по X)
            {
                // Определяем, какую ячейку создавать (черную или белую)
                bool isBlack = (i + j) % 2 == 0;
                GameObject squarePrefab = !isBlack ? BlackSquarePrefab : WhiteSquarePrefab;

                // Создаем ячейку с правильным смещением
                Vector3 position = new Vector3(j * 1f + Offset.x, -i * 1f + Offset.y, 0); // Смещение по X и Y
                GameObject squareGO = Instantiate(squarePrefab, position, Quaternion.identity);

                // Спавним ячейку на сервере и синхронизируем с клиентами
                NetworkServer.Spawn(squareGO);

                //squareGO.AddComponent<GreenBoxManagerScript>();

                //SetGreenBoxScriptParametrs(squareGO.GetComponent<GreenBoxManagerScript>());

                // Получаем компонент Square и настраиваем его
                Square square = squareGO.GetComponent<Square>();
                square.GO = squareGO; // Устанавливаем ссылку на GameObject
                square.ID1 = i;
                square.ID2 = j;

                // Добавляем ячейку в массив
                BoardReady[i, j] = square;
            }
        }
    }

    private void SetGreenBoxScriptParametrs (GreenBoxManagerScript GreenBoxScript)
    {
        GreenBoxScript.BoxPrefab = BoxPrefab;
        GreenBoxScript.MainGameManager = MainGameManager;
        GreenBoxScript.GreenBoxController = GreenBoxController;
    }
}