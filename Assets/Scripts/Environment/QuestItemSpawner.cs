using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemSpawner : MonoBehaviour
{
    [SerializeField] private string questName;

    private QuestManager questManager;

    // Start is called before the first frame update
    void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (questManager.QuestExist(questName))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            Destroy(this);
        }
    }
}
