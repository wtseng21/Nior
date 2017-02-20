using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectKeyUI : MonoBehaviour {
    public GameObject Player;
    public Sprite hasKey;
    public Sprite noKey;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().sprite = noKey;
	}
	
	void Update () {
        if (Player.GetComponent<PlayerController>().hasKey)
            gameObject.GetComponent<SpriteRenderer>().sprite = hasKey;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = noKey;
	}
}
