using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanelState : MainMenuBaseState
{
    public MainPanelState(MainMenuController mainMenuController, MainMenuStateFactory mainMenuStateFactory)
    : base(mainMenuController, mainMenuStateFactory)
    { }

    private GameObject startButton;
    private GameObject exitButton;
    private GameObject loadButton;
    private GameObject resumeButton;
    private GameObject saveButton;
    private bool isLoadTriggered;
    private bool isSaveTriggered;
    public override void EnterState()
    {
        ctx.MainPanel.SetActive(true);
        isLoadTriggered = false;

        startButton = ctx.MainPanel.transform.Find("StartButton").gameObject;
        exitButton = ctx.MainPanel.transform.Find("ExitButton").gameObject;
        loadButton = ctx.MainPanel.transform.Find("LoadButton").gameObject;
        resumeButton = ctx.MainPanel.transform.Find("ResumeButton").gameObject;
        saveButton = ctx.MainPanel.transform.Find("SaveButton").gameObject;

        startButton.GetComponent<Button>().onClick.AddListener(StartGameOnClick);
        exitButton.GetComponent<Button>().onClick.AddListener(ExitGameOnClick);
        loadButton.GetComponent<Button>().onClick.AddListener(LoadGameOnClick);
        resumeButton.GetComponent<Button>().onClick.AddListener(ResumeGameOnClick);
        saveButton.GetComponent<Button>().onClick.AddListener(SaveGameOnClick);
    }
    public override void UpdateState()
    {
        if (ctx.IsMidGame)
        {
            startButton.SetActive(false);
            resumeButton.SetActive(true);
            saveButton.SetActive(true);
        }

        CheckSwitchState();
    }
    public override void ExitState()
    {
        startButton.GetComponent<Button>().onClick.RemoveAllListeners();
        exitButton.GetComponent<Button>().onClick.RemoveAllListeners();
        loadButton.GetComponent<Button>().onClick.RemoveAllListeners();
        resumeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        saveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        
        ctx.MainPanel.SetActive(false);
    }
    public override void CheckSwitchState()
    {
        if (isLoadTriggered)
        {
            SwitchState(factory.LoadGamePanelState());
            return;
        }

        if (isSaveTriggered)
        {
            SwitchState(factory.SaveGamePanelState());
            return;
        }
    }

    private void StartGameOnClick()
    {
        string levelName = "BillyDreamScene";

        if (ctx.IsDevMode)
        {
            levelName = "TestScene";
        }

        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        GameObject.Find("Player").GetComponent<PlayerController>().CurrentState.CheckSwitchState();
        SceneManager.UnloadSceneAsync("MainMenuScene");
        Debug.Log("hello");
    }

    private void ExitGameOnClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void LoadGameOnClick()
    {
        isLoadTriggered = true;
    }

    private void ResumeGameOnClick()
    {
        SceneManager.UnloadSceneAsync("MainMenuScene");
        GameObject.Find("Player").GetComponent<PlayerController>().CurrentState.CheckSwitchState();
    }

    private void SaveGameOnClick()
    {
        isSaveTriggered = true;
    }
}