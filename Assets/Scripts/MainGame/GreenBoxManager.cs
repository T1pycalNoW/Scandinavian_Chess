using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBoxManager : MonoBehaviour
{
    public List<GameObject> GreenBoxArray = new List<GameObject>();
    private float timer = 0f;
    public float timerToDestroy;
    public bool canDestroy = false;

    public void AddBox (GameObject box)
    {
        GreenBoxArray.Add(box);

        canDestroy = true;
    }

    void Update() 
    {
        if (canDestroy)
        {
            timer += Time.deltaTime;
        }

        if (timer >= timerToDestroy)
        {
            for (int i = 0; i < GreenBoxArray.Count; i++)
            {
                if(GreenBoxArray[i])
                {
                    Destroy(GreenBoxArray[i]);
                }
            }
            GreenBoxArray.Clear();
            
            canDestroy = false;
            timer = 0f;
        }
    }

    public void ClearBox ()
    {
        GreenBoxArray.Clear();
    }
}
