using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
  public float PlayerMovementSpeed;
  public Rigidbody2D PlayerRB;
  public GameObject Player;

  Vector3 playerFacingLeft = new Vector3(-1, 1, 1);
  Vector3 playerFacingRight = new Vector3(1, 1, 1);

  void Start()
  {
    setupPosters();
  }

  void Update()
  {
    movePlayer();
  }

  void setupPosters()
  {
    var path = Application.persistentDataPath + "/posterTexture.png";
    Texture2D texture = LoadPNG(Application.persistentDataPath + "/posterTexture.png");
    Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    GameObject[] posters = GameObject.FindGameObjectsWithTag("Poster");
    foreach (GameObject p in posters)
    {
      p.GetComponent<SpriteRenderer>().sprite = sprite;
      p.transform.localScale = Vector3.one / 7f;
    }
  }

  void movePlayer()
  {
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      //PlayerRB.AddForce(new Vector2(-PlayerMovementSpeed, 0f));
      PlayerRB.velocity = new Vector2(-PlayerMovementSpeed, PlayerRB.velocity.y);
      Player.transform.localScale = playerFacingLeft;

    }
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      //PlayerRB.AddForce(new Vector2(PlayerMovementSpeed, 0f));
      PlayerRB.velocity = new Vector2(PlayerMovementSpeed, PlayerRB.velocity.y);
      Player.transform.localScale = playerFacingRight;
    }
        if ((Input.GetKeyUp(KeyCode.A)) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            PlayerRB.velocity = (PlayerRB.velocity / 3);
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
