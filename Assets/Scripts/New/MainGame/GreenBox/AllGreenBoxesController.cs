using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AllGreenBoxesController : NetworkBehaviour
{
    public static AllGreenBoxesController Instance;
    public GameObject ConnectedField;
    public bool GreenBoxExist;
    public GameObject GreenBox;
    public List<GameObject> GreenBoxArray = new List<GameObject>();

    void Awake()
    {
        Instance = this;    
    }

    public void AddCopy (GameObject GreenBoxCopy)
    {
        GreenBoxArray.Add(GreenBoxCopy);
    }

    public void DeleteCopies ()
    {
        for(int i = 0; i < GreenBoxArray.Count; i++) 
        {
            //NetworkObjectDestoyer.instance.TellServerToDestroyObject(GreenBoxArray[i]);    
            Destroy(GreenBoxArray[i]);
        }

        GreenBoxArray.Clear();
    }

    public void SetNewCopy(GameObject _ConnectedField, GameObject GreenBoxCopy)
    {
        GreenBoxExist = true;

        ConnectedField = _ConnectedField;

        AddCopy(GreenBoxCopy);
    }

    public void ResetBasicFields ()
    {
        GreenBoxExist = false;
        ConnectedField = null;

        DeleteCopies();
    }
}
