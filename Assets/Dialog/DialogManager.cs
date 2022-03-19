using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
  public GameObject DialogContainer;
  public TextMeshProUGUI DialogText;
  public TextMeshProUGUI NameText;
  public Animator DialogAnimator;

  enum DialogTextState { closed, writing, written };
  DialogTextState currentState = DialogTextState.closed;
  List<DialogGroup> dialogGroups = new List<DialogGroup>();
  DialogGroup currentGroup;
  int currentLineInd;
  int currentCharacterInd;
  string currentId;

  // Whether or not dialog can be opened
  bool available = false;

  void Start()
  {
    currentId = "bobby-greeting";
    currentLineInd = 0;

    parseCSVIntoDialogGroups();
    foreach (DialogGroup d in dialogGroups)
    {
      print(d.Description());
    }

    DialogContainer.SetActive(false);
    available = true;
  }

  void Update()
  {
    if (!available)
    {
      return;
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      DialogNextStep();
    }
  }

  void DialogNextStep()
  {
    if (!available)
    {
      return;
    }

    switch (currentState)
    {
      case DialogTextState.closed:
        currentState = DialogTextState.writing;
        ShowDialog();
        return;
      case DialogTextState.writing:
        currentState = DialogTextState.written;
        DialogText.text = currentGroup.Lines[currentLineInd];
        return;
      case DialogTextState.written:
        if (currentLineInd == currentGroup.Lines.Count - 1)
        {
          print("end case");
          currentState = DialogTextState.closed;
          StartCoroutine("hideDialog");
        }
        else
        {
          print("next line case");
          currentState = DialogTextState.writing;
          currentLineInd += 1;
          ShowDialog();
        }
        return;
    }
  }

  void ShowDialog()
  {
    if (!available)
    {
      return;
    }

    print("show dialog");
    currentCharacterInd = 0;
    DialogContainer.SetActive(true);
    DialogAnimator.SetBool("Show", true);
    int ind = dialogGroupsContainsID(currentId);
    if (ind == -1)
    {
      print("Dialog " + currentId + " not found!");
      return;
    }

    currentGroup = dialogGroups[ind];
    NameText.text = currentGroup.PersonName;
    DialogText.text = "";
    print("line: " + currentGroup.Lines[currentLineInd]);
    StartCoroutine(ShowOneCharAtATime(currentGroup.Lines[currentLineInd]));
  }

  IEnumerator ShowOneCharAtATime(string line)
  {
    print("ShowOneCharAtATime " + line);
    if (currentState != DialogTextState.writing)
    {
      yield break;
    }

    if (currentCharacterInd == 0)
    {
      yield return new WaitForSeconds(0.4f);
    }

    yield return new WaitForSeconds(0.025f);
    DialogText.text += line[currentCharacterInd];
    currentCharacterInd++;

    if (currentCharacterInd < line.Length)
    {
      print("not finished writing, go to next char");
      StartCoroutine(ShowOneCharAtATime(line));
    }
    else
    {
      print("finished writing");
      currentState = DialogTextState.written;
    }
  }

  void parseCSVIntoDialogGroups()
  {
    string fullString = Resources.Load<TextAsset>("dialog").text;
    fullString = fullString.Replace("\"", "");
    var lines = fullString.Split('\n');

    // Skip the top row header
    for (int x = 1; x < lines.Length; x++)
    {
      var lineParts = lines[x].Split(',');
      if (lineParts.Length < 3)
      {
        print("skip");
        continue;
      }

      string id = lineParts[0];
      string name = lineParts[1];
      string message = lineParts[2];

      int index = dialogGroupsContainsID(id);
      if (index != -1)
      {
        dialogGroups[index].Lines.Add(message);
      }
      else
      {
        dialogGroups.Add(new DialogGroup(id, name, new List<string> { message }));
      }
    }
  }

  int dialogGroupsContainsID(string id)
  {
    for (int x = 0; x < dialogGroups.Count; x++)
    {
      if (dialogGroups[x].ID == id)
      {
        return x;
      }
    }
    return -1;
  }

  IEnumerator hideDialog()
  {
    available = false;
    DialogAnimator.SetBool("Show", false);
    yield return new WaitForSeconds(1f);
    DialogContainer.SetActive(false);
    available = true;
  }
}

class DialogGroup
{
  public string ID;
  public string PersonName;
  public List<string> Lines;

  public DialogGroup(string newID, string name, List<string> lines)
  {
    ID = newID;
    PersonName = name;
    Lines = lines;
  }

  public string Description()
  {
    return "id:" + ID + " name:" + PersonName + " line count:" + Lines.Count;
  }
}