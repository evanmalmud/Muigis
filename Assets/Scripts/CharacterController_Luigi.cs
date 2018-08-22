using System;
using TMPro;
using UnityEngine;

public class CharacterController_Luigi : MonoBehaviour {

    public GameObject MenuManager;
    public ScreenManager screenManager;

    public int health = 3;
    public GameObject[] healthIcons;
    public bool dead = false;

    public GameObject ghostCount;
    private TextMeshProUGUI ghostCountText;
    public int ghostsCaptured = 0;

    //public GameObject cashCount;
    //private TextMeshProUGUI cashCountText;
    //public int cashCollected = 0;

    //public float maxSpeed = 10f;
    bool facingRight = true;
    bool facingUp = true;
    //bool scream = false;

    bool flashlight = false;
    public GameObject Flashlight;

    bool vac = false;
    public GameObject Vac;

    public float speed = 30;
    public float knockbackSpeed = 50;

    private Rigidbody2D rb;

    private Animator anim;

    public bool controlsEnabled = false;

    public float invulnerableTime = 2f;
    private float invulnerableCount = 0f;
    public bool invulnerable = false;



	// Use this for initialization
	void Start () {
        MenuManager = GameObject.Find("Manager");
        screenManager = MenuManager.GetComponent<ScreenManager>();
        ghostCount = GameObject.Find("GhostText");
        ghostCountText = ghostCount.GetComponent<TextMeshProUGUI>();
        //cashCount = GameObject.Find("CashText");
        //cashCountText = cashCount.GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        SetFlashlight(false);
        SetVac(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(controlsEnabled)
        {
            Move();
            CheckDirection();
            CheckFlashlight();
            CheckVac();
            //print("flash: " + flashlight + " vac: " + vac);
            InvulnerableCheck();
        }
    }

    private void InvulnerableCheck()
    {
        if (invulnerableCount >= invulnerableTime)
        {
            invulnerable = false;
            invulnerableCount = 0f;
        }
        else
        {
            invulnerableCount += Time.deltaTime;
        }
    }

    public void resetCharacter()
    {
        health = 3;
        UpdateHealth();
        ghostsCaptured = 0;
        //UpdateCashCount();
        //cashCollected = 0;
        UpdateGhostCount();
        SetFlashlight(false);
        SetVac(false);
        invulnerable = false;
        invulnerableCount = 0f;
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
        float xVel = Mathf.Abs(rb.velocity.x);
        float yVel = Mathf.Abs(rb.velocity.y);
        //print(xVel + yVel);
        if (xVel < 0.5f && yVel < 0.5f)
        {
            anim.SetBool("WalkDown", false);
            anim.SetBool("WalkRight", false);
            anim.SetBool("WalkLeft", false);
            anim.SetBool("WalkUp", false);
            anim.SetBool("Idle", true);
        }
        else if (xVel > yVel) //Moving faster in X axis
        {
            if (facingRight)
            {
                anim.SetBool("WalkDown", false);
                anim.SetBool("WalkRight", true);
                anim.SetBool("WalkLeft", false);
                anim.SetBool("WalkUp", false);
                anim.SetBool("Idle", false);
            }
            else{
                anim.SetBool("WalkDown", false);
                anim.SetBool("WalkRight", false);
                anim.SetBool("WalkLeft", true);
                anim.SetBool("WalkUp", false);
                anim.SetBool("Idle", false);
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
                anim.SetBool("Idle", false);
            }
            else
            {
                anim.SetBool("WalkDown", true);
                anim.SetBool("WalkRight", false);
                anim.SetBool("WalkLeft", false);
                anim.SetBool("WalkUp", false);
                anim.SetBool("Idle", false);
            }
        }
    }

    void CheckFlashlight()
    {
        if(Input.GetKeyDown("f"))
        {
            ToggleFlashlight();
        }
        if(flashlight && vac)
        {
            ToggleVac();
        }
    }

    void ToggleFlashlight()
    {
        flashlight = !flashlight;
        Flashlight.SetActive(flashlight);
    }

    void CheckVac()
    {
        if (Input.GetKeyDown("v"))
        {
            ToggleVac();
        }
        if (flashlight && vac)
        {
            ToggleFlashlight();
        }
    }

    void ToggleVac()
    {
        vac = !vac;
        Vac.SetActive(vac);
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
        UpdateGhostCount();
    }

    public void Hurt(Collider2D collider)
    {   
        if(!invulnerable)
        {
            health--;
            Vector2 knockback = new Vector2(transform.position.x - collider.gameObject.transform.position.x,
                                            transform.position.y - collider.gameObject.transform.position.y);
            knockback.Normalize();
            //print(knockback);
            rb.AddForce(knockback * speed * knockbackSpeed, ForceMode2D.Force);
            UpdateHealth();
            CheckDead();
            invulnerable = true;
        }
    }
    
    public void UpdateHealth()
    {
        int healthcount = 0;
        foreach (GameObject healthicon in healthIcons)
        {
            if (healthcount < health){
                healthicon.SetActive(true);
            } 
            else
            {
                healthicon.SetActive(false);
            }
            healthcount++;
        }
    }

    public void UpdateGhostCount()
    {
        ghostCountText.text = ghostsCaptured.ToString();
    }

    //public void UpdateCashCount()
    //{
    //   cashCountText.text = cashCollected.ToString();
    //}

    public void CheckDead()
    {
        if(health <= 0 )
        {
            dead = true;
            screenManager.gameOver();
        }
    }
}
