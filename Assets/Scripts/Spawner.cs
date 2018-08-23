using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject muigi;
    public GameObject muigiStartPoint;

    public GameObject[] ghosts;
    public GameObject boo;

    public Transform[] spawnPoints;

    public int maxGhosts = 10;

    public float spawnTime = 3f;
    public bool spawnActive = false;

    public List<GameObject> listGhosts;

    public float timePlaying = 0f;
    public bool startCounting = false;

    public AudioClip[] ghostlaughclips;
    private AudioSource audioData;

    // Use this for initialization
    void Start () {
        audioData = GetComponent<AudioSource>();
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}

    void Update()
    {
        if (startCounting)
        {
            timePlaying += Time.deltaTime;
            if (!audioData.isPlaying)
            {
                float val = Random.Range(0, 1000);
                //print(val);
                if (val > 998f)
                {
                    int audoClipint = UnityEngine.Random.Range(0, ghostlaughclips.Length);
                    audioData.clip = ghostlaughclips[audoClipint];
                    audioData.Play();
                }
            }
        }
    }

    // Update is called once per frame
    void Spawn () {
        if(spawnActive)
        {
            float val = Random.value;
            int enemyNum = 0;
            //if (timePlaying > 0f){
                if(val > .5) //50% chance
                {
                    enemyNum = 0;
                }
                else if ( val > .2)
                {
                    enemyNum = 1;
                }
                else if ( val > .1)
                {
                    enemyNum = 2;
                }
                else {
                    enemyNum = 3;
                }
            //}
            int spawnPointNum = Random.Range(0, spawnPoints.Length);
            GameObject temp = Instantiate(ghosts[enemyNum], spawnPoints[spawnPointNum].transform);
            listGhosts.Add(temp);
        }
	}

    public void deleteAll()
    {
        foreach(GameObject ghost in listGhosts)
        {
            Destroy(ghost);
        }
        listGhosts.Clear();
        spawnActive = false;
        startCounting = false;
    }

    public void setSpawn(bool state)
    {
        spawnActive = state;
        startCounting = state;
    }
}
