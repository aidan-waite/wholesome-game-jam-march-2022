using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
  public GameObject DialogContainer;
  public TextMeshProUGUI DialogText;
  public TextMeshProUGUI NameText;
  public Image PersonImage;
  public Animator DialogAnimator;
  public bool TestMode = false;

  public Sprite CrabbySprite;
  public Sprite LischaSprite;
  public Sprite SadieSprite;
  public Sprite ZoraSprite;

  PlayerCoreMovement playerCoreMovement;
  enum DialogTextState { closed, writing, written };
  DialogTextState currentState = DialogTextState.closed;
  List<DialogGroup> dialogGroups = new List<DialogGroup>();
  DialogGroup currentGroup;
  int currentLineInd;
  int currentCharacterInd;
  string currentId;
  int endEarlyInd;

  // Whether or not dialog can be opened
  bool available = false;

  bool playerEnabled = true;

  void Start()
  {
    currentLineInd = 0;

    if (TestMode)
    {
      LoadDialogWithFilename("crabdialog");
      PlayDialog("crabby-exchange", 1);
      DialogContainer.SetActive(true);
    }
    else
    {
      playerCoreMovement = GameObject.Find("Player").GetComponent<PlayerCoreMovement>();
      DialogContainer.SetActive(false);
    }
  }

  public void LoadDialogWithFilename(string filename)
  {
    parseCSVIntoDialogGroups(filename);
    foreach (DialogGroup d in dialogGroups)
    {
      print(d.Description());
    }
    available = true;
  }

  void Update()
  {
    if (playerCoreMovement)
    {
      bool shouldBeEnabled = currentState == DialogTextState.closed;
      if (shouldBeEnabled != playerEnabled)
      {
        if (!shouldBeEnabled)
        {
          playerCoreMovement.gameObject.GetComponent<Animator>().SetBool("Walking", false);
          playerCoreMovement.enabled = false;
        }
        else
        {
          playerCoreMovement.enabled = true;
        }

        playerEnabled = shouldBeEnabled;
      }
    }

    if (!available)
    {
      return;
    }

    if (TestMode && Input.GetKeyDown(KeyCode.P))
    {
      DialogNextStep();
    }

    if (currentState != DialogTextState.closed && Input.anyKeyDown)
    {
      DialogNextStep();
    }
  }

  public void PlayDialog(string id, int startingLineInd = 0, int end = -1)
  {
    print("PlayDialog " + id + " " + startingLineInd + " " + end);

    if (currentState != DialogTextState.closed)
    {
      print("Can't start a new dialog when an old one is active");
      return;
    }

    currentId = id;
    currentLineInd = startingLineInd;
    endEarlyInd = end;

    DialogNextStep();
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
        NameText.text = currentGroup.Lines[currentLineInd].PersonName;
        DialogText.text = "";
        DialogText.text = currentGroup.Lines[currentLineInd].Line;
        return;
      case DialogTextState.written:
        bool isEnd = false;

        if (currentLineInd == endEarlyInd)
        {
          isEnd = true;
        }

        if (currentLineInd == currentGroup.Lines.Count - 1)
        {
          isEnd = true;
        }

        if (isEnd)
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
    NameText.text = currentGroup.Lines[currentLineInd].PersonName;
    Sprite s = spriteForPersonName(currentGroup.Lines[currentLineInd].PersonName);
    PersonImage.sprite = s;
    PersonImage.SetNativeSize();
    PersonImage.rectTransform.sizeDelta *= 4;

    DialogText.text = "";
    print("line: " + currentGroup.Lines[currentLineInd]);
    StartCoroutine(ShowOneCharAtATime(currentGroup.Lines[currentLineInd].Line));
  }

  IEnumerator ShowOneCharAtATime(string line)
  {
    print("ShowOneCharAtATime " + line);
    if (currentState == DialogTextState.writing)
    {
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
  }

  void parseCSVIntoDialogGroups(string filename)
  {
    string fullString = Resources.Load<TextAsset>(filename).text;
    fullString = fullString.Replace("\"", "");
    var lines = fullString.Split('\n');

    // Skip the top row header
    for (int x = 1; x < lines.Length; x++)
    {
      int firstCommaInd = -1;
      int secondCommaInd = -1;

      for (int n = 0; n < lines[x].Length; n++)
      {
        if (lines[x][n] != ',')
        {
          continue;
        }

        if (firstCommaInd == -1)
        {
          firstCommaInd = n;
        }
        else if (secondCommaInd == -1)
        {
          secondCommaInd = n;
        }
        else
        {
          break;
        }
      }

      string line = lines[x];
      string id = line.Substring(0, firstCommaInd);
      string name = line.Substring(firstCommaInd + 1, secondCommaInd - firstCommaInd - 1);
      string message = line.Substring(secondCommaInd + 1, line.Length - secondCommaInd - 1);

      DialogLine v = new DialogLine(name, message);

      int index = dialogGroupsContainsID(id);
      if (index != -1)
      {
        dialogGroups[index].Lines.Add(v);
      }
      else
      {
        dialogGroups.Add(new DialogGroup(id, name, new List<DialogLine> { v }));
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

  Sprite spriteForPersonName(string name)
  {
    switch (name)
    {
      case "Crabby":
        return CrabbySprite;
      case "Lischa":
        return LischaSprite;
      case "Sadie":
        return SadieSprite;
      case "Zora":
        return ZoraSprite;
    }

    print("unable to find " + name);
    return null;
  }
}

class DialogGroup
{
  public string ID;
  public List<DialogLine> Lines;

  public DialogGroup(string newID, string name, List<DialogLine> lines)
  {
    ID = newID;
    Lines = lines;
  }

  public string Description()
  {
    return "id:" + ID + " line count:" + Lines.Count;
  }
}

class DialogLine
{
  public string PersonName;
  public string Line;

  public DialogLine(string personName, string line)
  {
    PersonName = personName;
    Line = line;
  }
}