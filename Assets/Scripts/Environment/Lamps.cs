using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamps : MonoBehaviour
{
    [SerializeField] private Material updateMat;
    private QuestManager questManager;
    private bool hasChanged;
    // Start is called before the first frame update
    void Start()
    {
        hasChanged = false;
        questManager = GameObject.FindObjectOfType<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasChanged)
        {
            return;
        }

        if (questManager.QuestExist("Kill Billy's monsters"))
        {
            ToggleLights();
        }
    }

    public void ToggleLights()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform lamp = transform.GetChild(i).GetChild(0);
            Material[] mats = lamp.GetComponent<MeshRenderer>().materials;
            mats[1] = updateMat;
            lamp.GetComponent<MeshRenderer>().materials = mats;
            lamp.GetChild(0).GetComponent<Light>().color = new Color(0.7169f, 0.1116f, 0.1872f, 1.0f);
        }

        hasChanged = true;
    }
}
