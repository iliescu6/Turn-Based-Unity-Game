using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public CharacterInfo player;
    public int turn;
    // Update is called once per frame

    public GameObject[] enemyType=new GameObject[3];
    public GameObject[] enemySpawners=new GameObject[3];

    private GameObject[] characterArray;

    private bool equalOnce=false,moveF=true,moveB=false,lap=false,writeL=true;

    public List<GameObject> character = new List<GameObject>();
    public List<Vector3> characterPos = new List<Vector3>();
    public int dealDmg;

    public int totalEarnedXp = 0,goldEarned=0;
    void Awake()
    {
        for (int i = 0; i <= 2; i++) {
            enemySpawners[i] = GameObject.Find("Enemy Spawner (" + i.ToString() + ")");
        }
        //If player's level is greater than 1, more enemies will added to a maximum of 3
        //The enemy the player hit in the world map will be placed in the middle 
        if (Player.instance.hitedEnemy == "Boss")
        {
            Instantiate(enemyType[2], enemySpawners[0].transform.position, Quaternion.identity);
        }
        else
        if (Player.instance.player.level == 1)
        {
            if (Player.instance.hitedEnemy == "Enemy(Clone)")
            {
                Instantiate(enemyType[0], enemySpawners[0].transform.position, Quaternion.identity);
            }
            else if (Player.instance.hitedEnemy == "Fabulous Enemy(Clone)")
            {
                Instantiate(enemyType[1], enemySpawners[0].transform.position, Quaternion.identity);
            }
        }
        else {
            if (Player.instance.hitedEnemy == "Enemy(Clone)")
            {
                Instantiate(enemyType[0], enemySpawners[0].transform.position, Quaternion.identity);
            }
            else if (Player.instance.hitedEnemy == "Fabulous Enemy(Clone)")
            {
                Instantiate(enemyType[1], enemySpawners[0].transform.position, Quaternion.identity);
            }
            int tempR = Random.Range(0, enemyType.Length-1);
            Instantiate(enemyType[tempR], enemySpawners[1].transform.position, Quaternion.identity);
            Instantiate(enemyType[tempR], enemySpawners[2].transform.position, Quaternion.identity);

        }
        MakeSingleton();
        characterArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject characterElement in characterArray) {
            character.Add(characterElement);
        }
        character.Add(GameObject.FindGameObjectWithTag("Player"));
        turn = character.Count - 1;

    }
    void Update()
    {
            character = character.OrderBy(o => o.GetComponent<CharacterInfo>().agility).ToList();
        if (equalOnce == false)
        {
            for (int i = 0; i <= character.Count - 1; i++)
            {
                characterPos.Add(character[i].transform.position);
                characterPos[i].ToString();
            }
            equalOnce = true;
        }

        //Check if enemy or player health reached 0
        for (int i = 0; i <= character.Count - 1; i++) {
            if (character[i].GetComponent<CharacterInfo>().currentHP <= 0 && character[i].name=="Boss(Clone)")
            {
                Destroy(GameObject.Find("Player"));
                Destroy(GameObject.Find("Canvas"));
                Destroy(GameObject.Find("EventSystem"));
                SceneManager.LoadScene("MainMenu");
            }else
            if (character[i].GetComponent<CharacterInfo>().currentHP <= 0 && character[i].GetComponent<CharacterInfo>().characterType == "Enemy") {
                totalEarnedXp += 5 * Player.instance.player.level;
                goldEarned += 5;
                Destroy(character[i]);
                character.Remove(character[i]);
                characterPos.Remove(characterPos[i]);
            } else if (character[i].GetComponent<CharacterInfo>().currentHP <= 0 && character[i].GetComponent<CharacterInfo>().characterType == "Player"){
                Destroy(GameObject.Find("Player"));
                Destroy(GameObject.Find("Canvas"));
                Destroy(GameObject.Find("EventSystem"));
                SceneManager.LoadScene("MainMenu");
            }
        }

        if (character.Count == 1 && character[0].name == "Player") {
            Player.instance.player.currentXP += totalEarnedXp;
            Player.instance.gold += goldEarned;
            if (!UI.instance.defaultBattlePanel.activeInHierarchy)
            {
                UI.instance.defaultBattlePanel.SetActive(true);
                UI.instance.skillsPanel.SetActive(false);
            }
            SceneManager.LoadScene("Level1");
        }

        if (Input.GetKeyDown("u"))
        {
            for (int i = 0; i <= character.Count - 1; i++)
            {
                Debug.Log(character.Count.ToString());
            }
        }

        if (character[turn].GetComponent<CharacterInfo>().characterType == "Enemy")
        {

            AttackAnimaiton(character[turn], GameObject.Find("Player"), characterPos[turn], out moveF, out moveB, out writeL, out lap, character[turn].GetComponent<CharacterInfo>().dmg,out UI.instance.attack);
            UI.instance.attack = false;
        }
        else if (character[turn].GetComponent<CharacterInfo>().characterType == "Player" && UI.instance.attack)
        {
            AttackAnimaiton(character[turn], UI.instance.selectedTarget, characterPos[turn], out moveF, out moveB, out writeL, out lap, dealDmg,out UI.instance.attack);

        }
        
    }

    void MakeSingleton() {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else {
            instance=this;
        }
    }
    void AttackAnimaiton(GameObject characterM, GameObject target,Vector3 originalPos,out bool moveForward,out bool moveBack,out bool writeLog,out bool l,int damageDealt,out bool a) {
        moveForward = moveF;
        moveBack = moveB;
        writeLog = writeL;
        l = lap;
        a = UI.instance.attack;
        if (moveForward )
        {
            AttackMovement(characterM, target.transform.position);
            if (writeLog)
            {

                UI.instance.combatLogString = characterM.name.ToString() + " dealt " + damageDealt + "\n" + UI.instance.combatLogString;
                writeLog = false;
            }
        }

        if (target == null) {
            if (turn != 0)
            {
                turn -= 1;
            }
            else { turn = character.Count - 1; }
            l = false;
            moveForward = true;
            moveBack = false;
            writeLog = true;
        }else 
        if (moveForward && Vector3.Distance(characterM.transform.position, target.transform.position) <= 1)
        {
            characterM.GetComponent<AudioSource>().Play();
            moveBack = true;
            moveForward = false;
        }
        if (moveBack)
        {
            AttackMovement(characterM, originalPos);
        }

        if (moveBack == true && Vector3.Distance(characterM.transform.position, originalPos) <= 0)
        {
            l = true;
            moveBack = false;
        }
        if (l)
        {
            target.GetComponent<CharacterInfo>().currentHP -= damageDealt;
            if (turn != 1 && target.GetComponent<CharacterInfo>().currentHP <= 0)
            {
                turn -= 2;
            }
            else
            if (turn == 1 && target.GetComponent<CharacterInfo>().currentHP <= 0)
            { turn = character.Count - 2; }
            else if (turn != 0 && target.GetComponent<CharacterInfo>().currentHP > 0)
            {
                turn -= 1;
            }
            else {
                turn = character.Count - 1;
            }

            l = false;
            moveForward = true;
            moveBack = false;
            writeLog = true;
            
            a = false;
        }

    }



    public void AttackMovement(GameObject startingPos, Vector3 endingPos) {
            startingPos.transform.position = Vector3.MoveTowards(startingPos.transform.position, endingPos, 10 * Time.deltaTime);       
        
    }

}

