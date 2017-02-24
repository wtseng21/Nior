using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float    jumpForce;
    public bool     hasKey;
    public int      lives;
    public Sprite   openGate;
    public Sprite   closeGate;
    public LayerMask whatIsGround;
    public Transform[] groundPoints;
    public float groundRadius;
    public bool isAttack;


    private Rigidbody2D rb2d;
    private Animator animate;

    private bool faceRight;
    private bool isGround;
    private bool jump;
    private int loadedLvl;
    private string curScene;

  
    
    void Start ()
    {
        faceRight = true;
        rb2d      = GetComponent<Rigidbody2D>();
        animate   = GetComponent<Animator>();
        hasKey    = false;
        lives     = 3;
        loadedLvl = SceneManager.GetActiveScene().buildIndex;
        curScene  = SceneManager.GetActiveScene().name;
	}


    void Update()
    {
        Inputs();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(curScene);
            
        if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene("Start Menu");

        if (Input.GetKeyDown(KeyCode.Z) && 0 < loadedLvl && loadedLvl < 3)
            SceneManager.LoadScene(loadedLvl + 1);

        if (lives == 0)
            StartCoroutine(playerDeath());
    }
		

	void FixedUpdate ()
    {
        Movement();

        float move = Input.GetAxis("Horizontal");
        animate.SetFloat("Speed", Mathf.Abs(move));

        isGround = isGrounded();

        if (!this.animate.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            Flip(move);

        Attack();
        ResetValues();
	}


    void OnTriggerEnter2D(Collider2D collision)
    {
        var collide = collision.gameObject;

        if (collision.gameObject.CompareTag("Key"))
        {
            collision.gameObject.SetActive(false);
            hasKey = true;
        }

        if (collision.gameObject.CompareTag("AdvanceLvl"))
            SceneManager.LoadScene(loadedLvl + 1);

        if (collision.gameObject.CompareTag("Fall Death"))
            SceneManager.LoadScene("Game Over");

        if (collision.gameObject.CompareTag("Health Kit") && lives < 3)
        {
            lives += 1;
            collision.gameObject.SetActive(false);
        }

        if (collide.gameObject.CompareTag("Start Sign"))
            SceneManager.LoadScene("Scene1");

        if (collide.gameObject.CompareTag("Quit Sign"))
            Application.Quit();

        if (collide.gameObject.CompareTag("Menu Sign"))
            SceneManager.LoadScene("Start Menu");

        if (collide.gameObject.CompareTag("Restart Sign"))
            SceneManager.LoadScene("Scene1");

        if (collide.gameObject.CompareTag("Flag"))
            SceneManager.LoadScene("Congrats");

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var collide = collision.gameObject;

        if (collide.CompareTag("Gate") && hasKey)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = openGate;
            collision.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0, 0);
            hasKey = false;
        }

        if (collide.gameObject.CompareTag("Enemy") && !animate.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            var coroutine = DamageRecover();
            StartCoroutine(coroutine);
        }
    }


    void Movement()
    {
        if (!this.animate.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector2.right * 5f * Time.deltaTime);

            if (Input.GetKey(KeyCode.A))
                transform.Translate(-Vector2.right * 5f * Time.deltaTime);

            if (isGround && jump)
            {
                isGround = false;

                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
                    rb2d.AddForce(new Vector2(0, jumpForce));
            }
        }
    }


    void Flip(float horizontal)
    {
        if (horizontal > 0 && !faceRight|| horizontal < 0 && faceRight)
        {
            faceRight = !faceRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }


    void Attack()
    {
        if (isAttack)
            animate.SetTrigger("Attack");
    }


    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.J))
            isAttack = true;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            jump = true;
    }


    void ResetValues()
    {
        isAttack    = false;
        jump        = false;
    }


    bool isGrounded()
    {
        if (rb2d.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                        return true;
                }
            }
        }
        return false;
    }

    IEnumerator DamageRecover()
    {
        GetComponent<PlayerController>().enabled = false;
        rb2d.AddForce(new Vector2(-250, 0));
        lives -= 1;
        animate.SetInteger("Lives", lives);

        if (lives == 0)
            StartCoroutine(playerDeath());
        else
        {
            yield return new WaitForSeconds(1.2f);
            GetComponent<PlayerController>().enabled = true;
        }
    }

    IEnumerator playerDeath()
    {
        yield return new WaitForSeconds(0.9f);
        animate.speed = 0f;
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Game Over");
    }
}
