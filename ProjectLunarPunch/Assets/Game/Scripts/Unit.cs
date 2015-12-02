using UnityEngine;
using System.Collections;

public enum ClassType
{
    Archer,
    Berserker,
    Cavalry,
    Spearman,
    Swordsman,
    Warhound,
    NotSet
}

public class Unit : MonoBehaviour
{
    // TODO: put real stats here
    public float hp, maxHp, att, def, speed, critChance;
    public ClassType type;
    public bool defending = false;
    public bool ally = false;

    protected enum AttackState
    {
        NotAttacking,
        Attacking,
        Returning
    }

    protected const float CRIT_BONUS = 1.5f;
    protected const float ADVANTAGE_BONUS = 1.2f;
    protected const float DEFENDING_MODIFIER = 0.5f;
    protected const float ATTACK_ANIMATION_TIME = 0.5f;

    protected AttackState attackState = AttackState.NotAttacking;
    protected Timer attackTimer = new Timer();
    protected Unit target;
    protected Vector3 originalPosition;

    void Update()
    {
        attackAnimation();
    }

    protected virtual void attackAnimation()
    {
        if (attackState == AttackState.Attacking)
        {
            attackTimer.update(Time.deltaTime);

            gameObject.transform.position = Vector3.Lerp(target.transform.position, originalPosition, attackTimer.getTime() / ATTACK_ANIMATION_TIME);
            gameObject.transform.LookAt(target.transform, new Vector3(0f, 1f, 0f));

            if (attackTimer.isDone())
            {
                dealDamage(target);
                attackTimer.setTime(ATTACK_ANIMATION_TIME);
                attackState = AttackState.Returning;
            }
        }
        else if (attackState == AttackState.Returning)
        {
            attackTimer.update(Time.deltaTime);

            gameObject.transform.position = Vector3.Lerp(originalPosition, target.transform.position, attackTimer.getTime() / ATTACK_ANIMATION_TIME);
            gameObject.transform.LookAt(target.transform, new Vector3(0f, 1f, 0f));

            if (attackTimer.isDone())
            {
                attackState = AttackState.NotAttacking;
                gameObject.transform.localRotation = new Quaternion();
            }
        }
    }

    protected void dealDamage(Unit target)
    {
        bool crit = (Random.Range(0f, 100f) < critChance);

        float damage = att * ((100f - target.def) / 100f);

        if (hasAdvantage(type, target.type))
        {
            damage *= ADVANTAGE_BONUS;
        }

        if (crit)
        {
            // TODO: add special logic for berserkers and swordsmen.
            damage *= CRIT_BONUS;
            //print("Critical hit!");
        }

        if (target.defending)
        {
            damage *= DEFENDING_MODIFIER;
        }

        //print(gameObject.name + " attacking " + target.gameObject.name + " for " + damage + " damage.");

        target.takeDamage(damage);
    }

    public void attack(Unit other)
    {
        if(!other.gameObject.activeSelf)
        {
            return;
        }

        attackState = AttackState.Attacking;
        attackTimer.setTime(ATTACK_ANIMATION_TIME);
        target = other;
        originalPosition = gameObject.transform.position;
    }

    public void takeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;

            // TODO: death logic here...
            gameObject.SetActive(false);
            //print(gameObject.name + " has died!");
        }
        //else
        //{
        //    print(gameObject.name + " has " + hp + " HP remaining.");
        //}
    }

    public static bool hasAdvantage(ClassType first, ClassType second)
    {
        switch(first)
        {
            case ClassType.Archer:
                return second == ClassType.Spearman;
            case ClassType.Berserker:
                return second == ClassType.Warhound;
            case ClassType.Cavalry:
                return second == ClassType.Swordsman;
            case ClassType.Spearman:
                return second == ClassType.Cavalry;
            case ClassType.Swordsman:
                return second == ClassType.Berserker;
            case ClassType.Warhound:
                return second == ClassType.Archer;
            default:
                return false;
        }
    }

    public bool isAttacking()
    {
        return attackState != AttackState.NotAttacking;
    }

    public bool isDead()
    {
        return Mathf.Approximately(hp, 0f);
    }
}
