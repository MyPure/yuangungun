﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType {
    Stand,
    Jump,
    DoubleJump,
    Drop
}

public class PlayerState : MonoBehaviour
{
    [HideInInspector]
    public StateType stateType;
    [HideInInspector]
    public Player player;
    public virtual void HandleInput()
    {
        //输入判定
    }
    public virtual void StateStart()
    {
        
    }
    public virtual void StateUpdate()
    {

    }
    public virtual void SetType()
    {

    }
    public override string ToString()
    {
        return stateType.ToString();
    }
    public static StateType Prase(string str)
    {

        switch (str)
        {
            case "Stand":return StateType.Stand;
            case "Jump":return StateType.Jump;
            case "DoubleJump":return StateType.DoubleJump;
            case "Drop":return StateType.Drop;
            default:return StateType.Stand;
        }
    }
    /// <summary>
    /// 状态切换
    /// </summary>
    /// <param name="type">目标状态</param>
    public void ChangeStateTo(StateType type)
    {
        foreach (PlayerState s in player.playerStates)
        {
            if(s.stateType == type)
            {
                player.currentState = s;
                player.currentState.StateStart();
                return;
            }
        }
        Debug.Log("未能找到对应的状态，状态转换失败");
    }
    public void HorizontalMove()//水平移动，请在HandleInput里调用
    {
        if (player.connectType == ConnectType.NotConnecting || player.connectType == ConnectType.ThisConnecting)
        {
            if (!player.canMove)
            {
                return;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                player.flip = false;
                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                for (int i = 0; i < player.rayX; i++)
                {
                    hits.Add(Physics2D.Raycast((Vector2)transform.position + new Vector2(player.width / 2 + 0.02f, -player.height / 2 + i * player.height / (player.rayX - 1)), Vector2.right, 0.04f, ~(1 << 8)));
                }
                bool haveObstacle = false;
                foreach (RaycastHit2D h in hits)
                {
                    if (h.collider && !h.collider.isTrigger)
                    {
                        haveObstacle = true;
                        transform.position = new Vector3(h.point.x - player.width / 2 - 0.03f, transform.position.y, 0);
                        break;
                    }
                }
                if (!haveObstacle) transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * player.speed * Time.deltaTime);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                player.flip = true;
                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                for (int i = 0; i < player.rayX; i++)
                {
                    hits.Add(Physics2D.Raycast((Vector2)transform.position + new Vector2(-player.width / 2 - 0.02f, -player.height / 2 + i * player.height / (player.rayX - 1)), Vector2.left, 0.04f, ~(1 << 8)));
                }
                bool haveObstacle = false;
                foreach (RaycastHit2D h in hits)
                {
                    if (h.collider && !h.collider.isTrigger)
                    {
                        haveObstacle = true;
                        transform.position = new Vector3(h.point.x + player.width / 2 + 0.03f, transform.position.y, 0);
                        break;
                    }
                }
                if (!haveObstacle) transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * player.speed * Time.deltaTime);
            }
        }
    }
}
