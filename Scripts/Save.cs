using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int passLevel;
    public List<bool>[] coin;
    public int deathNumber;
    public Save() { }
    public Save(bool clear)
    {
        passLevel = 0;
        deathNumber = 0;
        coin = new List<bool>[4];
        for(int i = 0; i < coin.Length; i++)
        {
            coin[i] = new List<bool>();
            coin[i].Add(true);
        }
    }
}
