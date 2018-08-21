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

    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Spawn () {
        if(spawnActive)
        {
            int spawnPointNum = Random.Range(0, spawnPoints.Length);
            int enemyNum = Random.Range(0, ghosts.Length);
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
    }

    public void setSpawn(bool state)
    {
        spawnActive = state;
    }
}
