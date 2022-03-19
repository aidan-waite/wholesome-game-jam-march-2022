using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CrabLevelManager : MonoBehaviour
{
  bool isWet = false;
  public BoxCollider2D puddle;
  public LayerMask PuddleObjects;
  public LayerMask TowelObjects;
  public LayerMask NPCs;
  public Transform GroundCheck;
  public float CheckRadius = 0.22f;
  public TextMeshProUGUI DryStatusText;
  public CapsuleCollider2D PlayerCollider;

  bool didInteractCrab = false;

  void Update()
  {
    checkPuddle();
    checkTowel();
    interactCrab();
  }

  void interactCrab()
  {
    if (!Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, NPCs))
    {
      return;
    }

    if (didInteractCrab)
    {
      return;
    }

    didInteractCrab = true;

    if (isWet)
    {
      // Lose Condition
      print("you lose!");
    }
    else
    {
      // Win codition
      print("You Win!");
    }
  }

  void checkPuddle()
  {
    if (!isWet && PlayerCollider.bounds.Intersects(puddle.bounds))
    {
      isWet = true;
      DryStatusText.text = "status: wet";
      print("you are in a puddle");
    }
  }

  void checkTowel()
  {
    if (isWet && Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, TowelObjects))
    {
      isWet = false;
      DryStatusText.text = "status: dry";
      print("you dried off on the towel");
    }
  }
}
