using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collider)
    {
        collider.transform.SetParent(transform);
        collider.gameObject.GetComponent<Rigidbody2D>().drag *= 25;
        collider.gameObject.GetComponent<PlayerCoreMovement>().JumpForce *=2.2f;

    }



    private void OnTriggerExit2D(Collider2D collider)
    {
        collider.transform.SetParent(null);
        collider.gameObject.GetComponent<Rigidbody2D>().drag /= 25;
        collider.gameObject.GetComponent<PlayerCoreMovement>().JumpForce /=2.2f;

    }
}
