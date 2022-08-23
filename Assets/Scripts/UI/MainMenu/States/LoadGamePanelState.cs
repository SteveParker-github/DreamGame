using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGamePanelState : MainMenuBaseState
{
    public LoadGamePanelState(MainMenuController mainMenuController, MainMenuStateFactory mainMenuStateFactory)
    : base(mainMenuController, mainMenuStateFactory)
    { }

    private GameObject loadGameButton;
    private GameObject backButton;
    private Transform saveFileContent;
    private RawImage screenImage;
    private bool isBackTriggered;
    private List<SaveFilePrefab> saveObjects;
    private int selection;
    private AsyncOperation loaded;
    private bool isLoading;
    public override void EnterState()
    {
        ctx.LoadGamePanel.SetActive(true);
        isBackTriggered = false;
        isLoading = false;
        Transform panel = ctx.LoadGamePanel.transform.Find("Panel");
        loadGameButton = panel.Find("LoadGameButton").gameObject;
        backButton = panel.Find("BackButton").gameObject;
        loadGameButton.GetComponent<Button>().onClick.AddListener(LoadGameButtonOnClick);
        backButton.GetComponent<Button>().onClick.AddListener(BackButtonOnClick);
        screenImage = panel.Find("ScreenImage").GetComponent<RawImage>();
        Transform scrollView = ctx.LoadGamePanel.transform.Find("Scroll View");
        Transform viewport = scrollView.Find("Viewport");
        saveFileContent = viewport.Find("Content");

        loadGameButton.GetComponent<Button>().interactable = false;



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
            int index = i;
            button.onClick.AddListener(delegate { ButtonSelectionOnClick(index); });
            Debug.Log(i);
            saveObjects.Add(saveFilePanel.GetComponent<SaveFilePrefab>());
        }
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        foreach (SaveFilePrefab item in saveObjects)
        {
            GameObject.Destroy(item.gameObject);
        }
        loadGameButton.GetComponent<Button>().onClick.RemoveAllListeners();
        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        ctx.LoadGamePanel.SetActive(false);
    }
    public override void CheckSwitchState()
    {
        if (isBackTriggered)
        {
            SwitchState(factory.MainPanelState());
            return;
        }

        if (isLoading)
        {
            isLoading = false;
            GameObject gameManagerObject = GameObject.Find("GameManager");
            GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
            gameManager.LoadGameFile(saveObjects[selection].GetName());
            return;
        }
    }

    private void LoadGameButtonOnClick()
    {
        isLoading = true;
    }

    private void BackButtonOnClick()
    {
        isBackTriggered = true;
    }

    private void ButtonSelectionOnClick(int index)
    {
        Debug.Log("index: " + index);
        Debug.Log("Open SaveGame :" + saveObjects[index].GetName());
        string saveName = saveObjects[index].GetName();
        selection = index;
        loadGameButton.GetComponent<Button>().interactable = true;
        string path = Application.persistentDataPath + "/SaveGame/" + saveName + "/ScreenShot.png";
        Texture2D newTexture = new Texture2D(2, 2);
        byte[] fileData = File.ReadAllBytes(path);
        newTexture.LoadImage(fileData);
        screenImage.texture = newTexture;
    }

}