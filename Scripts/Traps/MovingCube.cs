using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public Transform[] wayPoints;
    public float speed;
    private int index = 0;
    private int change = 1;

    private void Move()
    {
        transform.Translate((wayPoints[index].position-transform.position).normalized * Time.deltaTime * speed);
        if ((transform.position - wayPoints[index].position).magnitude < 0.1f)
        {            
            if (index == 0) change = 1;
            else if (index == wayPoints.Length-1) change = -1;
            index+=change;
        }
    }
    private void Update()
    {
        Move();
    }
}
