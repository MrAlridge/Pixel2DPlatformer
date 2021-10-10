using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerController : MonoBehaviour
{

    /// <summary>
    /// 重力的值
    /// </summary>
    public float gravityValue;
    /// <summary>
    /// 玩家的速度
    /// </summary>
    public float playerSpeed;
    /// <summary>
    /// 玩家的跳跃力度
    /// </summary>
    public float playerJumpSpeed;
    private Animator m_playerAnimator;
    private CharacterController m_playerController;
    private bool isGround;                          // 是否在地上
    private Vector3 m_moveDirction = Vector3.zero;                  // 角色控制器的移动方向
    private float m_horizontalInput;                                // 水平输入
    private bool m_jumpInput;                                       // 跳跃输入
    //private LayerMask m_groundLayer = LayerMask.GetMask("Ground");
    void Start()
    {
        m_playerController = GetComponentInChildren<CharacterController>();
    }

    void Update()
    {
        isGround = m_playerController.isGrounded;
        GetInput();
        Gravity();
        Move();
    }

    void Gravity()          // 重力计算
    {
        if(!isGround)
        {
            m_moveDirction.y -= gravityValue * Time.deltaTime;
        }
        else{
            m_moveDirction.y -= gravityValue;
        }
    }

    void Move()
    {
        m_moveDirction.x = m_horizontalInput * playerSpeed * Time.deltaTime;
        m_playerController.Move(m_moveDirction);
    }

    void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            m_jumpInput = true;
        }
    }

    void Jump()
    {
        if(m_jumpInput)
        {
            if(isGround)
            {
                // 跳跃的公式
                m_moveDirction.y = playerJumpSpeed;
            }
        }
    }
}
