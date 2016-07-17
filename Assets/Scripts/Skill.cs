
using UnityEngine;
using System.Collections;

public class Skill
{

    //type-type of skill (attack)/buff/debuff/heal
    public string type, name, description;
    //turnstowait-turns to wait after skill is used
    //stunWait-stunned for x turns

    public int manaCost, damageOrHeal, turnsToWait, stunWait, requiredLevel;

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public int ManaCost
    {
        get { return manaCost; }
        set { manaCost = value; }
    }

    public int DamageOrHeal
    {
        get { return damageOrHeal; }
        set { damageOrHeal = value; }
    }

    public int TurnsToWait
    {
        get { return turnsToWait; }
        set { turnsToWait = value; }
    }

    public int StunWait
    {
        get { return stunWait; }
        set { stunWait = value; }
    }

    public int RequiredLevel
    {
        get { return requiredLevel; }
        set { requiredLevel = value; }
    }
}
