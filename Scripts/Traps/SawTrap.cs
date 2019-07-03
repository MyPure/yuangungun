using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public GameObject[] saws;
    public float sawSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < saws.Length; i++)
        {
            saws[i].SetActive(true);
            saws[i].GetComponent<Rigidbody2D>()
                .AddForce(Vector2.left * Time.deltaTime * sawSpeed, ForceMode2D.Impulse);
        }
        
    }

    private void Update()
    {
        //if (saw.activeSelf == true)
        //{
        //    Vector3 vector3 = transform.position + Vector3.forward;
        //    saw.transform.Rotate(vector3, 5);            
        //}
    }
}
