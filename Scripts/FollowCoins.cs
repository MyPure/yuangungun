using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCoins : MonoBehaviour
{
    public List<FollowCoin> followCoins;
    public GameObject followCoinPrefab;//prefab
    public GameObject coinPos;
    public int followCoinsCount;
    public GameObject particle;
    void Start()
    {
        followCoins = new List<FollowCoin>();
        if (!coinPos)
        {
            coinPos = GameObject.Find("CoinPosition");
        }
    }

    void Update()
    {
        if (followCoins.Count > 0)
        {
            for (int i = 0; i < followCoins.Count; i++)
            {
                if (followCoins[i].follow)
                {
                    Vector3 dest = coinPos.transform.position + new Vector3(-0.25f * i, 0.3f * Mathf.Sin(2 * Time.time + i * 0.5f), 0);
                    Vector3 pos = followCoins[i].transform.position;
                    Vector3 dpos = Vector3.MoveTowards(pos, dest, (pos - dest).magnitude / 2 * 5 * Time.deltaTime);
                    followCoins[i].transform.position = dpos;
                }
            }
        }
    }

    public void AddFollowCoin(Vector3 pos)
    {
        GameObject c = Instantiate(followCoinPrefab, pos, coinPos.transform.rotation);
        followCoins.Add(c.GetComponent<FollowCoin>());
    }

    float giveTime;
    public IEnumerator GiveFollowCoins(Vector3 pos)
    {
        yield return null;
        giveTime = Time.time;
        for (int i = 0; i < followCoinsCount; i++)
        {
            while (Time.time - giveTime < 1.5f / followCoinsCount)
            {
                yield return null;
            }
            AddFollowCoin(pos);
            giveTime = Time.time;
        }
    }

    public void DestroyCoins()
    {
        foreach (FollowCoin coin in followCoins)
        {
            coin.GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(particle, coin.transform.position, coin.transform.rotation,transform);
        }
    }
}
