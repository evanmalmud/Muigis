using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuigiBox : MonoBehaviour {

    CharacterController_Luigi parentClass;

    private void Start()
    {
        parentClass = transform.parent.GetComponent<CharacterController_Luigi>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        //print("OnTriggerEnter2D" + collider.gameObject);
        if (collider.gameObject.tag == "Ghost")
        {
            parentClass.Hurt(collider);
        }
    }    
    void OnTriggerStay2D(Collider2D collider)
    {
        //print("OnTriggerStay2D" + collider.gameObject);
        if (collider.gameObject.tag == "Ghost")
        {
            parentClass.Hurt(collider);
        }
    }
}
