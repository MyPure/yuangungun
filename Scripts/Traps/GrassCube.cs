﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassCube : MonoBehaviour
{
    public GameObject coin;
    public bool containCoin;
    public GameObject targetEffect;
    public Animator animator;

    private void ShowCoin()
    {
        coin.transform.position = transform.position + Vector3.up*2;
        coin.transform.localScale = new Vector3(0.6f,0.6f,1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>().currentState.stateType==StateType.Jump)
        {            
            animator.SetBool("isUp", true);
        }
        else if (collision.GetComponent<Player>().currentState.stateType == StateType.DoubleJump)
        {
            GameObject effect=GameObject.Instantiate(targetEffect, transform.position, Quaternion.identity);
            if (containCoin)
            {
                ShowCoin();
            }
            Destroy(effect, 0.5f);
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("isUp", false);
    }
}
