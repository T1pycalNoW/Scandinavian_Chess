using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] Field;
    public Square[,] BoardReady = new Square[9, 9];
    public Sprite WhitePawn;
    public Sprite WhiteKing;
    public Sprite BlackPawn;
    public Sprite GreenBox;
    public PuzzleManager puzzleManager;
    public AnalysisManager analysisManager;
    public GameObject GreenBoxCopy;
    public GameObject WinnerMenu;
    public GameObject EscapeMenu;
    public Color SideToMove = Color.Black;
    public string CurrentGameText;
    public bool ActiveManager;
    public bool canMove = true;
    public bool GreenBoxCopyExist;
    public int coo1G1 = 0;
    public int coo2G1 = 0;
    public int coo1G2 = 0;
    public int coo2G2 = 0;
    public int a1 = 0;
    public int b1 = 0;
    public int a2 = 0;
    public int b2 = 0;

    void Start()
    {
        // Перенос массива GameObject в массив массивов Square
        for (int i = 0, m = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++, m++)
            {
                BoardReady[i, j] = new Square(Field[m], i, j);
            }
        }
        
            if (ActiveManager)
        {
            ClearBoard();
            CurrentGameText = "";
            SetPosition("b03b04b05b14b30b38b40b41b47b48b50b58b74b83b84b85w33w34w35w43k44w45w53w54w55");
            SideToMove = Color.Black;
            GameObject.FindGameObjectWithTag("InformationManager").GetComponent<InformationScript>().CurrentText = "";
        }
    }
    public void SetPosition(string str)
    {
        //Метод устанавливает позицию на доске по данному коду, код должен быть вида {Фигура, первая координата, вторая координата}, например: k34w78b00

        int i = 0;
        char name = ' ';
        Sprite currentFigure = null;
        Color currentColor = Color.White;
        float newScale = 1f;
        int a1 = 0;
        int a2 = 0;

        // расстановка позиции по заранее данной строке
        for (i = 0; i < str.Length; i += 3)
        {
            name = str[i];
            a1 = Convert.ToInt32(str[i + 1].ToString());
            a2 = Convert.ToInt32(str[i + 2].ToString());

            if (name == 'k')
            {
                currentFigure = WhiteKing;
                currentColor = Color.White;
                newScale = 0.85f;
            }
            if (name == 'w')
            {
                currentFigure = WhitePawn;
                currentColor = Color.White;
            }
            if (name == 'b')
            {
                currentFigure = BlackPawn;
                currentColor = Color.Black;
            }

            GameObject child = null;
            
            if(SearchChild(BoardReady[a1, a2].GO, "Figure"))
            {
                child = SearchChild(BoardReady[a1, a2].GO, "Figure");
            }

            child.SetActive(true);
            child.GetComponent<Image>().sprite = currentFigure;
            child.transform.localScale = new Vector3(newScale, newScale, newScale);

            newScale = 1f;

            BoardReady[a1, a2].HasFigure = true;
            BoardReady[a1, a2].FigureColor = currentColor;

            if (i + 3 >= str.Length)
            {
                break;
            }
        }
    }

    // квадратик для мышки
    public void SpawnGreenBox(Image image)
    {
        image.sprite = GreenBox;
    }

    // существует ли фигура
    public bool FigureExist(GameObject gameObject)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (BoardReady[i, j].GO == gameObject && BoardReady[i, j].HasFigure)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //ищет детей
    public GameObject SearchChild(GameObject gameObject, string name)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == name)
            {
                return gameObject.transform.GetChild(i).gameObject;
            }
        }

        return null;
    }

    public bool CheckRules(GameObject G1, GameObject G2)
    {
        coo1G1 = 0;
        coo2G1 = 0;
        coo1G2 = 0;
        coo2G2 = 0;

        bool checkedDestination = false;

        Destination destination = Destination.Up;

        //находит фигуры в двухмерном массиве
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (BoardReady[i, j].GO == G1)
                {
                    coo1G1 = i;
                    coo2G1 = j;
                }
                if (BoardReady[i, j].GO == G2)
                {
                    coo1G2 = i;
                    coo2G2 = j;
                }
            }
        }

        // запретные клетки
        if (SearchChild(BoardReady[coo1G1, coo2G1].GO, "Figure").GetComponent<Image>())
        {
            if (SearchChild(BoardReady[coo1G1, coo2G1].GO, "Figure").GetComponent<Image>().sprite != WhiteKing)
            {
                if ((coo1G2 == 4 && coo2G2 == 4) || (coo1G2 == 0 && coo2G2 == 0) || (coo1G2 == 0 && coo2G2 == 8) || (coo1G2 == 8 && coo2G2 == 0) || (coo1G2 == 8 && coo2G2 == 8))
                {
                    Debug.Log("Запретные клетки");
                    return false;
                }
            }
        }

        // если не сдвинулся
        if (coo1G1 == coo1G2 && coo2G1 == coo2G2)
        {
            Debug.Log("Фигура осталась на месте");
            return false;
        }

        // проверки на наличие других фигур в сторону, в которую идет фигура

        try {
            if (coo1G1 > coo1G2)
            {
                destination = Destination.Up;

                if (coo1G1 - coo1G2 == 1)
                {
                    if (BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else if (coo1G1 - coo1G2 == 2)
                {
                    if (BoardReady[coo1G2 + 1, coo2G2].HasFigure == true || BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                checkedDestination = true;
            }

            if (coo1G1 < coo1G2)
            {
                destination = Destination.Down;

                if (checkedDestination)
                {
                    return false;
                }

                if (coo1G2 - coo1G1 == 1)
                {
                    if (BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else if (coo1G2 - coo1G1 == 2)
                {
                    if (BoardReady[coo1G2 - 1, coo2G2].HasFigure == true || BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                checkedDestination = true;
            }

            if (coo2G1 > coo2G2)
            {
                destination = Destination.Left;

                if (checkedDestination)
                {
                    return false;
                }

                if (coo2G1 - coo2G2 == 1)
                {
                    if (BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else if (coo2G1 - coo2G2 == 2)
                {
                    if (BoardReady[coo1G2, coo2G2 + 1].HasFigure == true || BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                checkedDestination = true;
            }

            if (coo2G1 < coo2G2)
            {
                destination = Destination.Right;

                if (checkedDestination)
                {
                    return false;
                }

                if (coo2G2 - coo2G1 == 1)
                {
                    if (BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else if (coo2G2 - coo2G1 == 2)
                {
                    if (BoardReady[coo1G2, coo2G2 - 1].HasFigure == true || BoardReady[coo1G2, coo2G2].HasFigure == true)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                checkedDestination = true;
            }
        }
        
        catch (Exception e)
        {

        }

        // если идет по диагонали
        if (checkedDestination == false)
        {
            Debug.Log("Фигура идет по диагонали");
            return false;
        }

        // проверка и смена цвета ведущей стороны
        if (SideToMove != BoardReady[coo1G1, coo2G1].FigureColor)
        {
            Debug.Log("Ход не той стороны");
            return false;
        }
        else
        {
            if (SideToMove == Color.White)
            {
                SideToMove = Color.Black;
            }
            else
            {
                SideToMove = Color.White;
            }
        }

        CoordinateTransfer(coo1G1,coo1G2,coo2G1,coo2G2);

        puzzleManager.CheckSolving();

        G2.GetComponent<SquareColl>().coo1 = coo1G2;
        G2.GetComponent<SquareColl>().coo2 = coo2G2;

        if(ActiveManager)
        {
            CurrentGameText += coo1G1.ToString();
            CurrentGameText += coo2G1.ToString();
            CurrentGameText += coo1G2.ToString();
            CurrentGameText += coo2G2.ToString();
        }

        return true;
    }

    // проверка на взятие фигуры
    public void TakeFigure(Square square, int coo1, int coo2)
    {
        if (SearchChild(BoardReady[coo1, coo2].GO, "Figure"))
        {
            if (SearchChild(BoardReady[coo1, coo2].GO, "Figure").GetComponent<Image>().sprite == WhiteKing)
            {
                if ((coo1 == 0 && coo2 == 0) || (coo1 == 0 && coo2 == 8) || (coo1 == 8 && coo2 == 0) || (coo1 == 8 && coo2 == 8))
                {
                    Win(Color.White);
                }
            }
        }

        try {
            if (coo1 != 7 && coo1 != 8)
            {
                if (BoardReady[coo1 + 1, coo2].FigureColor != square.FigureColor)
                {
                    if (SearchChild(BoardReady[coo1 + 1, coo2].GO, "Figure"))
                    {
                        if (SearchChild(BoardReady[coo1 + 1, coo2].GO, "Figure").GetComponent<Image>().sprite == WhiteKing)
                        {
                            if (BoardReady[coo1 + 2, coo2].FigureColor == square.FigureColor && BoardReady[coo1 + 1, coo2 - 1].FigureColor == square.FigureColor && BoardReady[coo1 + 1, coo2 + 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BoardReady[coo1 + 2, coo2].FigureColor == square.FigureColor || (coo1+2 == 8 && coo2 == 0) || (coo1+2 == 8 && coo2 == 8))
                            {
                                SearchChild(BoardReady[coo1 + 1, coo2].GO, "Figure").GetComponent<Image>().sprite = null;
                                BoardReady[coo1 + 1, coo2].HasFigure = false;
                                BoardReady[coo1 + 1, coo2].FigureColor = Color.None;
                                SearchChild(BoardReady[coo1 + 1, coo2].GO, "Figure").SetActive(false);
                            }
                        }
                    }
                }
            }

            if (coo1 != 0 && coo1 != 1)
            {
                if (BoardReady[coo1 - 1, coo2].FigureColor != square.FigureColor)
                {
                    if (SearchChild(BoardReady[coo1 - 1, coo2].GO, "Figure"))
                    {
                        if (SearchChild(BoardReady[coo1 - 1, coo2].GO, "Figure").GetComponent<Image>().sprite == WhiteKing)
                        {
                            if (BoardReady[coo1 - 2, coo2].FigureColor == square.FigureColor && BoardReady[coo1 - 1, coo2 - 1].FigureColor == square.FigureColor && BoardReady[coo1 - 1, coo2 + 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BoardReady[coo1 - 2, coo2].FigureColor == square.FigureColor || (coo1-2 == 0 && coo2 == 0) || (coo1-2 == 0 && coo2 == 8))
                            {
                                SearchChild(BoardReady[coo1 - 1, coo2].GO, "Figure").GetComponent<Image>().sprite = null;
                                BoardReady[coo1 - 1, coo2].HasFigure = false;
                                BoardReady[coo1 - 1, coo2].FigureColor = Color.None;
                                SearchChild(BoardReady[coo1 - 1, coo2].GO, "Figure").SetActive(false);
                            }
                        }
                    }

                }
            }

            if (coo2 != 7 && coo2 != 8)
            {
                if (BoardReady[coo1, coo2 + 1].FigureColor != square.FigureColor)
                {
                    if (SearchChild(BoardReady[coo1, coo2+1].GO, "Figure"))
                    {
                        if (SearchChild(BoardReady[coo1, coo2 + 1].GO, "Figure").GetComponent<Image>().sprite == WhiteKing)
                        {
                            if (BoardReady[coo1, coo2 + 2].FigureColor == square.FigureColor && BoardReady[coo1 - 1, coo2 + 1].FigureColor == square.FigureColor && BoardReady[coo1 + 1, coo2 + 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BoardReady[coo1, coo2 + 2].FigureColor == square.FigureColor || (coo1 == 8 && coo2 +2 == 8) || (coo1 == 0 && coo2+2 == 8))
                            {
                                SearchChild(BoardReady[coo1, coo2 + 1].GO, "Figure").GetComponent<Image>().sprite = null;
                                BoardReady[coo1, coo2 + 1].HasFigure = false;
                                BoardReady[coo1, coo2 + 1].FigureColor = Color.None;
                                SearchChild(BoardReady[coo1, coo2 + 1].GO, "Figure").SetActive(false);
                            }
                        }
                    }

                }
            }

            if (coo2 != 1 && coo2 != 0)
            {
                if (BoardReady[coo1, coo2 - 1].FigureColor != square.FigureColor)
                {
                    if (SearchChild(BoardReady[coo1, coo2-1].GO, "Figure"))
                    {
                        if (SearchChild(BoardReady[coo1, coo2 - 1].GO, "Figure").GetComponent<Image>().sprite == WhiteKing)
                        {
                            if (BoardReady[coo1, coo2 - 2].FigureColor == square.FigureColor && BoardReady[coo1 - 1, coo2 - 1].FigureColor == square.FigureColor && BoardReady[coo1 + 1, coo2 - 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BoardReady[coo1, coo2 - 2].FigureColor == square.FigureColor || (coo1 == 8 && coo2 - 2 == 0) || (coo1 == 0 && coo2 - 2 == 0)) 
                            {
                                SearchChild(BoardReady[coo1, coo2 - 1].GO, "Figure").GetComponent<Image>().sprite = null;
                                BoardReady[coo1, coo2 - 1].HasFigure = false;
                                BoardReady[coo1, coo2 - 1].FigureColor = Color.None;
                                SearchChild(BoardReady[coo1, coo2 - 1].GO, "Figure").SetActive(false);
                            }
                        }
                    }

                }
            }
        }
        catch (Exception e)
        {

        }
    }

    // меняет местами фигуры и их параметры
    public void MoveFigures(GameObject G1, GameObject G2)
    {
        Sprite sprite = SearchChild(G1, "Figure").GetComponent<Image>().sprite;
        Square S1 = null;
        Square S2 = null;
        Color color = Color.None;

        if (SearchChild(G1, "Figure"))
        {
            SearchChild(G1, "Figure").GetComponent<Image>().sprite = null;
        }

        if (SearchChild(G2, "Figure"))
        {
            SearchChild(G2, "Figure").GetComponent<Image>().sprite = sprite;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (BoardReady[i, j].GO == G1)
                {
                    S1 = BoardReady[i, j];
                }
                if (BoardReady[i, j].GO == G2)
                {
                    S2 = BoardReady[i, j];
                }
            }
        }

        S1.HasFigure = false;
        S2.HasFigure = true;

        color = S1.FigureColor;
        S1.FigureColor = Color.None;
        S2.FigureColor = color;
    }

    public void SwapActive(GameObject G1, GameObject G2)
    {
        if (SearchChild(G1, "Figure"))
        {
            SearchChild(G1, "Figure").SetActive(false);
        }

        if (SearchChild(G2, "Figure"))
        {
            SearchChild(G2, "Figure").SetActive(true);
        }
    }

    public void CoordinateTransfer (int _a1, int _b1, int _a2, int _b2)
    {
        a1 = _a1;
        b1 = _b1;
        a2 = _a2;
        b2 = _b2;
    }

    public int [] CoordinateTransfer ()
    {
        return new int [] {a1, a2, b1, b2};
    }

    public void Win(Color color)
    {
        if (ActiveManager)
        {
            canMove = false;

            WinnerMenu.SetActive(true);
            if (ActiveManager)
            {
                GameObject.FindGameObjectWithTag("InformationManager").GetComponent<InformationScript>().CurrentText = CurrentGameText;
            }
            SearchChild(WinnerMenu, "ColorOfWinner").GetComponent<Text>().text = $"{color}".ToUpper();
        }
    }

    public void SwapToMainMenu ()
    {
        canMove = true;
        ClearBoard();
        SceneManager.LoadScene("MainScene");
        
        if (ActiveManager)
        {
            GameObject.FindGameObjectWithTag("InformationManager").GetComponent<InformationScript>().CurrentText = CurrentGameText;
        }
        
        ActiveManager = false;
        puzzleManager.ActiveManager = false;
        analysisManager.ActiveManager = false;
    }

    public void Restart ()
    {
        SceneManager.LoadScene("SampleScene");
        canMove = true;
    }

    public void ClearBoard ()
    {
        for(int i = 0; i < 9; i++) 
        {
            for(int j = 0; j < 9; j++) 
            {
                if (SearchChild(BoardReady[i,j].GO, "Figure"))
                {
                    if(SearchChild(BoardReady[i,j].GO, "Figure").GetComponent<Image>())
                    {
                        SearchChild(BoardReady[i, j].GO, "Figure").GetComponent<Image>().sprite = null;
                        SearchChild(BoardReady[i, j].GO, "Figure").SetActive(false);
                        BoardReady[i, j].HasFigure = false;
                        BoardReady[i, j].FigureColor = Color.None;
                    }
                    
                }
            }           
        }
    }

    public void CloseEscMenu ()
    {
        EscapeMenu.SetActive(false);
    }

    public void DoMoves (int coo1, int coo2, int coo3, int coo4)
    {
        BoardReady[coo3, coo4].HasFigure = true;
        BoardReady[coo3, coo4].FigureColor = BoardReady[coo1, coo2].FigureColor;
        SearchChild(BoardReady[coo3, coo4].GO, "Figure").GetComponent<Image>().sprite = SearchChild(BoardReady[coo1, coo2].GO, "Figure").GetComponent<Image>().sprite;
        SearchChild(BoardReady[coo1, coo2].GO, "Figure").GetComponent<Image>().sprite = null;
        SearchChild(BoardReady[coo3, coo4].GO, "Figure").SetActive(true);
        SearchChild(BoardReady[coo1, coo2].GO, "Figure").SetActive(false);
        BoardReady[coo1, coo2].HasFigure = false;
        BoardReady[coo1, coo2].FigureColor = Color.None;

    }

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeMenu.SetActive(true);
        }
    }
}

public class Square
{
    public GameObject GO;
    public int ID1;
    public int ID2;
    public Color FigureColor = Color.None;
    public bool HasFigure = false;

    public Square(GameObject _GO, int _ID1, int _ID2)
    {
        GO = _GO;
        ID1 = _ID1;
        ID2 = _ID2;
    }
}

public class Position
{
    public string ID;
    public string Name;
}

public enum Color
{
    White,
    Black,
    None
}

public enum Destination
{
    Up,
    Down,
    Left,
    Right
}


// Ключ для стандартной расстановки: b03b04b05b14b30b38b40b41b47b48b50b58b74b83b84b85w33w34w35w43k44w45w53w54w55