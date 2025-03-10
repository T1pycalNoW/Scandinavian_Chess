using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DefaultGameManagerScript : MonoBehaviour
{
    public BasicBoardScript BasicBoard;
    public Color SideToMove = Color.Black;
    public Sprite WhiteKing;
    public bool ActiveManager = true;
    public string CurrentGameText;

    private int coo1G1 = 0;
    private int coo2G1 = 0;
    private int coo1G2 = 0;
    private int coo2G2 = 0;
    private Destination destination;
    private bool checkedDestination;
    private GameObject CurrentFigure;

    public bool CheckRules(GameObject G1, GameObject G2)
    {
        //находит фигуры в двухмерном массиве
        FindFigure(G1, G2);

        Debug.Log($"{coo1G1}, {coo2G1}, {coo1G2}, {coo2G2}");

        // запретные клетки
        if (BasicBoardScript.BoardReady[coo1G1, coo2G1].HasFigure)
        {
            if (BasicBoardScript.BoardReady[coo1G1, coo2G1].FigureType != TypeOfFigure.King)
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
        if (AnotherFiguresExist() == false)
        {
            Debug.Log("Другая фигура");
            return false;
        }


        // если идет по диагонали
        if (checkedDestination == false)
        {
            Debug.Log("Фигура идет по диагонали");
            return false;
        }

        // проверка и смена цвета ведущей стороны
        if(RightSideToMove() == false)
        {
            return false;
        }

        return true;
    }

    public bool AnotherFiguresExist()
    {
        checkedDestination = false;

        if (coo1G1 > coo1G2)
        {
            destination = Destination.Up;

            if (coo1G1 - coo1G2 == 1)
            {
                if (BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
                {
                    return false;
                }
            }
            else if (coo1G1 - coo1G2 == 2)
            {
                if (BasicBoardScript.BoardReady[coo1G2 + 1, coo2G2].HasFigure == true || BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
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
                if (BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
                {
                    return false;
                }
            }
            else if (coo1G2 - coo1G1 == 2)
            {
                if (BasicBoardScript.BoardReady[coo1G2 - 1, coo2G2].HasFigure == true || BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
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
                if (BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
                {
                    return false;
                }
            }
            else if (coo2G1 - coo2G2 == 2)
            {
                if (BasicBoardScript.BoardReady[coo1G2, coo2G2 + 1].HasFigure == true || BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
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
                if (BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
                {
                    return false;
                }
            }
            else if (coo2G2 - coo2G1 == 2)
            {
                if (BasicBoardScript.BoardReady[coo1G2, coo2G2 - 1].HasFigure == true || BasicBoardScript.BoardReady[coo1G2, coo2G2].HasFigure == true)
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

        return true;
    }

    public bool RightSideToMove ()
    {
        if (SideToMove != BasicBoardScript.BoardReady[coo1G1, coo2G1].FigureColor)
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

        return true;
    }

    public void TakeFigure(GameObject G1, GameObject G2)
    {
        FindFigure(G1, G2);

        int coo1 = coo1G2;
        int coo2 = coo2G2;

        Square square = BasicBoardScript.BoardReady[coo1, coo2];

        if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2].GO, "Figure"))
        {
            if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2].GO, "Figure").GetComponent<SpriteRenderer>().sprite == WhiteKing)
            {
                if ((coo1 == 0 && coo2 == 0) || (coo1 == 0 && coo2 == 8) || (coo1 == 8 && coo2 == 0) || (coo1 == 8 && coo2 == 8))
                {
                    Win(Color.White);
                }
            }
        }
            if (coo1 != 7 && coo1 != 8)
            {
                if (BasicBoardScript.BoardReady[coo1 + 1, coo2].FigureColor != square.FigureColor)
                {
                    if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 + 1, coo2].GO, "Figure"))
                    {
                        if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 + 1, coo2].GO, "Figure").GetComponent<SpriteRenderer>().sprite == WhiteKing)
                        {
                            if (BasicBoardScript.BoardReady[coo1 + 2, coo2].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 + 1, coo2 - 1].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 + 1, coo2 + 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BasicBoardScript.BoardReady[coo1 + 2, coo2].FigureColor == square.FigureColor || (coo1+2 == 8 && coo2 == 0) || (coo1+2 == 8 && coo2 == 8))
                            {
                                if(BasicBoardScript.BoardReady[coo1 + 1, coo2].FigureColor == Color.Black && ActiveManager)
                                {
                                    CurrentGameText += $"b{coo1 + 1}{coo2}";
                                }

                                else if(ActiveManager)
                                {
                                    CurrentGameText += $"w{coo1 + 1}{coo2}";
                                }

                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 + 1, coo2].GO, "Figure").GetComponent<SpriteRenderer>().sprite = null;
                                BasicBoardScript.BoardReady[coo1 + 1, coo2].HasFigure = false;
                                BasicBoardScript.BoardReady[coo1 + 1, coo2].FigureColor = Color.None;
                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 + 1, coo2].GO, "Figure").SetActive(false);
                            }
                        }
                    }
                }
            }

            if (coo1 != 0 && coo1 != 1)
            {
                if (BasicBoardScript.BoardReady[coo1 - 1, coo2].FigureColor != square.FigureColor)
                {
                    if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 - 1, coo2].GO, "Figure"))
                    {
                        if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 - 1, coo2].GO, "Figure").GetComponent<SpriteRenderer>().sprite == WhiteKing)
                        {
                            if (BasicBoardScript.BoardReady[coo1 - 2, coo2].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 - 1, coo2 - 1].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 - 1, coo2 + 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BasicBoardScript.BoardReady[coo1 - 2, coo2].FigureColor == square.FigureColor || (coo1-2 == 0 && coo2 == 0) || (coo1-2 == 0 && coo2 == 8))
                            {
                                if(BasicBoardScript.BoardReady[coo1 - 1, coo2].FigureColor == Color.Black && ActiveManager)
                                {
                                    CurrentGameText += $"b{coo1 - 1}{coo2}";
                                }

                                else if (ActiveManager)
                                {
                                    CurrentGameText += $"w{coo1 - 1}{coo2}";
                                }

                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 - 1, coo2].GO, "Figure").GetComponent<SpriteRenderer>().sprite = null;
                                BasicBoardScript.BoardReady[coo1 - 1, coo2].HasFigure = false;
                                BasicBoardScript.BoardReady[coo1 - 1, coo2].FigureColor = Color.None;
                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1 - 1, coo2].GO, "Figure").SetActive(false);
                            }
                        }
                    }

                }
            }

            if (coo2 != 7 && coo2 != 8)
            {
                if (BasicBoardScript.BoardReady[coo1, coo2 + 1].FigureColor != square.FigureColor)
                {
                    if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2+1].GO, "Figure"))
                    {
                        if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2 + 1].GO, "Figure").GetComponent<SpriteRenderer>().sprite == WhiteKing)
                        {
                            if (BasicBoardScript.BoardReady[coo1, coo2 + 2].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 - 1, coo2 + 1].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 + 1, coo2 + 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BasicBoardScript.BoardReady[coo1, coo2 + 2].FigureColor == square.FigureColor || (coo1 == 8 && coo2 +2 == 8) || (coo1 == 0 && coo2+2 == 8))
                            {
                                if(BasicBoardScript.BoardReady[coo1, coo2 + 1].FigureColor == Color.Black && ActiveManager)
                                {
                                    CurrentGameText += $"b{coo1}{coo2 + 1}";
                                }

                                else if (ActiveManager)
                                {
                                    CurrentGameText += $"w{coo1}{coo2 + 1}";
                                }

                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2 + 1].GO, "Figure").GetComponent<SpriteRenderer>().sprite = null;
                                BasicBoardScript.BoardReady[coo1, coo2 + 1].HasFigure = false;
                                BasicBoardScript.BoardReady[coo1, coo2 + 1].FigureColor = Color.None;
                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2 + 1].GO, "Figure").SetActive(false);
                            }
                        }
                    }

                }
            }

            if (coo2 != 1 && coo2 != 0)
            {
                if (BasicBoardScript.BoardReady[coo1, coo2 - 1].FigureColor != square.FigureColor)
                {
                    if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2-1].GO, "Figure"))
                    {
                        if (HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2 - 1].GO, "Figure").GetComponent<SpriteRenderer>().sprite == WhiteKing)
                        {
                            if (BasicBoardScript.BoardReady[coo1, coo2 - 2].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 - 1, coo2 - 1].FigureColor == square.FigureColor && BasicBoardScript.BoardReady[coo1 + 1, coo2 - 1].FigureColor == square.FigureColor)
                            {
                                Win(Color.Black);
                            }
                        }
                        else
                        {
                            if (BasicBoardScript.BoardReady[coo1, coo2 - 2].FigureColor == square.FigureColor || (coo1 == 8 && coo2 - 2 == 0) || (coo1 == 0 && coo2 - 2 == 0)) 
                            {
                                if(BasicBoardScript.BoardReady[coo1, coo2 - 1].FigureColor == Color.Black && ActiveManager)
                                {
                                    CurrentGameText += $"b{coo1}{coo2 - 1}";
                                }

                                else if (ActiveManager)
                                {
                                    CurrentGameText += $"w{coo1}{coo2 - 1}";
                                }
                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2 - 1].GO, "Figure").GetComponent<SpriteRenderer>().sprite = null;
                                BasicBoardScript.BoardReady[coo1, coo2 - 1].HasFigure = false;
                                BasicBoardScript.BoardReady[coo1, coo2 - 1].FigureColor = Color.None;
                                HelpfulMode.SearchChild(BasicBoardScript.BoardReady[coo1, coo2 - 1].GO, "Figure").SetActive(false);
                            }
                        }
                    }

                }
            }

        /*if(isLocalPlayer)
        {
            chessGameOnline.CmdUpdateBoardState(ScanBoard());
        }*/
    }

    public void Win(Color color)
    {
        if (ActiveManager)
        {
            /*canMove = false;

            WinnerMenu.SetActive(true);
            if (ActiveManager)
            {
                GameObject.FindGameObjectWithTag("InformationManager").GetComponent<InformationScript>().CurrentText = CurrentGameText;
            }
            SearchChild(WinnerMenu, "ColorOfWinner").GetComponent<Text>().text = $"{color}".ToUpper();*/

            Debug.Log($"{color}".ToUpper());
        }
    }

    public void MoveFigures(GameObject G1, GameObject G2)
    {
        UpdateCurrentFigure(G1);

        Square S1 = null;
        Square S2 = null;
        Color color = Color.None;


        CurrentFigure.transform.position = G2.transform.position;
        CurrentFigure.transform.SetParent(null);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (BasicBoardScript.BoardReady[i, j].GO == G1)
                {
                    S1 = BasicBoardScript.BoardReady[i, j];
                }
                if (BasicBoardScript.BoardReady[i, j].GO == G2)
                {
                    S2 = BasicBoardScript.BoardReady[i, j];
                }
            }
        }

        S1.HasFigure = false;
        S2.HasFigure = true;

        color = S1.FigureColor;
        S2.FigureType = S1.FigureType;
        S1.FigureType = TypeOfFigure.None;
        S1.FigureColor = Color.None;
        S2.FigureColor = color;
    }

    public void FindFigure(GameObject G1, GameObject G2)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (BasicBoardScript.BoardReady[i, j].GO == G1)
                {
                    coo1G1 = i;
                    coo2G1 = j;
                }
                if (BasicBoardScript.BoardReady[i, j].GO == G2)
                {
                    coo1G2 = i;
                    coo2G2 = j;
                }
            }
        }
    }

    public void UpdateCurrentFigure (GameObject G1)
    {
        CurrentFigure = HelpfulMode.SearchChild(G1, "Figure");
    }
}
