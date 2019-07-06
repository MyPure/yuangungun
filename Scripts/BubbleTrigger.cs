using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    public int myFontSize;
    public bool playOnce;
    public GameObject bubble;
    public GameObject bubbletext;
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
            bubbletext.GetComponent<TextMesh>().fontSize = myFontSize;
            bubbletext.GetComponent<TextMesh>().text = text;
            bubble.GetComponent<Animator>().Play("气泡_1");
            if (playOnce) bubble.GetComponent<Animator>().SetBool("playOnce", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bubble.GetComponent<Animator>().Play("气泡_3");
        }
        if (playOnce)
        {
            Destroy(gameObject);
        }
    }
}
