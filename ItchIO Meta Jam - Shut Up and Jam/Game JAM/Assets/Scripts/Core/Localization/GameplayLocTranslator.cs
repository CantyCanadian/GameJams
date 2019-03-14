using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayLocTranslator : MonoBehaviour
{
	void Start ()
    {
        Text btnText = GetComponent<Text>();

        string text = GameManager.Instance.Data.GetGameplayLoc(btnText.text);
        text = text.Replace("{n}", "\n");

        btnText.text = text;
    }
}