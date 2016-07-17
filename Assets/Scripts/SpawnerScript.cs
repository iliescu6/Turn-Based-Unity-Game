using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public static SpawnerScript instance;

    public GameObject[] enemyTypes;
    public GameObject[] spawner = new GameObject[5];
    public bool[] spawnerCol = new bool[5];
    public bool spawnedEnemy = false;

    //Assign spawners
    void Awake() {
        instance = this;
        for (int i = 0; i <= 4; i++) {
            spawner[i] = GameObject.Find("Enemy Spawner ("+i.ToString()+")");
            spawnerCol[i] = false;
        }
    }
    //Pick a random spawner and check if the player is colliding with it
    //If the player is colliding with it pick a new spawner
    //This is done in case when the player finishes a battle he collides with another enemy 
    //When he's sent back to the "world map"
    void Update() {
        if (!spawnedEnemy) {
            int n=Random.Range(0,5);
            if (!spawnerCol[n])
            {
                int j = Random.Range(0, 2);
                Instantiate(enemyTypes[j], spawner[n].transform.position, Quaternion.identity);
                spawnedEnemy = true;
            }
            else {
                n = Random.Range(0, 4);
            }
        }
    }
    }
