using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcher : MonoBehaviour
{
    [Header("Projectile")]
    [Tooltip("Game object this weapon fires")]
    [SerializeField]
    private GameObject projectile;
    private Quaternion defaultLocalRotation;
    private GameObject web;
    private Material webMaterial;
    private Dictionary<string, Vector4> materialTypes;
    private float rotationTimer;
    // Start is called before the first frame update
    void Start()
    {
        web = transform.GetChild(0).GetChild(0).gameObject;
        // webMaterial = web.GetComponent<Renderer>().material;
        // materialTypes = new Dictionary<string, Vector4>();
        // materialTypes.Add("default", webMaterial.color);
        // materialTypes.Add("hit", new Vector4(0, 1, 0, 0.53f));
        // materialTypes.Add("block", new Vector4(1, 0, 0, 0.53f));
        defaultLocalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotateToDefault()
    {
        ChangeColour("default", 0);
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
    }

    public void ChangeColour(string colourType, float remainingAbsorb)
    {
        // Vector4 newColour = materialTypes[colourType];
        // if (colourType == "hit")
        // {
        //     newColour.w = 1 - remainingAbsorb * 0.5f;
        // }

        // webMaterial.color = newColour;
    }
}
