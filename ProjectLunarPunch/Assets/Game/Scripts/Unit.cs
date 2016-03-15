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
    public bool debuff_dog = false;
    public int debuff_dog_time;

    protected enum AttackState
    {
        NotAttacking,
        Attacking,
        Returning
    }

    bool runin = true;
    float count = 0;

    protected const float CRIT_BONUS = 1.5f;
    protected const float DOG_DEBUFF_REDUCTION = 0.5f;
    protected const float ADVANTAGE_BONUS = 1.2f;
    protected const float DEFENDING_MODIFIER = 0.5f;
    protected const float ATTACK_ANIMATION_TIME = 1.0f;
    protected const int DOG_DEBUFF_DURATION = 3;

    protected AttackState attackState = AttackState.NotAttacking;
    protected Timer attackTimer = new Timer();
    protected Unit target;
    protected Vector3 originalPosition;

    int attStateHash = Animator.StringToHash("Base Layer.run");

    void Update()
    {
        attackAnimation();
    }

    protected virtual void attackAnimation()
    {
        //gameObject.transform.Rotate(Vector3.up, 180);

        //gameObject.transform.LookAt(target.originalPosition);

        AnimatorStateInfo animState = gameObject.GetComponent<spearmanAnim>().anim.GetCurrentAnimatorStateInfo(0);

        if (attackState == AttackState.Attacking)
        {
            attackTimer.update(Time.deltaTime);

            //TODO: remove this
            Vector3 targetPosition = target.transform.position + target.transform.forward * 100f;
            targetPosition.y = 0;

            //print(Vector3.Distance(targetPosition, originalPosition));

            gameObject.transform.position = Vector3.Lerp(targetPosition, originalPosition, attackTimer.getTime() / (ATTACK_ANIMATION_TIME));
            gameObject.transform.LookAt(target.transform.position, new Vector3(0f, 1f, 0f));

            Debug.Log(attackState);

            gameObject.GetComponent<spearmanAnim>().setRun(true);

            if (attackTimer.isDone())
            {
                if(runin)
                {
                    gameObject.GetComponent<spearmanAnim>().callAttack();
                    gameObject.GetComponent<spearmanAnim>().setRun(false);
                    runin = false;
                }

                count += 0.02f;
                
                //gameObject.GetComponent<spearmanAnim>().anim.anima
               
                Debug.Log(gameObject.GetComponent<spearmanAnim>().anim.GetCurrentAnimatorStateInfo(0).ToString());

                if(count > 1)
                {
                    dealDamage(target);
                    attackTimer.setTime(ATTACK_ANIMATION_TIME);
                    attackState = AttackState.Returning;
                    runin = true;
                    count = 0;

                    Debug.Log(attackState);
                }
                
            }
        }
        else if (attackState == AttackState.Returning)
        {
            attackTimer.update(Time.deltaTime);

            //TODO: remove this
            Vector3 Target = target.transform.position;
            Target.y = 0;
            gameObject.transform.Rotate(Vector3.up, 180);

            gameObject.transform.position = Vector3.Lerp(originalPosition, Target, attackTimer.getTime() / ATTACK_ANIMATION_TIME);
            gameObject.transform.LookAt(originalPosition, new Vector3(0f, 1f, 0f));

            gameObject.GetComponent<spearmanAnim>().setRun(true);

            if (attackTimer.isDone())
            {
                attackState = AttackState.NotAttacking;
                gameObject.transform.localRotation = new Quaternion();

                gameObject.GetComponent<spearmanAnim>().setRun(false);
            }
        }
    }

    protected void dealDamage(Unit target)
    {
        if (debuff_dog)
        {
            debuff_dog_time -= 1;
            if (debuff_dog_time <= 0)
            {
                debuff_dog = false;
            }
        }
        
        string log = "";
        bool crit = (Random.Range(0f, 100f) < critChance);

        float damage = att * Random.Range(0.9f, 1.1f);
        damage *= (100f / (100f + target.def));



        if (hasAdvantage(type, target.type))
        {
            damage *= ADVANTAGE_BONUS;
        }

        if (crit)
        {
            // TODO: add special logic for berserkers and swordsmen.
            damage *= CRIT_BONUS;
            log += "Critical hit! ";
        }

        if (target.defending)
        {
            damage *= DEFENDING_MODIFIER;
        }

        if (debuff_dog)
        {
            damage *= DOG_DEBUFF_REDUCTION;
        }

        string firstUnit, secondUnit;
        if(ally)
        {
            firstUnit = "Ally ";
            secondUnit = "Enemy ";
        }
        else
        {
            firstUnit = "Enemy ";
            secondUnit = "Ally ";
        }

        log += firstUnit + gameObject.name + " attacking " + secondUnit + target.gameObject.name + " for " + damage + " damage.\n";
        BattleManager.logBattle(log);

        target.takeDamage(damage);
        if (type == ClassType.Warhound)
        {
            target.ApplyDebuff();
        }
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
        gameObject.GetComponent<spearmanAnim>().callHit();

        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;

            // TODO: death logic here...b
            gameObject.GetComponent<spearmanAnim>().setDeath(true);

            gameObject.SetActive(false);
            BattleManager.logBattle(gameObject.name + " has died!\n");
        }
        else
        {
            BattleManager.logBattle(gameObject.name + " has " + hp + " HP remaining.\n");
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

    public void ApplyDebuff()
    {
        debuff_dog_time = DOG_DEBUFF_DURATION;
        debuff_dog = true;
    }
}
