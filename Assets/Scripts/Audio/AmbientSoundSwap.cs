using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundSwap : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private string questName;
    private QuestManager questManager;
    private PlayerController playerController;
    // private bool hasSwapped;
    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (questManager.QuestExist(questName))
        {
            playerController.AmbientSound.clip = audioClip;
            playerController.AmbientSound.Play();

            Destroy(this.gameObject);
        }
    }
}
