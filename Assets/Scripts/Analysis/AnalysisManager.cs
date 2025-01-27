using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    gameManager.DoMoves(Convert.ToInt32(CurrentGameText[CurrentIndex].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex + 1].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex + 2].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex + 3].ToString()));
                
                    CurrentIndex+=4;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (CurrentIndex - 3 > 0)
                {
                    gameManager.DoMoves(Convert.ToInt32(CurrentGameText[CurrentIndex - 2].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex - 1].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex - 4].ToString()), Convert.ToInt32(CurrentGameText[CurrentIndex - 3].ToString()));
                
                    CurrentIndex -=4;
                }
            }
        }
    }
}