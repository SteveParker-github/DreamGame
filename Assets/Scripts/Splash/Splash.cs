using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    private AsyncOperation globalScene;
    private AsyncOperation mainMenuScene;

    // Start is called before the first frame update
    void Start()
    {
        globalScene = SceneManager.LoadSceneAsync("GlobalScene", LoadSceneMode.Additive);
        mainMenuScene = SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Additive);
        globalScene.allowSceneActivation = false;
        mainMenuScene.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(globalScene.progress);
        if (globalScene.progress >= 0.90f && mainMenuScene.progress >= 0.90f)
        {
            globalScene.allowSceneActivation = true;
            mainMenuScene.allowSceneActivation = true;
            if (globalScene.isDone && mainMenuScene.isDone)
            {
                SceneManager.UnloadSceneAsync("SplashScene");
            }
        }
    }
}
