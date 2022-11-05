using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestTracker : MonoBehaviour
{
    private QuestManager questManager;
    private TextMeshProUGUI questTitleText;
    private TextMeshProUGUI questObjectiveText;
    private bool isTrackerOn;

    // Start is called before the first frame update
    void Awake()
    {
        isTrackerOn = true;
        questManager = GameObject.FindObjectOfType<QuestManager>();
        questTitleText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questObjectiveText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        ToggleTracker();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTrackerOn) return;

        (string, string) quest = questManager.GetCurrentQuestInfo();

        if (string.IsNullOrWhiteSpace(quest.Item1))
        {
            questTitleText.text = "No current Quest";
            questObjectiveText.text = "";
            return;
        }

        questTitleText.text = quest.Item1;
        questObjectiveText.text = quest.Item2;
    }

    public void ToggleTracker()
    {
        isTrackerOn = !isTrackerOn;
        questTitleText.gameObject.SetActive(isTrackerOn);
        questObjectiveText.gameObject.SetActive(isTrackerOn);
    }
}
