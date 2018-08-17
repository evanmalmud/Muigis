using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCircle : MonoBehaviour {

    public int collCount = 0;
    private GhostAI parentAI;

    void Start ()
    {
        parentAI = transform.parent.GetComponent<GhostAI>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        print("GhostCircle OnTriggerEnter2D: " + collider);
        if (collider.gameObject.tag == "Muigi")
        {
            collCount++;
            parentAI.setPlayerInRange(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        print("GhostCircle OnTriggerExit2D: " + collider);
        if (collider.gameObject.tag == "Muigi")
        {
            collCount--;
            if (collCount < 1)
            {
                parentAI.setPlayerInRange(false);
                collCount = 0;
            }
        }
    }
}
