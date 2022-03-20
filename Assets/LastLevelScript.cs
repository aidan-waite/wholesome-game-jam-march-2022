using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLevelScript : MonoBehaviour
{
    CapsuleCollider2D playerCollider;

    public BoxCollider2D catCollider;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerCollider = player.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchFomo();
    }

    void TouchFomo()
    {
        if(playerCollider.bounds.Intersects(catCollider.bounds))
    {

            print("You Win!");
        }
    }
}
