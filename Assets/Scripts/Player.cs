using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public static Player instance;
    public CharacterInfo player;

    private int speed = 5;

    private Vector3 posBeforeBattle,posForBattle;

    public bool loaded,battle;
    public string hitedEnemy;
    public int gold;


    void Awake() {
        player = GameObject.Find("Player").GetComponent<CharacterInfo>();
        MakeSingleton();
        battle = true;
        gold = 50;

        //Debug.Log(player.agility.ToString());
        //transform.position = posBeforeBattle;
    }
	// Use this for initialization
	void Start () {
        //Create a lvl 1 player wi x health, x mana, x max xp and agility
        loaded = false;
        battle = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (player.level >= 3 && GameObject.Find("Door") != null) {
            Destroy(GameObject.Find("Door"));
        }
        if (player.currentXP >= player.currentMaxXP) {
            int temp = player.currentXP - player.currentMaxXP;
            player.level += 1;
            player.maxHP += 2;
            player.maxMana += 2;
            player.strength += 1;
            player.intelligence += 1;
            player.agility += 1;
            player.dmg += 2;
            player.currentXP = temp;
            player.currentHP = player.maxHP;
            player.currentMana = player.maxMana;
        }
            if (SceneManager.GetActiveScene().name == "Battle")
            {
                if (battle)
                    transform.position = new Vector3(0.0f, -0.34f, 3.58f);
                battle = false;
            }else
         if (SceneManager.GetActiveScene().name == "Level1") {
            //Movement
            if (loaded) {
                transform.position = posBeforeBattle;
               loaded = false;
            }
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.localEulerAngles = new Vector3(0, 0, 0);
            if (transform.position.y != -3.43f)
            {
                transform.position = new Vector3(transform.position.x, -3.43f, transform.position.z);
            }


            if (Input.GetKey("w"))
            {
                transform.Translate(Vector3.right*speed * Time.deltaTime);
            }
            if (Input.GetKey("d"))
            {
                transform.Translate(Vector3.forward * -speed * Time.deltaTime);
            }
            if (Input.GetKey("s"))
            {
                transform.Translate(Vector3.right * -speed * Time.deltaTime);
            }
            if (Input.GetKey("a"))
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
        
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Enemy" && SceneManager.GetActiveScene().name!="Battle") {
            posBeforeBattle = gameObject.transform.position;
            hitedEnemy = col.name;
            SceneManager.LoadScene("Battle");
            battle = true;
        }
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
