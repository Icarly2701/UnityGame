using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    Animator anim;
    BoxCollider2D boxCollider;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();   
        boxCollider = GetComponent<BoxCollider2D>(); 
        rigid = GetComponent<Rigidbody2D>();
    }

    bool updown = true;
    // Update is called once per frame
    void Update()
    {
        
        if (updown)
        {
            transform.Translate(Vector2.up * 2.5f * Time.deltaTime);
            CheckPlat();
        }
        else
        {
            transform.Translate(Vector2.down * 2.5f  * Time.deltaTime);
            CheckPlat();
        }
    }

    void CheckPlat()
    {
        RaycastHit2D rayHitD = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHitU = Physics2D.Raycast(rigid.position, Vector3.up, 1, LayerMask.GetMask("Platform"));

        if (rayHitD.collider == null && rayHitU.collider != null)
        {
            updown = false;
        }
        else if (rayHitU.collider == null && rayHitD.collider != null)
        {
            updown = true;
        }
    }
}
