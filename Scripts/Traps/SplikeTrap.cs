using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplikeTrap : MonoBehaviour
{
    public GameObject splikeGroup;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        splikeGroup.SetActive(true);
    }
}
