using UnityEngine;

public class GhostAI : MonoBehaviour {

    public GameObject ChaseRange;
    public GameObject Box;

    public GameObject RedHeart;
    public GameObject BlueHeart;

    public GameObject muigi;
    public bool playerInRange = false;
    public bool stunned = false;
    public float stunCountDefault = 1f;
    float stunCountdown;
    public float speed = 3f;
    public float runAwaySpeed = 6f;
    public float health = 100f;
    public float drainRate = 8000f;
    public float attackRange = 1.5f;
    public int attackDamage = 2;

    public bool beingSucked = false;
    private float suckCountDefault = 1f;
    //float suckCountdown;

    private float scaleScaling = 0.0f;
    private float smoothDampTime = 1.5f;
    private float countIfDeathOver = 0f;
    public bool dead = false;
    private float zRot = 0;

    bool facingRight = true;
    //Rigidbody2D rb;

    bool movingRandomly = false;
    float movingRandomlyTime = 2f;
    float movingRandomlyCount = 0f;
    Vector2 travelPos = new Vector2();

    bool hitByLight = false;
    bool hitByVac = false;
    bool canBeHitByFlash = true;
    public float flashLightCooldownDefault = 2f;
    private float flashLightCooldownCount = 0f;

    public float runTimeDefault = 1f;
    public float runTimeCount = 0f;
    public bool running = false;

    private AudioSource audioData;

    // Use this for initialization
    void Start() {
        audioData = GetComponent<AudioSource>();
        muigi = GameObject.Find("Muigi");
        //rb = GetComponent<Rigidbody2D>();
        stunCountdown = stunCountDefault;
        //suckCountdown = suckCountDefault;
        flashLightCooldownCount = flashLightCooldownDefault;
        canBeHitByFlash = true;
        stunned = false;
        beingSucked = false;
        hitByLight = false;
        hitByVac = false;
        showBlueHeart(false);
        showRedHeart(false);
        runTimeCount = 0f;
        running = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hitByLight) {
            //check for light cooldown
            if (canBeHitByFlash)
            {
                audioData.Play();
                stunned = true;
                canBeHitByFlash = false;
            }
            hitByLight = false;
        }
        FlashlightCooldown();
        if (hitByVac)
        {
            //
            if (stunned || beingSucked) {
                beingSucked = true;
                stunCountdown = stunCountDefault;
            }
            else {
                beingSucked = false;
            }
            stunned = false;
            hitByVac = false;
        }
        else
        {
            if (beingSucked)
            {
                beingSucked = false;
                running = true;
                runTimeCount = runTimeDefault;
            }
        }

        if (playerInRange && !stunned && health > 0 && !beingSucked && !running) 
        {
            MoveToPlayer();
        } 
        else if (!stunned && health > 0 && !beingSucked && !running) 
        {
            MoveRandom();
        } 
        else if (running) 
        {
            RunFromPlayer();
        }

        RunningCountdown();
        StunnedCountdown();
        if (stunned) {
            showBlueHeart(true);
            showRedHeart(false);
        } else {
            showBlueHeart(false);
            showRedHeart(false);
        }

        if (beingSucked)
        {
            health -= Time.deltaTime * drainRate;
            showBlueHeart(false);
            showRedHeart(true);
            updateRedHeart();
            RunFromPlayer();
        }

