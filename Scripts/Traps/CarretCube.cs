using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarretCube : MonoBehaviour
{
    public GameObject tip;
    public GameObject originCarret;
    private GameObject carret;
    public GameObject carretPrafab;

    private float timer=0;
    private float coldTime = 3;
    public GameObject effect;
    
    private bool isPlayerEnter = false;
    public GameObject player;
    public GameObject carrotUI;
    

    private void PullCarret()
    {
        if (Input.GetKey(KeyCode.Q) && isPlayerEnter)
        {
            if (carret == null)
            {
                carret = GameObject.Instantiate(carretPrafab, player.transform.position+Vector3.left, Quaternion.identity);                
            }
            timer += Time.deltaTime;
            effect.SetActive(true);
            if (timer >= coldTime)
            {
                carrotUI.SetActive(true);
                tip.SetActive(true);
                Destroy(originCarret);
                Destroy(gameObject);
                effect.SetActive(false);
            }
        }
        else
        {
            timer = 0;            
            Destroy(carret);
            effect.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerEnter = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerEnter = false;
    }
    private void Update()
    {
        PullCarret();
    }
}
