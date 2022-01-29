using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitPoint = 10;
    public int maxHitpoint = 10;
    public float pushRecoveryspeed = 0.2f;

    protected float immuneTime = 1.0f;
    protected float lastImmune;

    protected Vector3 pushDirection;

    protected virtual void  ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 60, Color.red, transform.position, transform.up * 45, 0.5f);

            if(hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        
    }
}
