using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteractable
{
    [Header("Scene")]
    [Tooltip("Point to scene where this portal goes")]
    [SerializeField] private string loadLevel;
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;

    private GameManager gameManager;

    private void start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Interact()
    {
        string unloadLevel = "";
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            string testName = SceneManager.GetSceneAt(i).name;
            print(testName);
            if (testName != "GlobalScene" && testName != "MainMenuScene")
            {
                unloadLevel = testName;
            }
            gameManager.NewPlayerlocation(position, rotation);
        }

        try
        {
            SceneManager.UnloadSceneAsync(unloadLevel);
            SceneManager.LoadScene(loadLevel, LoadSceneMode.Additive);
        }
        catch (Exception e)
        {
            print("error: " + e);
        }

    }

    public string Message()
    {
        return "Enter portal";
    }
}
