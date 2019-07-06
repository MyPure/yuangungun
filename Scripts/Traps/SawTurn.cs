using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTurn : MonoBehaviour
{
    public List<GameObject> wayPoints;
    int nowWay;
    public float speed;
    void Start()
    {
        nowWay = 0;
    }

    void Update()
    {
        if(transform.position != wayPoints[nowWay].transform.position)
        {
            Vector3 dest = Vector3.MoveTowards(transform.position, wayPoints[nowWay].transform.position, speed*Time.deltaTime);
            transform.Translate(dest - transform.position,Space.World);
        }
        else
        {
            nowWay++;
            if(nowWay == wayPoints.Count)
            {
                nowWay = 0;
            }
        }
    }
}
