using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuspicionPrompt : MonoBehaviour
{
    private GameObject promptObject;
    private TextMeshProUGUI text;
    private float cooldown;
    private string[] messages;

    // Start is called before the first frame update
    void Start()
    {
        messages = new string[] {
            "Damn something doesn't feel right, I better watch myself.", 
            "There's something weird going on in here, I don't like it.", 
            "Aww shi…"};
        promptObject = transform.GetChild(1).gameObject;
        text = promptObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!promptObject.activeInHierarchy) return;

        if (cooldown > 0.0f)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        promptObject.SetActive(false);
    }

    public void ShowWarning(int warningMessage)
    {
        text.text = messages[warningMessage];
        promptObject.SetActive(true);
        cooldown = 3.0f;
    }
}
