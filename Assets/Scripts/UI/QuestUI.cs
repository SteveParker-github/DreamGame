using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("GameObject for Name")]
    [SerializeField] private GameObject nameObject;
    [Tooltip("GameObject for Description")]
    [SerializeField] private GameObject descriptionObject;
    [Tooltip("GameObject for Status")]
    [SerializeField] private GameObject statusObject;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI statusText;

    private void Awake()
    {
        nameText = nameObject.GetComponent<TextMeshProUGUI>();
        descriptionText = descriptionObject.GetComponent<TextMeshProUGUI>();
        statusText = statusObject.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(string newName, string newDescription, string newStatus)
    {
        nameText.text = newName;
        descriptionText.text = newDescription;
        statusText.text = newStatus;
    }
}
