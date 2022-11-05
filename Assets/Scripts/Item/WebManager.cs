using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebManager : MonoBehaviour
{
    private const string QUESTNAME = "Fetch Hall Pass";
    private List<MeshRenderer> webs;
    private QuestManager questManager;
    private bool isFinished;

    // Start is called before the first frame update
    void Start()
    {
        webs = new List<MeshRenderer>();
        questManager = GameObject.FindObjectOfType<QuestManager>();

        if (questManager.QuestMatchStatus(QUESTNAME, "Complete"))
        {
            Destroy(this.gameObject);
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            webs.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
        }    

        isFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished)
        {
            Destroy(this.gameObject);
            return;
        }

        if (!questManager.QuestMatchStatus(QUESTNAME, "Complete")) return;

        Color colour = webs[0].material.color;
        colour.a -= Time.deltaTime / 3;

        foreach (MeshRenderer web in webs)
        {
            web.material.color = colour;
        }

        isFinished = colour.a <= 0;
    }
}
