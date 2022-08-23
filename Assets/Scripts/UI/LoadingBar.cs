using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingBar : MonoBehaviour
{
    private GameObject panel;
    private Image loadingBar;
    private TextMeshProUGUI loadingText;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.Find("Panel").gameObject;
        loadingBar = panel.transform.Find("LoadingBar").GetComponent<Image>();
        loadingText = panel.transform.Find("LoadingText").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateProgress(float percentage)
    {
        loadingBar.fillAmount = percentage;
    }

    public void ChangeText(string message)
    {
        loadingText.text = message;
    }
}
