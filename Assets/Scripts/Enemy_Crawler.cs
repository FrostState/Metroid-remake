using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Crawler : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator anim;

    public Upgrades rockets3;
    public Upgrades energy10;
    public Transform itemSpawner;

    bool isHit;
    public int state;
    public float speed;
    public bool isGrounded;
    public bool isOnWall;
    public float groundCheckRadius;
    public float wallDistance;
    public int health =3;
    int itemDrop;

    public LayerMask isGroundLayer;
    public LayerMask isWallLayer;

    public Transform wallCheck;
    public Transform groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        if (groundCheckRadius <= 0)
        {
            // Set a default value to variable if not set in Inspector
            groundCheckRadius = 0.2f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("GroundCheckRadius not set on " + name + ". Defaulting to " + groundCheckRadius);
        }
        if (0 == speed) { speed = 3f; }
        if (wallDistance <= 0)
        {
            wallDistance = 0.05f;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            health--;
            isHit = true;
        }else if(col.gameObject.tag == "Rocket")
        {
            health -= 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit) { }
        else
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position,
                   groundCheckRadius, isGroundLayer);
            isOnWall = Physics2D.OverlapCircle(groundCheck.position,
                groundCheckRadius, isWallLayer);
            if (this.transform.localScale.x > 0)
            {
                if (state == 0)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(90);
                        rb2d.velocity = new Vector2(0, 15);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(270);
                        rb2d.velocity = new Vector2(0, -15);
                    }
                    else
                    {
                        MoveRight();
                    }
                }
                else if (state == 90)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(180);
                        rb2d.velocity = new Vector2(-15, 0);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(0);
                        rb2d.velocity = new Vector2(15, 0);
                    }
                    else
                    {
                        MoveUp();
                    }
                }
                else if (state == 180)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(270);
                        rb2d.velocity = new Vector2(0, -15);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(90);
                        rb2d.velocity = new Vector2(0, 15);
                    }
                    else
                    { MoveLeft(); }
                }
                else if (state == 270)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(0);
                        rb2d.velocity = new Vector2(15, 0);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(180);
                        rb2d.velocity = new Vector2(-15, 0);
                    }
                    else
                    {
                        MoveDown();
                    }
                }
            }
            else
            {
                if (state == 0)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(270);
                        rb2d.velocity = new Vector2(0, 15);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(90);
                        rb2d.velocity = new Vector2(0, -15);
                    }
                    else
                    {
                        MoveLeft();
                    }
                }
                else if (state == 90)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(0);
                        rb2d.velocity = new Vector2(-15, 0);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(180);
                        rb2d.velocity = new Vector2(15, 0);
                    }
                    else
                    {
                        MoveDown();
                    }
                }
                else if (state == 180)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(90);
                        rb2d.velocity = new Vector2(0, -15);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(270);
                        rb2d.velocity = new Vector2(0, 15);
                    }
                    else
                    { MoveRight(); }
                }
                else if (state == 270)
                {
                    if (Physics2D.OverlapCircle(wallCheck.position, wallDistance, isWallLayer) || Physics2D.OverlapCircle(wallCheck.position, wallDistance, isGroundLayer))
                    {
                        RotateTo(180);
                        rb2d.velocity = new Vector2(15, 0);
                    }
                    else if (!isGrounded && !isOnWall)
                    {
                        RotateTo(0);
                        rb2d.velocity = new Vector2(-15, 0);
                    }
                    else
                    {
                        MoveUp();
                    }
                }
            }
        }
        anim.SetBool("isHit", isHit);
        if (health <= 0)
        {
            Upgrades temp;
            itemDrop = Random.Range(1, 5);
            if (itemDrop == 1 || itemDrop == 3)
            {
                temp = Instantiate(energy10);
                temp.transform.position = itemSpawner.position;
            }else if (itemDrop == 2)
            {
                temp = Instantiate(rockets3);
                temp.transform.position = itemSpawner.position;
            }
            Destroy(gameObject);
        }
    }

    void MoveLeft()
    {
        rb2d.velocity = new Vector2(-speed, 0);
    }

    void MoveRight()
    {
        rb2d.velocity = new Vector2(speed, 0);
    }

    void MoveUp()
    {
        rb2d.velocity = new Vector2(0, speed);
    }

    void MoveDown()
    {
        rb2d.velocity = new Vector2(0, -speed);
    }

    void RotateTo(int rotation)
    {
        state = rotation;
        this.transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}
