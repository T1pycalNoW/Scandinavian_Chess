using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class InformationScript : MonoBehaviour
{
    public string CurrentText;
    public bool isOnlyOne = false;
    public bool canAddAnother = false;
    void Awake()
    {
        if(isOnlyOne)
        {
            DontDestroyOnLoad(this.gameObject);
        }  
    }
}
