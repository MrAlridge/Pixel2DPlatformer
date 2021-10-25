using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    private TilemapCollider2D collider2D;
    void Start()
    {
        collider2D = this.GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            PlayerStatus.isHurt = true;
        }
    }
}
