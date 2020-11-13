using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debug1 : MonoBehaviour
{
    public static TextMeshProUGUI txt;

    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    public static void Log(string content)
    {
        if (txt != null)
            txt.text += content;
    }
}
