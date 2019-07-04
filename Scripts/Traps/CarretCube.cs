using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarretCube : MonoBehaviour
{
    public GameObject originCarret;
    private GameObject carret;
    public GameObject carretPrafab;

    private float timer=0;
    private float coldTime = 3;
    public GameObject effect;
    
    private bool isPlayerEnter = false;
    private GameObject player;

    /// <summary>
    /// 按照世界坐标放置UI
    /// </summary>
    /// <param name="point">世界坐标</param>
    /// <param name="rect">需要设置的UI坐标</param>
    private void SetUIPos(Vector3 point, RectTransform rect)
    {
        //Vector2 pos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Camera.main.WorldToScreenPoint(point), myCamera, out pos);
        //rect.anchoredPosition = pos;
    }
    

    private void PullCarret()
    {
        if (Input.GetKey(KeyCode.Q)&&isPlayerEnter)
        {
            if (carret == null)
            {
                carret = GameObject.Instantiate(carretPrafab, transform.position, Quaternion.identity);                
            }
            timer += Time.deltaTime;
            effect.SetActive(true);
            if (timer >= 3)
            {
                Destroy(originCarret);
                effect.SetActive(false);
            }
        }
        else
        {
            timer = 0;
            Destroy(carret);
            effect.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject;
        isPlayerEnter = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerEnter = false;
    }
    private void Update()
    {
        PullCarret();
    }
}
