using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamProjectile : MonoBehaviour
{
    CircleCollider2D cirCollid;
    Rigidbody2D rb;
    public float speed;
    public float lifeTime;
    public Transform charTransform;
    public bool isVertFire;


    // Start is called before the first frame update
    void Start()
    {
        cirCollid = GetComponent<CircleCollider2D>();
        charTransform = GetComponent<Transform>();

        if (speed == 0)
        {
            speed = 7.0f;
            Debug.Log("speed was not set. Defaulting to " + speed);
        }
        if (lifeTime <= 0)
        {
            lifeTime = 1.0f;
            Debug.Log("lifeTime was not set. Defaulting to " + lifeTime);
        }

        if (!isVertFire)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        }
        else {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        }
        Destroy(gameObject, lifeTime);
    }


    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.gameObject.CompareTag("Wall"))
    //    {
    //        Destroy(gameObject);
    //    }
    //    //if (col.gameObject.CompareTag("Door"))
    //    //{
    //    //    Destroy(this.gameObject);
    //    //}
    //    if (col.gameObject.CompareTag("Enemy"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Door" || other.tag == "Red_Door")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
