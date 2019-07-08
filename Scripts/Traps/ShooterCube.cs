using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterCube : MonoBehaviour
{
    private bool startTiming = false;
    private float timer=0;
    public bool hit = false;
    private Vector2 originPos;
    public Slider slider;
    public Canvas canvas;
    public GameObject bullet;
    public float force;
    [SerializeField]private float index = 0.005f;
    [SerializeField]private bool isPlayerEnter = false;
    [SerializeField] private bool hasShot = false;
    private void Shoot()
    {
        //重新开始
        if (isPlayerEnter && hasShot && !hit&&timer>3)
        {
            startTiming = false;
            hasShot = false;
            bullet.GetComponent<Rigidbody2D>().angularVelocity = 0;
            bullet.transform.rotation = Quaternion.identity;
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bullet.transform.position = originPos;
        }
        //蓄力
        if (!hasShot&&isPlayerEnter && Input.GetKey(KeyCode.E))
        {
            slider.gameObject.SetActive(true);
            SetUIPos(transform.position, slider.GetComponent<RectTransform>());            
            if (slider.value == 0 || slider.value == 1)
                index = -index;
            slider.value += index;
        }
        //发射
        if (Input.GetKeyUp(KeyCode.E))
        {
            startTiming = true;
            hasShot = true;
            slider.gameObject.SetActive(false);
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force * slider.value,ForceMode2D.Impulse);
        }
        
    }

    private void Timing(bool temp)
    {
        if (temp)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerEnter = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerEnter = false;
    }
    private void Start()
    {        
        originPos = bullet.transform.position;
    }
    private void Update()
    {
        Shoot();
        Timing(startTiming);
    }

    /// <summary>
    /// 按照世界坐标放置UI
    /// </summary>
    /// <param name="point">世界坐标</param>
    /// <param name="rect">需要设置的UI坐标</param>
    private void SetUIPos(Vector3 point, RectTransform rect)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Camera.main.WorldToScreenPoint(point), new Camera(), out pos);
        rect.anchoredPosition = pos;
    }
}
