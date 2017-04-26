using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour {

    private Text textUI;
    public string defaultText;
    private string currentText;

    float resetTime;
    TextType currentType;

	void Start () {
        textUI = GetComponent<Text>();
        textUI.text = defaultText;
        currentType = TextType.Default;
	}

    private void Update()
    {
        if(Time.time >= resetTime && !currentType.Equals(TextType.Default))
        {
            currentText = defaultText;
        }
        textUI.text = currentText;
    }

    internal void SetLevelText(string text, TextType textType)
    {
        currentType = textType;
        currentText = text;
        switch (textType)
        {
            case TextType.Default:
                defaultText = text;
                break;
            case TextType.Short:
                resetTime = Time.time + 5;
                break;
            case TextType.Long:
                resetTime = Time.time + 10;
                break;
            case TextType.Extended:
                resetTime = Time.time + 15;
                break;

        }
    }

    public enum TextType
    {
        Default, Short, Long, Extended
    }
}
