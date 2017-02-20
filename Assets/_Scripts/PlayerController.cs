using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float    jumpForce;
    public bool     hasKey;
    public int      lives;


    private Rigidbody2D rb2d;
    private Animator animate;

    private bool faceRight;
    private bool isAttack;
    private bool isGround;
    private bool jump;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    

    void Start ()
    {
        faceRight = true;
        rb2d      = GetComponent<Rigidbody2D>();
        animate   = GetComponent<Animator>();
        hasKey    = false;
        lives     = 3;
	}


    void Update()
    {
        Inputs();

        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (collision.gameObject.CompareTag("Key"))
        {
            collision.gameObject.SetActive(false);
            hasKey = true;
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
        isAttack = false;
        jump = false;
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
}
