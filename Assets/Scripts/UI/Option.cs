using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Option : MonoBehaviour
{
    public void Setup(int index, string text)
    {
        gameObject.name = "Option" + index;
        transform.GetComponent<TextMeshProUGUI>().text = text;
    }
}
