using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    public float duration;
    public SawTurn[] saws;
    public BoxCollider2D[] boxes;
    public GameObject effect;
    public Animator[] animators;
    public Animator myAnimation;
    private float timer = 0;
    private float change = 0;    

    private void TimeStop(bool state)
    {
        for (int i = 0; i < saws.Length; i++)
        {
            saws[i].enabled = state;
        }
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].isTrigger = state;
        }
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = state;
        }

        myAnimation.SetBool("TimeStop", !state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        change = 0.02f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        change = -0.02f;
    }

    private void Update()
    {
        timer += change;
        if (timer > 3)
        {
            TimeStop(false);
            timer = 0;
        }else if (timer < -duration)
        {
            TimeStop(true);
            change = 0;
            timer = 0;
        }
    }
}
