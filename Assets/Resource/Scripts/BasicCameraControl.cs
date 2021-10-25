using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraControl : MonoBehaviour
{
    public Transform MidLine;
    public Transform camPos1, camPos2;
    private Transform player;
    private Vector3 playerPos;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.position;
        transform.position = new Vector3(camPos1.position.x, 0.1f, -10f);
        /*
        if(playerPos.x > MidLine.position.x)
        {
            
        }else if(playerPos.x < MidLine.position.x){
            
        }
        */
    }
    /*
    void MoveTo(int index)
    {
        if(index == 1)
        {
            if(transform.position != camPos1.position)
            {
                transform.Translate(new Vector3(Mathf.Lerp()) * Time.deltaTime);
            }
        }else if(index == 2)
        {
            if(transform.position != camPos2.position)
            {
                transform.Translate(camPos2.position * Time.deltaTime);
            }
        }
    }
    */
}
