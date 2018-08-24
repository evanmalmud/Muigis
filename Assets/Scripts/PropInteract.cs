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

    public GameObject[] drops;
    public List<GameObject> dropList;
    public int maxDrops = 20;
    public float timeBetweenDrops = .5f;
    float dropCountdown = 0f;
    bool canDrop = true;

    public GameObject coinholder;

    // Use this for initialization
    void Start () {
        coinholder = GameObject.Find("CoinHolder");
        audioData = GetComponent<AudioSource>();
        maxRads = maxAngle * Mathf.Deg2Rad;
        dropCountdown = 0f;
    }

    public void Reset()
    {
        foreach(GameObject drop in dropList)
        {
            if (drop)
                Destroy(drop);
        }
        dropList.Clear();
        dropCountdown = 0f;
        canDrop = true;
        shakeCount = shakeCountDefault;
        shake = false;
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
            shakeDrops();
        }
        else
        {
            recenter = true;
            shakeRot();
        }
        ShakeCountdown();
        DropCountdown();
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
            //print("here1" + transform.rotation.z);
            if (transform.rotation.z < -maxRads)
            {
                //print("here2");
                goingRight = true;
                rot.z = rotSpeed;
            }
            else
            {
                //print("here3");
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
    void DropCountdown()
    {
        if (!canDrop)
        {
            if (dropCountdown < timeBetweenDrops)
            {
                dropCountdown += Time.deltaTime;
            }
            else
            {
                dropCountdown = 0f;
                canDrop = true;
            }
        }
    }

    void shakeDrops()
    {
        if(dropList.Count < maxDrops && canDrop){
            int index = Random.Range(0, drops.Length);
            GameObject temp = Instantiate(drops[index], transform);
            temp.transform.SetParent(coinholder.transform);
            //Rigidbody2D tempRb = temp.GetComponent<Rigidbody2D>();
            //float randomX = Random.Range(-1f, 1f);
            dropList.Add(temp);

            //tempRb.AddForce(new Vector2(randomX, 1f) * 2);
            canDrop = false;
        }
    }
}
