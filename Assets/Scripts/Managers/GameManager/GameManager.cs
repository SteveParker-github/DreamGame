using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, (Vector3, Quaternion)> houseCords = new Dictionary<string, (Vector3, Quaternion)> {
        {"BillyDreamScene", (new Vector3(-1.69f, 4.25f, -11.0f), Quaternion.Euler(0, -90, 0))}
    };
    private Dictionary<string, string> skyboxes = new Dictionary<string, string> {
        {"BillyDreamScene", "Night2"},
        {"HouseHubScene", "Day4"}
    };

    private string path;
    private InventoryManager inventoryManager;
    private DialogueManager dialogueManager;
    private QuestManager questManager;
    private EnemiesManager enemiesManager;
    private GameObject player;
    private GameObject screenShotCamera;
    private PlayerController playerController;
    private GameObject loadingCanvas;
    private LoadingBar loadingBar;
    private PlayerSave playerSave;
    private bool isPaused;
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    private void Start()
    {
        path = Application.persistentDataPath + "/SaveGame/";

        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        enemiesManager = GameObject.Find("EnemiesManager").GetComponent<EnemiesManager>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        screenShotCamera = GameObject.Find("ScreenShotCamera");
        screenShotCamera.SetActive(false);

        loadingCanvas = GameObject.Find("LoadingCanvas");
        loadingBar = loadingCanvas.GetComponent<LoadingBar>();
        loadingCanvas.SetActive(false);

        playerSave = new PlayerSave(new Vector3(0.0f, 0.0f, 12.0f), new Quaternion(0, 1, 0, 0), 0.0f, 0);
        UpdatePlayer();
    }

    public void SaveGameFile(string folderName)
    {
        player = GameObject.Find("Player");
        string saveFolder = path + "/" + folderName;
        SaveFile saveFile = new SaveFile();

        saveFile.quests = questManager.GetFullQuestList();
        saveFile.currentQuest = questManager.GetCurrentQuest();
        saveFile.inventory = inventoryManager.GetItemList();
        saveFile.dialogue = dialogueManager.GetDialogue();
        saveFile.enemies = enemiesManager.GetEnemiesList();
        saveFile.player = new PlayerSave(player.transform.position, player.transform.rotation, playerController.DamageHealth, playerController.DialogueFails);
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

        questManager.LoadQuests(saveFile.quests, saveFile.currentQuest);
        inventoryManager.LoadItems(saveFile.inventory);
        dialogueManager.LoadDialogues(saveFile.dialogue);
        enemiesManager.LoadEnemies(saveFile.enemies);
        playerSave = saveFile.player;
        playerController.DreamCatcher.ResetCrystals();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            string sceneName = SceneManager.GetSceneAt(i).name;
            if (sceneName != "GlobalScene" && sceneName != "MainMenuScene")
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        playerController.UIManager.isLoading = true;
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
                UpdatePlayer();
                RenderSettings.skybox = Resources.Load<Material>("Skybox/" + skyboxes[sceneName]);
                playerController.AmbientSound.clip = Resources.Load<AudioClip>("Audio/Ambient/" + sceneName);
                playerController.AmbientSound.Play();
                playerController.CurrentState.CheckSwitchState();
                playerController.UIManager.SuspicionMeterOn();

                if (sceneName == "HouseHubScene")
                {
                    playerController.UIManager.SuspicionMeterOff();
                    playerController.ResetSuspicion();
                }

                loadingCanvas.SetActive(false);
            }

            yield return null;
        }
    }

    public void TravelToScene(string sceneName, Vector3 position, Quaternion rotation)
    {
        loadingCanvas.SetActive(true);
        playerController.AmbientSound.clip = Resources.Load<AudioClip>("Audio/Ambient/" + sceneName);
        playerController.AmbientSound.Play();

        StartCoroutine(OpenScene(sceneName, position, rotation));
    }

    public void TravelBack(string sceneName)
    {
        loadingCanvas.SetActive(true);
        playerController.AmbientSound.clip = Resources.Load<AudioClip>("Audio/Ambient/" + sceneName);
        playerController.AmbientSound.Play();
        (Vector3, Quaternion) cords = GetHouseCords(sceneName);
        StartCoroutine(OpenScene("HouseHubScene", cords.Item1, cords.Item2));
    }

    private IEnumerator OpenScene(string sceneName, Vector3 position, Quaternion rotation)
    {
        yield return null;
        AsyncOperation loaded = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!loaded.isDone)
        {
            loadingBar.UpdateProgress(loaded.progress + 0.1f);
            loadingBar.ChangeText((loaded.progress * 100) + "%");

            if (loaded.progress >= 0.9f)
            {
                playerController.AmbientSound.clip = Resources.Load<AudioClip>("Audio/Ambient/" + sceneName);
                playerController.AmbientSound.Play();
                NewPlayerlocation(position, rotation);
                playerController.UIManager.SuspicionMeterOn();

                if (sceneName == "HouseHubScene")
                {
                    playerController.UIManager.SuspicionMeterOff();
                    playerController.ResetSuspicion();
                }
                RenderSettings.skybox = Resources.Load<Material>("Skybox/" + skyboxes[sceneName]);
                loadingCanvas.SetActive(false);
            }
            yield return null;
        }
    }

    public void StartGame()
    {
        StartCoroutine("OpenCutScene");
    }

    private IEnumerator OpenCutScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("OpeningCutscene", LoadSceneMode.Additive);
        GameObject cineCamera = null;

        //Wait for the level to load
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        //find the new camera and videoPlayer
        //turn off the player and turn on the new camera
        cineCamera = GameObject.FindWithTag("CineCamera");
        GameObject videoPlayerObject = GameObject.Find("DreamFibresOpenCine_T2");
        VideoPlayer videoPlayer = null;
        playerController.gameObject.SetActive(false);
        cineCamera.SetActive(true);

        //Wait for videoplayer to be found
        while (videoPlayer == null)
        {
            videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
            videoPlayer.Play();
            yield return null;
        }

        //Wait for the videoPlayer to be playing
        while (!videoPlayer.isPlaying)
        {
            yield return null;
        }

        bool skipped = false;

        //Wait for the video to be finished
        while ((((ulong)videoPlayer.frame < videoPlayer.frameCount - 1) || videoPlayer.isPlaying) && !skipped)
        {
            skipped = Keyboard.current.anyKey.wasPressedThisFrame;
            yield return null;
        }

        //Load a new game
        //revert the camera and player to orignal status
        SceneManager.UnloadSceneAsync("OpeningCutscene");
        string sceneName = "HouseHubScene";
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        playerController.gameObject.SetActive(true);
        cineCamera.SetActive(false);
        RenderSettings.skybox = Resources.Load<Material>("Skybox/" + skyboxes[sceneName]);
        playerController.CurrentState.CheckSwitchState();
        questManager.NewQuest("Into Billy's dream", new Quest("Into Billy's dream", "Find Billy's lighter"));
        playerController.UIManager.SuspicionMeterOff();
        playerController.AmbientSound.clip = Resources.Load<AudioClip>("Audio/Ambient/" + sceneName);
        playerController.AmbientSound.Play();
    }

    public (Vector3, Quaternion) GetHouseCords(string sceneName)
    {
        return houseCords[sceneName];
    }

    public void QuickSave()
    {
        SaveGameFile("QuickSave");
    }

    public void QuickLoad()
    {
        LoadGameFile("QuickSave");
    }

    public void UpdatePlayer()
    {
        playerController.CharacterController.enabled = false;
        player.transform.SetPositionAndRotation(playerSave.position.GetVector(), playerSave.rotation.GetRotation());
        playerController.DamageHealth = playerSave.damageHealth;
        playerController.DialogueFails = playerSave.dialogueFails;
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
