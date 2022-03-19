using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public float PlayerMovementSpeed;
    public Rigidbody2D PlayerRB;
    public GameObject Player;

    //jump variables
    public float JumpForce;
    public Transform GroundCheck;
    public LayerMask groundObjects;
    public float CheckRadius;
    public bool isGrounded;
    bool DoubleJump;

    //CrabLevelVariables
    bool isWet;
    public BoxCollider2D puddle;
    public LayerMask PuddleObjects;
    public LayerMask PosterOjects;

    Vector3 playerFacingLeft = new Vector3(-1, 1, 1);
    Vector3 playerFacingRight = new Vector3(1, 1, 1);

    void Start()
    {
        setupPosters();
    }

    void Update()
    {
        movePlayer();
        GetWet();
    }

     void GetWet()
    {
        if (Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, PuddleObjects) && isWet == false)
        {
            isWet = true;
            print("you are in a puddle");
        }
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
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, groundObjects);
        if (isGrounded)
        {
            DoubleJump = true;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            PlayerRB.velocity = new Vector2(-PlayerMovementSpeed, PlayerRB.velocity.y);
            Player.transform.localScale = playerFacingLeft;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            PlayerRB.velocity = new Vector2(PlayerMovementSpeed, PlayerRB.velocity.y);
            Player.transform.localScale = playerFacingRight;
        }
 
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerRB.AddForce(new Vector2(0f, JumpForce));
            return;
        }
        else if(DoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerRB.AddForce(new Vector2(0f, JumpForce));
            DoubleJump = false;
            return;

        }


        //slows player down without feeling like you're walking on ice.
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