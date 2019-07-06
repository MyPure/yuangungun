using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Player player;
    public bool carrot;
    public float carrotTime;

    public void Initialization(string target)
    {
        carrotTime = 60;        
    }

    public void UseCarrot()
    {
        if (Input.GetKey(KeyCode.K))
        {
            player.speed = 6;
            carrotTime -= Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            player.speed = 4;
        }
    }
    private void Update()
    {
        if (carrot && carrotTime > 0)
        {
            UseCarrot();
        }
    }
}
