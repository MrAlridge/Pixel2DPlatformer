using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int maxiumHealthValue;                   // 玩家最大血量
    public static bool isOnLadder = false;          // 玩家是否在爬梯子
    public static bool isHurt;                      // 玩家是否受伤
    public static int healthValue;                  // 玩家血量
    public int hurtCDTime;                          //
    private int currentTime;                        // 受伤冷却倒计时

    void Start()
    {
        currentTime = hurtCDTime;
    }

    public void StartHurtCoolDown()
    {
        StartCoroutine("HurtCooldown");
    }

    public IEnumerator HurtCooldown()
    {
        while(currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSeconds(1f);
        }
        if(currentTime == 0)
        {
            isHurt = false;
            currentTime = hurtCDTime;
            yield break;
        }
    }
}
