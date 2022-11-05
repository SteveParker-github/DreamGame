using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHubPortals : MonoBehaviour
{
    private InventoryManager inventoryManager;

    private bool isBillyOpen;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        isBillyOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBillyOpen) return;
        if (inventoryManager.Exist("Lighter"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
