using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samus : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public Transform projectileSpawnPointUp;
    public BeamProjectile beamProjectilePrefab;
    public RocketProjectile rocketProjectilePrefab;

    //public Projectile_Fireball projectilePrefab;
    public float projectileForce;

    // Method 1: Keeps a reference Rigidbody2D through script
    // - Not shown in Inspector
    Rigidbody2D rb;
    BoxCollider2D bc2d;
    CircleCollider2D cc2d;

    // Method 2: Keeps a reference Rigidbody2D through script
    // - Shown in Inspector
    public Rigidbody2D rb2;

    public float speed;
    public float jumpForce;

    // Is 'Character' on ground
    // - Are they able to jump
    // - Must be grounded to jump (aka no double jump)
    public bool isGrounded;

    // What is the Ground? 
    // - Player can only jump on GameObjects that are on "Ground" layer  
    public LayerMask isGroundLayer;
    public LayerMask isWallLayer;

    // Tells script where to check if 'Characer' is on ground
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform wallCheckTop;
    public Transform wallCheckBottom;
    public BoxCollider2D doorTrigger;

    // Size of overlapping circle being checked against ground Colliders
    public float groundCheckRadius;
    public float wallDistance;

    public bool isFacingLeft;

    Animator anim;

    public bool canBallMode;
    bool isBall;
    bool aimUp;
    bool isRocketMode;

    public int rockets=0;
    public int maxRockets=0;
    public int energy=99;
    public int maxEnergy=99;

    public AudioSource samSource;
    public AudioClip beamSnd;
    public AudioClip rocketSnd;

    // Use this for initialization
    void Start()
    {

        if (!projectileSpawnPoint)
            Debug.LogError("Missing projectileSpawn");
        //if (!projectilePrefab)
        //    Debug.LogError("Missing projectilePrefab");
        if (projectileForce == 0)
        {
            projectileForce = 7.0f;
            Debug.Log("projectileForce was not set. Defaulting to" + projectileForce);
        }

        // Method 1: Save a reference of Component in script
        // - Component must be added in Inspector
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        cc2d = GetComponent<CircleCollider2D>();

        // Check if Component exists
        if (!rb) // or if(rb == null)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("Rigidbody2D not found on " + name);
        }

        // Method 1: Save a reference of Component in script
        // - Component must be added in Inspector
        // - Component should be dragged into variable in Script through Inspector
        if (!rb2)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("Rigidbody2D not found on " + name);
        }

        // Check if variable is set to something not 0
        if (speed <= 0)
        {
            // Set a default value to variable if not set in Inspector
            speed = 5.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("Speed not set on " + name + ". Defaulting to " + speed);
        }

        // Check if variable is set to something not 0
        if (jumpForce <= 0)
        {
            // Set a default value to variable if not set in Inspector
            jumpForce = 5.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("JumpForce not set on " + name + ". Defaulting to " + jumpForce);
        }

        // Check if variable is set to something
        if (!groundCheck)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("GroundCheck not found on " + name);
        }

        // Check if variable is set to something not 0
        if (groundCheckRadius <= 0)
        {
            // Set a default value to variable if not set in Inspector
            groundCheckRadius = 0.2f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("GroundCheckRadius not set on " + name + ". Defaulting to " + groundCheckRadius);
        }

        if (wallDistance <= 0)
        {
            wallDistance = 0.05f;
        }

        // Save a reference of Component in script
        // - Component must be added in Inspector
        anim = GetComponent<Animator>();

        // Check if Component exists
        if (!anim)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogError("Animator not found on " + name);
        }
    }


    // Update is called once per frame
    void Update()
    {

        // Checks if Left (or a) or Right (or d) is pressed
        // - "Horizontal" must exist in Input Manager (Edit-->Project Settings-->Input)
        // - Returns -1(left), 1(right), 0(nothing)
        // - Use GetAxis for value -1-->0-->1 and all decimal places. (Gradual change in values)
        float moveValue = Input.GetAxisRaw("Horizontal");
        float vertValue = Input.GetAxisRaw("Vertical");
        float jumpValue = Input.GetAxisRaw("Jump");
        bool fireValue = Input.GetButton("Fire1");
        bool useRocketButton = Input.GetButtonDown("Fire2");

        
        bool jump = false;
       
        if (jumpValue > 0.2f)
        {
                jump = true;
        }

        if(useRocketButton && maxRockets > 0 && !isRocketMode)
        {
            isRocketMode = true;
        }else if ((isRocketMode && useRocketButton )|| rockets == 0)
        {
            isRocketMode = false;
        }

        if (vertValue<0 && canBallMode)
        {
            isBall = true;
        }else if (vertValue>0 && isBall)
        {
            isBall = false;
            aimUp = false;
        }else if (vertValue>0)
        {
            aimUp = true;
        }else if (vertValue <= 0)
        {
            aimUp = false;
        }
        if (isBall)
        {
            bc2d.enabled = false;
            cc2d.enabled = true;
        } else
        {
            bc2d.enabled = true;
            cc2d.enabled = false;
        }
        

        if ((isFacingLeft && moveValue > 0) || (!isFacingLeft && moveValue < 0))
        {
            Flip();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (isRocketMode && rockets > 0)
            {
                fireRocket();
                rockets--;
            }
            else
            {
                Debug.Log("Pew pew");
                fire();
            }
        }


        if (transform.localRotation.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            rb.AddForce(Vector2.right * speed * Time.deltaTime);
        }

        // Check if 'groundCheck' GameObject is touching a Collider on Ground Layer
        // - Can change 'groundCheckRadius' to a smaller value for better precision or if 'Character' is smaller or bigger
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,
            groundCheckRadius, isGroundLayer);

        // Check if "Jump" button was pressed
        // - "Jump" must exist in Input Manager (Edit-->Project Settings-->Input)
        // - Configuration can be changed later
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.Log("Jump");

            if (isBall)
            {
                isBall = false;
            }
            else
            {
                // Zeros out force before applying a new force
                // - If force is not zeroed out, the force of gravity will have an effect on the jump
                // - Not setting velocity to 0
                //   - Gravity is -9.8 and force up would be 5 causing a force of -4.8 to be applied
                // - Setting velocity to 0
                //   - Gravity is reset to and force up would be 5 causing a force of 5.0 to be applied
                rb.velocity = Vector2.zero;

                // Unit Vector shortcuts that can be used
                // - Vector2.up --> new Vector2(0,1);
                // - Vector2.down --> new Vector2(0,-1);
                // - Vector2.right --> new Vector2(1,0);
                // - Vector2.left --> new Vector2(-1,0);

                // Applies a force in the UP direction
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        if (this.transform.localScale.x < 0 && wallDistance > 0)
        {
            wallDistance *= -1;
        }else if (this.transform.localScale.x > 0 && wallDistance < 0)
        {
            wallDistance *= -1;
        }
        if (Physics2D.OverlapCircle(wallCheck.position,wallDistance, isWallLayer) || 
                (Physics2D.OverlapCircle(wallCheckTop.position, wallDistance, isWallLayer) && !isBall) || 
                Physics2D.OverlapCircle(wallCheckBottom.position, wallDistance, isWallLayer))
        {
            moveValue = 0;
        }
        

        // Check if Left Control was pressed

        // Move Character using Rigidbody2D
        // - Uses moveValue from GetAxis to move left or right
        rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);

        // Tells Animator to transition to another Clip
        // - Parameter must be created in Animator window under Parameter tab
        if (jump || !isGrounded)
        {
            jumpValue = 1.0f;
        }
        else
        {
            jumpValue = 0.0f;
        }
        anim.SetFloat("moveValue", Mathf.Abs(moveValue));
        anim.SetBool("idleFire", fireValue);
        anim.SetFloat("jump", jumpValue);
        anim.SetBool("ballMode", isBall);
        anim.SetBool("aimUp", aimUp);
        anim.SetBool("rocketMode", isRocketMode);
    }



    void Flip()
    {
        isFacingLeft = !isFacingLeft;

        Vector3 scaleFactor = transform.localScale;

        scaleFactor.x *= -1;
        //or scaleFactor.x = -scaleFactor.x;

        transform.localScale = scaleFactor;
    }

    void fire()
    {
        BeamProjectile temp;
        if (aimUp)
        {
            temp =
                Instantiate(beamProjectilePrefab,
                projectileSpawnPointUp.position,
                projectileSpawnPointUp.rotation);
        }
        else
        {
            temp =
                Instantiate(beamProjectilePrefab,
                projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);
        }
        if (aimUp)
        {
            temp.isVertFire = true;
            temp.speed = projectileForce;
        }
        else if (isFacingLeft)
        {
            temp.speed = projectileForce * (-1);
        }
        else
        {
            temp.speed = projectileForce;
        }
        PlaySound(beamSnd, 1.0f);
    }

   void fireRocket()
    {
        RocketProjectile temp;
        float rocketForce = projectileForce * 2 / 3;
        if (aimUp)
        {
            temp =
                Instantiate(rocketProjectilePrefab,
                projectileSpawnPointUp.position,
                projectileSpawnPointUp.rotation);
        }
        else
        {
            if (isFacingLeft)
            {
                temp =
                    Instantiate(rocketProjectilePrefab,
                    projectileSpawnPoint.position,
                    Quaternion.Euler(0, 0, 90));
            }
            else
            {
                temp =
                    Instantiate(rocketProjectilePrefab,
                    projectileSpawnPoint.position,
                    Quaternion.Euler(0, 0, -90));
            }
        }
        if (aimUp)
        {
            temp.isVertFire = true;
            temp.speed = rocketForce;
        }
        else if (isFacingLeft)
        {
            temp.speed = rocketForce * (-1);
        }
        else
        {
            temp.speed = rocketForce;
        }
        PlaySound(rocketSnd, 1.0f);
    }
   
    public void PlaySound(AudioClip clip, float volume=1.0f)
    {
        samSource.clip = clip;
        samSource.volume = volume;
        samSource.Play();
    }

}
