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
  public float CheckRadius = 0.22f;
  public TextMeshProUGUI DryStatusText;
  public GameObject Crab;
  public AudioClip WetClip;

  CapsuleCollider2D playerCollider;
  Transform groundCheck;
  DialogManager dialogManager;
  bool didInteractCrab = false;
  bool didSetup = false;
  AudioSource audioSource;

  IEnumerator Start()
  {
    yield return new WaitForEndOfFrame();
    yield return new WaitForEndOfFrame();
    yield return new WaitForEndOfFrame();

    GameObject player = GameObject.Find("Sadie");
    groundCheck = player.transform.Find("GroundCheck");
    playerCollider = player.GetComponent<CapsuleCollider2D>();
    dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
    dialogManager.LoadDialogWithFilename("crabdialog");
    audioSource = GetComponent<AudioSource>();

    didSetup = true;
  }

  void Update()
  {
    if (!didSetup)
    {
      return;
    }

    checkPuddle();
    checkTowel();
    interactCrab();
  }

  void interactCrab()
  {
    float dist = Vector3.Distance(groundCheck.transform.position, Crab.transform.position);
    if (dist < 1f && !didInteractCrab)
    {
      didInteractCrab = true;

      if (isWet)
      {
        print("You lose!");
        dialogManager.PlayDialog("crabby-exchange", 0, 0);
      }
      else
      {
        print("You Win!");
        dialogManager.PlayDialog("crabby-exchange", 1);
      }
    }

    if (dist > 1.5f && didInteractCrab)
    {
      didInteractCrab = false;
    }
  }

  void checkPuddle()
  {
    if (!isWet && playerCollider.bounds.Intersects(puddle.bounds))
    {
      audioSource.PlayOneShot(WetClip);
      isWet = true;
      DryStatusText.text = "status: wet";
      DryStatusText.transform.localScale *= 2;
      print("you are in a puddle");
    }
  }

  void checkTowel()
  {
    if (isWet && Physics2D.OverlapCircle(groundCheck.position, CheckRadius, TowelObjects))
    {
      isWet = false;
      DryStatusText.text = "status: dry";
      DryStatusText.transform.localScale /= 2;
      print("you dried off on the towel");
    }
  }
}
