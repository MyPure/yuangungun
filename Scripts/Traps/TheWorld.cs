using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheWorld : MonoBehaviour
{
    //计时器UI
    public GameObject clock;
    public Text clockText;

    //时停持续时间
    public float duration;

    //时停影响的组件
    public SawTurn[] saws;
    public BoxCollider2D[] boxes;    
    public Animator[] animators;

    public Animator myAnimation;

    [SerializeField]private float startTime;        //开启时停的时间
    [SerializeField]private float timer = 0;
    [SerializeField]private bool isPlayerEnter = false;
    [SerializeField] private bool hasTimeStopped = false;

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
        if (!hasTimeStopped)
        {
            timer = 0;
            clock.SetActive(true);
            clockText.text = Convert.ToString(30.00);
            isPlayerEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasTimeStopped)
        {
            clock.SetActive(false);
        }
        isPlayerEnter = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && isPlayerEnter)
        {
            timer += Time.deltaTime;
            myAnimation.SetBool("StopTiming", false);
            myAnimation.SetBool("Timing", true);
        }else if ((Input.GetKeyUp(KeyCode.Q) || !isPlayerEnter) && !hasTimeStopped)
        {
            timer = 0;
            myAnimation.SetBool("Timing", false);
            myAnimation.SetBool("StopTiming", true);
        }
        
        if (timer > 3 && !hasTimeStopped)
        {
            startTime = Time.time;   //获取开始时间
            hasTimeStopped = true;
            TimeStop(false);
            timer = 0;
        }else if (timer > duration)
        {
            isPlayerEnter = false;
            hasTimeStopped = false;
            clock.SetActive(false);
            TimeStop(true);
            timer = 0;
        }
        if (hasTimeStopped)
        {
            timer = Time.time - startTime;
            clockText.text = Convert.ToString(duration-timer);
        }
    }
}
