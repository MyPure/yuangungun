using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }
    }
    void Update()
    {
        transform.position = player.transform.position + new Vector3(1, 1, 0);
    }
}
