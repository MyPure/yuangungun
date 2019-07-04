using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int passLevel;
    public Save() { }
    public Save(bool clear)
    {
        passLevel = 0;
    }
}
