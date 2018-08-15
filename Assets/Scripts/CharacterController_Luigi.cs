using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_Luigi : MonoBehaviour {

    public float health = 100;

    public float acc = 2;

    public float crawlToWalk = 5;
    public float walkToRun = 15;

    private Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 AxisInput = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        print(AxisInput);

        rb.AddForce(AxisInput * acc, ForceMode2D.Impulse); // For ForceMode see 2)
    }
}
