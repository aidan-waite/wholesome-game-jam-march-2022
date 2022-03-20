using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PosterFrame : MonoBehaviour
{
  public LayerMask PlayerObjects;
  public float CheckRadius = 0.22f;
  public AudioClip Hit;
  public GameObject PosterEffect;

  AudioSource audioSource;
  bool placed = false;
  Texture2D texture;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();

    var path = Application.persistentDataPath + "/posterTexture.png";
    texture = LoadPNG(Application.persistentDataPath + "/posterTexture.png");
  }

  void Update()
  {
    if (placed)
    {
      return;
    }

    if (Physics2D.OverlapCircle(transform.position, CheckRadius, PlayerObjects))
    {
      StartCoroutine("spawnEffect");

      audioSource.volume = 0.2f;
      audioSource.PlayOneShot(Hit);
      placed = true;
      Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

      GetComponent<SpriteRenderer>().color = Color.white;
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

  IEnumerator spawnEffect()
  {
    GameObject obj = Instantiate(PosterEffect, transform.position, transform.rotation);
    yield return new WaitForSeconds(2f);
    Destroy(obj);
  }
}
