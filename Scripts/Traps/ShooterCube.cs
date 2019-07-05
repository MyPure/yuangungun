using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterCube : MonoBehaviour
{

    public Slider slider;
    private Camera myCamera;
    public Canvas canvas;
    public Rigidbody2D bulletRigid;
    public float force;
    private float index = -0.02f;
    [SerializeField]private bool isPlayerEnter = false;
    [SerializeField] private bool hasShot = false;
    private void Shoot()
    {
        if (isPlayerEnter && Input.GetKey(KeyCode.E))
        {
            Debug.Log(slider.value);
            slider.gameObject.SetActive(true);
            //SetUIPos(transform.position, slider.GetComponent<RectTransform>());
            Debug.Log(index);
            if (slider.value == 0 && slider.value == 1)
                index = -index;
            slider.value += index;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            hasShot = true;
            bulletRigid.AddForce(Vector2.left * force * slider.value);
        }
    }

    private void Replay()
    {
        if (isPlayerEnter)
        {
            
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
    private void Update()
    {
        Shoot();
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
