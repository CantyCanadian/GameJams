using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    string[] GetSaveableData();
    void SetSaveableData();
    void SetSaveableData(string[] data);
}
