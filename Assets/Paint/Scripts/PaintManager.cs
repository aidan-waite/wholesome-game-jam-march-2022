using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class PaintManager : MonoBehaviour
{
  public Slider BrushSizeSlider;
  public NiceColors NiceColors;
  public GameObject ColorButtonPrefab;
  public GridLayoutGroup ButtonColorGrid;

  GameObject canvas;
  Texture2D texture;
  Sprite sprite;
  SpriteRenderer sr;
  MeshCollider mc;
  List<GameObject> buttonColorButtons = new List<GameObject>();

  int width;
  int height;

  Color brushColor;

  void Start()
  {
    canvas = new GameObject();
    width = Camera.main.pixelWidth;
    height = Camera.main.pixelHeight;

    canvas.name = "Paint Canvas Object";
    mc = canvas.AddComponent<MeshCollider>();
    sr = canvas.AddComponent<SpriteRenderer>() as SpriteRenderer;

    texture = new Texture2D(width, height);
    sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    sr.sprite = sprite;

    FillCanvasWith(Color.white);

    print("canvas width: " + width);
    print("canvas height: " + height);

    setupColors();
  }

  void Update()
  {
    if (Input.GetKey(KeyCode.Mouse0) && Input.mousePosition.y > 80)
    {
      int x = Mathf.RoundToInt(Input.mousePosition.x);
      int y = Mathf.RoundToInt(Input.mousePosition.y);

      Color[] cols = texture.GetPixels(0);

      int size = Mathf.FloorToInt(BrushSizeSlider.value);
      for (int a = -size; a <= size; a++)
      {
        for (int b = -size; b <= size; b++)
        {
          int i = ind(x + a, y + b);
          if (i > 0 && i < cols.Length)
          {
            cols[i] = brushColor;
          }
        }
      }

      texture.SetPixels(cols, 0);
      texture.Apply();
    }
  }

  public void RandomizeColors()
  {
    for (int x = 0; x < buttonColorButtons.Count; x++)
    {
      Destroy(buttonColorButtons[x]);
    }

    buttonColorButtons.Clear();
    NiceColors.RandomizeColors();
    setupColors();
  }

  // Convert x,y to the flattened array for GetPixels
  int ind(int x, int y)
  {
    return y * width + x;
  }

  void FillCanvasWith(Color color)
  {
    Color[] cols = new Color[width * height];
    for (int x = 0; x < width * height; x++)
    {
      if (x < 100)
      {
        cols[x] = Color.yellow;
      }
      else
      {
        cols[x] = color;
      }
    }

    texture.SetPixels(cols, 0);
    texture.Apply();
  }

  void setupColors()
  {
    for (int x = 0; x < 52; x++)
    {
      Color randomColor = NiceColors.GetRandomColor();
      GameObject button = Instantiate(ColorButtonPrefab);
      button.transform.SetParent(ButtonColorGrid.transform);
      button.GetComponent<Image>().color = randomColor;
      button.GetComponent<Button>().onClick.AddListener(delegate { setBrushColor(randomColor); });
      buttonColorButtons.Add(button);

      if (x == 0)
      {
        brushColor = randomColor;
      }
    }
  }

  void setBrushColor(Color color)
  {
    brushColor = color;
  }

  public void Continue()
  {
    var path = Application.persistentDataPath + "/posterTexture.png";
    print("Save texture to " + path);
    File.WriteAllBytes(path, (byte[])texture.EncodeToPNG());

    SceneManager.LoadScene("JumpTutorial");
  }
}
