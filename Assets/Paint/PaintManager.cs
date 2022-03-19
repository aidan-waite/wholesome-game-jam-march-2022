using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PaintManager : MonoBehaviour
{
  GameObject canvas;
  Texture2D texture;
  Sprite sprite;
  SpriteRenderer sr;
  MeshCollider mc;

  int width;
  int height;

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
  }

  void Update()
  {
    if (Input.GetKey(KeyCode.Mouse0))
    {
      int x = Mathf.RoundToInt(Input.mousePosition.x);
      int y = Mathf.RoundToInt(Input.mousePosition.y);

      Color[] cols = texture.GetPixels(0);

      for (int a = -2; a <= 2; a++)
      {
        for (int b = -2; b <= 2; b++)
        {
          cols[ind(x + a, y + b)] = Color.green;
        }
      }

      texture.SetPixels(cols, 0);
      texture.Apply();
    }
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
}
