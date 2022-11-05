using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour, IInteractable
{
    [Header("Object")]
    [Tooltip("Name of the object")]
    [SerializeField] private string objectName;
    [Tooltip("Description of the object")]
    [SerializeField] private string description;
    [Tooltip("Object Sprite")]
    [SerializeField] private Sprite sprite;
    [SerializeField] private string questName;
    [Tooltip("Description of when the items are collected")]
    [SerializeField] private string questProgressDescription;
    [Tooltip("Description of when there is still more items to collect")]
    [SerializeField] private string questFetchDescription;
    [Tooltip("List of items needed for a quest to progress")]
    [SerializeField] private List<string> itemNeededNames;
    private Renderer objectRenderer;

    private InventoryManager inventoryManager;
    private QuestManager questManager;

    public string ObjectName { get => objectName; }
    public string Description { get => description; }
    public Sprite Sprite { get => sprite; }

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        questManager = FindObjectOfType<QuestManager>();
        objectRenderer = GetComponent<Renderer>();

        if (inventoryManager.Exist(objectName))
        {
            Destroy(this.gameObject);
        }

        objectRenderer.material.SetColor("_EmissionColor", Color.white);
        Deselect();
    }

    public void Interact()
    {
        if (!string.IsNullOrEmpty(questName))
        {
            string questDescription = questProgressDescription;
            if (itemNeededNames.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < itemNeededNames.Count; i++)
                {
                    if (inventoryManager.Exist(itemNeededNames[i]))
                    {
                        count++;
                    }
                }

                if (count < itemNeededNames.Count)
                {
                    questDescription = questFetchDescription + " (" + (count + 1) + "/" + (itemNeededNames.Count + 1) + ")";
                }
            }
            questManager.UpdateQuestDescription(questName, questDescription);
        }

        inventoryManager.AddItem(gameObject, this);

    }

    public string Message()
    {
        objectRenderer.material.EnableKeyword("_EMISSION");
        return "Pickup " + objectName;
    }

    public void Deselect()
    {
        objectRenderer.material.DisableKeyword("_EMISSION");
        // objectRenderer.material.SetColor("_EmissionColor", Color.black);
    }
}
