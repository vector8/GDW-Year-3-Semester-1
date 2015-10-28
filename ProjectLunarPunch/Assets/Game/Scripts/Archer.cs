using UnityEngine;
using System.Collections;

public class Archer : Unit
{
    public GameObject arrowPrefab;

    private GameObject arrow;

    protected override void attackAnimation()
    {
        if (attackState == AttackState.Attacking)
        {
            arrow = Instantiate<GameObject>(arrowPrefab);
            attackState = AttackState.Returning;
            gameObject.transform.LookAt(target.transform, new Vector3(0f, 1f, 0f));
        }
        else if (attackState == AttackState.Returning)
        {
            attackTimer.update(Time.deltaTime);

            Vector3 yOffset = new Vector3(0f, 100f, 0f);

            arrow.transform.position = Vector3.Lerp(target.transform.position + yOffset, originalPosition + yOffset, attackTimer.getTime() / ATTACK_ANIMATION_TIME);
            arrow.transform.LookAt(target.transform.position + yOffset);

            if (attackTimer.isDone())
            {
                dealDamage(target);
                attackState = AttackState.NotAttacking;
                Destroy(arrow);
                gameObject.transform.localRotation = new Quaternion();
            }
        }
    }
}
