using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public GameObject saw;
    public float sawSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(1);
        saw.SetActive(true);
        saw.GetComponent<Rigidbody2D>()
            .AddForce(Vector2.left* Time.deltaTime * sawSpeed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (saw.activeSelf == true)
        {
            Vector3 vector3 = transform.position + Vector3.forward;
            saw.transform.Rotate(vector3, 5);            
        }
    }
}
