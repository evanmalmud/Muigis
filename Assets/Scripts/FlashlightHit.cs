using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ghost")
        {
            GhostAI temp = collider.gameObject.GetComponent<GhostAI>();
            temp.HitByLight();
        }

    }

    void OnTriggerStay2D(Collider2D collider)
    {
        print("test" + collider);
        if (collider.gameObject.tag == "Ghost")
        {
            GhostAI temp = collider.gameObject.GetComponent<GhostAI>();
            temp.HitByLight();
        }

    }
}
