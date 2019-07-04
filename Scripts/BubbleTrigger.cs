using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    public GameObject bubble;
    public TextMesh bubbletext;
    public string text;
    private void Start()
    {
        bubble.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            bubble.SetActive(true);
            bubbletext.text = text;
            bubble.GetComponent<Animator>().Play("气泡_1");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bubble.GetComponent<Animator>().Play("气泡_3");
        }
    }
}
