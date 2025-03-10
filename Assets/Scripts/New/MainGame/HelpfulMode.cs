using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class HelpfulMode
{
    public static GameObject SearchChild(GameObject gameObject, string name)
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
}
public enum Color
{
    White,
    Black,
    None
}

public enum TypeOfFigure
{
    King, 
    Pawn,
    None
}