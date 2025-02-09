using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    public static MenuSystem instance;

    public GameObject EnteredMenu;

    public Animator playPanelAnimator;
    public List<GameObject> createdSubPanels = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playPanelAnimator = EnteredMenu.GetComponent<Animator>();
    }
}
