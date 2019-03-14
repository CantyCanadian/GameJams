using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownValueChange : MonoBehaviour
{
    public Text DropdownText;

    public void OnValueChanged(Dropdown origin)
    {
        DropdownText.text = origin.options[origin.value].text;
    }
}
