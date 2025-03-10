using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    bool PuzzleManager = false;
    bool GameManager = false;
    bool AnalysisManager = false;
    public string CurrentText;

    void Awake()
    {
        if(GameObject.FindGameObjectWithTag("InformationManager"))
        {
            GameObject.FindGameObjectWithTag("InformationManager").GetComponent<InformationScript>().isOnlyOne = true;
        }      
    }

    void Start ()
    {
        PuzzleManager = false;
        GameManager = false;
        AnalysisManager = false;

        Debug.Log("Game started");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            Debug.Log("Scene loaded");

            if (GameObject.FindGameObjectWithTag("PuzzleManager") && PuzzleManager)
            {
                GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>().ActiveManager = true;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ActiveManager = false;
                GameObject.FindGameObjectWithTag("AnalysisManager").GetComponent<AnalysisManager>().ActiveManager = false;
            }
            else
            {
                Debug.Log("Not foundedPuzzle");
            }

            if (GameObject.FindGameObjectWithTag("GameManager") && GameManager)
            {
                GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>().ActiveManager = false;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ActiveManager = true;
                GameObject.FindGameObjectWithTag("AnalysisManager").GetComponent<AnalysisManager>().ActiveManager = false;
            }
            else
            {
                Debug.Log("Not foundedGame");
            }

            if (GameObject.FindGameObjectWithTag("AnalysisManager") && AnalysisManager)
            {
                GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>().ActiveManager = false;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ActiveManager = false;
                GameObject.FindGameObjectWithTag("AnalysisManager").GetComponent<AnalysisManager>().ActiveManager = true;
            }
            else
            {
                Debug.Log("Not foundedAnalysis");
            }
        }
        
    }
    public void LoadMainGame ()
    {
        GameManager = true;
        PuzzleManager = false;
        AnalysisManager = false;

        Debug.Log("Main");

        SceneManager.LoadScene("BoardNew");
    }

    public void LoadPuzzle ()
    {
        PuzzleManager = true;
        GameManager = false;
        AnalysisManager = false;

        Debug.Log("Puzzle");

        SceneManager.LoadScene("SampleScene");
    }

    public void LoadAnalysis ()
    {
        PuzzleManager = false;
        GameManager = false;
        AnalysisManager = true;

        Debug.Log("Analysis");

        SceneManager.LoadScene("SampleScene");
    }

    /*void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 

        instanceExists = false; 
    }*/
}
