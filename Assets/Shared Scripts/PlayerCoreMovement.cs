using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreMovement : MonoBehaviour
{

  public float PlayerMovementSpeed;
  public float JumpForce;
  public Transform GroundCheck;
  public LayerMask groundObjects;
  public float CheckRadius = 0.22f;
  public bool IsGrounded;

  bool canDoubleJump;

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
    movePlayer();
    anim.SetBool("Walking", IsGrounded && Mathf.Abs(rb.velocity.x) > 0.01);
  }

  void movePlayer()
  {
    IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, groundObjects);
    if (IsGrounded)
    {
      canDoubleJump = true;
    }
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      rb.velocity = new Vector2(-PlayerMovementSpeed, rb.velocity.y);
      transform.localScale = playerFacingLeft;
    }
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      rb.velocity = new Vector2(PlayerMovementSpeed, rb.velocity.y);
      transform.localScale = playerFacingRight;
    }

    if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
    {
      rb.AddForce(new Vector2(0f, JumpForce));
      return;
    }
    else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
    {
      rb.AddForce(new Vector2(0f, JumpForce));
      canDoubleJump = false;
      return;

    }

    //slows player down without feeling like you're walking on ice.
    if ((Input.GetKeyUp(KeyCode.A)) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
    {
      rb.velocity = (rb.velocity / 3);
    }
  }
}
