using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DialogGroup
{
  string ID;
  string PersonName;
  List<string> Lines;
}

public class DialogManager : MonoBehaviour
{
  List<DialogGroup> dialogGroups = new List<DialogGroup>();

  void Start()
  {
    parseCSVIntoDialogGroups();
  }

  void parseCSVIntoDialogGroups()
  {
    string fullString = Resources.Load<TextAsset>("dialog").text;
    var lines = fullString.Split('\n');

    foreach (string line in lines)
    {
      var lineParts = line.Split('\n');
      var id = lineParts[0];
      var name = lineParts[1];
      var message = lineParts[2];
    }
  }
}
