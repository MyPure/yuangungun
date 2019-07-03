using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    private bool playerEnter=false;
    public bool visible;
    public float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        visible = true;
        playerEnter = true;
    }
    private void Update()
    {
        if (playerEnter == true)
        {
            transform.Translate(Vector2.down * speed);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Die();
        }
    }
}
