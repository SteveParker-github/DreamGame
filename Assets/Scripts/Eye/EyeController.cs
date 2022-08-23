using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    private PlayerController playerController;
    private Transform eyelidTop;
    private Transform eyelidBottom;
    private float suspicionHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        eyelidTop = transform.Find("eyelid_top");
        eyelidBottom = transform.Find("eyelid_bottom");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.SuspicionHealth == suspicionHealth) return;
        suspicionHealth = playerController.SuspicionHealth;

        int angle = Mathf.FloorToInt(suspicionHealth / 100);
        print(angle);
        eyelidTop.localRotation = Quaternion.Euler(-90 + 10 * angle, 0, 0);
        eyelidBottom.localRotation = Quaternion.Euler(-90 - 5 * angle, 0, 0);
    }
}
