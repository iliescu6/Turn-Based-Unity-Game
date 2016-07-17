using UnityEngine;
using System.Collections;

public class Item
{
    public string itemName,type;
    public int health,mana,strength,intelligence,agility,damage,gold;

    public Item(string name,string t, int hp, int m, int s, int i, int a,int dmg,int g)
    {
        itemName = name;
        type = t;
        health = hp;
        mana = m;
        strength = s;
        intelligence = i;
        agility = a;
        damage = dmg;
        gold = g;

    }
    public Item() { }
}