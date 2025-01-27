using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public bool ActiveManager;
    public GameManager gameManager;
    public Puzzle ActivePuzzle;
    public GameObject Menu;
    public Sprite CorrectSprite;
    public Sprite InCorrectSprite;
    public Level ActiveLevel;
    public bool Checked = true;
    public int Rating = 0;
    private int i = 0;
    public List<Puzzle> AllPuzzles = new List<Puzzle>()
    {
        new Puzzle(Level.MegaEasy, "k12w41w71w14w37b30b21b02b03b63b46b77", "12101000", "3020", Color.White, 70),
        new Puzzle(Level.MegaEasy, "k15w31w51w56b04b05b28b38b63b77", "151717070708", "28187757", Color.White, 80),
        new Puzzle(Level.MegaEasy, "k53w33w73w17w47b30b41b25b83b66", "5351517171707080","305083816646", Color.White, 80),
        new Puzzle(Level.MegaEasy, "k41w60w23w63b57b10b20b50b02b15b37b74b86", "416161818180","74727270", Color.White, 80),
        new Puzzle(Level.UltraEasy, "k15w33w72w37w48b12b31b61b06b18b27b66", "1517482817181808","060712141416", Color.White, 85),
        new Puzzle(Level.SuperEasy, "k67w32w15w64w87w58w87b41b71b24b65b85b48b77b78", "6768", "3020", Color.White, 85),
    };

    void Start ()
    {
        if (ActiveManager)
        {
            NewPuzzle(Level.MegaEasy);
        }
    }

    public void NewPuzzle (Level level)
    {
        if(ActiveManager)
        {
            System.Random rnd = new System.Random();

            int i = rnd.Next(0, AllPuzzles.Count);

            while (AllPuzzles[i].level != level)
            {
                i = rnd.Next(0, AllPuzzles.Count);
            }

            ActivePuzzle = AllPuzzles[i];

            gameManager.ClearBoard();
            gameManager.SetPosition(ActivePuzzle.ID);
            gameManager.SideToMove = ActivePuzzle.SideToMove;
            gameManager.canMove = true;

            Menu.SetActive(false);
        }
    }

    public void CheckSolving()
    {
        if (ActiveManager)
        {
            Checked = true;
            if (gameManager.SideToMove != ActivePuzzle.SideToMove)
            {
                for (i = 0; i < 4; i++)
                {
                    Debug.Log($"Массив char: {Convert.ToInt32(ActivePuzzle.RightMove[i].ToString())}. Функция {gameManager.CoordinateTransfer()[i]}");
                    if (gameManager.CoordinateTransfer()[i] != Convert.ToInt32(ActivePuzzle.RightMove[i].ToString()))
                    {
                        Checked = false;
                        break;
                    }
                }

                if (Checked)
                {
                    i = 0;
                    gameManager.TakeFigure(gameManager.BoardReady[int.Parse(ActivePuzzle.RightMove[2].ToString()), int.Parse(ActivePuzzle.RightMove[i].ToString())], int.Parse(ActivePuzzle.RightMove[3].ToString()), int.Parse(ActivePuzzle.RightMove[3].ToString()));
                    ActivePuzzle.RightMove.RemoveRange(0, 4);
                }
                else
                {
                    PuzzleFalled();
                    return;
                }

                if (ActivePuzzle.OpAnwser.Count > 0)
                {
                    gameManager.DoMoves(Convert.ToInt32(ActivePuzzle.OpAnwser[0].ToString()), Convert.ToInt32(ActivePuzzle.OpAnwser[1].ToString()), Convert.ToInt32(ActivePuzzle.OpAnwser[2].ToString()), Convert.ToInt32(ActivePuzzle.OpAnwser[3].ToString()));
                    gameManager.TakeFigure(gameManager.BoardReady[Convert.ToInt32(ActivePuzzle.OpAnwser[2].ToString()), Convert.ToInt32(ActivePuzzle.OpAnwser[3].ToString())], Convert.ToInt32(ActivePuzzle.OpAnwser[2].ToString()), Convert.ToInt32(ActivePuzzle.OpAnwser[3].ToString()));
                    gameManager.SideToMove = ActivePuzzle.SideToMove;
                    ActivePuzzle.OpAnwser.RemoveRange(0, 4);
                }
            }

            if(ActivePuzzle.RightMove.Count == 0)
            {
                gameManager.canMove = false;
                Menu.SetActive(true);
                gameManager.SearchChild(Menu, "UpperPanel").GetComponent<Image>().sprite = CorrectSprite;
                gameManager.SearchChild(Menu, "UpperPanelText").GetComponent<Text>().text = "CORRECT";
                AllPuzzles.Remove(ActivePuzzle);
                Rating +=ActivePuzzle.GivenRating;
                gameManager.SearchChild(Menu, "Rating").transform.GetChild(1).GetComponent<Text>().text = Convert.ToString(Rating);
            }
        }
    }

    public void PuzzleFalled ()
    {
        gameManager.canMove = false;
        Menu.SetActive(true);
        gameManager.SearchChild(Menu, "UpperPanel").GetComponent<Image>().sprite = InCorrectSprite;
        gameManager.SearchChild(Menu, "UpperPanelText").GetComponent<Text>().text = "INCORRECT";
        AllPuzzles.Remove(ActivePuzzle);
        Rating = Rating - ActivePuzzle.GivenRating/4;
        gameManager.SearchChild(Menu, "Rating").transform.GetChild(1).GetComponent<Text>().text = Convert.ToString(Rating);
    }

    public void NextPuzzle ()
    {
        if (Rating < 250)
        {
            ActiveLevel = Level.MegaEasy;
        }

        if (Rating > 250 && Rating < 500)
        {
            ActiveLevel = Level.UltraEasy;
        }

        if (Rating > 500 && Rating < 750)
        {
            ActiveLevel = Level.SuperEasy;
        }

        if (Rating > 750 && Rating < 1000)
        {
            ActiveLevel = Level.Easy;
        }

        NewPuzzle(ActiveLevel);
    }
}

public class Puzzle
{
    public Level level = Level.None;
    public string ID = "";
    public string RightSolve;
    public string OpSolve;
    public int GivenRating;
    public List<char> RightMove = new List<char>();
    public List<char> OpAnwser = new List<char>();
    public Color SideToMove;

    public Puzzle (Level _level, string _ID, string _RightSolve, string _OpSolve, Color _SideToMove, int _GivenRating)
    {
        level = _level;
        ID = _ID;
        RightSolve = _RightSolve;
        OpSolve = _OpSolve;
        SideToMove = _SideToMove;
        GivenRating = _GivenRating;

        for(int i = 0; i < RightSolve.Length; i++) 
        {
            RightMove.Add(RightSolve[i]);    
        }

        for(int i = 0; i < OpSolve.Length; i++) 
        {
            OpAnwser.Add(OpSolve[i]);    
        }
    }
}

public enum Level
{
    None,
    MegaEasy,
    UltraEasy,
    SuperEasy,
    Easy,
    LowerMid,
    Middle,
    UpperMid,
    SuperHard,
    UltraHard,
    MegaHard,
    Insane
}