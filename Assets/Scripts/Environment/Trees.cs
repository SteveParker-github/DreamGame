using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    private QuestManager questManager;
    private List<AudioSource> audioSources;
    private GameManager gameManager;
    private bool soundOff;
    private bool hasChanged;
    // Start is called before the first frame update
    void Start()
    {
        hasChanged = false;
        questManager = GameObject.FindObjectOfType<QuestManager>();
        gameManager = FindObjectOfType<GameManager>();
        audioSources = new List<AudioSource>();
        soundOff = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasChanged)
        {
            if (gameManager.IsPaused && !soundOff)
            {
                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].Stop();
                }
                soundOff = true;
                return;
            }

            if (!gameManager.IsPaused && soundOff)
            {
                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].Play();
                }
                soundOff = false;
            }
            return;
        }

        if (questManager.QuestExist("Kill Billy's monsters"))
        {
            TurnOffLeaves();
        }
    }

    private void TurnOffLeaves()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            Transform tree = transform.GetChild(i).GetChild(2);
            tree.GetComponent<Fire>().TurnOnFire();
            AudioSource audioSource = tree.GetComponent<AudioSource>();
            audioSources.Add(audioSource);
            audioSource.Play();
        }

        hasChanged = true;
    }
}
