using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassCube : MonoBehaviour
{
    public GameObject coin;
    public bool containCoin;
    public GameObject targetEffect;
    public Animator animator;

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
                coin.GetComponent<Animator>().Play("ShowCoin");
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
