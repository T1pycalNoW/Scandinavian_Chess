using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public List<MenuItemInfo> ButtonList = new List<MenuItemInfo>();
    public GameObject SubItemPrefab;
    private bool isClicked = false;
    private MenuSystem menuSystem;

    void Start()
    {
        menuSystem = MenuSystem.instance;
    }

    public void ButtonIsClicked ()
    {
        if(isClicked == false)
        {
            for (int i = 0; i < menuSystem.createdSubPanels.Count; i++)
            {
                Destroy(menuSystem.createdSubPanels[i]);
            }

            menuSystem.createdSubPanels.Clear();

            for (int i = 0; i < ButtonList.Count; i++)
            {
                GameObject item = Instantiate(SubItemPrefab, menuSystem.EnteredMenu.transform);

                item.GetComponent<Button>().onClick.AddListener(ButtonList[i].OnClick.Invoke);

                item.GetComponentInChildren<Text>().text = ButtonList[i].Name;

                menuSystem.createdSubPanels.Add(item);
            }

            isClicked = true;

            menuSystem.playPanelAnimator.SetBool("isShow", true);
        }
        
        else
        {
            isClicked = false;

            menuSystem.playPanelAnimator.SetBool("isShow", false);
        }
    }
}

[Serializable]
public class MenuItemInfo 
{
    public string Name;
    public UnityEvent OnClick;
}