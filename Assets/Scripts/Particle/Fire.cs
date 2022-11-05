using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;
    private bool isPlayerNear;
    private List<ParticleSystem> particles;
    private PlayerController playerController;

    void Start()
    {
        particles = new List<ParticleSystem>();

        for (int i = 0; i < transform.childCount; i++)
        {
            particles.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }

        isPlayerNear = false;
    }

    private void Update()
    {
        if (!isPlayerNear) return;

        playerController.Hit(damage * Time.deltaTime);
    }

    public void TurnOnFire()
    {
        foreach (ParticleSystem item in particles)
        {
            item.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        if (playerController == null)
        {
            playerController = other.GetComponent<PlayerController>();
        }

        isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        isPlayerNear = false;
    }
}
