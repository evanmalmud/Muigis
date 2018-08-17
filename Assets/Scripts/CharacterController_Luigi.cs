using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_Luigi : MonoBehaviour {

    public float health = 100;

    public float maxSpeed = 10f;
    bool facingRight = true;
    bool facingUp = true;

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

        //print(AxisInput);

        rb.AddForce(AxisInput * acc, ForceMode2D.Force); // For ForceMode see 2)

        if (AxisInput.x > 0 && !facingRight)
            FlipHorizontal();
        else if (AxisInput.x < 0 && facingRight)
            FlipHorizontal();

        if (AxisInput.y > 0 && !facingUp)
            FlipVertical();
        else if (AxisInput.y < 0 && facingUp)
            FlipVertical();
            
    }

    void FlipHorizontal()
    {
        facingRight = !facingRight;
        //print("FlipHorizontal facingRight: " + facingRight);
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void FlipVertical()
    {
        facingUp = !facingUp;
        //print("FlipVertical facingUp: " + facingUp);
    }

    void CheckforLightHits()
    {
        
    }

}
