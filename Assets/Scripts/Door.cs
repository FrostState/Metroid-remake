using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Rigidbody2D playrb;
    BoxCollider2D boxCollid2D;
    //SpriteRenderer spr[2];
    //public SpriteRenderer doorCover;
    SpriteRenderer[] spr;
    // Start is called before the first frame update
    void Start()
    {
        boxCollid2D = GetComponent<BoxCollider2D>();
        spr = GetComponentsInChildren<SpriteRenderer>();

        spr[0].enabled = true;
        spr[1].enabled = false;
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Projectile")
    //    {
    //        boxCollid2D.enabled = false;
    //        doorCover.enabled = true;
    //        StartCoroutine(RemakeDoor());
    //    }
    //}


    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.gameObject.CompareTag("Projectile"))
    //    {
    //        Destroy(col.gameObject);
    //        boxCollid2D.enabled = false;
    //        doorCover.enabled = true;
    //        StartCoroutine(RemakeDoor());
    //    }
    //}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.gameObject.CompareTag("Red_Door"))
        {
            if (collider.gameObject.CompareTag("Rocket"))
            {
                boxCollid2D.enabled = false;
                spr[1].enabled = true;
                spr[0].enabled = false;
                StartCoroutine(RemakeDoor());
            }
        }else if (this.gameObject.CompareTag("Door"))
        {
            if (collider.gameObject.CompareTag("Projectile") || collider.gameObject.CompareTag("Rocket"))
            {
                boxCollid2D.enabled = false;
                spr[1].enabled = true;
                spr[0].enabled = false;
                StartCoroutine(RemakeDoor());
            }
        }
        else if (collider.gameObject.CompareTag("Enemy"))
        {
        }
        else if (collider.gameObject.CompareTag("Player"))
        {
            playrb = collider.gameObject.GetComponent<Rigidbody2D>();
            playrb.velocity = new Vector2(0, playrb.velocity.y);
            boxCollid2D.enabled = true;
            boxCollid2D.isTrigger = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RemakeDoor()
    {
        yield return new WaitForSeconds(3.0f);
        boxCollid2D.enabled = true;
        spr[1].enabled = false;
        boxCollid2D.isTrigger = false;
        spr[0].enabled = true;
    }
}
