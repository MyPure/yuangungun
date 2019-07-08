using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassFlag : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject passLevelUI;
    List<FollowCoin> followCoins;
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
            StartCoroutine(StartCollection());
            collision.gameObject.GetComponent<Player>().canMove = false;
            if(gameManager.passLevel <= gameManager.nowLevel)
            {
                gameManager.passLevel = gameManager.nowLevel + 1;
            }
            gameManager.SaveCoin();
            gameManager.SaveGame();
        }
    }

    float time;
    IEnumerator StartCollection()
    {
        followCoins = GameObject.Find("FollowCoins").GetComponent<FollowCoins>().followCoins;
        GetComponent<Animator>().Play("Chest_2");
        int index = 0;
        if (followCoins.Count > 0)
        {
            while (index < followCoins.Count)
            {
                time = Time.time;
                while (Time.time - time < 0.25f)
                {
                    yield return null;
                }
                if (index == followCoins.Count - 1)
                {
                    StartCoroutine(CollectCoin(index++, true));
                }
                else
                {
                    StartCoroutine(CollectCoin(index++, false));
                }
            }
        }
        else
        {
            Instantiate(passLevelUI);
        }
    }

    IEnumerator CollectCoin(int index, bool last)
    {
        followCoins[index].follow = false;
        while (followCoins[index].transform.position != transform.position)
        {
            Vector3 dest = transform.position;
            Vector3 pos = followCoins[index].transform.position;
            Vector3 dpos = Vector3.MoveTowards(pos, dest, Mathf.Max(0.2f, (pos - dest).magnitude / 2) * 10 * Time.deltaTime);
            followCoins[index].transform.position = dpos;
            yield return null;
        }
        followCoins[index].GetComponent<SpriteRenderer>().enabled = false;
        if (last)
        {
            Instantiate(passLevelUI);
        }
    }
}
