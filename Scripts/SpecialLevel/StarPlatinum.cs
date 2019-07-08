using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPlatinum : MonoBehaviour
{
    
    public GameObject player;
    public FlyTrap[] flyTraps;
    public SawTurn[] saws;
    public Animator[] animators;
    public float speed;
    private float timer = 0;

    private void TimeStop(bool state)
    {
        for (int i = 0; i < saws.Length; i++)
        {
            saws[i].enabled = state;
        }
        for (int i = 0; i < flyTraps.Length; i++)
        {
            flyTraps[i].enabled = state;
        }
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = state;
        }
    }

}
