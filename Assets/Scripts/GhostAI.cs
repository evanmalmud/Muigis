using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour {

    public GameObject ChaseRange;
    public GameObject Box;

    public GameObject muigi;
    public bool playerInRange = false;
    public bool stunned = false;
    public float stunCountDefault = 1f;
    float stunCountdown;
    public float speed = 3f;
    public float health = 100f;
    public float drainRate = 8000f;
    public float attackRange = 1.5f;
    public int attackDamage = 2;

    public bool beingSucked = false;
    private float suckCountDefault = 1f;
    float suckCountdown;

    private float scaleScaling = 0.0f;
    private float smoothDampTime = 1.5f;
    private float countIfDeathOver = 0f;
    public bool dead = false;
    private float zRot = 0;

    // Use this for initialization
    void Start () {
        stunCountdown = stunCountDefault;
        suckCountdown = suckCountDefault;
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
		if (playerInRange && !stunned && health > 0) {
            MoveToPlayer();
        }

        if(stunned) {
            StunnedCountdown();
        }

        if(beingSucked) {
            health -= Time.deltaTime * drainRate;
            //print(health);
            SuckedCountdown();
        }

        if(health < 0){
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

    public void MoveToPlayer ()
    {
        Vector2 muigiPos = new Vector2(muigi.transform.position.x, muigi.transform.position.y);
        Vector2 objPos = new Vector2(transform.position.x, transform.position.y);
        float journeyLength = Vector2.Distance(objPos, muigiPos);
        float distanceFrac = speed * Time.deltaTime / journeyLength;
        if (journeyLength > attackRange) {
            transform.position = Vector2.Lerp(objPos, muigiPos, distanceFrac);
        }
        else
        {
         //ATTACK!
        }
    }

    void StunnedCountdown()
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

    void SuckedCountdown()
    {
        if (suckCountdown > 0)
        {
            suckCountdown -= Time.deltaTime;
        }
        else
        {
            suckCountdown = suckCountDefault;
            beingSucked = false;
        }
    }

    public void HitByLight()
    {
        stunned = true;
    }

    public void HitByVac()
    {
        beingSucked = true;
    }

    public void setPlayerInRange(bool range)
    {
        playerInRange = range;
    }
}
