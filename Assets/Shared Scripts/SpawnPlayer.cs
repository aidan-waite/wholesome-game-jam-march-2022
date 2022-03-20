using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
  public GameObject PlayerPrefab;

  void Awake()
  {
    GameObject p = Instantiate(PlayerPrefab, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
    p.name = "Player";
  }
}
