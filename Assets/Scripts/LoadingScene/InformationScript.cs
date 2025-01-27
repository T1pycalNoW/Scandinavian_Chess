using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationScript : MonoBehaviour
{
    public string CurrentText;
    public bool isOnlyOne = false;
    public bool canAddAnother = false;
    void Start()
    {
        if(isOnlyOne)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
