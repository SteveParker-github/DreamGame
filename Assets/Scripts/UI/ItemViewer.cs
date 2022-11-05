using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemViewer : MonoBehaviour
{
    public float cooldownTimer;
    // Start is called before the first frame update
    void Awake()
    {
        cooldownTimer = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        gameObject.SetActive(false);
    }
}
