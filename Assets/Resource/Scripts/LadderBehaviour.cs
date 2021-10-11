using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class LadderBehaviour : MonoBehaviour
{
    private Collider2D m_ladderCollider;
    void Start()
    {
        m_ladderCollider = GetComponent<TilemapCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            PlayerStatus.isOnLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            PlayerStatus.isOnLadder = false;
        }
    }
}
