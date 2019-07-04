using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassFlag : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject passLevelUI;
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
            Instantiate(passLevelUI);
            collision.gameObject.SetActive(false);
            gameManager.passLevel++;
            gameManager.SaveGame();
        }
    }
}
