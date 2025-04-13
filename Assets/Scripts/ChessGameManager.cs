using UnityEngine;
using Mirror;

public class ChessGameManager : NetworkBehaviour
{
    public static ChessGameManager Instance;
    public ChessPiece[,] board = new ChessPiece[9, 9]; // Для доски 9x9
    private ChessPiece selectedPiece;
    [SyncVar] public bool isWhiteTurn = true;

    [Server]
    void InitializeBoard()
    {
        Debug.Log("Сервер: началась инициализация доски");
        
        ChessPiece[] allPieces = FindObjectsOfType<ChessPiece>();
        Debug.Log($"Найдено фигур: {allPieces.Length}");

        foreach (ChessPiece piece in allPieces)
        {
            int x = Mathf.RoundToInt(piece.transform.position.x);
            int y = Mathf.RoundToInt(piece.transform.position.y);
            
            Debug.Log($"Обработка {piece.name} на позиции ({x},{y})");

            if (x >= 0 && x < 9 && y >= 0 && y < 9)
            {
                board[x, y] = piece;
                piece.x = x;
                piece.y = y;
                NetworkServer.Spawn(piece.gameObject);
                Debug.Log($"Добавлено в board[{x},{y}]: {piece.name}");
            }
            else
            {
                Debug.LogError($"Фигура {piece.name} имеет недопустимые координаты ({x},{y})!");
            }
        }

        // Правильная проверка заполнения board
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (board[x, y] != null)
                {
                    Debug.Log($"board[{x},{y}] содержит {board[x, y].name}");
                }
            }
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (isServer)
        {
            InitializeBoard();
        }
    }

    [Server]
    public void ServerMovePiece(int fromX, int fromY, int toX, int toY, bool movingPlayerIsWhite)
    {
        // Проверяем, что ход делает правильный игрок
        if (isWhiteTurn != movingPlayerIsWhite)
        {
            Debug.Log("Не ваш ход!");
            return;
        }

        ChessPiece piece = board[fromX, fromY];
        if (piece != null && piece.isWhite == movingPlayerIsWhite)
        {
            board[toX, toY] = piece;
            board[fromX, fromY] = null;
            piece.MoveTo(toX, toY);
            
            // Меняем очередь
            isWhiteTurn = !isWhiteTurn;
            Debug.Log(isWhiteTurn ? "Ход белых" : "Ход чёрных");
        }
    }

    public ChessPiece GetPieceAt(int x, int y)
    {
        if (x >= 0 && x < 9 && y >= 0 && y < 9)
            return board[x, y];
        return null;
    }
}
