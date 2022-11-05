using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private string questName;
    private QuestManager questManager;
    private GameObject portal;
    // Start is called before the first frame update
    void Start()
    {
        questManager = GameObject.FindObjectOfType<QuestManager>();
        portal = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (questManager.QuestMatchStatus(questName, "Complete"))
        {
            portal.SetActive(true);
            Destroy(this);
            return;
        }
    }
}
