using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public enum PickedType
    {
        unPicked,
        tempPicked,
        picked
    }
    public GameManager gameManager;
    public PickedType pickedType;
    public FollowCoins followCoins;
    private void Start()
    {
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (!followCoins)
        {
            followCoins = GameObject.Find("FollowCoins").GetComponent<FollowCoins>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(pickedType == PickedType.unPicked)
            {
                followCoins.AddFollowCoin(transform.position);
            }
            pickedType = PickedType.tempPicked;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
