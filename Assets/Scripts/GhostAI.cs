using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour {

    public CircleCollider2D attackSpace;
    public BoxCollider2D hitSpace;

    public GameObject muigi;
    public bool playerInRange = false;
    public bool stunned = false;
    public float stunCountDefault = 1f;
    float stunCountdown;
    public float speed = 3f;
    public float health = 100f;
    public float drainRate = 8f;
    public float attackRange = 1f;
    public int attackDamage = 2;

	// Use this for initialization
	void Start () {
        stunCountdown = stunCountDefault;
        attackSpace = GetComponent<CircleCollider2D>();
        hitSpace = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
		if (playerInRange && !stunned) {
            MoveToPlayer();
        }

        if(stunned) {
            StunnedCountdown();
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
        if(stunCountdown > 0) 
        {
            stunCountdown -= Time.deltaTime;
        }
        else 
        {
            stunCountdown = stunCountDefault;
            stunned = false;
        }
    }

    public void HitByLight()
    {
        stunned = true;
    }

    public void setPlayerInRange(bool range)
    {
        playerInRange = range;
    }
}
