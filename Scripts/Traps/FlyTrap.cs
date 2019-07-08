using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrap : MonoBehaviour
{
    public bool isUp;
    public GameObject thorn;
    public float speed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(thorn, 3);
            StartCoroutine(Fly());
        }
    }
    IEnumerator Fly()
    {
        while (thorn)
        {
            if(isUp)
            thorn.transform.Translate(Vector3.up * speed * Time.deltaTime);
            else
            thorn.transform.Translate(Vector3.down * speed * Time.deltaTime);
            yield return null;
        }
    }
}
