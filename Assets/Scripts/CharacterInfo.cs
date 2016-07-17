using UnityEngine;
using System.Collections;

public class CharacterInfo : MonoBehaviour
{
    //[SerializeField]
    public string battlePhase, characterType;
    //[SerializeField]
    public int currentHP, currentMana, currentXP, level,currentMaxXP;
   // [SerializeField]
    public int maxHP, maxMana;
    //[SerializeField]
    public int agility,strength,intelligence;
    public int dmg;
    private int[] maxXP = new int[5];

    void Awake() {
        maxXP[0] = 5;
        maxXP[1] = 25;
        maxXP[2] = 50;
        maxXP[3] = 75;
        maxXP[4] = 100;
        if (gameObject.name == "Player") {
            Character("Player", 1, 20, 10,3, 3,1,1);
        }
        if (gameObject.name == "Enemy(Clone)")
        {
            Character("Enemy", Player.instance.player.level, 10, 5,1, 2,1,2);
        }
        if (gameObject.name == "Fabulous Enemy(Clone)")
        {
            Character("Enemy", Player.instance.player.level, 10, 5,1, 2,1,2);
        }
        if (gameObject.name == "Boss(Clone)")
        {
            Character("Enemy", Player.instance.player.level, 20, 5, 3, 2, 1, 2);
        }
    }
    //type=character type(player/enemy)
    public void Character(string type, int lvl, int hp, int mana,int baseDmg, int a,int s,int i)
    {
        battlePhase = "Waiting";
        characterType = type;
        level = lvl;
        currentHP = maxHP = hp;
        currentMana = maxMana = mana;
        currentXP = 0;
        agility = a;
        strength = s;
        intelligence = i;
        dmg = baseDmg+strength;
        currentMaxXP =10;
    }

    void Update() {
        if (level>=1) {
            currentMaxXP = maxXP[level - 1];
        }
    }


}
