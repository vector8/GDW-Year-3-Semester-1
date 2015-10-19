using UnityEngine;
using System.Collections;

public enum ClassType
{
    Archer,
    Berserker,
    Horseman,
    Spearman,
    Swordsman,
    Warhound
}

public class Unit : MonoBehaviour
{
    // TODO: put real stats here
    public float hp, maxHp, att, def, speed, critChance;
    public ClassType type;
    public bool defending = false;
    public bool ally = false;

    private const float CRIT_BONUS = 1.5f;
    private const float ADVANTAGE_BONUS = 1.2f;
    private const float DEFENDING_MODIFIER = 0.5f;

    public void attack(Unit other)
    {
        if(!other.gameObject.activeSelf)
        {
            return;
        }

        bool crit = (Random.Range(0f, 100f) < critChance);

        float damage = att * ((100f - other.def) / 100f);

        if(hasAdvantage(type, other.type))
        {
            damage *= ADVANTAGE_BONUS;
        }

        if (crit)
        {
            // TODO: add special logic for berserkers and swordsmen.
            damage *= CRIT_BONUS;
            print("Critical hit!");
        }

        if (other.defending)
        {
            damage *= DEFENDING_MODIFIER;
        }

        // TODO: Add animation logic?
        print(gameObject.name + " attacking " + other.gameObject.name + " for " + damage + " damage.");

        other.takeDamage(damage);
    }

    public void takeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;

            // TODO: death logic here...
            gameObject.SetActive(false);
            print(gameObject.name + " has died!");
        }
        else
        {
            print(gameObject.name + " has " + hp + " HP remaining.");
        }
    }

    public static bool hasAdvantage(ClassType first, ClassType second)
    {
        switch(first)
        {
            case ClassType.Archer:
                return second == ClassType.Spearman;
            case ClassType.Berserker:
                return second == ClassType.Warhound;
            case ClassType.Horseman:
                return second == ClassType.Swordsman;
            case ClassType.Spearman:
                return second == ClassType.Horseman;
            case ClassType.Swordsman:
                return second == ClassType.Berserker;
            case ClassType.Warhound:
                return second == ClassType.Archer;
            default:
                return false;
        }
    }
}
