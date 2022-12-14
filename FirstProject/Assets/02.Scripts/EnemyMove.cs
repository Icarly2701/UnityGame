using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();  
        Invoke("Think", 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if(raycast.collider == null)
        {
            Turn();
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);
        
        anim.SetInteger("WalkSpeed", nextMove);

        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == -1;
        
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = (nextMove == -1);

        CancelInvoke();
        Invoke("Think", 2);
    }

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        Invoke("DeActive", 5);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
