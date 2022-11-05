using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePortal : MonoBehaviour, IInteractable
{
    [Header("Scene")]
    [Tooltip("Point to scene where this portal goes")]
    [SerializeField] private string loadLevel;
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        // string unloadLevel = "";
        // for (int i = 0; i < SceneManager.sceneCount; i++)
        // {
        //     string testName = SceneManager.GetSceneAt(i).name;

        //     if (testName != "GlobalScene" && testName != "MainMenuScene")
        //     {
        //         unloadLevel = testName;
        //     }
        // }

        // try
        // {
        //     gameManager.TravelToScene(loadLevel, position, rotation);
        //     SceneManager.UnloadSceneAsync(unloadLevel);
        // }
        // catch (Exception e)
        // {
        //     print("error: " + e);
        // }

        SceneManager.LoadScene(loadLevel);

    }

    public string Message()
    {
        return "Enter portal";
    }

    public void Deselect()
    { }
}
