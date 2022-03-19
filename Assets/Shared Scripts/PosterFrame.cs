using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PosterFrame : MonoBehaviour
{
  public LayerMask PlayerObjects;
  public float CheckRadius = 0.22f;

  bool placed = false;

  void Update()
  {
    if (placed)
    {
      return;
    }

    if (Physics2D.OverlapCircle(transform.position, CheckRadius, PlayerObjects))
    {
      placed = true;

      var path = Application.persistentDataPath + "/posterTexture.png";
      Texture2D texture = LoadPNG(Application.persistentDataPath + "/posterTexture.png");
      Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

      GetComponent<SpriteRenderer>().sprite = sprite;
      transform.localScale = Vector3.one / 11f;
    }
  }

  // http://answers.unity.com/answers/802424/view.html
  public Texture2D LoadPNG(string filePath)
  {
    Texture2D tex = null;
    byte[] fileData;

    if (File.Exists(filePath))
    {
      fileData = File.ReadAllBytes(filePath);
      tex = new Texture2D(2, 2);
      tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
    }
    return tex;
  }
}
