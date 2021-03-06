﻿using System.Collections;
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
            StartCoroutine(SFCC());
            Active();
        }
    }

    IEnumerator SFCC()
    {
        yield return null;//等一帧再执行
        gameManager.SaveFollowCoinCount();
    }
    public void Active()
    {
        saved.SetActive(true);
    }
}
