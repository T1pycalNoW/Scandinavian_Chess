using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SyncVar] public bool isWhite; // Синхронизированный цвет игрока
    private ChessPiece selectedPiece;
    private ChessGameManager gameManager;

    public override void OnStartLocalPlayer()
    {
        gameManager = ChessGameManager.Instance;
        
        if (isServer)
        {
            // Первый игрок (хост) — белые
            isWhite = true;
            Debug.Log("Вы играете за белых (сервер)");
        }
        else
        {
            // Второй игрок (клиент) — чёрные
            isWhite = false;
            Debug.Log("Вы играете за чёрных (клиент)");
        }
    }

    void Update()
    {
        if (!isLocalPlayer || gameManager == null) return;

        // Проверяем, что сейчас ход этого игрока
        if (gameManager.isWhiteTurn != isWhite) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (hit.collider != null)
            {
                ChessCell cell = hit.collider.GetComponent<ChessCell>();
                if (cell != null)
                {
                    if (selectedPiece == null)
                    {
                        // Выбираем только свои фигуры
                        ChessPiece piece = gameManager.GetPieceAt(cell.x, cell.y);
                        if (piece != null && piece.isWhite == isWhite)
                        {
                            selectedPiece = piece;
                            Debug.Log($"Выбрана фигура: {piece.name}");
                        }
                    }
                    else
                    {
                        // Отправляем ход на сервер
                        CmdTryMove(selectedPiece.x, selectedPiece.y, cell.x, cell.y);
                        selectedPiece = null;
                    }
                }
            }
        }
    }

    [Command]
    void CmdTryMove(int fromX, int fromY, int toX, int toY)
    {
        gameManager.ServerMovePiece(fromX, fromY, toX, toY, isWhite);
    }
}
