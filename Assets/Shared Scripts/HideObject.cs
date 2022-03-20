using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
  public GameObject ObjectToHide;

  public void DoHideObject()
  {
    ObjectToHide.SetActive(false);
  }
}