        if (health < 0){
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            dead = true;
            // kill colliders
            ChaseRange.SetActive(false);
            Box.SetActive(false);
                // pull towards muigi
            //float newXpos = Mathf.SmoothDamp(transform.position.x, muigi.transform.position.x, ref xscaling, smoothDampTime);
            //float newYpos = Mathf.SmoothDamp(transform.position.y, muigi.transform.position.y, ref yscaling, smoothDampTime);
            //transform.position = new Vector3(newXpos, newYpos, transform.position.z);
            Vector2 muigiPos = new Vector2(muigi.transform.position.x, muigi.transform.position.y);
            Vector2 objPos = new Vector2(transform.position.x, transform.position.y);
            float journeyLength = Vector2.Distance(objPos, muigiPos);
            float distanceFrac = speed * Time.deltaTime / journeyLength;
            VerifyMovingDirection(transform.position, Vector2.Lerp(objPos, muigiPos, distanceFrac));
            transform.position = Vector2.Lerp(objPos, muigiPos, distanceFrac);
                // shrink in size
            //float newScale = Mathf.SmoothDamp(transform.localScale.x, 0f, ref scaleScaling, smoothDampTime);
            //transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
            float newScale = Mathf.Lerp(transform.localScale.x, 0f, scaleScaling);
            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
            scaleScaling += Time.deltaTime/10;
            // spin
            zRot += Time.deltaTime * 5f;
            transform.Rotate(new Vector3(0f, 0f, zRot));
            countIfDeathOver += Time.deltaTime;
            // destroy
            if(countIfDeathOver > smoothDampTime){
                muigi.GetComponent<CharacterController_Luigi>().GhostCaptured();
                Destroy(this.gameObject);
            }
        }

    }

    private void VerifyMovingDirection(Vector3 position, Vector2 vector2)
    {
        if (position.x > vector2.x && !facingRight) //Moving Left
        {
            FlipHorizontal();
        }
        else if (position.x < vector2.x && facingRight) //Moving Right
        {
            FlipHorizontal();
        }
    }

    private void showBlueHeart(bool state)
    {
        BlueHeart.SetActive(state);
    }

    private void updateRedHeart()
    {
        RedHeart.transform.localScale = new Vector3(health / 100f, health / 100f, 1f);
    }

    private void showRedHeart(bool state)
    {
        RedHeart.SetActive(state);
    }

    public void MoveToPlayer ()
    {
        Vector2 muigiPos = new Vector2(muigi.transform.position.x, muigi.transform.position.y);
        Vector2 objPos = new Vector2(transform.position.x, transform.position.y);
        float journeyLength = Vector2.Distance(objPos, muigiPos);
        float distanceFrac = speed * Time.deltaTime / journeyLength;
        if (journeyLength > attackRange) {
            VerifyMovingDirection(transform.position, Vector2.Lerp(objPos, muigiPos, distanceFrac));
            transform.position = Vector2.Lerp(objPos, muigiPos, distanceFrac);
        }
        else
        {
         //ATTACK!
        }
        //
    }

    void MoveRandomReset()
    {
        movingRandomly = false;
        movingRandomlyCount = 0f;
    }

        public void MoveRandom()
    {
        if (movingRandomly && movingRandomlyCount <= movingRandomlyTime)
        {
            movingRandomlyCount += Time.deltaTime;
            //Keep Moving to position
            Vector2 objPos = new Vector2(transform.position.x, transform.position.y);
            float journeyLength = Vector2.Distance(objPos, travelPos);
            float distanceFrac = speed * Time.deltaTime / journeyLength;
            VerifyMovingDirection(transform.position, Vector2.Lerp(objPos, travelPos, distanceFrac));
            transform.position = Vector2.Lerp(objPos, travelPos, distanceFrac);
        }
        else
        {
            movingRandomly = true;
            movingRandomlyCount = 0f;
            //Calc new position to move to
            // 0  1  2
            // 3     4
            // 5  6  7
            int randomDir = Random.Range(0, 7);
            Vector3 temp = transform.position;
            if (randomDir <= 2)
            {
                temp.y = 1f;
            }
            else if (randomDir >= 5)
            {
                temp.y = -1f;
            }
            else
            {
                temp.y = 0f;
            }
            if (randomDir == 0 || randomDir == 3 || randomDir == 5)
            {
                temp.x = -1f;
            }
            else if (randomDir == 2 || randomDir == 4 || randomDir == 7)
            {
                temp.x = 1f;
            }
            else
            {
                temp.x = 0f;
            }
            travelPos = new Vector2(temp.x + transform.position.x, temp.y + transform.position.y);
            Vector2 objPos = new Vector2(transform.position.x, transform.position.y);
            float journeyLength = Vector2.Distance(objPos, travelPos);
            float distanceFrac = speed * Time.deltaTime / journeyLength;
            VerifyMovingDirection(transform.position, Vector2.Lerp(objPos, travelPos, distanceFrac));
            transform.position = Vector2.Lerp(objPos, travelPos, distanceFrac);
        }
    }

    public void RunFromPlayer()
    {
        Vector3 runVect = transform.position - muigi.transform.position;
        VerifyMovingDirection(transform.position, transform.position + (runVect.normalized * runAwaySpeed * Time.deltaTime));
        transform.Translate(runVect.normalized * runAwaySpeed * Time.deltaTime);
    }

    void StunnedCountdown()
    {
        if(stunned)
        {
            if (stunCountdown > 0)
            {
                stunCountdown -= Time.deltaTime;
            }
            else
            {
                stunCountdown = stunCountDefault;
                stunned = false;
            }
        }
    }

    void RunningCountdown()
    {  
        if(running)
        {
            if (runTimeCount > 0)
            {
                runTimeCount -= Time.deltaTime;
            }
            else
            {
                runTimeCount = suckCountDefault;
                running = false;
            }
        }
    }

    void FlashlightCooldown()
    {
        if (!canBeHitByFlash)
        {
            if (flashLightCooldownCount > 0)
            {
                flashLightCooldownCount -= Time.deltaTime;
            }
            else
            {
                flashLightCooldownCount = flashLightCooldownDefault;
                canBeHitByFlash = true;
            }
        }
    }

    public void HitByLight()
    {
        hitByLight = true;
    }

    public void HitByVac()
    {
        hitByVac = true;
    }

    public void setPlayerInRange(bool range)
    {
        playerInRange = range;
    }

    void FlipHorizontal()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
