using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove player;
    Rigidbody2D rigid;
    SpriteRenderer render;
    Animator anim;
    CapsuleCollider2D capsule;
    AudioSource audioSource;

    float jumpForce = 12.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 4.0f;
    
    public GameManager manager;
    public AudioClip audioJump;
    public AudioClip audioBanana;
    public AudioClip audioStraw;
    public AudioClip audioHit;
    public AudioClip audioDie;
    public AudioClip audioAttack;

    bool isFire = true;
    public bool Left;
    public bool Right;
    public bool Up;
    public float h;
    void Start()
    {
        this.rigid = GetComponent<Rigidbody2D>();
        this.render = GetComponent<SpriteRenderer>();
        this.anim = GetComponent<Animator>();
        this.capsule = GetComponent<CapsuleCollider2D>();
        this.audioSource = GetComponent<AudioSource>();

    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (player == null)
            player = this;
    }

    private void FixedUpdate()
    {
        if (Left)
        {
            h = -1;

            render.flipX = h == 1;
        }

        if (Right)
        {
            h = 1;

            render.flipX = h == 1;
        }


        if (!Left && !Right) h = 0;

        //점프 애니메이션 끄고 다시 바꾸기
        Vector3 front = new Vector3(rigid.transform.position.x + 0.1f, rigid.transform.position.y, -1);
        Vector3 back = new Vector3(rigid.transform.position.x - 0.1f, rigid.transform.position.y, -1);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHitfront = Physics2D.Raycast(front, Vector3.down, 1, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHitback = Physics2D.Raycast(back, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rigid.velocity.y < 0)
        {
            if (rayHit.collider != null || rayHitfront.collider != null || rayHitback.collider != null)
            {
                if (rayHit.distance < 0.75f || rayHitfront.distance < 0.75f || rayHitback.distance < 0.75f)
                {
                    anim.SetBool("isJump", false);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h , ForceMode2D.Impulse);

        //점프한다.
        if ((Input.GetButtonDown("Jump")||Up) && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            PlaySound("JUMP");
            Up = false;
        }

        if (rigid.velocity.x > maxWalkSpeed || Left)
        {
            rigid.velocity = new Vector2(maxWalkSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxWalkSpeed * (-1) || Right)
        {
            rigid.velocity = new Vector2(maxWalkSpeed * (-1), rigid.velocity.y);
        }

        if (rigid.velocity.y > walkForce)
            rigid.velocity = new Vector2(rigid.velocity.x, walkForce);


        if (Input.GetButton("Horizontal"))
        {
            render.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //걷기 애니메이션바꾸고 다시 바꾸기
        if (rigid.velocity.normalized.x == 0)
        {
            anim.SetBool("IsWalk", false);
        }
        else
        {
            anim.SetBool("IsWalk", true);
        }

        

        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home))
            {
                Application.Quit();
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("Opening");
                manager.DieManager();
                Destroy(gameObject);
            }
        }
    }

    //collision에 부딪쳤을때 나오는 메소드
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y
                && collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
            {
                OnAttack(collision.transform);
            }
            else
            {
                if(!gameObject.layer.Equals(LayerMask.NameToLayer("PlayerDamaged")))
                    OnDamaged(collision.transform.position);
            }
        }else if(collision.gameObject.tag == "Jump")
        {
            if(rigid.velocity.y<0 && transform.position.y > collision.transform.position.y)
            {
                PlaySound("JUMP");
                anim.SetBool("isJump", true);
                rigid.AddForce(Vector2.up * (jumpForce * 1.8f), ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //부딪히면 바나나가 사라진다
        if (collision.gameObject.tag == "Banana")
        {
            manager.stagePoint += 100;

            if(manager.stagePoint % 1000 == 0)
            {
                manager.HealthUp();
            }

            collision.gameObject.SetActive(false);
            PlaySound("BANANA");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            manager.stageIndex++;
            int a = SceneManager.GetActiveScene().buildIndex+1;
            SceneManager.LoadScene(a);
            transform.position = new Vector3(-8.5f, -2.5f, 0);
            Left = false;
            Right = false;
            Up = false;
        }
        else if (collision.gameObject.tag == "Strawberry")
        {
            manager.starPoint += 1;
            collision.gameObject.SetActive(false);
            PlaySound("STRAW");
        }
        else if(collision.gameObject.tag == "kiwi")
        {
            isFire = false;
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "fire" && isFire)
        {
            transform.position = new Vector3(-8.5f, -2.5f, 0);
            manager.HealthDown();
        }
        else if(collision.gameObject.tag == "Trophy")
        {
            if(manager.starPoint == 6) 
            {
                SceneManager.LoadScene("HiddenEnding");
            }
            else
            {
                SceneManager.LoadScene("Ending");
            }
            manager.DieManager();
            Destroy(gameObject);
        }
    }

    void OnAttack(Transform enemy)
    {
        PlaySound("ATTACK");
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        manager.stagePoint += 100;
        if (manager.stagePoint % 1000 == 0)
        {
            manager.HealthUp();
        }
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 target)
    {
        PlaySound("HIT");

        manager.HealthDown();

        gameObject.layer = 9;
        Debug.Log(gameObject.layer);

        render.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x - target.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 6, ForceMode2D.Impulse);

        Invoke("OffDamaged", 2f);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        render.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        SceneManager.LoadScene("Die");
        manager.DieManager();
        Destroy(gameObject);
    }

    void PlaySound(string action)
    {
        switch(action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "BANANA":
                audioSource.clip = audioBanana;
                break;
            case "STRAW":
                audioSource.clip = audioStraw;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "HIT":
                audioSource.clip = audioDie;
                break;
        }
        audioSource.Play();
    }

    public void leftdown()
    {   
        Right = true;
    }

    public void leftup()
    {
        Right = false;
    }

    public void rightdown()
    {
       Left = true;
    }

    public void rightup()
    {
        Left = false;
    }

    public void jumpdown()
    {
       if(!anim.GetBool("isJump"))
        Up = true;
    }
}
