using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveFilePrefab : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI timeStampText;
    private GameObject selectionBorder;

    // Start is called before the first frame update
    void Awake()
    {
        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        timeStampText = transform.Find("TimeStamp").GetComponent<TextMeshProUGUI>();
        selectionBorder = transform.Find("SelectionBorder").gameObject;
    }

    public void UpdateInfo(string fileName, string dateText)
    {
        nameText.text = fileName;
        timeStampText.text = dateText;
    }

    public void ToggleBorder()
    {
        selectionBorder.SetActive(!selectionBorder.activeSelf);
    }

    public string GetName()
    {
        return nameText.text;
    }
}
