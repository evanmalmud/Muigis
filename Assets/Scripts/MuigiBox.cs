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
        //print("test" + collider.gameObject);
        if (collider.gameObject.tag == "Ghost")
        {
            parentClass.Hurt(collider);
        }
    }
}
