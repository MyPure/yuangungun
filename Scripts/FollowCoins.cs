using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCoins : MonoBehaviour
{
    public List<GameObject> followCoins;
    public GameObject followCoin;//prefab
    public GameObject coinPos;
    public int followCoinsCount;
    public GameObject particle;
    public bool follow = true;
    void Start()
    {
        followCoins = new List<GameObject>();
        if (!coinPos)
        {
            coinPos = GameObject.Find("CoinPosition");
        }
    }

    void Update()
    {
        if (followCoins.Count > 0 && follow)
        {
            for (int i = 0; i < followCoins.Count; i++)
            {
                Vector3 dest = coinPos.transform.position + new Vector3(-0.25f * i, 0, 0);
                
                Vector3 pos = followCoins[i].transform.position;
                Vector3 dpos = Vector3.MoveTowards(pos, dest, (pos - dest).magnitude / 2 * 5 * Time.deltaTime);
                followCoins[i].transform.position = dpos;
            }
        }
    }

    public void AddFollowCoin(Vector3 pos)
    {
        GameObject c = Instantiate(followCoin, pos, coinPos.transform.rotation,transform);
        followCoins.Add(c);
    }

    float giveTime;
    public IEnumerator GiveFollowCoins(Vector3 pos)
    {
        yield return null;
        giveTime = Time.time;
        for (int i = 0; i < followCoinsCount; i++)
        {
            while (Time.time - giveTime < 0.25f)
            {
                yield return null;
            }
            AddFollowCoin(pos);
            giveTime = Time.time;
        }
    }

    public void DestroyCoins()
    {
        foreach (GameObject coin in followCoins)
        {
            coin.GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(particle, coin.transform.position, coin.transform.rotation);
        }
    }
}
