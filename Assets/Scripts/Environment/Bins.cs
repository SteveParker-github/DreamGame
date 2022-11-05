using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bins : MonoBehaviour
{
    private QuestManager questManager;
    private GameManager gameManager;
    private List<AudioSource> audioSources;
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
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform bin = transform.GetChild(i).GetChild(2);
                bin.GetComponent<Fire>().TurnOnFire();
                AudioSource audioSource = bin.GetComponent<AudioSource>();
                audioSources.Add(audioSource);
                audioSource.Play();
                soundOff = false;
            }

            hasChanged = true;
        }
    }
}
