//using System.Diagnostics;
//using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BaseEnemyController : MonoBehaviour 
{
    // -----Status-----
    public int damage;  // 敌人造成的伤害
    /// <summary>
    /// 移动速度，以帧计算
    /// </summary>
    public float enemySpeed = 2.0f;
 
    /// <summary>
    /// 碰撞图层
    /// </summary>
    public LayerMask layer;

    /// <summary>
    /// 敌人前上方位置，墙壁检测
    /// </summary>
    public Transform enemyForwardUp;

    /// <summary>
    /// 敌人前下方位置，悬崖检测
    /// </summary>
    public Transform enemyForwardDown;

    /// <summary>
    /// 敌人中心位置，攻击检测
    /// </summary>
    public Transform enemyCenter;

    private bool _collided; // 敌人碰撞状态
    private Color red = Color.red, green = Color.green;

    private Rigidbody2D _rigidbody2D; // 敌人刚体
    private CapsuleCollider2D _capsuleCollider2D; // 胶囊检测器
    private Animator _Animator; // 动画状态机
    private SpriteRenderer _spriteRender;   // 渲染

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _Animator = GetComponent<Animator>();
        _spriteRender = GetComponentInChildren<SpriteRenderer>();
    }
 
    private void FixedUpdate() // 移动状态
    {
        // 移动方向
        Vector3 movement = new Vector3(enemySpeed, _rigidbody2D.velocity.y, 0);
        // 左右翻转
        {
            if(movement.x > 0)
            {
                _spriteRender.flipX = false;
                enemyForwardUp.localPosition = new Vector3(-enemyForwardUp.localPosition.x, enemyForwardUp.localPosition.y, enemyForwardUp.localPosition.z);
                enemyForwardDown.localPosition = new Vector3(-enemyForwardUp.localPosition.x, enemyForwardUp.localPosition.y, enemyForwardUp.localPosition.z);
            }else if(movement.x < 0)
            {
                _spriteRender.flipX = true;
                enemyForwardUp.localPosition = new Vector3(-enemyForwardUp.localPosition.x, enemyForwardUp.localPosition.y, enemyForwardUp.localPosition.z);
                enemyForwardDown.localPosition = new Vector3(-enemyForwardUp.localPosition.x, enemyForwardUp.localPosition.y, enemyForwardUp.localPosition.z);
            }
        }
        // 平面移动
        transform.position += movement * Time.deltaTime;

        // 两点连线碰撞检测
        _collided = Physics2D.Linecast(enemyForwardUp.position, enemyForwardDown.position, layer);

        if (_collided) // 发生碰撞
        {
            // 调试辅助线
            Debug.DrawLine(enemyForwardUp.position, enemyForwardDown.position, red);
            // 改变移动方向
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, 0);
            enemySpeed *= -1f;
        }
        else // 无碰撞
        {
            // 调试辅助线
            Debug.DrawLine(enemyForwardUp.position, enemyForwardDown.position, green);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) // 被玩家攻击碰撞
    {
        if (other.gameObject.tag == "PlayerAttack") // 遇到攻击
        {
            // 攻击与敌人的距离
            float length = System.Math.Abs(other.contacts[0].point.x - enemyCenter.position.x);

            if (length <= 1.0f) // 有效攻击距离
            {
                // 击杀敌人效果待写
                // 敌人死亡，停止移动
                enemySpeed = 0f;
                // 死亡动画
                _Animator.SetTrigger("die");
                // 取消碰撞
                _capsuleCollider2D.enabled = false;
                _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

                // 1s消失动画
                Destroy(gameObject, 1f);
            }

        }
        if(other.gameObject.tag == "Player")    // 触碰事件
        {
            if(other.transform.position.x > transform.position.x)
            {
                BasePlayerController.Hurt(1f, damage);
            }else if(other.transform.position.x < transform.position.x)
            {
                BasePlayerController.Hurt(-1f, damage);
            }
        }
    }
}