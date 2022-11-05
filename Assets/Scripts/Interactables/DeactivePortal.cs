using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeactivePortal : MonoBehaviour, IInteractable
{
    private GameObject truePortal;
    private bool isActive;
    private PlayerController playerController;
    private float unabsorbCooldown;

    private void Awake()
    {
        truePortal = transform.parent.GetChild(1).gameObject;
        isActive = false;
    }

    public void Interact()
    {
        isActive = true;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.IsPaused = true;
        playerController.DreamCatcher.PlayExpel();
        unabsorbCooldown = 2.0f;

    }

    public string Message()
    {
        return "Deactive portal";
    }

    public void Deselect()
    { }

    private void Update()
    {
        if (!isActive) return;

        if (unabsorbCooldown > 0.0f)
        {
            unabsorbCooldown -= Time.deltaTime;
            return;
        }

        int counter = playerController.DreamCatcher.Unabsorb();

        if (counter > 0)
        {
            unabsorbCooldown = 2.0f;
            return;
        }
        
        playerController.DreamCatcher.StopExpel();
        playerController.IsPaused = false;
        truePortal.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
