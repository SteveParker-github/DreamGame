using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hallPassPrefab;
    private QuestManager questManager;
    private bool hasSpawned;
    // Start is called before the first frame update
    void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        hasSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned && questManager.QuestExist("Fetch Hall Pass"))
        {
            GameObject itemObject = Instantiate(hallPassPrefab);
            itemObject.name = itemObject.GetComponent<Clue>().ObjectName;
            hasSpawned = true;
        }
    }
}
