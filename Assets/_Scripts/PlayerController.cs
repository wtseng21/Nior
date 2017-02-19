using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    Animator animate;

    // Use this for initialization
    void Start () {

        animate = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Movement();

        {
            float move = Input.GetAxis("Horizontal");
            animate.SetFloat("Speed", move);
        }
	}

    void Movement() {
        if (Input.GetKey (KeyCode.D)) {
            transform.Translate(Vector2.right * 5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-Vector2.right * 5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {

        }
    }
}
