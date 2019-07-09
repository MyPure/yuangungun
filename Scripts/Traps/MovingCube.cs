using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    [SerializeField]private bool stop = true;
    public bool trigger=false;
    public bool isElevator;
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
            stop = !stop;
        }
    }

    private void ElevatorMove()
    {
        if (!stop && trigger)
        {
            Move();
        }
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (trigger)
        {
            stop = false;
        }
    }

    
    private void Update()
    {
        Debug.Log(1);
        if (isElevator) ElevatorMove();
        else Move();
    }
}
