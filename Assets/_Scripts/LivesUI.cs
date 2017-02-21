using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour {
    public GameObject player;
    public Sprite threelife;
    public Sprite twolife;
    public Sprite onelife;
    public Sprite nolife;

	// Use this for initialization
	void Start () {
            GetComponent<SpriteRenderer>().sprite = threelife;
	}
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<PlayerController>().lives == 3)
            GetComponent<SpriteRenderer>().sprite = threelife;
        if (player.GetComponent<PlayerController>().lives == 2)
            GetComponent<SpriteRenderer>().sprite = twolife;
        if (player.GetComponent<PlayerController>().lives == 1)
            GetComponent<SpriteRenderer>().sprite = onelife;
        if (player.GetComponent<PlayerController>().lives == 0)
            GetComponent<SpriteRenderer>().sprite = nolife;
    }
}
