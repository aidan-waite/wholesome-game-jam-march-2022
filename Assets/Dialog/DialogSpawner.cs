using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSpawner : MonoBehaviour
{
  public GameObject DialogPrefab;

  void Awake()
  {
    GameObject x = Instantiate(DialogPrefab);
    x.name = "DialogManager";
  }
}
