using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterCube : MonoBehaviour
{
    private bool startTiming = false;
    private float timer=0;
    public bool hit = false;
    private Vector3 originPos;
    public Slider slider;
    private Camera myCamera;
    public Canvas canvas;
    public Rigidbody2D bulletRigid;
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
            bulletRigid.velocity = Vector2.zero;
            bulletRigid.transform.position = originPos;
            hasShot = false;
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
            bulletRigid.AddForce(Vector2.right * force * slider.value,ForceMode2D.Impulse);
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
        originPos = bulletRigid.transform.position;
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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Camera.main.WorldToScreenPoint(point), myCamera, out pos);
        rect.anchoredPosition = pos;
    }
}
