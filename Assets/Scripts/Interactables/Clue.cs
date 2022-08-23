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

    private InventoryManager inventoryManager;

    public string ObjectName { get => objectName; }
    public string Description { get => description; }
    public Sprite Sprite { get => sprite; }

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        if (inventoryManager.Exist(objectName))
        {
            Destroy(this.gameObject);
        }
    }

    public void Interact()
    {
        inventoryManager.AddItem(gameObject, this);
    }

    public string Message()
    {
        return "Pickup " + objectName;
    }
}
