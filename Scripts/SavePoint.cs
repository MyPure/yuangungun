using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject saved;
    public GameManager gameManager;
    private void Start()
    {
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.savePoint = true;
            gameManager.savePointPosition = transform.position;
            gameManager.SaveTempCoin();
            Active();
        }
    }
    public void Active()
    {
        saved.SetActive(true);
    }
}
