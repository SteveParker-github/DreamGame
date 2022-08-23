using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("PreFabs")]
    [Tooltip("Game object for option box")]
    [SerializeField]
    private GameObject optionBox;
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
    public int MaxOptions { get { return maxOptions; } }

    // Start is called before the first frame update
    void Awake()
    {
        crosshair = transform.Find("Crosshair").gameObject;
        captionPanel = transform.Find("CaptionPanel").gameObject;
        caption = captionPanel.transform.Find("Caption").gameObject;
        captionText = caption.GetComponent<TextMeshProUGUI>();
        dialogueBox = transform.Find("DialogueBox").gameObject;
        dialogueContent = dialogueBox.transform.Find("Viewport").Find("Content").gameObject;
        dialogueContentRT = dialogueContent.GetComponent<RectTransform>();
        dialogueScrollBar = dialogueBox.transform.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        dialogueGameObjects = new List<GameObject>();
        dialogueBoxes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleCrosshair()
    {
        crosshair.SetActive(!crosshair.activeSelf);
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
    }

    public void DialogOptions(Dictionary<string, bool> dialogueOptions)
    {
        DestroyPrevDialogue();

        List<string> keys = new List<string>(dialogueOptions.Keys);
        maxOptions = dialogueOptions.Count;

        dialogueContentRT.offsetMax = new Vector2(0, 50 * maxOptions);
        dialogueContentRT.anchoredPosition = new Vector2(0, 25 * maxOptions);

        for (int i = 0; i < maxOptions; i++)
        {
            GameObject newOptionBox = Instantiate(optionBox);
            GameObject newOption = new GameObject("option" + i);
            TextMeshProUGUI newOptionText = newOption.AddComponent<TextMeshProUGUI>();
            newOptionText.text = keys[i];
            
            if (dialogueOptions[keys[i]])
            {
                newOptionText.color = new Vector4(1, 1, 1, 0.5f);
            }

            newOptionBox.transform.SetParent(newOption.transform);
            
            RectTransform newOptionBoxRT = newOptionBox.GetComponent<RectTransform>();
            newOptionBoxRT.offsetMin = new Vector2(-10, 0);
            newOptionBoxRT.offsetMax = new Vector2(10, 0);
            newOption.transform.SetParent(dialogueContent.transform);

            RectTransform newOptionRT = newOption.GetComponent<RectTransform>();
            newOptionRT.anchorMin = new Vector2(0, 1);
            newOptionRT.anchorMax = new Vector2(1, 1);
            newOptionRT.pivot = new Vector2(0.5f, 1);
            newOptionRT.offsetMin = new Vector2(50, -50);
            newOptionRT.offsetMax = new Vector2(-50, 0);
            newOptionRT.anchoredPosition = new Vector2(newOptionRT.anchoredPosition.x, -50 * i);

            dialogueGameObjects.Add(newOption);
            dialogueBoxes.Add(newOptionBox);
            newOptionBox.SetActive(false);
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

    public void MoveOptionBox(int prevLocation, int newLocation)
    {
        float multiplier = 0.3f;
        dialogueBoxes[prevLocation].SetActive(false);
        dialogueBoxes[newLocation].SetActive(true);

        float movePosition = prevLocation < newLocation ? -1 : 1;
        dialogueScrollBar.value += movePosition * multiplier;
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
}
