using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    public GameObject destnitation;
    public GameObject particalStay;
    public GameObject particalGo;
    float stayTime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stayTime = 0;
            particalStay.SetActive(true);
            particalGo.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stayTime = 0;
            particalStay.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stayTime += Time.deltaTime;
            if (stayTime >= 1.5f)
            {
                collision.transform.position = destnitation.transform.position;
                particalGo.SetActive(true);
            }

        }
    }
}
