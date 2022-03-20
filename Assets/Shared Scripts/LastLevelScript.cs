using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LastLevelScript : MonoBehaviour
{
  public TextMeshProUGUI FinalTimeText;

  void Start()
  {
    GameObject.Find("Controls Canvas").SetActive(false);

    GameObject timeCanvas = GameObject.Find("Time Canvas");
    if (timeCanvas && timeCanvas.activeSelf)
    {
      FinalTimeText.gameObject.SetActive(true);
      FinalTimeText.text = "final time: " + GameObject.Find("Time Canvas").GetComponent<TimeCanvas>().TimeText.text;
      timeCanvas.SetActive(false);
    }
    else
    {
      FinalTimeText.gameObject.SetActive(false);
    }
  }
}
