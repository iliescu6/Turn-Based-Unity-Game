using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour {

    public static UI instance;

    //PANELS
    [HideInInspector]
    public GameObject battle,notBattle,shopPanel,skillsPanel,defaultBattlePanel,confirmPanel,insufficientPanel;
    public CharacterInfo player;

    public Text stats,turnText,invalidTarget,combatLog,goldText;
    public string combatLogString="";
    private bool loaded = true;

    //For selecting and attacking an enemy
    private RaycastHit hit;
    public GameObject selectedTarget;
    public GameObject[] skillArray;
    public Skill[] skill=new Skill[4];

    public bool attack = false;

    void MakeSkill(Skill s, string _type, string _name, string _description, int _mC, int _dOH, int _tTW, int _sW, int _rLvl)
    {
        s.Type = _type;
        s.Name = _name;
        s.Description = _description;
        s.ManaCost = _mC;
        s.DamageOrHeal = _dOH;
        s.TurnsToWait = _tTW;
        s.StunWait = _sW;
        s.RequiredLevel = _rLvl;
    }

    void Awake() {
        MakeSingleton();
        player = GameObject.Find("Player").GetComponent<CharacterInfo>();
        skillArray = new GameObject[4];
        skill[0] = new Skill();
        skill[1] = new Skill();
        skill[2] = new Skill();
        skill[3] = new Skill();
        for (int i = 0; i < 4; i++) {
            skillArray[i] = GameObject.Find("Skill (" + i.ToString() + ")");         
        }
        //Panels
        battle = GameObject.Find("Battle Panel");
        notBattle = GameObject.Find("Out Of Battle Panel");
        shopPanel = GameObject.Find("Shop Panel");
        skillsPanel = GameObject.Find("Skills Panel");
        defaultBattlePanel = GameObject.Find("Default Battle Panel");
        confirmPanel = GameObject.Find("Confirm Panel");
        insufficientPanel = GameObject.Find("Insufficient Panel");


        skillsPanel.SetActive(false);
        insufficientPanel.SetActive(false);
       

        //Text
        stats = GameObject.Find("Player Stats").GetComponent<Text>();
        turnText = GameObject.Find("Turn Text").GetComponent<Text>();
        invalidTarget = GameObject.Find("Invalid Target Text").GetComponent<Text>();
        combatLog = GameObject.Find("Combat Log Text").GetComponent<Text>();
        goldText = GameObject.Find("Gold Text").GetComponent<Text>();
    }
	// Update is called once per frame
	void Update () {


        //SKILLS
        MakeSkill(skill[0], "Damage", "Heavy Strike", "Deals base damage x 2 "+"("+ (GameObject.Find("Player").GetComponent<CharacterInfo>().dmg * 2).ToString()+" damage)", 10, GameObject.Find("Player").GetComponent<CharacterInfo>().dmg * 2, 3, 0, 2);
        MakeSkill(skill[1], "Damage", "Kewl Strike", "Deals base damage + strength ("+ (player.dmg + player.strength).ToString()+" damage)", 10, player.dmg +player.strength, 3, 0, 2);
        MakeSkill(skill[2], "Damage", "Kewl Strike", "Deals base damage + strength (" + (player.dmg + player.strength).ToString() + " damage)", 10, player.dmg + player.strength, 3, 0, 2);
        MakeSkill(skill[3], "Damage", "Kewl Strike", "Deals base damage + strength (" + (player.dmg + player.strength).ToString() + " damage)", 10, player.dmg + player.strength, 3, 0, 2);


        //PLAYER STATS
        stats.text = "Level: " + player.level + "\n" + "Health: " + player.currentHP + "/" + player.maxHP + "\n" + "Mana: " + player.currentMana + "/" + player.maxMana + "\n" + "Damage: " + player.dmg + "\n" + "Strength: " + player.strength +
            "\n" + "Intelligence: " + player.intelligence + "\n" + "Agility: " + player.agility + "\n" + "Experience points: " + player.currentXP + "/" + player.currentMaxXP + "\n";
        goldText.text = "Gold:" + Player.instance.gold.ToString();
        
        //Out of battle code
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            turnText.text = "";
            invalidTarget.text = "";         
            if (battle.activeInHierarchy)
            {
                battle.SetActive(false);
            }
            if (!notBattle.activeInHierarchy)
            {
                notBattle.SetActive(true);
            }
            attack = false;
            loaded = true;
        }
        //In battle code
        else if (SceneManager.GetActiveScene().name == "Battle")
        {
            if (loaded == true) {
                combatLogString = "";
                loaded = false;
            }
            combatLog.text = combatLogString;
            turnText.text = BattleManager.instance.character[BattleManager.instance.turn].name + "'s turn";

            if (hit.transform == null) {
                invalidTarget.text = "Target:none";
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        selectedTarget = GameObject.Find(hit.transform.gameObject.name);
                        invalidTarget.text = "Target:" + selectedTarget.name;
                    }
                    else if (hit.transform.gameObject.tag == "Player")
                    {
                        selectedTarget = GameObject.Find(hit.transform.gameObject.name);
                        invalidTarget.text = "Target:" + selectedTarget.name;
                    }
                }
            }
            if (selectedTarget == null) {
                selectedTarget = GameObject.Find("Player");
                invalidTarget.text = "Target:" + selectedTarget.name;
            }
            if (!battle.activeInHierarchy)
            {
                battle.SetActive(true);
                
            }
            if (notBattle.activeInHierarchy)
            {
                notBattle.SetActive(false);
            }
        }

    }

    //Singleton
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

   

    //Button methods
    public void ShopPanel() {
        if (!shopPanel.activeInHierarchy) {
            shopPanel.SetActive(true);
        }
    }

    public void InventoryPanel() {
        if (Inventory.instance.inven != true) {
            Inventory.instance.inven = true;

        }
    }

    public void Attack() {
        if (BattleManager.instance.character[BattleManager.instance.turn].GetComponent<CharacterInfo>().characterType == "Player") {
            if (selectedTarget.tag == "Enemy")
            {
                attack = true;
                BattleManager.instance.dealDmg = Player.instance.player.dmg;
            }
            
        }
    }

    public void SkillAttack()
    {
        
        string n = EventSystem.current.currentSelectedGameObject.name;
        Skill s=new Skill();
        if (BattleManager.instance.character[BattleManager.instance.turn].GetComponent<CharacterInfo>().characterType == "Player")
        {
            if (n == "Skill (0)")
        {
            s = skill[0];
        }
        else if (n == "Skill (1)")
        {
            s = skill[1];
        }
        else if (n == "Skill (2)")
        {
            s = skill[2];
        }
        else if (n == "Skill (3)") {
            s = skill[3];
        }
        if (GameObject.Find("Player").GetComponent<CharacterInfo>().currentMana>=s.manaCost){
                if (s.type == "Damage")
                {
                    player.currentMana -= s.manaCost;
                    if (selectedTarget.gameObject.tag == "Enemy")
                    {
                        attack = true;
                        BattleManager.instance.dealDmg = s.damageOrHeal;
                    }
                    else {
                        //Show on ui invalid target
                    }
                    
                }
                else if (s.type == "Heal") {
                    if (player.currentHP < player.maxHP)
                    {
                        player.currentHP += s.damageOrHeal;
                    }
                    else {
                        //Hp is full
                    }
                }
        }
        }
    }

    public void Skills() {
        if (!skillsPanel.activeInHierarchy) {
            defaultBattlePanel.SetActive(false);
            skillsPanel.SetActive(true);
        }
    }

    public void ReturnToDefaultPanel() {
        if (!defaultBattlePanel.activeInHierarchy) {
            defaultBattlePanel.SetActive(true);
            skillsPanel.SetActive(false);
        }
    }
}
