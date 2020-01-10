using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;//要跟随的物体
    public Player player;
    public Vector2 preset;//初始偏移量
    void Start()
    {
        if (target)
        {
            player = target.GetComponent<Player>();
            preset = transform.position - target.transform.position;
        }
    }
    void Update()
    {
        if (target)
        {
            Vector2 d = (Vector2)target.transform.position + preset;
            Vector3 destination = new Vector3(d.x, d.y, transform.position.z);
            Vector3 _destination = Vector3.MoveTowards(transform.position, new Vector3(destination.x, destination.y, transform.position.z), Mathf.Min(1, Mathf.Abs((destination - transform.position).magnitude) / 2.0f) * Mathf.Max(8.0f, player.currentVelocity) * Time.deltaTime);
            transform.Translate(_destination - transform.position);
        }
    }
}
