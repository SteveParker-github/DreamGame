using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapNPCController : MonoBehaviour, IInteractable
{
    [Header("NPC")]
    [Tooltip("Name of NPC")]
    [SerializeField]
    private string characterName;
    [Tooltip("Prefab of monster")]
    [SerializeField]
    private GameObject preFabMonster;
    private GameObject mainPlayer;
    private PlayerController playerController;


    private void Start()
    {
        mainPlayer = GameObject.Find("Player");
        playerController = mainPlayer.GetComponent<PlayerController>();
    }

    public void Interact()
    {
        Instantiate(preFabMonster, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public string Message()
    {
        return "Talk to " + characterName;
    }
}
