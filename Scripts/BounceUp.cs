using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceUp : MonoBehaviour
{
    float defaultH;
    public float jumpH;
    Jump jump;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            jump = collision.GetComponent<Jump>();
            defaultH = jump.jumpH;
            jump.jumpH = jumpH;
            collision.GetComponent<Player>().currentState.ChangeStateTo(StateType.Jump);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (jump)
            {
                jump.jumpH = defaultH;
            }
        }
    }
}
