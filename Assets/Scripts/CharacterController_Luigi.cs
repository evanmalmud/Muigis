using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_Luigi : MonoBehaviour {

    public float health = 100;

    //public float maxSpeed = 10f;
    bool facingRight = true;
    bool facingUp = true;
    bool scream = false;

    bool flashlight = false;
    public GameObject Flashlight;

    bool vac = false;
    public GameObject Vac;

    public float speed = 30;

    public int ghostsCaptured = 0;

    private Rigidbody2D rb;

    private Animator anim;



	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        SetFlashlight(false);
        SetVac(false);
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        CheckDirection();
        CheckFlashlight();
        CheckVac();
    }

    void FlipHorizontal()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void FlipVertical()
    {
        facingUp = !facingUp;
        //print("FlipVertical facingUp: " + facingUp);
    }

    void Move()
    {
        Vector2 AxisInput = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        rb.AddForce(AxisInput * speed, ForceMode2D.Force);
    }

    void CheckDirection()
    {
        Vector2 AxisInput = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        if (AxisInput.x > 0 && !facingRight)
            FlipHorizontal();
        else if (AxisInput.x < 0 && facingRight)
            FlipHorizontal();

        if (AxisInput.y > 0 && !facingUp)
            FlipVertical();
        else if (AxisInput.y < 0 && facingUp)
            FlipVertical();

        // Animator Setup
        if ((Input.GetAxis("Horizontal") > Input.GetAxis("Vertical"))) //Moving faster in X axis
        {
            if (facingRight)
            {
                anim.SetBool("WalkDown", false);
                anim.SetBool("WalkRight", true);
                anim.SetBool("WalkLeft", false);
                anim.SetBool("WalkUp", false);
            }
            else{
                anim.SetBool("WalkDown", false);
                anim.SetBool("WalkRight", false);
                anim.SetBool("WalkLeft", true);
                anim.SetBool("WalkUp", false);
            }
        }
        else //Moving faster in Y axis
        {
            if (facingUp)
            {
                anim.SetBool("WalkDown", false);
                anim.SetBool("WalkRight", false);
                anim.SetBool("WalkLeft", false);
                anim.SetBool("WalkUp", true);
            }
            else
            {
                anim.SetBool("WalkDown", true);
                anim.SetBool("WalkRight", false);
                anim.SetBool("WalkLeft", false);
                anim.SetBool("WalkUp", false);
            }
        }
    }

    void CheckFlashlight()
    {
        if(Input.GetKeyDown("f"))
        {
            flashlight = !flashlight;
            Flashlight.SetActive(flashlight);
        }
    }

    void CheckVac()
    {
        if (Input.GetKeyDown("v"))
        {
            vac = !vac;
            Vac.SetActive(vac);
        }
    }

    void SetFlashlight(bool setVal)
    {
        flashlight = setVal;
        Flashlight.SetActive(flashlight);
    }

    void SetVac(bool setVal)
    {
        vac = setVal;
        Vac.SetActive(vac);
    }

    public void GhostCaptured()
    {
        ghostsCaptured++;
    }
}
