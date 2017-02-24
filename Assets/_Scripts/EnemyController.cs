using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject   player;

    private Animator    animate;
    private bool        faceRight;
    private Rigidbody2D rb2d;
    private int         life;

	// Use this for initialization
	void Start ()
    {
        faceRight = true;
        rb2d = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
        life = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
	}

    void changeDirection()
    {
        faceRight = !faceRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    void Movement()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= 5 && faceRight)
        {
            transform.Translate(-Vector2.right * 2f * Time.deltaTime);
            changeDirection();
        }
        else if (faceRight)
            transform.Translate(Vector2.right * 2f * Time.deltaTime);
        else if (!faceRight)
            transform.Translate(-Vector2.right * 2f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Edge"))
            changeDirection();

        if (collision.gameObject.CompareTag("Fall Death"))
            gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            StartCoroutine(AttackRoutine());

        else if (collision.gameObject.CompareTag("Enemy"))
            changeDirection();
    }

    IEnumerator AttackRoutine()
    {
        GetComponent<EnemyController>().enabled = false;
        rb2d.AddForce(new Vector2(250, 0));
        life -= 1;
        animate.SetInteger("Life", life);

        if (life == 0)
            StartCoroutine(EnemyDeath());
        else
        {
            yield return new WaitForSeconds(0.5f);
            GetComponent<EnemyController>().enabled = true;
        }
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(0.5f);
        animate.speed = 0f;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    
}
