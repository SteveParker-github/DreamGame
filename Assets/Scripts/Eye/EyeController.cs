using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    private PlayerController playerController;
    private GameManager gameManager;
    private AudioSource audioSource;
    private Transform eyelidTop;
    private Transform eyelidBottom;
    private float suspicionHealth = 0;
    private int currentSuspicionLevel;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        eyelidTop = transform.Find("eyelid_top");
        eyelidBottom = transform.Find("eyelid_bottom");
        currentSuspicionLevel = 0;
        UpdateEyeLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPaused)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            return;
        }

        audioSource.UnPause();

        if (playerController.SuspicionHealth == suspicionHealth) return;

        suspicionHealth = playerController.SuspicionHealth;

        int angle = Mathf.FloorToInt(suspicionHealth / 100);
        angle = Mathf.Clamp(angle, 0, 4);

        if (angle != currentSuspicionLevel && !audioSource.isPlaying)
        {
            audioSource.Play();
            currentSuspicionLevel = angle;
        }

        eyelidTop.localRotation = Quaternion.Euler(-90 + 10 * angle, 0, 0);
        eyelidBottom.localRotation = Quaternion.Euler(-90 - 5 * angle, 0, 0);
    }

    public void UpdateEyeLevel()
    {
        suspicionHealth = playerController.SuspicionHealth;

        int angle = Mathf.FloorToInt(suspicionHealth / 100);
        angle = Mathf.Clamp(angle, 0, 4);

        if (angle != currentSuspicionLevel)
        {
            currentSuspicionLevel = angle;
        }

        eyelidTop.localRotation = Quaternion.Euler(-90 + 10 * angle, 0, 0);
        eyelidBottom.localRotation = Quaternion.Euler(-90 - 5 * angle, 0, 0);
    }
}
