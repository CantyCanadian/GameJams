using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class BasicInput : MonoBehaviour
{
    public Dictionary<KeyCode, UnityEvent> KeyEvent;
    
    private void Update()
    {
        foreach(KeyValuePair<KeyCode, UnityEvent> kvp in KeyEvent)
        {
            if (Input.GetKey(kvp.Key))
            {
                kvp.Value.Invoke();
            }
        }
    }
}