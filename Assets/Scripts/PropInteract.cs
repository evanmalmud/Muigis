using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropInteract : MonoBehaviour {

    public bool shake;
    public float shakeCountDefault = 1f;
    public float shakeCount = 0f;

    private AudioSource audioData;

    public float maxAngle = 3f;
    private float maxRads;
    public float rotSpeed = 1f;

    public bool goingRight = false;
    public bool recenter = false;

    // Use this for initialization
    void Start () {
        audioData = GetComponent<AudioSource>();
        maxRads = maxAngle * Mathf.Deg2Rad;
    }
	
	// Update is called once per frame
	void Update () {
        if(shake){
            //move object
            //play sound
            shakeRot();
            recenter = false;
            if (!audioData.isPlaying)
                audioData.Play();
        }
        else
        {
            recenter = true;
            shakeRot();
        }
        ShakeCountdown();
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        //print("OnTriggerEnter2D" + collider.gameObject);
        if (collider.gameObject.tag == "Vac")
        {
            shake = true;
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        //print("OnTriggerStay2D" + collider.gameObject);
        if (collider.gameObject.tag == "Vac")
        {
            shake = true;
        }
    }

    void shakeRot()
    {
        Vector3 rot = Vector3.zero;
        if(recenter)
        {
            if (transform.rotation.z > rotSpeed)
            {
                rot.z = -rotSpeed;
            }
            else if (transform.rotation.z < -rotSpeed)
            {
                rot.z = rotSpeed;
            } 
            else
            {
                rot.z = -transform.rotation.z;
            }

        }
        else if (goingRight)
        {
            if (transform.rotation.z > maxRads)
            {
                goingRight = false;
                rot.z = -rotSpeed;
            }
            else{
                rot.z = rotSpeed;
            }
        }
        else if (!goingRight)
        {
            print("here1" + transform.rotation.z);
            if (transform.rotation.z < -maxRads)
            {
                print("here2");
                goingRight = true;
                rot.z = rotSpeed;
            }
            else
            {
                print("here3");
                rot.z = - rotSpeed;
            }
        }

        transform.Rotate(rot);
    }

    void ShakeCountdown()
    {
        if (shake)
        {
            if (shakeCount > 0)
            {
                shakeCount -= Time.deltaTime;
            }
            else
            {
                shakeCount = shakeCountDefault;
                shake = false;
            }
        }
    }
}
