using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private string name;
    private string description;
    private GameObject itemObject;
    private Sprite sprite;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public GameObject ItemObject { get { return itemObject; } }
    public Sprite Sprite { get { return sprite; } }
    public Item(string name, string description, GameObject itemObject, Sprite sprite)
    {
        this.name = name;
        this.description = description;
        this.itemObject = itemObject;
        this.sprite = sprite;
    }
}
