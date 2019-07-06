using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : MonoBehaviour
{
    public GameObject tip;
    public ShooterCube shooter;
    public GameObject effect;
    public MovingCube targetTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Bullet")
        {
            targetTrigger.trigger = true;
            shooter.hit = true;
            GameObject effectGo = GameObject.Instantiate(effect, transform.position, Quaternion.identity);
            tip.SetActive(true);
            Destroy(effectGo, 1.5f);
            Destroy(gameObject);
        }
    }
}
