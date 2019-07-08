using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Player player;
    public float carrotTime;
    public Image image;
    private float originTime;
    private void Start()
    {
        originTime = carrotTime;
    }

    public void UseCarrot()
    {
        if (Input.GetKey(KeyCode.K))
        {
            player.speed = 6;
            carrotTime -= Time.deltaTime;
            image.fillAmount = 1-carrotTime / originTime;
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            player.speed = 4;
        }
    }
    private void Update()
    {
        if (carrotTime > 0)
        {
            UseCarrot();
        }
        else if (carrotTime < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
