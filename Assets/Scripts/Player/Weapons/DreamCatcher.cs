using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcher : MonoBehaviour
{
    [Header("Projectile")]
    [Tooltip("Game object this weapon fires")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Material crystalDefault;
    [SerializeField] private Material crystalLit;
    private Quaternion defaultLocalRotation;
    private GameObject web;
    private float rotationTimer;
    private List<Renderer> crystals;
    private int crystalCount;
    private ParticleSystem absorbParticle;
    private ParticleSystem shootParticle;
    private ParticleSystem expelParticle;

    // Start is called before the first frame update
    void Start()
    {
        web = transform.GetChild(0).GetChild(0).gameObject;
        crystals = new List<Renderer>();
        Transform crystal = transform.GetChild(1);

        for (int i = 0; i < crystal.childCount; i++)
        {
            crystals.Add(crystal.GetChild(i).GetComponent<Renderer>());
        }

        Transform particles = transform.GetChild(0).GetChild(1);
        absorbParticle = particles.GetChild(0).GetComponent<ParticleSystem>();
        shootParticle = particles.GetChild(1).GetComponent<ParticleSystem>();
        expelParticle = particles.GetChild(2).GetComponent<ParticleSystem>();
        crystalCount = 0;
        defaultLocalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotateToDefault()
    {
        if (transform.localRotation != defaultLocalRotation)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, defaultLocalRotation, rotationTimer);
            rotationTimer += Time.deltaTime;
        }
    }

    public void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 targetDirection = targetPosition - transform.position;
        targetDirection.y = 0.0f;

        Vector3 newDreamCacherDirection = Vector3.RotateTowards(transform.forward, targetDirection, 180, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDreamCacherDirection);
        rotationTimer = 0;
    }

    public void FireProjectile()
    {
        Instantiate(projectile, web.transform.position, transform.rotation);
        shootParticle.Play();
    }

    public void LightUpCrystal()
    {
        crystals[crystalCount].material = crystalLit;
        crystalCount++;
    }

    public void ResetCrystals()
    {
        foreach (Renderer crystal in crystals)
        {
            crystal.material = crystalDefault;
        }
        crystalCount = 0;
    }

    public void StartAbsorbing()
    {
        if (absorbParticle.isPlaying) return;

        absorbParticle.Play();
    }

    public void StopAbsorbing()
    {
        if (!absorbParticle.isPlaying) return;

        absorbParticle.Stop();
    }

    public int Unabsorb()
    {
        crystalCount--;
        crystals[crystalCount].material = crystalDefault;
        return crystalCount;
    }

    public void PlayExpel()
    {
        expelParticle.Play();
    }

    public void StopExpel()
    {
        expelParticle.Stop();
    }
}
