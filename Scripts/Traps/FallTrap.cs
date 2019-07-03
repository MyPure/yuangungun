using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    public float interal;
    public GameObject[] splikes;
    public bool[] ids;
    private bool playerEnter=false;
    public float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(StartFall());
        playerEnter = true;
    }
    private void Update()
    {
        if (playerEnter == true)
        {
            for (int i = 0; i < splikes.Length; i++)
            {
                if(ids[i])
                splikes[i].transform.Translate(Vector2.down * speed);
            }
        }
    }

    IEnumerator StartFall()
    {
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = true;
            yield return new WaitForSeconds(interal);
        }
        
    }
    
}
