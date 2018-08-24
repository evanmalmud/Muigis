using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsIneract : MonoBehaviour {

    float startingY = 0f;
    public float offset = 1.5f;
    //float fireAngle = 0f;

    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        startingY = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        float randomX = Random.Range(-15f, 15f);
        rb.AddForce(new Vector2(randomX, 18f),ForceMode2D.Force);
        offset += Random.Range(0f, .5f);
    }

    // Update is called once per frame
    void Update () 
    {
        if (transform.position.y <= startingY - offset)
        {
            //disable rigidbody
            //stop movement
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
	}
}
