using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCanvas : MonoBehaviour
{
  public TextMeshProUGUI TimeText;
  public int elapsedSeconds = 0;

  void Awake()
  {
    DontDestroyOnLoad(gameObject);
    TimeText.text = "00:00";
    InvokeRepeating("Increment", 1f, 1f);
  }

  void Increment()
  {
    elapsedSeconds += 1;

    int mins = Mathf.FloorToInt(elapsedSeconds / 60);
    string minsString = "";
    if (mins < 10)
    {
      minsString += "0";
    }
    minsString += mins;

    int secs = elapsedSeconds % 60;
    string secString = "";
    if (secs < 10)
    {
      secString += "0";
    }
    secString += secs;

    TimeText.text = minsString + ":" + secString;
  }
}
