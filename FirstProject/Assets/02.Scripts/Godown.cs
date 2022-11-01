using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godown : MonoBehaviour
{
    Rigidbody2D rigidbody;
    BoxCollider2D collider;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("istouch", true);
            Destroy(gameObject, 0.7f);
        }
    }
}
