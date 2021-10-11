using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerController : MonoBehaviour
{
    /// <summary>
    /// 冲刺所用时间，以帧计算
    /// </summary>
    public float dashTime;
    /// <summary>
    /// 冲刺的速度
    /// </summary>
    public float dashSpeed;
    /// <summary>
    /// 重力的值
    /// </summary>
    public float gravityValue;
    /// <summary>
    /// 玩家的速度
    /// </summary>
    public float playerSpeed;
    /// <summary>
    /// 玩家爬梯子的速度
    /// </summary>
    public float playerClimbSpeed;
    /// <summary>
    /// 玩家的跳跃力度
    /// </summary>
    public float playerJumpSpeed;
    /// <summary>
    /// 角色脚下的偏移量
    public float footOffset = 0.375f;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;
    /// </summary> 
    // -----Status-----
    public Animator playerAnimator;                                 // 动画状态机
    private CharacterController m_playerController;                 // 角色控制器
    private SpriteRenderer m_playerRender;                          // 精灵渲染器
    private bool isGround;                                          // 是否在地上
    private bool isMoveableLeft;                                    // 是否可以左移动
    private bool isMoveableRight;                                   // 是否可以右移动
    private bool isDashing = false;                                 // 是否在冲刺
    private Vector3 m_moveDirction = Vector3.zero;                  // 角色控制器的移动方向
    private float m_horizontalInput;                                // 水平输入
    private float m_verticalInput;                                  // 垂直输入
    private bool m_jumpInput;                                       // 跳跃输入
    private int jumpCount;                                          // 跳跃次数
    private float dashDir;                                          // 控制冲刺方向的变量
    private float currentDashTime;                                  // 冲刺使用的时间
    void Start()
    {
        m_playerController = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
        m_playerRender = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        GroundCheck();
        if(!PlayerStatus.isOnLadder || !isDashing)
        {
            Gravity();
        }
        GetInput();
        Jump();
        ScanDashDir();
        if(!PlayerStatus.isOnLadder)
        {
            Move(m_horizontalInput);
        }else{
            Climb();
        }
    }

    void Gravity()          // 重力计算
    {
        if(!isGround)
        {
            m_moveDirction.y -= gravityValue * Time.deltaTime;
        }
        else{
            m_moveDirction.y = 0;           // 重力不生效
        }
    }

    /// <summary>
    /// 检测冲刺方向的函数
    /// </summary>
    void ScanDashDir()
    {
        if(m_playerRender.flipX)
        {
            dashDir = -1f;
        }else{
            dashDir = 1f;
        }
    }

    void Move(float inputDir)               // 两个方向的移动
    {
        /// <summary>
        /// 从这里开始到结束，都是手动实现的水平方向墙壁碰撞处理
        if(inputDir < 0 && !isMoveableLeft)
        {
            m_moveDirction.x = 0;
        }else{
            if(inputDir > 0 && !isMoveableRight)
            {
                m_moveDirction.x = 0;
            }else{
                m_moveDirction.x = inputDir * playerSpeed * Time.deltaTime;
            }
        }
        if(isDashing)
        {
            Debug.Log("OK");
            Dash();
            return;
        }
        /// </summary>
        m_playerController.Move(m_moveDirction);            // 让角色移动
        /// <summary>
        /// 设置移动时的动画
        if(Mathf.Abs(m_playerController.velocity.x) >= 0.05f)
        {
            playerAnimator.SetBool("isMove", true);
        }else{
            playerAnimator.SetBool("isMove", false);
        }
        /// </summary>
        /// <summary>
        /// 精灵渲染器水平翻转
        if(m_playerController.velocity.x > 0.2f){
            m_playerRender.flipX = false;
        }
        else{
            if(m_playerController.velocity.x < -0.2f)
            {
                m_playerRender.flipX = true;
            }
        }
        /// </summary>
    }

    void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        /*
        // -这一段暂时弃用，因为跟Move不在一个函数，导致限制水平输入的功能无法实现-
        if(!isMoveableLeft && m_moveDirction.x < 0)
        {
            m_horizontalInput = 0;
        }
        if(!isMoveableRight && m_moveDirction.x > 0)
        {
            m_horizontalInput = 0;
        }
        */
        // 跳跃处理
        if(!isDashing)
        {
            if(Input.GetButtonDown("Dash"))
            {
                isDashing = true;
                playerAnimator.SetBool("isDashing", true);
                currentDashTime = dashTime;
            }
        }
        m_jumpInput = Input.GetButtonDown("Jump");
    }

    /// <summary>
    /// 跳跃函数
    /// </summary>
    void Jump()
    {
        if(m_jumpInput && isGround)
        {
            if(jumpCount == 0)          // 改这个条件可以实现多段跳
            {
                // 跳跃的公式
                m_moveDirction.y = playerJumpSpeed;
                playerAnimator.SetTrigger("jumpTrigger");
                jumpCount++;
            }
        }
    }
    /// <summary>
    /// 冲刺函数
    /// </summary>
    void Dash()
    {
        if (currentDashTime <= 0)
        {
            isDashing = false;
            currentDashTime = 0;
            playerAnimator.SetBool("isDashing", false);
            return;
        }
        // 冲刺的移动速度逻辑
        m_playerController.Move(new Vector3(dashDir * dashSpeed * Time.deltaTime, 0f, 0f));
        currentDashTime = currentDashTime - Time.deltaTime;
    }
    /// <summary>
    /// 爬梯子的函数
    /// </summary>
    void Climb()
    {
        m_playerController.Move(new Vector3(m_horizontalInput * playerSpeed * Time.deltaTime, m_verticalInput * playerClimbSpeed * Time.deltaTime, 0f));
    }

    /// <summary>
    /// 地面检测用的函数
    /// </summary>
    void GroundCheck()
    {
        // -----上下碰撞检测-----
        RaycastHit2D leftFootCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D righFootCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D leftHeadCheck = Raycast(new Vector2(-footOffset, 1.3f), Vector2.up, groundDistance, groundLayer);
        RaycastHit2D rightHeadCheck = Raycast(new Vector2(footOffset, 1.3f), Vector2.up, groundDistance, groundLayer);
        // -----左右碰撞检测-----
        RaycastHit2D leftUpCheck = Raycast(new Vector2(-footOffset, 1.3f), Vector2.left, groundDistance, groundLayer);
        RaycastHit2D leftMidCheck = Raycast(new Vector2(-footOffset, 0.65f), Vector2.left, groundDistance, groundLayer);
        RaycastHit2D leftDownCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.left, groundDistance, groundLayer);
        RaycastHit2D rightUpCheck = Raycast(new Vector2(footOffset, 1.3f), Vector2.right, groundDistance, groundLayer);
        RaycastHit2D rightMidCheck = Raycast(new Vector2(footOffset, 0.65f), Vector2.right, groundDistance, groundLayer);
        RaycastHit2D rightDownCheck = Raycast(new Vector2(footOffset, 0f), Vector2.right, groundDistance, groundLayer);
        if (leftFootCheck || righFootCheck)
        {
            isGround = true;
            jumpCount = 0;
        }
        else
        {
            isGround = false;
        }
        if(leftHeadCheck || rightHeadCheck)
        {
            m_moveDirction.y = 0;
        }
        // -----左右墙壁碰撞处理-----
        if(leftUpCheck || leftMidCheck || leftDownCheck)
        {
            isMoveableLeft = false;
            m_moveDirction.x = 0;
            dashDir = 0f;
            currentDashTime = 0f;
        }else{
            isMoveableLeft = true;
        }
        if(rightUpCheck || rightMidCheck || rightDownCheck)
        {
            isMoveableRight = false;
            m_moveDirction.x = 0;
            dashDir = 0f;
            currentDashTime = 0f;
        }else{
            isMoveableRight = true;
        }
    }
    /// <summary>
    /// 地面检测中的raycast方法
    /// </summary>
    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiraction,float length,LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length, color);

        return hit;
    }
}
