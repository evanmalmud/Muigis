using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBox : MonoBehaviour {

    private GhostAI parentAI;

    void Start()
    {
        parentAI = transform.parent.GetComponent<GhostAI>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        print("GhostBox OnTriggerEnter2D: " + collider);
        if (collider.gameObject.tag == "Flashlight")
        {
            parentAI.HitByLight();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        print("GhostBox OnTriggerStay2D: " + collider);
        if (collider.gameObject.tag == "Flashlight")
        {
            parentAI.HitByLight();
        }
    }
}
