using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalysisManager : MonoBehaviour
{
    public bool ActiveManager;
    public GameManager gameManager;
    public string CurrentGameText = "";
    public int CurrentIndex;

    void Start()
    {
        if (ActiveManager)
        {
            CurrentIndex = 0;

            if(GameObject.FindGameObjectWithTag("InformationManager"))
            {
                CurrentGameText = GameObject.FindGameObjectWithTag("InformationManager").GetComponent<InformationScript>().CurrentText;
            }
            else {
                ActiveManager = false;
            }

            gameManager.ClearBoard();
            gameManager.SetPosition("b03b04b05b14b30b38b40b41b47b48b50b58b74b83b84b85w33w34w35w43k44w45w53w54w55");
            gameManager.canMove = false;
        }
    }

    void Update ()
    {
        if (ActiveManager)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (CurrentGameText.Length - CurrentIndex - 1 > 0)
                {
                    if(CurrentGameText[CurrentIndex+4].ToString() != "b" && CurrentGameText[CurrentIndex+4].ToString() != "w")
                    {
                        gameManager.DoMoves(Convert.ToInt32(CurrentGameText[CurrentIndex].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex + 1].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex + 2].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex + 3].ToString()));
                        CurrentIndex+=4;
                    }
                    else
                    {
                        gameManager.TakeFigure(gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())], Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString()));
                        CurrentIndex+=7;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (CurrentIndex - 3 > 0)
                {
                    if(CurrentGameText[CurrentIndex-3].ToString() == "b" || CurrentGameText[CurrentIndex-3].ToString() == "w")
                    {
                        if(CurrentGameText[CurrentIndex-3].ToString() != "b")
                        {
                            gameManager.SearchChild(gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].GO, "Figure").GetComponent<Image>().sprite = gameManager.BlackPawn;
                            gameManager.SearchChild(gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].GO, "Figure").SetActive(true);
                            gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].HasFigure = false;
                            gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].FigureColor = Color.Black;
                            CurrentIndex -=3;
                        }
                        else
                        {
                            gameManager.SearchChild(gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].GO, "Figure").GetComponent<Image>().sprite = gameManager.WhitePawn;
                            gameManager.SearchChild(gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].GO, "Figure").SetActive(true);
                            gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].HasFigure = false;
                            gameManager.BoardReady[Convert.ToInt32(CurrentGameText[CurrentIndex+5].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex+6].ToString())].FigureColor = Color.White;
                            CurrentIndex -=3;
                        }
                        
                        gameManager.DoMoves(Convert.ToInt32(CurrentGameText[CurrentIndex - 2].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex - 1].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex - 4].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex - 3].ToString()));
                        CurrentIndex -=4;
                    }
                }
            }
        }
    }
}