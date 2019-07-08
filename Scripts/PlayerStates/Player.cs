﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerState currentState;//当前状态
    public List<PlayerState> playerStates;//所有状态
    public float height = 2.0f;//角色高
    public float width = 1.0f;//角色宽
    public float speed = 4.0f;//角色移动速度
    public float G = 20.0f;//角色受到的重力
    public int rayY = 3;//竖直方向发射的射线数量
    public int rayX = 5;//水平方向发射的射线数量
    public bool flip;//true = 朝右
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject deathAnimation;
    public GameObject deathUI;
    [HideInInspector]
    public float currentVelocity;
    [HideInInspector]
    public float verticalVelocity;
    [HideInInspector]
    public float horizontalVelocity;
    public bool canMove = true;
    public GameManager gameManager;
    /// <summary>
    /// 状态在第一次运行或切换时调用
    /// </summary>
    private void Start()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        foreach(PlayerState state in playerStates)
        {
            //进行状态默认设置
            state.player = this;
            state.SetType();
        }
        //设置默认状态
        if (!currentState)
        {
            currentState = GetComponent<Stand>();
        }
        flip = false;
        currentState.StateStart();
        
    }
    public bool death;//是否死亡
    /// <summary>
    /// 状态在每帧更新时调用
    /// </summary>
    private void Update()
    {
        if (!death)
        {
            currentState.StateUpdate();
        }

        if(!death && transform.position.y < - 30)
        {
            Die();
        }
        if (spriteRenderer)
        {
            spriteRenderer.flipX = flip;
        }

        horizontalVelocity = speed * Input.GetAxis("Horizontal");
        if (currentState.stateType == StateType.Stand)
        {
            verticalVelocity = 0;
        }
        currentVelocity = Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity);
    }

    public void Die()
    {
        death = true;
        List<Component> comList = new List<Component>();
        foreach (var component in gameObject.GetComponents<Component>())
        {
            if (!(component is Transform || component is Player))
                comList.Add(component);
        }
        foreach (Component item in comList)
        {
            Destroy(item);
        }
        Instantiate(deathAnimation, transform.position ,transform.rotation);
        GameObject.Find("FollowCoins").GetComponent<FollowCoins>().DestroyCoins();
        StartCoroutine(InstantiateDeathUI());
        if (gameManager) gameManager.deathNumber++;
    }

    IEnumerator InstantiateDeathUI()
    {
        float time = 0;
        while (time <= 1.2f) { 
            time += Time.deltaTime;
            yield return null;
        }
        Instantiate(deathUI);
    }
}