using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevelOnCollide : MonoBehaviour
{
  public string SceneToLoad;

  void OnCollisionEnter2D()
  {
    SceneManager.LoadScene(SceneToLoad);
  }
}
