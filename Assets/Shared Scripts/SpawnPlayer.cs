using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
  public GameObject PlayerPrefab;

  void Awake()
  {
    Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
  }
}
