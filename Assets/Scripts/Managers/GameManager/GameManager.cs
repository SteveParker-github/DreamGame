using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string path;
    private InventoryManager inventoryManager;
    private DialogueManager dialogueManager;
    private QuestManager questManager;
    private GameObject player;
    private GameObject screenShotCamera;
    private PlayerController playerController;
    private GameObject loadingCanvas;
    private LoadingBar loadingBar;
    private PlayerSave playerSave;

    private void Start()
    {
        path = Application.persistentDataPath + "/SaveGame/";

        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        screenShotCamera = GameObject.Find("ScreenShotCamera");
        screenShotCamera.SetActive(false);

        loadingCanvas = GameObject.Find("LoadingCanvas");
        loadingBar = loadingCanvas.GetComponent<LoadingBar>();
        loadingCanvas.SetActive(false);

        playerSave = new PlayerSave(new Vector3(7.46999979f, 4.8f, -38.4799995f), new Quaternion(0, 1, 0, 0));
        UpdatePlayerLocation();
    }

    public void SaveGameFile(string folderName)
    {
        player = GameObject.Find("Player");
        string saveFolder = path + "/" + folderName;
        SaveFile saveFile = new SaveFile();

        saveFile.quests = questManager.GetFullQuestList();
        saveFile.inventory = inventoryManager.GetItemList();
        saveFile.dialogue = dialogueManager.GetDialogue();
        saveFile.player = new PlayerSave(player.transform.position, player.transform.rotation);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            string sceneName = SceneManager.GetSceneAt(i).name;

            if (sceneName != "GlobalScene" && sceneName != "MainMenuScene")
            {
                saveFile.location = sceneName;
            }
        }

        string json = JsonUtility.ToJson(saveFile);

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        StreamWriter writer = new StreamWriter(saveFolder + "/Save.json");
        writer.AutoFlush = true;
        writer.Write(json);
        writer.Close();

        TakeScreenShot(saveFolder + "/ScreenShot.png");
    }

    public void LoadGameFile(string folderName)
    {
        string saveFilePath = path + "/" + folderName + "/Save.json";

        if (!File.Exists(saveFilePath))
        {
            print("File not found!: " + saveFilePath);
            return;
        }

        loadingCanvas.SetActive(true);
        loadingBar.UpdateProgress(0);

        string saveText = File.ReadAllText(saveFilePath);

        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(saveText);

        questManager.LoadQuests(saveFile.quests);
        inventoryManager.LoadItems(saveFile.inventory);
        dialogueManager.LoadDialogues(saveFile.dialogue);
        playerSave = saveFile.player;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            string sceneName = SceneManager.GetSceneAt(i).name;
            if (sceneName != "GlobalScene" && sceneName != "MainMenuScene")
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        StartCoroutine(LoadScene(saveFile.location, saveFile.player));
    }

    IEnumerator LoadScene(string sceneName, PlayerSave playerFile)
    {
        yield return null;
        AsyncOperation loaded = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loaded.isDone)
        {
            loadingBar.UpdateProgress(loaded.progress + 0.1f);
            loadingBar.ChangeText((loaded.progress * 100) + "%");
            if (loaded.progress >= 0.9f)
            {
                UpdatePlayerLocation();
                playerController.CurrentState.CheckSwitchState();
                loadingCanvas.SetActive(false);
            }

            yield return null;
        }
    }

    public void QuickSave()
    {
        SaveGameFile("QuickSave");
    }

    public void QuickLoad()
    {
        LoadGameFile("QuickSave");
    }

    public void UpdatePlayerLocation()
    {
        playerController.CharacterController.enabled = false;
        player.transform.SetPositionAndRotation(playerSave.position.GetVector(), playerSave.rotation.GetRotation());
        playerController.CharacterController.enabled = true;
    }

    public void NewPlayerlocation(Vector3 position, Quaternion rotation)
    {
        playerController.CharacterController.enabled = false;
        player.transform.SetPositionAndRotation(position, rotation);
        playerController.CharacterController.enabled = true;
    }

    private void TakeScreenShot(string fileName)
    {
        screenShotCamera.SetActive(true);
        RenderTexture rt = new RenderTexture(1920, 1080, 24);
        Camera camera = screenShotCamera.GetComponent<Camera>();
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(fileName, bytes);
        screenShotCamera.SetActive(false);
    }
}
