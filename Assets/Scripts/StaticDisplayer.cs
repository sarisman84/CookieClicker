using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TMP_Text))]
public class StaticDisplayer : MonoBehaviour
{
    private TMP_Text _textElement;

    public string extraText;

    // Start is called before the first frame update
    private void Awake()
    {
        _textElement = GetComponent<TMP_Text>();
    }


    public void UpdateText(string text)
    {
        if (!string.IsNullOrEmpty(extraText))
            _textElement.text = $"{extraText} - {text}";
        else
            _textElement.text = text;
    }
}