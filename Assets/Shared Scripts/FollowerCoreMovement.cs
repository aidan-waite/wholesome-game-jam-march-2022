using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerCoreMovement : MonoBehaviour
{
  public string TargetName;
  public float MovementSpeed = 2.5f;

  Transform Target;
  Vector3 playerFacingLeft = new Vector3(-1, 1, 1);
  Vector3 playerFacingRight = new Vector3(1, 1, 1);
  Rigidbody2D rb;
  Animator anim;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
  }

  void Update()
  {
    if (!Target)
    {
      Target = GameObject.Find(TargetName).transform;
      return;
    }

    move();
    anim.SetBool("Walking", Mathf.Abs(rb.velocity.x) > 0.01);
  }

  void move()
  {
    float dist = Mathf.Abs(Target.position.x - transform.position.x);
    if (dist > 0.85f)
    {
      // Target is on our left so move left
      if (Target.position.x < transform.position.x)
      {
        rb.velocity = new Vector2(-MovementSpeed, rb.velocity.y);
        transform.localScale = playerFacingLeft;
      }
      else
      {
        rb.velocity = new Vector2(MovementSpeed, rb.velocity.y);
        transform.localScale = playerFacingRight;
      }
    }
  }
}
