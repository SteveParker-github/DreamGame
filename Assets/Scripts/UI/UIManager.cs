using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("PreFabs")]
    [Tooltip("Game object for option box")]
    [SerializeField] private GameObject optionBox;
    [SerializeField] private List<Texture> eyes;
    private GameObject crosshair;
    private GameObject captionPanel;
    private GameObject caption;
    private TextMeshProUGUI captionText;
    private GameObject dialogueBox;
    private GameObject dialogueContent;
    private Scrollbar dialogueScrollBar;
    private RectTransform dialogueContentRT;
    private List<GameObject> dialogueGameObjects;
    private List<GameObject> dialogueBoxes;
    private int maxOptions;
    private bool isEnd;
    private PlayerController playerController;
    private GameObject suspicionMeter;
    private GameObject itemViewer;
    private SuspicionPrompt suspicionPrompt;
    private int currentEyeLevel;
    private QuestTracker questTracker;
    private float loadingTimer;
    public int MaxOptions { get { return maxOptions; } }
    public bool isLoading;

    // Start is called before the first frame update
    void Awake()
    {
        crosshair = transform.GetChild(0).gameObject;
        captionPanel = transform.GetChild(2).gameObject;
        caption = captionPanel.transform.GetChild(0).gameObject;
        captionText = caption.GetComponent<TextMeshProUGUI>();
        dialogueBox = transform.GetChild(1).gameObject;
        dialogueContent = dialogueBox.transform.Find("Viewport").Find("Content").gameObject;
        dialogueContentRT = dialogueContent.GetComponent<RectTransform>();
        dialogueScrollBar = dialogueBox.transform.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        Transform suspicion = transform.GetChild(3);
        suspicionMeter = suspicion.GetChild(0).gameObject;
        suspicionPrompt = suspicion.GetComponent<SuspicionPrompt>();
        itemViewer = transform.GetChild(5).gameObject;
        itemViewer.SetActive(false);
        dialogueGameObjects = new List<GameObject>();
        dialogueBoxes = new List<GameObject>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        currentEyeLevel = 0;
        questTracker = transform.GetChild(4).GetComponent<QuestTracker>();
        isLoading = false;
        loadingTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEyeLevel();
    }

    private void UpdateEyeLevel()
    {
        if (isLoading && loadingTimer == 0)
        {
            loadingTimer = 2.0f;
            return;
        }

        if (loadingTimer > 0)
        {
            loadingTimer -= Time.deltaTime;
        }
        else
        {
            loadingTimer = 0;
            isLoading = false;
        }

        int eyeLevel = Mathf.FloorToInt(playerController.SuspicionHealth / 100);
        eyeLevel = Mathf.Clamp(eyeLevel, 0, 3);

        if (eyeLevel == currentEyeLevel) return;

        if (eyeLevel > currentEyeLevel && !isLoading) suspicionPrompt.ShowWarning(currentEyeLevel);

        suspicionMeter.GetComponent<RawImage>().texture = eyes[eyeLevel];
        currentEyeLevel = eyeLevel;
    }

    public void ToggleCrosshair()
    {
        crosshair.SetActive(!crosshair.activeSelf);
        suspicionMeter.SetActive(!suspicionMeter.activeSelf);
    }

    public void ToggleCaption()
    {
        captionPanel.SetActive(!captionPanel.activeSelf);
    }

    public void ChangeCaption(string message, bool isEnd)
    {
        captionText.text = message;
        this.isEnd = isEnd;
    }

    public void ToggleDialogueBox()
    {
        dialogueBox.SetActive(!dialogueBox.activeSelf);
        dialogueBox.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }

    public void ToggleQuestTracker()
    {
        questTracker.ToggleTracker();
    }

    public void DialogOptions(Dictionary<string, bool> dialogueOptions)
    {
        DestroyPrevDialogue();

        List<string> keys = new List<string>(dialogueOptions.Keys);
        maxOptions = dialogueOptions.Count;

        for (int i = 0; i < maxOptions; i++)
        {
            GameObject newOption = Instantiate(Resources.Load<GameObject>("UIPreFab/Option"), dialogueContent.transform);
            Option option = newOption.GetComponent<Option>();
            option.Setup(i, keys[i]);

            if (dialogueOptions[keys[i]])
            {
                option.GetComponent<TextMeshProUGUI>().color = new Vector4(1, 1, 1, 0.5f);
            }

            dialogueGameObjects.Add(newOption);
            dialogueBoxes.Add(newOption.transform.GetChild(0).gameObject);
        }

        dialogueBoxes[0].SetActive(true);
    }

    private void DestroyPrevDialogue()
    {
        foreach (GameObject item in dialogueBoxes)
        {
            GameObject.Destroy(item);
        }

        foreach (GameObject item in dialogueGameObjects)
        {
            GameObject.Destroy(item);
        }


        dialogueGameObjects.Clear();
        dialogueBoxes.Clear();
    }

    public void SuspicionMeterOn()
    {
        if (suspicionMeter.activeInHierarchy) return;

        suspicionMeter.SetActive(true);
    }

    public void SuspicionMeterOff()
    {
        if (!suspicionMeter.activeInHierarchy) return;

        suspicionMeter.SetActive(false);
    }

    public void MoveOptionBox(int prevLocation, int newLocation)
    {
        dialogueBoxes[prevLocation].SetActive(false);
        dialogueBoxes[newLocation].SetActive(true);

        float multiplier = 1.0f / (maxOptions - 1);
        float movePosition = prevLocation < newLocation ? -1 : 1;

        dialogueBox.GetComponent<ScrollRect>().verticalNormalizedPosition += multiplier * movePosition;
    }

    public string GetMessage(int location)
    {
        TextMeshProUGUI optionText = dialogueGameObjects[location].GetComponent<TextMeshProUGUI>();
        optionText.color = new Vector4(1, 1, 1, 0.5f);

        return optionText.text;
    }

    public bool IsEnd()
    {
        return isEnd;
    }

    public void ShowItemViewer()
    {
        itemViewer.SetActive(true);
    }

    public bool IsItemViewActive()
    {
        return itemViewer.activeInHierarchy;
    }
}
