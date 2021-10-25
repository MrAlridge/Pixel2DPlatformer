using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static bool isOnClimb = false;          // 玩家是否在爬
    public static bool isAbleClimb;                 // 是否具备爬的资格
    public static bool isHurt;                      // 玩家是否受伤
    private static bool isGround;
    private static Vector2 checkPointPos;                // 玩家上次落地的地方
    public static Vector2 climbStartPos;                // 开始攀爬的地方
    private Coroutine currentCoroutine;
    public static int climbCountDown = 0;
    public static bool corotineFlag = false;
    void Start()
    {
        
    }

    void Update()
    {
        if(isOnClimb && corotineFlag == false)
        {
            climbCountDown = 8;
            currentCoroutine = StartCoroutine(ClimbCountDown());
        }
        if((!isOnClimb && corotineFlag == true) || isGround)
        {
            StopCoroutine(currentCoroutine);
        }
        // Debug.Log(isOnClimb.ToString() + ',' + corotineFlag.ToString());
    }

    public static void SetCheckPoint(Vector2 newPos)
    {
        if(newPos != checkPointPos)
        {
            checkPointPos = newPos;
        }
    }

    public static Vector2 GetCheckPoint()
    {
        return checkPointPos;
    }

    IEnumerator ClimbCountDown()
    {
        corotineFlag = true;
        while(climbCountDown > 0)
        {
            climbCountDown--;
            yield return new WaitForSeconds(1f);
        }
        if(climbCountDown == 0)
        {
            isOnClimb = false;
            corotineFlag = false;
            isAbleClimb = false;
            yield return 0;
        }
    }
}
