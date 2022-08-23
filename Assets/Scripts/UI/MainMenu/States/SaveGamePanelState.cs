using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveGamePanelState : MainMenuBaseState
{
    public SaveGamePanelState(MainMenuController mainMenuController, MainMenuStateFactory mainMenuStateFactory)
    : base(mainMenuController, mainMenuStateFactory)
    { }

    private GameObject saveGameButtonObject;
    private Button saveGameButton;
    private GameObject backButton;
    private TMP_InputField saveNameInput;
    private Transform saveFileContent;
    private bool isBackTriggered;
    private List<SaveFilePrefab> saveObjects;
    private int selection;

    public override void EnterState()
    {
        ctx.SaveGamePanel.SetActive(true);
        isBackTriggered = false;
        Transform panel = ctx.SaveGamePanel.transform.Find("Panel");
        saveGameButtonObject = panel.Find("SaveGameButton").gameObject;
        backButton = panel.Find("BackButton").gameObject;
        saveGameButton = saveGameButtonObject.GetComponent<Button>();
        saveGameButton.onClick.AddListener(SaveGameButtonOnClick);
        backButton.GetComponent<Button>().onClick.AddListener(BackButtonOnClick);
        Transform scrollView = panel.Find("Scroll View");
        Transform viewport = scrollView.Find("Viewport");
        saveFileContent = viewport.Find("Content");
        saveNameInput = panel.Find("SaveNameInput").GetComponent<TMP_InputField>();

        saveGameButton.interactable = false;

        ShowSaveGames();
    }
    public override void UpdateState()
    {
        saveGameButton.interactable = saveNameInput.text.Length > 0;

        CheckSwitchState();
    }
    public override void ExitState()
    {
        foreach (SaveFilePrefab item in saveObjects)
        {
            GameObject.Destroy(item.gameObject);
        }
        saveGameButton.onClick.RemoveAllListeners();
        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        ctx.SaveGamePanel.SetActive(false);
    }
    public override void CheckSwitchState()
    {
        if (isBackTriggered)
        {
            SwitchState(factory.MainPanelState());
            return;
        }
    }

    private void SaveGameButtonOnClick()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SaveGameFile(saveNameInput.text);

        saveNameInput.text = "";

        foreach (SaveFilePrefab item in saveObjects)
        {
            GameObject.Destroy(item.gameObject);
        }
        saveObjects.Clear();

        ShowSaveGames();
    }

    private void BackButtonOnClick()
    {
        isBackTriggered = true;
    }

    private void ButtonSelectionOnClick(string saveName)
    {
        saveNameInput.text = saveName;
    }

    private void ShowSaveGames()
    {
        string path = Application.persistentDataPath + "/SaveGame/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DirectoryInfo dir = new DirectoryInfo(path);
        DirectoryInfo[] dirs = dir.GetDirectories();

        List<(string, string)> saveNames = new List<(string, string)>();
        saveObjects = new List<SaveFilePrefab>();

        for (int i = 0; i < dirs.Length; i++)
        {
            Debug.Log(dirs[i].Name);
            if (dirs[i].Name == "QuickSave") continue;

            FileInfo[] file = dirs[i].GetFiles("Save.json");

            if (file.Length < 1)
            {
                Debug.Log("File not found!");
                continue;
            }

            string DateTime = file[0].LastWriteTime.ToString();
            GameObject saveFilePanel = GameObject.Instantiate(Resources.Load<GameObject>("UIPreFab/SaveFilePanel"));
            saveFilePanel.GetComponent<SaveFilePrefab>().UpdateInfo(dirs[i].Name, DateTime);
            saveFilePanel.transform.SetParent(saveFileContent);
            Button button = saveFilePanel.GetComponent<Button>();
            string saveName = dirs[i].Name;
            button.onClick.AddListener(delegate { ButtonSelectionOnClick(saveName); });
            Debug.Log(i);
            saveObjects.Add(saveFilePanel.GetComponent<SaveFilePrefab>());
        }
    }

}